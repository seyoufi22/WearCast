namespace WearCast.Api.Entities;

public class PlatformSettings
{
    public int Id { get; set; }
    public decimal CommissionPercentage { get; set; } = 2m; // Default 2%
    public string? UpdatedById { get; set; }
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
}
