using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using MultiTenantDemo3.Model;

namespace MultiTenantDemo3.Infrastructure
{
    public class TestContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //注入Sql链接字符串
            optionsBuilder.UseSqlServer("Data Source=hejianneng.top,1434;User Id=sa;Password=$Z5as#Ur; Database=LanTest");
        }
        //DBSet类表示一个实体的集合，用来创建、更新、删除、查询操作，DBSet<TEntity>是DBSet的泛型版本
        public DbSet<People> People { get; set; }

    }
}
