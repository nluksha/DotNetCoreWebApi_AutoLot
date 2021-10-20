using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetCore_AutoLotDAL.EF;
using DotNetCore_AutoLotDAL.Repos;
using DotNetCore_AutoLotDAL.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace DotNetCoreWebApi_AutoLot.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly Mapper mapper;
        private readonly IInventoryRepo repo;

        public InventoryController(IInventoryRepo repo)
        {
            this.repo = repo;

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Inventory, Inventory>().ForMember(x => x.Orders, opt => opt.Ignore())
            );

            mapper = new Mapper(config);
        }

        // GET: api/Inventory
        [HttpGet]
        public IEnumerable<Inventory> GetCars()
        {
            var inventories = repo.GetAll();

            return mapper.Map<List<Inventory>, List<Inventory>>(inventories);
        }

        // GET: api/Inventory/5
        [HttpGet("{id}", Name = "DispayRoute")]
        public async Task<IActionResult> GetInventory([FromRoute] int id)
        {
            var inventory = repo.GetOne(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Inventory, Inventory>(inventory));
        }

        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory([FromRoute] int id, [FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inventory.Id)
            {
                return BadRequest();
            }

            repo.Update(inventory);

            return NoContent();
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<IActionResult> PostInventory([FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repo.Add(inventory);

            return CreatedAtRoute("DispayRoute", new { id = inventory.Id }, inventory);
        }

        // DELETE: api/Inventory/5
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> DeleteInventory([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
            {
                timestamp = $"\"{timestamp}\"";
            }

            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);

            repo.Delete(id, ts);

            return Ok();
        }
    }
}
