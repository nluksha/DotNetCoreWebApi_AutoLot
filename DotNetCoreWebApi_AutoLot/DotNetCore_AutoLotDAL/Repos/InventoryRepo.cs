using DotNetCore_AutoLotDAL.EF;
using DotNetCore_AutoLotDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.EF;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_AutoLotDAL.Repos
{
    public class InventoryRepo : BaseRepo<Inventory>, IInventoryRepo
    {
        public InventoryRepo()
        {
        }

        public InventoryRepo(AutoLotContext context): base(context)
        {
        }

        public override List<Inventory> GetAll() => GetAll(x => x.PetName, true).ToList();

        public List<Inventory> GetPinkCars() => GetSome(x => x.Color == "Pink");

        public List<Inventory> Search(string searchString) => 
            Context.Cars.Where(x => Functions.Like(x.PetName, $"%{searchString}%")).ToList();

        public List<Inventory> GetRelatedData() => 
            Context.Cars.FromSql("SELECT * FROM Inventory").Include(x => x.Orders).ThenInclude(x => x.Customer).ToList();
    }
}
