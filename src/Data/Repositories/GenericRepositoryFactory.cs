using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class GenericRepositoryFactory : IRepositoryFactory
    {
        private readonly CampaignDbContext _dbContext;

        public GenericRepositoryFactory(CampaignDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<T> GetRepository<T>() where T : EntityBase
        {
            return new GenericRepository<T>(_dbContext);
        }
    }
}
