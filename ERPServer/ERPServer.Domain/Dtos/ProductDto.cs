﻿namespace ERPServer.Domain.Dtos;

public sealed record class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }=string.Empty;
    public decimal Quantity { get; set; }


}


