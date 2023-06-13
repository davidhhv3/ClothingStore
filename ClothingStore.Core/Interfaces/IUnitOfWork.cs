namespace ClothingStore.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICountryRepository CountryRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }

}
