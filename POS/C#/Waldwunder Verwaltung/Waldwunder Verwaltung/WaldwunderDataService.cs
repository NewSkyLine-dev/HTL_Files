using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel;
using LinqToDB;

namespace Waldwunder_Verwaltung
{
    public class WaldwunderDataService
    {
        public static Task<List<Waldwunder>> GetAllWaldwundersAsync()
        {
            using var db = new WaldwunderDb();
            return db.Waldwunders.ToListAsyncLinqToDB();
        }

        public static Task<Waldwunder?> GetWaldwunderByIdAsync(long id)
        {
            using var db = new WaldwunderDb();
            return db.Waldwunders.FirstOrDefaultAsyncLinqToDB(w => w.Id == id);
        }

        public static Task<List<Bilder>> GetBildersForWaldwunderAsync(long wonderId)
        {
            using var db = new WaldwunderDb();
            return db.Bilders.Where(b => b.Wonder == wonderId).ToListAsyncLinqToDB();
        }

        public static Task AddNewWaldwunderWithBilderAsync(Waldwunder newWaldwunder, List<Bilder> bilder)
        {
            using var db = new WaldwunderDb();
            db.Insert(newWaldwunder);
            foreach (var bild in bilder)
            {
                bild.Wonder = newWaldwunder.Id;
                db.Insert(bild);
            }
            return Task.CompletedTask;
        }

        public static Task<List<Waldwunder>> SearchByKeyword(string keyword)
        {
            using var db = new WaldwunderDb();
            return db.Waldwunders
                .Where(w => (w.Name ?? "").Contains(keyword) || (w.Description ?? "").Contains(keyword))
                .ToListAsyncLinqToDB();
        }

        public static Task<List<Waldwunder>> SearchByType(string type)
        {
            using var db = new WaldwunderDb();
            return db.Waldwunders
                .Where(w => w.Type == type)
                .ToListAsyncLinqToDB();
        }

        public static Task<List<Waldwunder>> SearchByLocation(float latitude, float longitude)
        {
            using var db = new WaldwunderDb();
            return db.Waldwunders
                .Where(w => w.Latitude >= (decimal)(latitude - 0.5) && w.Latitude <= (decimal)(latitude + 0.5)
                          && w.Longitude >= (decimal)(longitude - 0.5) && w.Longitude <= (decimal)(longitude + 0.5))
                .ToListAsyncLinqToDB();
        }

        public static Task<List<string>> GetAllTypes()
        {
            using var db = new WaldwunderDb();
            return db.Waldwunders.Select(w => w.Type ?? "").Distinct().ToListAsyncLinqToDB();
        }
    }

    public static class LinqToDBAsyncExtensions
    {
        public static async Task<List<T>> ToListAsyncLinqToDB<T>(this IQueryable<T> query)
        {
            return await query.ToListAsync(); // Use LinqToDB's built-in async support
        }

        public static async Task<T?> FirstOrDefaultAsyncLinqToDB<T>(this IQueryable<T> query, System.Linq.Expressions.Expression<System.Func<T, bool>> predicate)
        {
            return await query.FirstOrDefaultAsync(predicate); // Use LinqToDB's built-in async support
        }
    }

}
