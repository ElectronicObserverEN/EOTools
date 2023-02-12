﻿// <auto-generated />
using System;
using EOTools.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EOTools.Migrations
{
    [DbContext(typeof(EOToolsDbContext))]
    [Migration("20230212083038_EquipmentDataBase")]
    partial class EquipmentDataBase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("EOTools.Models.EquipmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameEN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameJP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UpgradeData")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("EOTools.Translation.QuestManager.Events.EventModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EndOnUpdateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("StartOnUpdateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EOTools.Translation.QuestManager.Quests.QuestModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AddedOnUpdateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DescEN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DescJP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameEN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameJP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("RemovedOnUpdateId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SeasonId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tracker")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Code", "ApiId")
                        .IsUnique();

                    b.ToTable("Quests");
                });

            modelBuilder.Entity("EOTools.Translation.QuestManager.Seasons.SeasonModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AddedOnUpdateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("RemovedOnUpdateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("EOTools.Translation.QuestManager.Updates.UpdateModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan?>("UpdateEndTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("UpdateStartTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("WasLiveUpdate")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Updates");
                });
#pragma warning restore 612, 618
        }
    }
}
