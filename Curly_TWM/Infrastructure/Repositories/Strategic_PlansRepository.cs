using Curly_TWM.Domain.Entities;
using Curly_TWM.Domain.Interfaces;
using Curly_TWM.Infrastructure.DbaseContext;

namespace Curly_TWM.Infrastructure.Repositories
{
    public class Strategic_PlansRepository : GenericRepository<Strategic_Plans>, IStrategic_PlansRepository
    {

        public Strategic_PlansRepository(TWMDB _context) : base(_context)
        {

        }
     
    }

}