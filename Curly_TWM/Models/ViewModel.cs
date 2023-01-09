using System.Collections.Generic;
using Curly_TWM.Domain.Entities;

namespace Curly_TWM.Models
{
    public class ViewModel
    {
        public IEnumerable<Initiatives> Initiative { get; set; }
        public IEnumerable<Teams> Teams { get; set; }
    }
}