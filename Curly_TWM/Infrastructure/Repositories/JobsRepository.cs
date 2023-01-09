using Curly_TWM.Domain.Entities;
using Curly_TWM.Domain.Interfaces;
using Curly_TWM.Infrastructure.DbaseContext;

namespace Curly_TWM.Infrastructure.Repositories
{
    public class JobsRepository : GenericRepository<Jobs>, IJobsRepository
    {

        public JobsRepository(TWMDB _context) : base(_context)
        {

        }
     
    }

}