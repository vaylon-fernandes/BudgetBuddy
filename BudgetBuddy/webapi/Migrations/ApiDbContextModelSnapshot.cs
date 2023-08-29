﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webapi.Data;

#nullable disable

namespace webapi.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    partial class ApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("webapi.Entities.Budget", b =>
                {
                    b.Property<int>("BudgetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("budget_id");

                    b.Property<decimal>("LimitAmount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("limit_amount");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("BudgetId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("budget");
                });

            modelBuilder.Entity("webapi.Entities.Expenses", b =>
                {
                    b.Property<int>("ExpenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("expense_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("amount");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("description");

                    b.Property<DateTime>("EnteredDate")
                        .HasColumnType("datetime")
                        .HasColumnName("date_entered");

                    b.Property<string>("ExpenseCategory")
                        .IsRequired()
                        .HasColumnType("varchar(25)")
                        .HasColumnName("expense_category");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("ExpenseId");

                    b.HasIndex("UserId");

                    b.ToTable("expenses");
                });

            modelBuilder.Entity("webapi.Entities.FinancialGoal", b =>
                {
                    b.Property<int>("GoalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("goal_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("amount");

                    b.Property<decimal>("CurrentlySaved")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("currently_saved");

                    b.Property<string>("GoalName")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("goal_name");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.Property<int?>("UsersUserId")
                        .HasColumnType("int");

                    b.HasKey("GoalId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("financial_goals");
                });

            modelBuilder.Entity("webapi.Entities.Savings", b =>
                {
                    b.Property<int>("SavingsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("savings_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("amount");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("SavingsId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("savings");
                });

            modelBuilder.Entity("webapi.Entities.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("salt");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("user_name");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("role");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("webapi.Entities.Budget", b =>
                {
                    b.HasOne("webapi.Entities.Users", null)
                        .WithOne("Budget")
                        .HasForeignKey("webapi.Entities.Budget", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("webapi.Entities.Expenses", b =>
                {
                    b.HasOne("webapi.Entities.Users", "User")
                        .WithMany("Expenses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("webapi.Entities.FinancialGoal", b =>
                {
                    b.HasOne("webapi.Entities.Users", null)
                        .WithMany("FinancialGoals")
                        .HasForeignKey("UsersUserId");
                });

            modelBuilder.Entity("webapi.Entities.Savings", b =>
                {
                    b.HasOne("webapi.Entities.Users", "User")
                        .WithOne("Savings")
                        .HasForeignKey("webapi.Entities.Savings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("webapi.Entities.Users", b =>
                {
                    b.Navigation("Budget");

                    b.Navigation("Expenses");

                    b.Navigation("FinancialGoals");

                    b.Navigation("Savings")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
