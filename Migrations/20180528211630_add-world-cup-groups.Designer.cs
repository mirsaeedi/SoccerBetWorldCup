﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoccerBet.Data;

namespace SoccerBet.Migrations
{
    [DbContext(typeof(SoccerBetDbContext))]
    [Migration("20180528211630_add-world-cup-groups")]
    partial class addworldcupgroups
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<long>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SoccerBet.Data.Models.BetGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("BetGroups");
                });

            modelBuilder.Entity("SoccerBet.Data.Models.Match", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime");

                    b.Property<int>("MatchType");

                    b.Property<long?>("StadiumId");

                    b.Property<long>("StaudiumId");

                    b.HasKey("Id");

                    b.HasIndex("StadiumId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("SoccerBet.Data.Models.Prediction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("GameId");

                    b.Property<long?>("MatchId");

                    b.Property<long>("UserBetGroupId");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("UserBetGroupId");

                    b.ToTable("Predictions");
                });

            modelBuilder.Entity("SoccerBet.Data.Models.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                });

            modelBuilder.Entity("SoccerBet.Data.Models.Stadium", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Stadiums");
                });

            modelBuilder.Entity("SoccerBet.Data.Models.Team", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FifaCode");

                    b.Property<string>("FlagUrl");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("SoccerBet.Data.Models.TeamWorldCupGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("TeamId");

                    b.Property<long>("WorldCupGroupId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("WorldCupGroupId");

                    b.ToTable("TeamWorldCupGroups");
                });

            modelBuilder.Entity("SoccerBet.Data.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

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
                });

            modelBuilder.Entity("SoccerBet.Data.Models.UserBetGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BetGroupId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BetGroupId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBetGroups");
                });

            modelBuilder.Entity("SoccerBet.Data.Models.WorldCupGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("WorldCupGroups");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("SoccerBet.Data.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("SoccerBet.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("SoccerBet.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.HasOne("SoccerBet.Data.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SoccerBet.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.HasOne("SoccerBet.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoccerBet.Data.Models.Match", b =>
                {
                    b.HasOne("SoccerBet.Data.Models.Stadium", "Stadium")
                        .WithMany()
                        .HasForeignKey("StadiumId");

                    b.OwnsOne("SoccerBet.Data.Models.TeamScore", "AwayTeamScore", b1 =>
                        {
                            b1.Property<long>("MatchId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<short?>("MatchResult");

                            b1.Property<short?>("PenaltyResult");

                            b1.Property<long>("TeamId");

                            b1.HasIndex("TeamId");

                            b1.ToTable("Matches");

                            b1.HasOne("SoccerBet.Data.Models.Match")
                                .WithOne("AwayTeamScore")
                                .HasForeignKey("SoccerBet.Data.Models.TeamScore", "MatchId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.HasOne("SoccerBet.Data.Models.Team", "Team")
                                .WithMany()
                                .HasForeignKey("TeamId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("SoccerBet.Data.Models.TeamScore", "HomeTeamScore", b1 =>
                        {
                            b1.Property<long>("MatchId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<short?>("MatchResult");

                            b1.Property<short?>("PenaltyResult");

                            b1.Property<long>("TeamId");

                            b1.HasIndex("TeamId");

                            b1.ToTable("Matches");

                            b1.HasOne("SoccerBet.Data.Models.Match")
                                .WithOne("HomeTeamScore")
                                .HasForeignKey("SoccerBet.Data.Models.TeamScore", "MatchId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.HasOne("SoccerBet.Data.Models.Team", "Team")
                                .WithMany()
                                .HasForeignKey("TeamId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("SoccerBet.Data.Models.Prediction", b =>
                {
                    b.HasOne("SoccerBet.Data.Models.Match", "Match")
                        .WithMany()
                        .HasForeignKey("MatchId");

                    b.HasOne("SoccerBet.Data.Models.UserBetGroup", "UserBetGroup")
                        .WithMany()
                        .HasForeignKey("UserBetGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("SoccerBet.Data.Models.TeamScore", "AwayTeamScore", b1 =>
                        {
                            b1.Property<long>("PredictionId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<short?>("MatchResult");

                            b1.Property<short?>("PenaltyResult");

                            b1.Property<long>("TeamId");

                            b1.HasIndex("TeamId");

                            b1.ToTable("Predictions");

                            b1.HasOne("SoccerBet.Data.Models.Prediction")
                                .WithOne("AwayTeamScore")
                                .HasForeignKey("SoccerBet.Data.Models.TeamScore", "PredictionId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.HasOne("SoccerBet.Data.Models.Team", "Team")
                                .WithMany()
                                .HasForeignKey("TeamId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("SoccerBet.Data.Models.TeamScore", "HomeTeamScore", b1 =>
                        {
                            b1.Property<long>("PredictionId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<short?>("MatchResult");

                            b1.Property<short?>("PenaltyResult");

                            b1.Property<long>("TeamId");

                            b1.HasIndex("TeamId");

                            b1.ToTable("Predictions");

                            b1.HasOne("SoccerBet.Data.Models.Prediction")
                                .WithOne("HomeTeamScore")
                                .HasForeignKey("SoccerBet.Data.Models.TeamScore", "PredictionId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.HasOne("SoccerBet.Data.Models.Team", "Team")
                                .WithMany()
                                .HasForeignKey("TeamId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("SoccerBet.Data.Models.TeamWorldCupGroup", b =>
                {
                    b.HasOne("SoccerBet.Data.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SoccerBet.Data.Models.WorldCupGroup", "WorldCupGroup")
                        .WithMany()
                        .HasForeignKey("WorldCupGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoccerBet.Data.Models.UserBetGroup", b =>
                {
                    b.HasOne("SoccerBet.Data.Models.BetGroup", "BetGroup")
                        .WithMany()
                        .HasForeignKey("BetGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SoccerBet.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}