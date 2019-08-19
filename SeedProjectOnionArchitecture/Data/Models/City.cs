using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class City : BaseEntity
    {
        public City()
        {
            Town = new HashSet<Town>();
        }

        public int IdCountry { get; set; }
        public string Name { get; set; }
        public string PlateNo { get; set; }
        public string PhoneCode { get; set; }

        public virtual ICollection<Town> Town { get; set; }
    }
}
