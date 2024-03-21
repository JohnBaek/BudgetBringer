﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models.DataModels;

#nullable disable

namespace Models.DataModels.MIgrations
{
    [DbContext(typeof(AnalysisDbContext))]
    partial class AnalysisDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Models.DataModels.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasComment("역할의 고유 식별자");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext")
                        .HasComment("병행 처리를 위한 스탬프");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("역할 이름");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(255)")
                        .HasComment("역할 이름의 정규화된 형태");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "NormalizedName" }, "RoleNameIndex")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Models.DataModels.RoleClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasComment("역할 권한의 고유 식별자");

                    b.Property<string>("ClaimType")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("권한 유형");

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("권한 값");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)")
                        .HasComment("역할의 고유 식별자");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "RoleId" }, "IX_RoleClaims_RoleId");

                    b.ToTable("RoleClaims", t =>
                        {
                            t.HasComment("역할 별 권한 정보");
                        });
                });

            modelBuilder.Entity("Models.DataModels.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasComment("아이디 값");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int(11)")
                        .HasComment("로그인 실패 횟수");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext")
                        .HasComment("동시성 제어 스탬프");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)")
                        .HasComment("유저 타입 구분자");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("이메일 주소");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)")
                        .HasComment("이메일 인증 여부");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)")
                        .HasComment("계정 잠금 가능 여부");

                    b.Property<DateTime?>("LockoutEnd")
                        .HasMaxLength(6)
                        .HasColumnType("datetime(6)")
                        .HasComment("잠금 해제 시간");

                    b.Property<string>("LoginId")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("로그인 ID");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(255)")
                        .HasComment("대문자로 변환된 이메일 주소");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(255)")
                        .HasComment("대문자로 변환된 사용자 이름");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext")
                        .HasComment("비밀번호 해시");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext")
                        .HasComment("전화번호");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)")
                        .HasComment("전화번호 인증 여부");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext")
                        .HasComment("보안 스탬프");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)")
                        .HasComment("2단계 인증 활성화 여부");

                    b.Property<string>("UserName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("사용자 이름");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex("LoginId")
                        .IsUnique()
                        .HasDatabaseName("IX_Users_LoginId");

                    b.HasIndex(new[] { "NormalizedEmail" }, "EmailIndex");

                    b.HasIndex(new[] { "LoginId" }, "LoginIdIndex");

                    b.HasIndex(new[] { "NormalizedUserName" }, "UserNameIndex")
                        .IsUnique();

                    b.ToTable("Users", null, t =>
                        {
                            t.HasComment("사용자 정보");
                        });
                });

            modelBuilder.Entity("Models.DataModels.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)")
                        .HasComment("로그인 제공자");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)")
                        .HasComment("제공자 키");

                    b.Property<string>("ProviderDisplayName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("제공자의 표시 이름");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)")
                        .HasComment("사용자의 고유 식별자");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "UserId" }, "IX_UserLogins_UserId");

                    b.ToTable("UserLogins", t =>
                        {
                            t.HasComment("사용자의 로그인 관련 정보");
                        });
                });

            modelBuilder.Entity("Models.DataModels.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("char(36)")
                        .HasComment("사용자 아이디");

                    b.Property<Guid>("RoleId")
                        .HasMaxLength(36)
                        .HasColumnType("char(36)")
                        .HasComment("역할 아이디");

                    b.HasKey("UserId", "RoleId")
                        .HasName("PK_UserRoles");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Models.DataModels.UserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)")
                        .HasComment("사용자의 고유 식별자");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)")
                        .HasComment("로그인 제공자");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)")
                        .HasComment("토큰 이름");

                    b.Property<string>("Value")
                        .HasColumnType("longtext")
                        .HasComment("토큰 값");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                    b.ToTable("UserTokens", t =>
                        {
                            t.HasComment("사용자의 로그인 토큰 정보");
                        });
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("RolesId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("char(36)");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("Models.DataModels.RoleClaim", b =>
                {
                    b.HasOne("Models.DataModels.Role", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Models.DataModels.UserLogin", b =>
                {
                    b.HasOne("Models.DataModels.User", "User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.DataModels.UserToken", b =>
                {
                    b.HasOne("Models.DataModels.User", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("Models.DataModels.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.DataModels.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.DataModels.Role", b =>
                {
                    b.Navigation("RoleClaims");
                });

            modelBuilder.Entity("Models.DataModels.User", b =>
                {
                    b.Navigation("UserLogins");

                    b.Navigation("UserTokens");
                });
#pragma warning restore 612, 618
        }
    }
}