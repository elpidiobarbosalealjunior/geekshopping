﻿namespace GeekShopping.Web.Models;

public class CouponViewModel
{
    public int CouponId { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
}
