using System.Net;
using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Sellers.SellerApplications.GetAllSellerApplications.DTOs;

namespace WearCast.Api.Features.Sellers.SellerApplications.GetAllSellerApplications.Handlers
{
    public class GetAllSellerApplicationsHandler : IRequestHandler<GetAllSellerApplicationsRequestDTO, Result<PagingViewModel<GetAllSellerApplicationsResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllSellerApplicationsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingViewModel<GetAllSellerApplicationsResponseDTO>>> Handle(
            GetAllSellerApplicationsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var query = _context.SellerApplications.AsNoTracking();

            if (request.Status.HasValue)
            {
                query = query.Where(a => a.Status == request.Status);
            }
            if (request.IsEmailConfirmed.HasValue)
            {
                query = query.Where(a => a.ManagerEmailConfirmed == request.IsEmailConfirmed);
            }
            if (!string.IsNullOrWhiteSpace(request.SellerName))
            {
                query = query.Where(a => a.SellerName.Contains(request.SellerName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.SellerEmail))
            {
                query = query.Where(a => a.SellerEmail.Contains(request.SellerEmail.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.ManagerFirstName))
            {
                query = query.Where(a => a.ManagerFirstName.Contains(request.ManagerFirstName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.ManagerLastName))
            {
                query = query.Where(a => a.ManagerLastName.Contains(request.ManagerLastName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.City))
            {
                query = query.Where(a => a.SellerAddress.City.Contains(request.City.Trim()));
            }
            if (request.CreatedFrom.HasValue)
            {
                query = query.Where(a => a.CreatedOn >= request.CreatedFrom);
            }
            if (request.CreatedTo.HasValue)
            {
                query = query.Where(a => a.CreatedOn <= request.CreatedTo);
            }

            query = request.SortBy switch
            {
                SortBy.Oldest => query.OrderBy(a => a.CreatedOn),
                _ => query.OrderByDescending(a => a.CreatedOn)
            };

            var applicationsQuery = query
                .Select(a => new GetAllSellerApplicationsResponseDTO
                {
                    Id = a.Id,
                    SellerName = a.SellerName,
                    ManagerName = a.ManagerFirstName + " " + a.ManagerLastName,
                    City = a.SellerAddress.City, 
                    Status = a.Status,
                    SellerEmail=a.SellerEmail,
                    IsEmailConfirmed = a.ManagerEmailConfirmed,
                    CreatedOn = a.CreatedOn
                });

            var pagedResult = await PagingHelper.CreateAsync(applicationsQuery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}