﻿using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer;

public class RabbitMQCheckoutConsumer : BackgroundService
{
    private readonly OrderRepository _orderRepository;
    private IConnection _connection;
    private IModel _channel;
    private IRabbitMQMessageSender _rabbitMQMessageSender;

    public RabbitMQCheckoutConsumer(OrderRepository orderRepository,
        IRabbitMQMessageSender rabbitMQMessageSender)
    {
        _orderRepository = orderRepository;
        _rabbitMQMessageSender = rabbitMQMessageSender;
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (channel, evt) =>
        {
            var content = Encoding.UTF8.GetString(evt.Body.ToArray());
            CheckoutHeaderVO vo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);
            ProcessarOrder(vo).GetAwaiter().GetResult();
            _channel.BasicAck(evt.DeliveryTag, false);
        };
        _channel.BasicConsume("checkoutqueue", false, consumer);
        return Task.CompletedTask;
    }

    private async Task ProcessarOrder(CheckoutHeaderVO vo)
    {
        OrderHeader order = new()
        {
            UserId = vo.UserId,
            FirstName = vo.FirstName,
            LastName = vo.LastName,
            OrderDetails = new List<OrderDetail>(),
            CardNumber = vo.CardNumber,
            CouponCode = vo.CouponCode,
            CVV = vo.CVV,
            DiscountAmount = vo.DiscountAmount,
            Email = vo.Email,
            ExpireMonthYear = vo.ExpireMonthYear,
            OrderTime = DateTime.Now,
            PurchaseAmount = vo.PurchaseAmount,
            PaymentStatus = false,
            Phone = vo.Phone,
            DateTime = vo.DateTime,                
        };

        foreach (var details in vo.CartDetails)
        {
            OrderDetail detail = new OrderDetail()
            {
                ProductId = details.ProductId,
                ProductName = details.Product.Name,
                Price = details.Product.Price,
                Count = details.Count
            };
            order.CartTotalItens += details.Count;
            order.OrderDetails.Add(detail);
        }
        await _orderRepository.AddOrder(order);

        PaymentVO payment = new()
        {
            Name = order.FirstName + " " + order.LastName,
            CardNumber = order.CardNumber,
            CVV = order.CVV,
            ExpiryMonthYear = order.ExpireMonthYear,
            OrderId = order.OrderHeaderId,
            PurchaseAmount = order.PurchaseAmount,
            Email = order.Email
        };
        try
        {
            _rabbitMQMessageSender.SendMessage(payment, "orderpaymentprocessorqueue");
        }
        catch (Exception)
        {
            throw;
        }
    }
}
