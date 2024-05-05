﻿using Ardalis.SmartEnum;

namespace ERPServer.Domain.Enums;

public sealed class OrderStatusEnum : SmartEnum<OrderStatusEnum>
{
    public static readonly OrderStatusEnum Pending = new("Bekliyor", 1);
    public static readonly OrderStatusEnum RequirentmentsPlanWorked = new("İhtiyaç Planı Çalıştırıldı", 2);
    public static readonly OrderStatusEnum Completed = new("Tamamlandı", 3);


    public OrderStatusEnum(string name, int value) : base(name, value)
    {
    }
}
