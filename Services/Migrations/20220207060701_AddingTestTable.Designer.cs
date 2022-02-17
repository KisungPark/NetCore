﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Net.Service.Data;

#nullable disable

namespace Net.Service.Migrations
{
    [DbContext(typeof(CodeFirstDbContext))]
    [Migration("20220207060701_AddingTestTable")]
    partial class AddingTestTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Net.Data.DataModels.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnOrder(0)
                        .HasComment("사용자ID");

                    b.Property<DateTime>("CreatedUtcDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnOrder(5)
                        .HasDefaultValueSql("sysutcdatetime()")
                        .HasComment("생성일");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(4)
                        .HasComment("삭제일");

                    b.Property<DateTime?>("ModifiedUtcDate")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(6)
                        .HasComment("최종 수정일");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(130)
                        .HasColumnType("nvarchar(130)")
                        .HasColumnOrder(3)
                        .HasComment("암호");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("varchar(320)")
                        .HasColumnOrder(2)
                        .HasComment("이메일");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnOrder(1)
                        .HasComment("사용자명");

                    b.HasKey("UserId");

                    b.HasIndex("UserEmail");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Net.Data.DataModels.UserRole", b =>
                {
                    b.Property<string>("RoleId")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnOrder(0)
                        .HasComment("역할ID");

                    b.Property<DateTime>("CreatedUtcDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnOrder(3)
                        .HasDefaultValueSql("sysutcdatetime()")
                        .HasComment("생성일");

                    b.Property<DateTime?>("ModifiedUtcDate")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(4)
                        .HasComment("최종 수정일");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnOrder(1)
                        .HasComment("역할명");

                    b.Property<byte>("RolePriority")
                        .HasColumnType("tinyint")
                        .HasColumnOrder(2)
                        .HasComment("우선순위");

                    b.HasKey("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Net.Data.DataModels.UserRolesByUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnOrder(0)
                        .HasComment("사용자ID");

                    b.Property<string>("RoleId")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnOrder(1)
                        .HasComment("역할ID");

                    b.Property<DateTime>("OwnedUtcDate")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(2)
                        .HasComment("적용일");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRolesByUser");
                });

            modelBuilder.Entity("Net.Data.DataModels.UserRolesByUser", b =>
                {
                    b.HasOne("Net.Data.DataModels.UserRole", "UserRole")
                        .WithMany("UserRolesByUsers")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Net.Data.DataModels.User", "User")
                        .WithMany("UserRolesByUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("Net.Data.DataModels.User", b =>
                {
                    b.Navigation("UserRolesByUsers");
                });

            modelBuilder.Entity("Net.Data.DataModels.UserRole", b =>
                {
                    b.Navigation("UserRolesByUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
