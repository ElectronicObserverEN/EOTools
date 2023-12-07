using EOTools.Models;
using EOTools.Models.EquipmentUpgrade;
using EOTools.Models.Ships;
using EOTools.Translation.QuestManager.Events;
using EOTools.Translation.QuestManager.Quests;
using EOTools.Translation.QuestManager.Seasons;
using EOTools.Translation.QuestManager.Updates;
using Microsoft.EntityFrameworkCore;
using System;
using EOTools.Tools;

namespace EOTools.DataBase
{
    // dotnet ef migrations add <name> --context EOToolsDbContext
    public class EOToolsDbContext : DbContext
    {
        public DbSet<UpdateModel> Updates { get; set; }
        public DbSet<QuestModel> Quests { get; set; }
        public DbSet<SeasonModel> Seasons { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<EquipmentModel> Equipments { get; set; }
        public DbSet<ShipModel> Ships { get; set; }
        public DbSet<ShipClassModel> ShipClass { get; set; }
        public DbSet<EquipmentUpgradeDataModel> EquipmentUpgrades { get; set; }

        public string DbPath { get; }

        public EOToolsDbContext()
        {
            DbPath = DatabaseSyncService.DataBaseLocalPath;
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(DbPath));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<EquipmentUpgradeImprovmentModel>()
                .HasOne(e => e.ConversionData)
                .WithOne(e => e.ImprovmentModel)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasForeignKey(nameof(EquipmentUpgradeConversionModel), nameof(EquipmentUpgradeConversionModel.ImprovmentModelId));

            modelBuilder
                .Entity<EquipmentUpgradeImprovmentModel>()
                .HasMany(e => e.Helpers)
                .WithOne(e => e.Improvment)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasForeignKey(nameof(EquipmentUpgradeHelpersModel.EquipmentUpgradeImprovmentModelId));

            /*modelBuilder
                .Entity<EquipmentUpgradeImprovmentCost>()
                .HasOne(e => e.Cost0To5)
                .WithOne(e => e.EquipmentUpgradeImprovmentCost)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasForeignKey<EquipmentUpgradeImprovmentCostDetail0To5>(e => e.EquipmentUpgradeImprovmentCostId);

            modelBuilder
                .Entity<EquipmentUpgradeImprovmentCost>()
                .HasOne(e => e.Cost6To9)
                .WithOne(e => e.EquipmentUpgradeImprovmentCost)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasForeignKey<EquipmentUpgradeImprovmentCostDetail6To9>(e => e.EquipmentUpgradeImprovmentCostId);

            modelBuilder
                .Entity<EquipmentUpgradeImprovmentCost>()
                .HasOne(e => e.CostMax)
                .WithOne(e => e.EquipmentUpgradeImprovmentCost)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasForeignKey<EquipmentUpgradeImprovmentCostDetailMax>(e => e.EquipmentUpgradeImprovmentCostId);*/
        }
        /*
        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            if (entity is EquipmentUpgradeImprovmentCost cost)
            {
                Remove(cost.Cost0To5);
                Remove(cost.Cost6To9);
                if (cost.CostMax is not null) Remove(cost.CostMax);
            }

            return base.Remove(entity);
        }*/
    }

}
