namespace WearCast.Api.Features.Sellers.SellerManagers.DeleteSellerManager;

public record DeleteSellerManagerRequest(
    int SellerManagerId,  
    string CurrentUserId,  
    bool IsSuperAdmin,
    string Reason
) : IRequest<Result>;

public record DeleteSellerManagerBody(
    string Reason
);