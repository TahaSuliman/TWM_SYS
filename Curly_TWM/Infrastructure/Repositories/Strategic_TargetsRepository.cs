using Curly_TWM.Domain.Entities;
using Curly_TWM.Domain.Interfaces;
using Curly_TWM.Infrastructure.DbaseContext;

namespace Curly_TWM.Infrastructure.Repositories
{
    public class Strategic_TargetsRepository : GenericRepository<Strategic_Targets>, IStrategic_TargetsRepository
    {

        public Strategic_TargetsRepository(TWMDB _context) : base(_context)
        {

        }
     
    }

}