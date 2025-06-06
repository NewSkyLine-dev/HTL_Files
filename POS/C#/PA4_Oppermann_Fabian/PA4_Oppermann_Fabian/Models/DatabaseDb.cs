// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB;
using LinqToDB.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1573, 1591
#nullable enable

namespace DataModel
{
	public partial class DatabaseDb : DataConnection
	{
		public DatabaseDb()
		{
			InitDataContext();
		}

		public DatabaseDb(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		public DatabaseDb(DataOptions<DatabaseDb> options)
			: base(options.Options)
		{
			InitDataContext();
		}

		partial void InitDataContext();

		public ITable<Medium>    Media      => this.GetTable<Medium>();
		public ITable<MediaType> MediaTypes => this.GetTable<MediaType>();
	}

	public static partial class ExtensionMethods
	{
		#region Table Extensions
		public static Medium? Find(this ITable<Medium> table, long id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Medium?> FindAsync(this ITable<Medium> table, long id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static MediaType? Find(this ITable<MediaType> table, long id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<MediaType?> FindAsync(this ITable<MediaType> table, long id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}
		#endregion
	}
}
