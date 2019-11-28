﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Turniejowo.API.Models;

namespace Turniejowo.API.Migrations
{
    [DbContext(typeof(TurniejowoDbContext))]
    partial class TurniejowoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Turniejowo.API.Models.Discipline", b =>
                {
                    b.Property<int>("DisciplineId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("DisciplineId");

                    b.ToTable("Disciplines");
                });

            modelBuilder.Entity("Turniejowo.API.Models.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BracketIndex");

                    b.Property<int?>("GuestTeamId")
                        .IsRequired();

                    b.Property<int>("GuestTeamPoints");

                    b.Property<int?>("HomeTeamId")
                        .IsRequired();

                    b.Property<int>("HomeTeamPoints");

                    b.Property<bool>("IsFinished");

                    b.Property<DateTime>("MatchDateTime");

                    b.HasKey("MatchId");

                    b.HasIndex("GuestTeamId");

                    b.HasIndex("HomeTeamId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Turniejowo.API.Models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FName")
                        .IsRequired();

                    b.Property<string>("LName")
                        .IsRequired();

                    b.Property<int>("TeamId");

                    b.HasKey("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Turniejowo.API.Models.Points", b =>
                {
                    b.Property<int>("PointsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MatchId");

                    b.Property<int>("PlayerId");

                    b.Property<int>("PointsQty");

                    b.Property<int>("TournamentId");

                    b.HasKey("PointsId");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TournamentId");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("Turniejowo.API.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Draws");

                    b.Property<int>("Loses");

                    b.Property<int>("Matches");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Points");

                    b.Property<int>("TournamentId");

                    b.Property<int>("Wins");

                    b.HasKey("TeamId");

                    b.HasIndex("TournamentId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Turniejowo.API.Models.Tournament", b =>
                {
                    b.Property<int>("TournamentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AmountOfTeams");

                    b.Property<int>("CreatorId");

                    b.Property<DateTime>("Date");

                    b.Property<int>("DisciplineId");

                    b.Property<int>("EntryFee");

                    b.Property<bool>("IsBracket");

                    b.Property<string>("Localization")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("TournamentId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("DisciplineId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("Turniejowo.API.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("Token");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Turniejowo.API.Models.Match", b =>
                {
                    b.HasOne("Turniejowo.API.Models.Team", "GuestTeam")
                        .WithMany("GuestMatches")
                        .HasForeignKey("GuestTeamId")
                        .HasConstraintName("FK_Match_HomeTeam")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Turniejowo.API.Models.Team", "HomeTeam")
                        .WithMany("HomeMatches")
                        .HasForeignKey("HomeTeamId")
                        .HasConstraintName("FK_Match_GuestTeam")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Turniejowo.API.Models.Player", b =>
                {
                    b.HasOne("Turniejowo.API.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Turniejowo.API.Models.Points", b =>
                {
                    b.HasOne("Turniejowo.API.Models.Match", "Match")
                        .WithMany("MatchPoints")
                        .HasForeignKey("MatchId")
                        .HasConstraintName("FK_Points_Match")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Turniejowo.API.Models.Player", "Player")
                        .WithMany("PlayerPoints")
                        .HasForeignKey("PlayerId")
                        .HasConstraintName("FK_Points_Player")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Turniejowo.API.Models.Tournament", "Tournament")
                        .WithMany("TournamentPoints")
                        .HasForeignKey("TournamentId")
                        .HasConstraintName("FK_Points_Tournament")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Turniejowo.API.Models.Team", b =>
                {
                    b.HasOne("Turniejowo.API.Models.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Turniejowo.API.Models.Tournament", b =>
                {
                    b.HasOne("Turniejowo.API.Models.User", "Creator")
                        .WithMany("Tournaments")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Turniejowo.API.Models.Discipline", "Discipline")
                        .WithMany()
                        .HasForeignKey("DisciplineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
