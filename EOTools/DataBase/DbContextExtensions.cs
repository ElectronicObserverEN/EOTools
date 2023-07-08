using Microsoft.EntityFrameworkCore;

namespace EOTools.DataBase
{
    public static class DbContextExtensions
    {
        public static void SetEntityAsModified(this DbContext db, object model)
        {
            return;
            db.Entry(model).State = db.Entry(model).State switch
            {
                EntityState.Unchanged or EntityState.Detached => EntityState.Modified,
                _ => db.Entry(model).State
            };
        }
    }
}
