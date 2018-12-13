﻿// <auto-generated />
using System;
using ExpensePrediction.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExpensePrediction.WebAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181213112327_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.ActivationToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<string>("Token");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ActivationTokens");
                });

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.Expense", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("LinkedExpenseId");

                    b.Property<string>("UserId");

                    b.Property<double>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("LinkedExpenseId");

                    b.HasIndex("UserId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.ExpenseCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ExpenseCategories");

                    b.HasData(
                        new
                        {
                            Id = "27a62b11-7765-473d-a1b6-7ecda9915dd5",
                            Name = "Żarcie"
                        },
                        new
                        {
                            Id = "7edeb282-6172-49b1-9a8f-5c3b25b7d0b8",
                            Name = "Alkohol"
                        },
                        new
                        {
                            Id = "9def9750-e74a-4115-bdb7-e81bdccef26b",
                            Name = "Samochód"
                        });
                });

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.Income", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("UserId");

                    b.Property<double>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Incomes");

                    b.HasData(
                        new
                        {
                            Id = "336fe191-4041-43c1-ad99-f23b50f90240",
                            CategoryId = "fb9d333d-5e85-484b-bb35-ef307fd06379",
                            Date = new DateTime(2018, 12, 8, 12, 23, 27, 497, DateTimeKind.Local).AddTicks(3545),
                            Description = "Spadek po cioci",
                            UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                            Value = 2345.6700000000001
                        },
                        new
                        {
                            Id = "a91a0e86-dbde-4d14-bfb2-420a2d832f67",
                            CategoryId = "fb9d333d-5e85-484b-bb35-ef307fd06379",
                            Date = new DateTime(2018, 11, 8, 12, 23, 27, 504, DateTimeKind.Local).AddTicks(2297),
                            Description = "Spadek po cioci",
                            UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                            Value = 2345.6700000000001
                        },
                        new
                        {
                            Id = "165f9d5b-f38c-4b7d-8017-d6b686064311",
                            CategoryId = "fb9d333d-5e85-484b-bb35-ef307fd06379",
                            Date = new DateTime(2018, 10, 8, 12, 23, 27, 504, DateTimeKind.Local).AddTicks(2325),
                            Description = "Spadek po cioci",
                            UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                            Value = 2345.6700000000001
                        },
                        new
                        {
                            Id = "aaad10d5-c189-4e4d-9f27-1947ff724404",
                            CategoryId = "fb9d333d-5e85-484b-bb35-ef307fd06379",
                            Date = new DateTime(2018, 9, 8, 12, 23, 27, 504, DateTimeKind.Local).AddTicks(2330),
                            Description = "Spadek po cioci",
                            UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                            Value = 2345.6700000000001
                        },
                        new
                        {
                            Id = "3aa151cd-0418-4624-8fc3-409fb5b45c3c",
                            CategoryId = "fb9d333d-5e85-484b-bb35-ef307fd06379",
                            Date = new DateTime(2018, 8, 8, 12, 23, 27, 504, DateTimeKind.Local).AddTicks(2334),
                            Description = "Spadek po cioci",
                            UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                            Value = 2345.6700000000001
                        },
                        new
                        {
                            Id = "affc972b-9cbe-448a-a499-e27f38b48ee8",
                            CategoryId = "fb9d333d-5e85-484b-bb35-ef307fd06379",
                            Date = new DateTime(2018, 7, 8, 12, 23, 27, 504, DateTimeKind.Local).AddTicks(2339),
                            Description = "Spadek po cioci",
                            UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                            Value = 2345.6700000000001
                        });
                });

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.IncomeCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("IncomeCategories");

                    b.HasData(
                        new
                        {
                            Id = "fb9d333d-5e85-484b-bb35-ef307fd06379",
                            Name = "Przychód regularny"
                        },
                        new
                        {
                            Id = "10c0cb83-1cb2-4c24-84ca-1757225bb97d",
                            Name = "Przychód jednorazowy"
                        });
                });

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "2fdbec88-4aa9-430c-8359-8b27756cf1ca",
                            ConcurrencyStamp = "b7fabdd2-5d83-4b6a-8ba2-848a5e0ad5a6",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "307d5d07-87cf-49a0-9cd4-b925b5380963",
                            ConcurrencyStamp = "72b27c14-1474-4155-bf1a-bb181fd1b662",
                            Name = "user",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = "830f0d08-6a56-4a57-83c9-329c094f184b",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "2d042db9-0a07-468f-86ed-1774dab5931a",
                            Email = "user@user.com",
                            EmailConfirmed = false,
                            FirstName = "User",
                            LastName = "User",
                            LockoutEnabled = false,
                            NormalizedEmail = "USER@USER.COM",
                            NormalizedUserName = "USER",
                            PasswordHash = "AQAAAAEAACcQAAAAEODZFtx31yVlQlAo6GcTs2dIyi/Dcch0/uqv27PvT/xXzy5+JAZEMVS5SvM13yrMdQ==",
                            PhoneNumber = "123456780",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            UserName = "user"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                            RoleId = "2fdbec88-4aa9-430c-8359-8b27756cf1ca"
                        },
                        new
                        {
                            UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                            RoleId = "307d5d07-87cf-49a0-9cd4-b925b5380963"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.ActivationToken", b =>
                {
                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.Expense", b =>
                {
                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.ExpenseCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.Expense", "LinkedExpense")
                        .WithMany()
                        .HasForeignKey("LinkedExpenseId");

                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ExpensePrediction.DataAccessLayer.Entities.Income", b =>
                {
                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.IncomeCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ExpensePrediction.DataAccessLayer.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}