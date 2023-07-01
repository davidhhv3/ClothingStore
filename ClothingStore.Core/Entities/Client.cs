namespace ClothingStore.Core.Entities
{
    public class Client : BaseEntity
    {      
        public int Country { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public long IdentificationNumber { get; set; }
    }
}
