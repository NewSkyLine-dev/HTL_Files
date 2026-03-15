using LinqToDB;
using LinqToDB.Data;
using System.Linq.Expressions;

public class Database
{
    private readonly DataOptions _options;
    private static readonly Lazy<Database> _instance = new(() => new Database());

    private Database() : this("Data Source=Osterhase.sqlite") { }

    public Database(string connectionString)
    {
        _options = new DataOptions().UseSQLite(connectionString);
    }

    public static Database Instance => _instance.Value;

    public int Insert<T>(T entity) where T : class
    {
        using var db = new DataConnection(_options);
        return db.Insert(entity);
    }

    public int Update<T>(T entity) where T : class
    {
        using var db = new DataConnection(_options);
        return db.Update(entity);
    }

    public int Delete<T>(T entity) where T : class
    {
        using var db = new DataConnection(_options);
        return db.Delete(entity);
    }

    public List<T> Select<T>() where T : class
    {
        using var db = new DataConnection(_options);
        return [.. db.GetTable<T>()];
    }

    public List<T> Select<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        using var db = new DataConnection(_options);
        return [.. db.GetTable<T>().Where(predicate)];
    }

    public void CreateTable<T>(bool createIfExists = true) where T : class
    {
        using var db = new DataConnection(_options);
        db.CreateTable<T>(new CreateTableOptions(TableOptions: TableOptions.CreateIfNotExists));
    }
}
