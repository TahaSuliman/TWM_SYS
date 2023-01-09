using Curly_TWM.Domain.Entities;
using Curly_TWM.Domain.Interfaces;
using Curly_TWM.Infrastructure.DbaseContext;

namespace Curly_TWM.Infrastructure.Repositories
{
    public class Team_TasksRepository : GenericRepository<Team_Tasks>, ITeam_TasksRepository
    {

        public Team_TasksRepository(TWMDB _context) : base(_context)
        {

        }
     
    }

}