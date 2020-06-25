﻿// <auto-generated />
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MonkeyButler.Data.Database;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MonkeyButler.Migrations
{
    [DbContext(typeof(MonkeyButlerContext))]
    partial class MonkeyButlerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MonkeyButler.Data.Models.Database.Guild.GuildOptions", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("FreeCompanyId")
                        .HasColumnType("text");

                    b.Property<string>("Prefix")
                        .HasColumnType("text");

                    b.Property<string>("Server")
                        .HasColumnType("text");

                    b.Property<List<string>>("SignupEmotes")
                        .HasColumnType("text[]");

                    b.Property<string>("VerifiedRole")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GuildOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
