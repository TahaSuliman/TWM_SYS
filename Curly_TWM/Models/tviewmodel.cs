using System.Collections.Generic;
using Curly_TWM.Domain.Entities;

namespace Curly_TWM.Models
{
    //public class tviewmodel
    //{
    //    public Initiatives P { get;set;}
    //    public Teams FileId { get; set; }
    //}


    public class tviewmodel
    {
        public tviewmodel(List<Initiatives> Initiatives, List<Teams> Teams)
        {
            this.Initiatives = Initiatives;
            this.Teams = Teams;
        }
        public List<Initiatives> Initiatives { get; private set; }
        public List<Teams> Teams { get; private set; }
    }
}