using DotNetCore_AutoLotDAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNetCore_AutoLotDAL.Models
{
    public partial class Order : EntityBase
    {
        public int CunstomerId { get; set; }

        public int CarId { get; set; }

        [ForeignKey(nameof(CunstomerId))]
        public Customer Customer { get; set; }

        [ForeignKey(nameof(CarId))]
        public Inventory Car { get; set; }
    }
}
