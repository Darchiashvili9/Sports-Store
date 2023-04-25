namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext _Context;

        public EFStoreRepository(StoreDbContext Context)
        {
            _Context = Context;
        }

        public IQueryable<Product> Products => _Context.Products;
    }
}
