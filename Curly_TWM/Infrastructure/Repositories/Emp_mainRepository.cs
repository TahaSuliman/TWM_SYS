using Curly_TWM.Domain.Entities;
using Curly_TWM.Domain.Interfaces;
using Curly_TWM.Infrastructure.DbaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Curly_TWM.Infrastructure.Repositories
{
    public class Emp_mainRepository : GenericRepository<emp_main>, IEmp_mainRepository
    {
        public Emp_mainRepository(TWMDB _context) : base(_context)
        {

        }

        public int InsertEmp(emp_main obj)
        {
            _dbSet.Add(obj);
            Save();

            int x = obj.Id;
            return x;
        }
    }
}