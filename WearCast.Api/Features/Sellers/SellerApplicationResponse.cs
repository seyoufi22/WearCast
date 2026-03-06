namespace WearCast.Api.Features.Sellers
{
    public record SellerApplicationResponse
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;

        public string SellerName { get; init; } = string.Empty;
        public string CommercialRegisterNumber { get; init; } = string.Empty;
        public string TaxIdNumber { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string LogoUrl { get; init; } = string.Empty;

        public string State { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string Street { get; init; } = string.Empty;
        public string BuildingNumber { get; init; } = string.Empty;

        public Status Status { get; init; }
        public string? RejectionReason { get; init; }
    }
}