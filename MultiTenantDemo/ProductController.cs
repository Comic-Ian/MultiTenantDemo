using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantDemo.DAL;
using MultiTenantDemo.Infrastructure;
using static MultiTenantDemo.Infrastructure.MultipleTenancyExtension;

namespace MultiTenantDemo
{
    [ApiController]
    [Route("api/Products")]
    public class ProductController : ControllerBase
    {
        private readonly StoreDbContext _storeDbContext;

        public ProductController(StoreDbContext storeDbContext)
        {
            this._storeDbContext = storeDbContext;
            this._storeDbContext.Database.EnsureCreated();
        }

        [HttpPost("")]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            var rct = await this._storeDbContext.Products.AddAsync(product);

            await this._storeDbContext.SaveChangesAsync();

            return rct?.Entity;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get([FromRoute] int id)
        {

            var rct = await this._storeDbContext.Products.FindAsync(id);

            return rct;

        }

        [HttpGet("")]
        public async Task<ActionResult<List<Product>>> Search()
        {
            var rct = await this._storeDbContext.Products.ToListAsync();
            return rct;
        }
    }
}
