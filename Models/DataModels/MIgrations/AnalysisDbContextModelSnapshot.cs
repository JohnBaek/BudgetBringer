﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models.DataModels;

#nullable disable

namespace Models.DataModels.Migrations
{
    [DbContext(typeof(AnalysisDbContext))]
    partial class AnalysisDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Models.DataModels.DbModelBudgetApproved", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<double>("Actual")
                        .HasColumnType("double");

                    b.Property<double>("ApprovalAmount")
                        .HasColumnType("double");

                    b.Property<string>("ApprovalDate")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("ApproveDateValue")
                        .HasColumnType("date");

                    b.Property<string>("BossLineDescription")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)");

                    b.Property<Guid>("BusinessUnitId")
                        .HasColumnType("char(36)");

                    b.Property<string>("BusinessUnitName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("CostCenterId")
                        .HasColumnType("char(36)");

                    b.Property<string>("CostCenterName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("CountryBusinessManagerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("CountryBusinessManagerName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Day")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<string>("Description")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)");

                    b.Property<bool>("IsAbove500K")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsApprovalDateValid")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("ModDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ModId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ModName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Month")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<string>("OcProjectName")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)");

                    b.Property<int>("PoNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("RegId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RegName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("SectorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Year")
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessUnitId");

                    b.HasIndex("CostCenterId");

                    b.HasIndex("CountryBusinessManagerId");

                    b.HasIndex("SectorId");

                    b.ToTable("BudgetApproved", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelBudgetPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApprovalDate")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateOnly?>("ApproveDateValue")
                        .HasColumnType("date");

                    b.Property<string>("BossLineDescription")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)");

                    b.Property<double>("BudgetTotal")
                        .HasColumnType("double");

                    b.Property<Guid>("BusinessUnitId")
                        .HasColumnType("char(36)");

                    b.Property<string>("BusinessUnitName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("CostCenterId")
                        .HasColumnType("char(36)");

                    b.Property<string>("CostCenterName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("CountryBusinessManagerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("CountryBusinessManagerName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<string>("Description")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)");

                    b.Property<bool>("IsAbove500K")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsApprovalDateValid")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("ModDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ModId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ModName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<string>("OcProjectName")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("RegId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RegName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("SectorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessUnitId");

                    b.HasIndex("CostCenterId");

                    b.HasIndex("CountryBusinessManagerId");

                    b.HasIndex("SectorId");

                    b.ToTable("BudgetPlans", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelBusinessUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ModDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ModId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ModName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("RegId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RegName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("BusinessUnits", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelCostCenter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ModDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ModId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ModName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("RegId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RegName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("CostCenters", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelCountryBusinessManager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ModDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ModId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ModName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("RegId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RegName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("CountryBusinessManagers", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelLogAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("ActionType")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Contents")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("RegId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RegName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RegId");

                    b.ToTable("LogActions", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelSector", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ModDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ModId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ModName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("RegId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RegName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Sectors", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("CountryBusinessManagerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastPasswordChangeDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LoginId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("LoginId")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)");

                    b.HasKey("Id", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("Models.DataModels.DbModelBudgetApproved", b =>
                {
                    b.HasOne("Models.DataModels.DbModelBusinessUnit", "DbModelBusinessUnit")
                        .WithMany()
                        .HasForeignKey("BusinessUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DataModels.DbModelCostCenter", "DbModelCostCenter")
                        .WithMany()
                        .HasForeignKey("CostCenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DataModels.DbModelCountryBusinessManager", "DbModelCountryBusinessManager")
                        .WithMany()
                        .HasForeignKey("CountryBusinessManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DataModels.DbModelSector", "DbModelSector")
                        .WithMany()
                        .HasForeignKey("SectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DbModelBusinessUnit");

                    b.Navigation("DbModelCostCenter");

                    b.Navigation("DbModelCountryBusinessManager");

                    b.Navigation("DbModelSector");
                });

            modelBuilder.Entity("Models.DataModels.DbModelBudgetPlan", b =>
                {
                    b.HasOne("Models.DataModels.DbModelBusinessUnit", "DbModelBusinessUnit")
                        .WithMany()
                        .HasForeignKey("BusinessUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DataModels.DbModelCostCenter", "DbModelCostCenter")
                        .WithMany()
                        .HasForeignKey("CostCenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DataModels.DbModelCountryBusinessManager", "DbModelCountryBusinessManager")
                        .WithMany()
                        .HasForeignKey("CountryBusinessManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DataModels.DbModelSector", "Sector")
                        .WithMany()
                        .HasForeignKey("SectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DbModelBusinessUnit");

                    b.Navigation("DbModelCostCenter");

                    b.Navigation("DbModelCountryBusinessManager");

                    b.Navigation("Sector");
                });

            modelBuilder.Entity("Models.DataModels.DbModelLogAction", b =>
                {
                    b.HasOne("Models.DataModels.DbModelUser", "DbModelUser")
                        .WithMany("LogActions")
                        .HasForeignKey("RegId");

                    b.Navigation("DbModelUser");
                });

            modelBuilder.Entity("Models.DataModels.DbModelRoleClaim", b =>
                {
                    b.HasOne("Models.DataModels.DbModelRole", "DbModelRole")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DbModelRole");
                });

            modelBuilder.Entity("Models.DataModels.DbModelUserClaim", b =>
                {
                    b.HasOne("Models.DataModels.DbModelUser", "DbModelUser")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DbModelUser");
                });

            modelBuilder.Entity("Models.DataModels.DbModelUserLogin", b =>
                {
                    b.HasOne("Models.DataModels.DbModelUser", "DbModelUser")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DbModelUser");
                });

            modelBuilder.Entity("Models.DataModels.DbModelUserRole", b =>
                {
                    b.HasOne("Models.DataModels.DbModelRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DataModels.DbModelUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.DataModels.DbModelUserToken", b =>
                {
                    b.HasOne("Models.DataModels.DbModelUser", "DbModelUser")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DbModelUser");
                });

            modelBuilder.Entity("Models.DataModels.DbModelRole", b =>
                {
                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Models.DataModels.DbModelUser", b =>
                {
                    b.Navigation("LogActions");

                    b.Navigation("UserClaims");

                    b.Navigation("UserLogins");

                    b.Navigation("UserRoles");

                    b.Navigation("UserTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
