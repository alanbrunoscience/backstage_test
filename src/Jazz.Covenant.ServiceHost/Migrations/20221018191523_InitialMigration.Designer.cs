﻿// <auto-generated />
using System;
using Jazz.Core.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    [DbContext(typeof(CovenantDbContext))]
    [Migration("20221018191523_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Jazz.Covenant.Domain.Covenant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EndoserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IdentifierInEndoser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Organization")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EndoserId");

                    b.ToTable("Covenants");
                });

            modelBuilder.Entity("Jazz.Covenant.Domain.Endoser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EndoserIdentifier")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Endoser");
                });

            modelBuilder.Entity("Jazz.Covenant.Domain.Modality", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Modality");
                });

            modelBuilder.Entity("Jazz.Covenant.Domain.ModalityCovenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CovenantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ModalityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CovenantId");

                    b.HasIndex("ModalityId");

                    b.ToTable("ModalityCovenant");
                });

            modelBuilder.Entity("Jazz.Covenant.Domain.Covenant", b =>
                {
                    b.HasOne("Jazz.Covenant.Domain.Endoser", "Endoser")
                        .WithMany("Covenants")
                        .HasForeignKey("EndoserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Endoser");
                });

            modelBuilder.Entity("Jazz.Covenant.Domain.ModalityCovenant", b =>
                {
                    b.HasOne("Jazz.Covenant.Domain.Covenant", "Covenant")
                        .WithMany("ModalitiesCovenants")
                        .HasForeignKey("CovenantId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Jazz.Covenant.Domain.Modality", "Modality")
                        .WithMany("ModalitiesCovenants")
                        .HasForeignKey("ModalityId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Covenant");

                    b.Navigation("Modality");
                });

            modelBuilder.Entity("Jazz.Covenant.Domain.Covenant", b =>
                {
                    b.Navigation("ModalitiesCovenants");
                });

            modelBuilder.Entity("Jazz.Covenant.Domain.Endoser", b =>
                {
                    b.Navigation("Covenants");
                });

            modelBuilder.Entity("Jazz.Covenant.Domain.Modality", b =>
                {
                    b.Navigation("ModalitiesCovenants");
                });
#pragma warning restore 612, 618
        }
    }
}
