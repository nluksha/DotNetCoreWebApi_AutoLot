using DotNetCore_AutoLotDAL.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNetCore_AutoLotDAL.Models
{
    [ModelMetadataType(typeof(InventoryMetaData))]
    public partial class Inventory
    {
        [NotMapped]
        public string MakeColor => $"{Make} + ({Color})";

        public override string ToString()
        {
            return $"{this.PetName ?? "*** No Name ***"} is {this.Color}";
        }
    }
}
