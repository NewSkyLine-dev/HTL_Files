using DataModel;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_Oppermann_Fabian;

public class DataService
{
    public static Task<List<Medium>> GetMediaAsync()
    {
        using var db = new DatabaseDb();
        return db.Media.ToListAsync();
    }

    public static Task AddNewMedium(Medium medium)
    {
        using var db = new DatabaseDb();
        return db.InsertAsync(medium);
    }

    public static Task<List<Medium>> GetAllMediumByType(int typeId)
    {
        using var db = new DatabaseDb();
        return db.Media.LoadWith(m => m.FkMedia00).Where(m => m.MediaType == typeId).ToListAsync();
    }

    public static Task<List<Medium>> SearchAfterName(string name)
    {
        using var db = new DatabaseDb();
        return db.Media.LoadWith(m => m.FkMedia00).Where(m => m.Title.ToLower().Contains(name.ToLower())).ToListAsync();
    }

    public static Task<List<Medium>>? GetMediumsWithAcc()
    {
        using var db = new DatabaseDb();
        return db.Media.LoadWith(m => m.FkMedia00).ToListAsync();
    }

    public static Task<List<MediaType>> GetAllTypes()
    {
        using var db = new DatabaseDb();
        return db.MediaTypes.ToListAsync(); // LINQ ToList :)
    }
}
