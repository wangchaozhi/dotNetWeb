using Microsoft.EntityFrameworkCore;
using NetWebApi.Models;

namespace NetWebApi.Config;
[RegisterService]
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 配置使用 SQLite 数据库
        optionsBuilder.UseSqlite("Data Source=localdatabase.db");
    }
}
