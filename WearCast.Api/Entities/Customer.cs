namespace WearCast.Api.Entities;

public class Customer : BaseModel
{
    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";
    
    public string Email { get; set; } = "";
    
    public string PhoneNumber { get; set; } = "";
    
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = default!;
    
    // Drafts TBD
    
    // CartItems TBD
    
    // Favourite TBD
}