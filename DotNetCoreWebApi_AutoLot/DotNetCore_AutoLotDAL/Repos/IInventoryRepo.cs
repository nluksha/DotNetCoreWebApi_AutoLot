using DotNetCore_AutoLotDAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore_AutoLotDAL.Repos
{
    public interface IInventoryRepo: IRepo<Inventory>
    {
        List<Inventory> Search(string searchString);

        List<Inventory> GetPinkCars();

        List<Inventory> GetRelatedData();
    }
}
