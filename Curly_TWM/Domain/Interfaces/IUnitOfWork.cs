using Curly_TWM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curly_TWM.Domain.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {

        void Commit();

    }
}
