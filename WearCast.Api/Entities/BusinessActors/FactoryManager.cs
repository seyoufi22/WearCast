namespace WearCast.Api.Entities.BusinessActors
{
    public class FactoryManager : ISoftDeletable
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public int FactoryId { get; set; }

        public Factory? Factory { get; set; }
    }
}
