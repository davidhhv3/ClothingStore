namespace ClothingStore.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICountryRepository CountryRepository { get; }
        ISecurityRepository SecurityRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }

}
