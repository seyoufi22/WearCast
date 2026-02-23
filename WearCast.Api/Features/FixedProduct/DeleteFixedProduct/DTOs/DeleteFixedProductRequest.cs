using WearCast.Api.Abstractions;
using MediatR;

namespace WearCast.Api.Features.FixedProduct.DeleteFixedProduct.DTOs;

public record DeleteFixedProductRequest(int Id) : IRequest<Result>;
