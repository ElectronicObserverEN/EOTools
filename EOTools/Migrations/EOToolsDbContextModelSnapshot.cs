﻿// <auto-generated />
using System;
using EOTools.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EOTools.Migrations
{
    [DbContext(typeof(EOToolsDbContext))]
    partial class EOToolsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.HasKey("Id");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeConversionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentLevelAfter")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdEquipmentAfter")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImprovmentModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ImprovmentModelId")
                        .IsUnique();

                    b.ToTable("EquipmentUpgradeConversionModel");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeDataModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("EquipmentUpgrades");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeHelpersDayModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentUpgradeHelpersModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentUpgradeHelpersModelId");

                    b.ToTable("EquipmentUpgradeHelpersDayModel");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeHelpersModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentUpgradeImprovmentModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentUpgradeImprovmentModelId");

                    b.ToTable("EquipmentUpgradeHelpersModel");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeHelpersShipModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentUpgradeHelpersModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShipId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentUpgradeHelpersModelId");

                    b.ToTable("EquipmentUpgradeHelpersShipModel");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Ammo")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Bauxite")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cost0To5Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cost6To9Id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CostMaxId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Fuel")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Steel")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Cost0To5Id");

                    b.HasIndex("Cost6To9Id");

                    b.HasIndex("CostMaxId");

                    b.ToTable("EquipmentUpgradeImprovmentCost");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCostDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DevmatCost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImproveMatCost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SliderDevmatCost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SliderImproveMatCost")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("EquipmentUpgradeImprovmentCostDetail");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCostItemDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentUpgradeImprovmentCostDetailId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentUpgradeImprovmentCostDetailId1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentUpgradeImprovmentCostDetailId");

                    b.HasIndex("EquipmentUpgradeImprovmentCostDetailId1");

                    b.ToTable("EquipmentUpgradeImprovmentCostItemDetail");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CostsId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EquipmentUpgradeDataModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CostsId");

                    b.HasIndex("EquipmentUpgradeDataModelId");

                    b.ToTable("EquipmentUpgradeImprovmentModel");
                });

            modelBuilder.Entity("EOTools.Models.Ships.ShipClassModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApiId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameJapanese")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ShipClass");
                });

            modelBuilder.Entity("EOTools.Models.Ships.ShipModel", b =>
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

                    b.Property<int?>("ShipClassId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Ships");
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

                    b.Property<string>("EndTweetLink")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StartTweetLink")
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

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeConversionModel", b =>
                {
                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentModel", "ImprovmentModel")
                        .WithOne("ConversionData")
                        .HasForeignKey("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeConversionModel", "ImprovmentModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("ImprovmentModel");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeHelpersDayModel", b =>
                {
                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeHelpersModel", null)
                        .WithMany("CanHelpOnDays")
                        .HasForeignKey("EquipmentUpgradeHelpersModelId");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeHelpersModel", b =>
                {
                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentModel", "Improvment")
                        .WithMany("Helpers")
                        .HasForeignKey("EquipmentUpgradeImprovmentModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Improvment");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeHelpersShipModel", b =>
                {
                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeHelpersModel", null)
                        .WithMany("ShipIds")
                        .HasForeignKey("EquipmentUpgradeHelpersModelId");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCost", b =>
                {
                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCostDetail", "Cost0To5")
                        .WithMany()
                        .HasForeignKey("Cost0To5Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCostDetail", "Cost6To9")
                        .WithMany()
                        .HasForeignKey("Cost6To9Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCostDetail", "CostMax")
                        .WithMany()
                        .HasForeignKey("CostMaxId");

                    b.Navigation("Cost0To5");

                    b.Navigation("Cost6To9");

                    b.Navigation("CostMax");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCostItemDetail", b =>
                {
                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCostDetail", null)
                        .WithMany("ConsumableDetail")
                        .HasForeignKey("EquipmentUpgradeImprovmentCostDetailId");

                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCostDetail", null)
                        .WithMany("EquipmentDetail")
                        .HasForeignKey("EquipmentUpgradeImprovmentCostDetailId1");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentModel", b =>
                {
                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCost", "Costs")
                        .WithMany()
                        .HasForeignKey("CostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeDataModel", null)
                        .WithMany("Improvement")
                        .HasForeignKey("EquipmentUpgradeDataModelId");

                    b.Navigation("Costs");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeDataModel", b =>
                {
                    b.Navigation("Improvement");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeHelpersModel", b =>
                {
                    b.Navigation("CanHelpOnDays");

                    b.Navigation("ShipIds");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentCostDetail", b =>
                {
                    b.Navigation("ConsumableDetail");

                    b.Navigation("EquipmentDetail");
                });

            modelBuilder.Entity("EOTools.Models.EquipmentUpgrade.EquipmentUpgradeImprovmentModel", b =>
                {
                    b.Navigation("ConversionData");

                    b.Navigation("Helpers");
                });
#pragma warning restore 612, 618
        }
    }
}
