using ClothingStore.Core.Interfaces;
using ClothingStore.Infrastructure.Data;

namespace ClothingStore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClothingStoreContext _context;
        private readonly ICountryRepository? _countryRepository;
        private readonly ISecurityRepository? _securityRepository;

        public UnitOfWork(ClothingStoreContext context)
        {            
             _context = context;
        }
        public ICountryRepository CountryRepository => _countryRepository ?? new CountryRepository(_context);
        public ISecurityRepository SecurityRepository => _securityRepository ?? new SecurityRepository(_context);

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }

}
