using Microsoft.EntityFrameworkCore;
using Net.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Service.Data
{
    public class DBFirstDbContext : DbContext
    {
        public DBFirstDbContext(DbContextOptions<DBFirstDbContext> options) : base(options)
        {
        }

        //DB 테이블 리스트 지정
        public DbSet<User>? Users { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }
        public DbSet<UserRolesByUser>? UserRolesByUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //DB테이블이름 변경 및 매핑
            modelBuilder.Entity<User>().ToTable(name: "User");
            modelBuilder.Entity<UserRole>().ToTable(name: "UserRole");
            modelBuilder.Entity<UserRolesByUser>().ToTable(name: "UserRolesByUser");

            //복합키 지정
            modelBuilder.Entity<UserRolesByUser>().HasKey(c => new { c.UserId, c.RoleId });
        }
    }
}
