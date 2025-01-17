﻿using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly DbContextOptions<MySQLContext> _context;

    public OrderRepository(DbContextOptions<MySQLContext> context)
    {
        _context = context;
    }

    public async Task<bool> AddOrder(OrderHeader header)
    {
        if (header == null) return false;

        await using var _db = new MySQLContext(_context);
        _db.OrderHeaders.Add(header);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateOrderPaymentStatus(int orderHeaderId, bool status)
    {
        await using var _db = new MySQLContext(_context);
        var header = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.OrderHeaderId == orderHeaderId);
        if(header != null)
        {
            header.PaymentStatus = status;
            await _db.SaveChangesAsync();
        }               
        return true;
    }
}
