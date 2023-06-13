using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Infrastructure.Data;

namespace ClothingStore.Infrastructure.Repositories
{
    internal class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(ClothingStoreContext context) : base(context) { } 
    }

}
