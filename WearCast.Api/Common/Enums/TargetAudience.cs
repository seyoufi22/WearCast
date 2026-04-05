namespace WearCast.Api.Common.Enums;

[Flags]
public enum TargetAudience
{
    Men = 1,
    Women = 2,
    Unisex = Men | Women,
    Kids = 4,
    Babies = 8
}
