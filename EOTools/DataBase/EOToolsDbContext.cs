using EOTools.Translation.QuestManager.Event;
using EOTools.Translation.QuestManager.Quests;
using EOTools.Translation.QuestManager.Seasons;
using EOTools.Translation.QuestManager.Updates;
using Microsoft.EntityFrameworkCore;
using System;

namespace EOTools.DataBase
{
    public class EOToolsDbContext : DbContext
    {
        public DbSet<UpdateModel> Updates { get; set; }
        public DbSet<QuestModel> Quests { get; set; }
        public DbSet<SeasonModel> Seasons { get; set; }
        public DbSet<EventModel> Events { get; set; }

        public string DbPath { get; }

        public EOToolsDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            DbPath = System.IO.Path.Join(path, "EOTools", "EOTools.db");
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(DbPath));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
