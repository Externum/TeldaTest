using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeldaTest.Db;
using TeldaTest.Models;

namespace TeldaTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : Controller
    {
        private readonly BaseContext _regionsContext;

        public RegionsController(BaseContext context)
        {
            _regionsContext = context;
        }


        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<ActionResult<IEnumerable<Region>>> Get()
        {
            var t = await _regionsContext.Regions.ToListAsync();
            return t;
        }

        [HttpGet("{id}")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<ActionResult<Region>> Get(Guid id)
        {
            var region = await _regionsContext.Regions.FirstAsync(_ => _.Id.Equals(id));

            if (region == null)
                return NotFound();

            return region;
        }

        [HttpPost]
        public async Task<ActionResult<Region>> Post(Region region)
        {
            if (region == null)
            {
                return BadRequest("Не указан регион!");
            }

            if (string.IsNullOrEmpty(region.Name))
            {
                return BadRequest("Не указано наименование региона!");
            }

            var regions = _regionsContext.Regions.ToList();

            if (regions.Any(_ => _.Id.Equals(region.Id)))
            {
                return BadRequest("Регион с таким Id уже существует!");
            }

            if (regions.Any(_ => _.Name.Equals(region.Name)))
            {
                return BadRequest("Регион с таким именем уже существует!");
            }

            _regionsContext.Regions.Add(region);
            await _regionsContext.SaveChangesAsync();

            return Ok(region);
        }

        [HttpPut]
        public async Task<ActionResult<Region>> Put(Region region)
        {
            if (region == null)
            {
                return BadRequest("Не указан регион!");
            }

            if (string.IsNullOrEmpty(region.Name))
            {
                return BadRequest("Не указано наименование региона!");
            }

            var regions = await _regionsContext.Regions.ToListAsync();

            var regionInBase = regions.Find(_ => _.Id.Equals(region.Id));

            if (regionInBase == null)
            {
                return NotFound("Регион не найден!");
            }

            if (!region.Name.Equals(regionInBase.Name))
            {
                if (regions.Any(_ => _.Name.Equals(region.Name)))
                    return BadRequest("Регион с таким именем уже существует!");
            }

            _regionsContext.Entry(regionInBase).State = EntityState.Detached;

            _regionsContext.Update(region);

            await _regionsContext.SaveChangesAsync();

            return Ok(region);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Region>> Delete(Guid id)
        {
            var regions = _regionsContext.Regions;
            var deleteRegion = regions.First(_ => _.Id.Equals(id));

            if (deleteRegion == null)
            {
                return NotFound("Регион не найден!");
            }

            regions.Remove(deleteRegion);
            await _regionsContext.SaveChangesAsync();

            return Ok(deleteRegion);
        }
    }
}
