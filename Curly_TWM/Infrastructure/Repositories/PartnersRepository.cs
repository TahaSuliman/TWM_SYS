using Curly_TWM.Domain.Entities;
using Curly_TWM.Domain.Interfaces;
using Curly_TWM.Infrastructure.DbaseContext;

namespace Curly_TWM.Infrastructure.Repositories
{
    public class PartnersRepository : GenericRepository<Partners>, IPartnersRepository
    {

        public PartnersRepository(TWMDB _context) : base(_context)
        {

        }
     
    }

}