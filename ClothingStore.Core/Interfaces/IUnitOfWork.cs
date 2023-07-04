namespace ClothingStore.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICountryRepository CountryRepository { get; }

        IClientRepository ClientRepository { get; }
        ISecurityRepository SecurityRepository { get; }     
        Task SaveChangesAsync();
    }

}
