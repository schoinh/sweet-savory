using System.Collections.Generic;

namespace SweetSavory.Models
{
    public class Flavor
    {
        public int FlavorId { get; set; }
        public string Name { get; set; }

        public ICollection<FlavorTreat> Treats { get; }
        public virtual ApplicationUser User { get; set; }

        public Flavor()
        {
            this.Treats = new HashSet<FlavorTreat>();
        }
    }
}