using Microsoft.EntityFrameworkCore;
using Net.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Service.Data
{
    //Fluent API

    public class CodeFirstDbContext : DbContext
    {
        //생성자 상속(base 키워드로 받아옴)
        public CodeFirstDbContext(DbContextOptions<CodeFirstDbContext> options) : base(options)
        {

        }

        //DB 테이블 리스트 지정
        public DbSet<User>? Users { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }
        public DbSet<UserRolesByUser>? UserRolesByUsers { get; set; }

        //매서드 상속(부모 클래스에서 virtual로 지정되어 있어야 override 가능)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //DB테이블이름 변경 및 매핑
            modelBuilder.Entity<User>().ToTable(name: "User");
            modelBuilder.Entity<UserRole>().ToTable(name: "UserRole");
            modelBuilder.Entity<UserRolesByUser>().ToTable(name: "UserRolesByUser");

            //복합키 지정
            modelBuilder.Entity<UserRolesByUser>().HasKey(c => new { c.UserId, c.RoleId });

            //컬럼 기본값 지정(정적인 값은 HasDefaultValue, 동적인 값은 HasDefaultValueSql)
            modelBuilder.Entity<User>(e => e.Property(c => c.CreatedUtcDate).HasDefaultValueSql("sysutcdatetime()"));
            modelBuilder.Entity<UserRole>(e => e.Property(c => c.CreatedUtcDate).HasDefaultValueSql("sysutcdatetime()"));

            //이메일이 중복이 안되게 Unique 인덱스 지정
            modelBuilder.Entity<User>().HasIndex(c => new { c.UserEmail }).IsUnique(unique:true);
        }
    }
}
