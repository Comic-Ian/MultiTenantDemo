using Microsoft.EntityFrameworkCore;
using MultiTenantDemo2.Infrastructure;

namespace MultiTenantDemo2.DAL
{
    //实现 ITenantDbContext 接口，并且在构造函数上添加TenantInfo的注入
    public class StoreDbContext : DbContext, ITenantDbContext
    {
        //这里使用DbSet来获取对应的对象，因为表对象还是使用只读Property会好点。
        public DbSet<Product> Products => this.Set<Product>();

        public TenantInfo TenantInfo => _tenantInfo;

        private readonly TenantInfo _tenantInfo;

        public StoreDbContext(DbContextOptions options,TenantInfo tenantInfo):base(options)
        {
            this._tenantInfo = tenantInfo;
        }
        //主要规定EF core 的表实体（本文是Product）怎么跟数据库匹配的，简单来说就是配置。
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Tb_Product"+(this._tenantInfo.Name=="1"?"": this._tenantInfo.Name));
        }
    }
}
