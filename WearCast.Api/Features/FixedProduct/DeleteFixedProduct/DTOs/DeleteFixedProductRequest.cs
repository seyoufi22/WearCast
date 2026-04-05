using WearCast.Api.Abstractions;
using MediatR;

namespace WearCast.Api.Features.FixedProduct.DeleteFixedProduct.DTOs;

public record DeleteFixedProductRequest : IRequest<Result> { 
    public int Id { get; init; }
    [System.Text.Json.Serialization.JsonIgnore]
    public int SellerId { get; set; } = 0;
    [System.Text.Json.Serialization.JsonIgnore]
    public bool isAdminRequest { get; set; } = false;
}
