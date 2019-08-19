using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Town:BaseEntity
    {       
        public int IdCity { get; set; }
        public string Name { get; set; }
        public virtual City IdCityNavigation { get; set; }
    }
}
