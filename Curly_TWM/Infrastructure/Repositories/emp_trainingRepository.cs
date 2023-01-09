using Curly_TWM.Domain.Entities;
using Curly_TWM.Domain.Interfaces;
using Curly_TWM.Infrastructure.DbaseContext;

namespace Curly_TWM.Infrastructure.Repositories
{
    public class emp_trainingRepository : GenericRepository<emp_training>, Iemp_trainingRepository
    {

        public emp_trainingRepository(TWMDB _context) : base(_context)
        {

        }
     
    }

}