using DotNetCore_AutoLotDAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNetCore_AutoLotDAL.Models
{
    [Table("Inventory")]
    public partial class Inventory : EntityBase
    {
        [Required, StringLength(50)]
        public string Make { get; set; }

        [Required, StringLength(50)]
        public string Color { get; set; }

        [Required, StringLength(50)]
        public string PetName { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
