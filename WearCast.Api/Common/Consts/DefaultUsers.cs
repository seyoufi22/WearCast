namespace WearCast.Api.Common.Consts;

public static class DefaultUsers
{
    public static readonly List<(string Id, string Email, string Password, string SecurityStamp, string ConcurrencyStamp)> Admins =
    [
        (
            Id: "0198e260-1145-79be-a3d9-2e6cbab97464", // guid generator do not remove the dashes
            Email: "admin@wearcast.com",
            Password: "P@ssword123",
            SecurityStamp: "3AA96F0F9B1A45D3BE93622F0B4B52E3",  // tools + create guid 
            ConcurrencyStamp: "0198e260114579bea3d92e6d310f8026" // online guid generator but remove the dashes
        )
    ];
}
