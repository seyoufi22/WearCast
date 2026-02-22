namespace WearCast.Api.Common.ValueObjects
{
    [Owned]
    public class Address
    {
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
    }
}
