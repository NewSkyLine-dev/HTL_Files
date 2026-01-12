using LinqToDB;
using LinqToDB.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1573, 1591
#nullable enable

namespace DataModel
{
	public partial class WaldwunderDb : DataConnection
	{
		public WaldwunderDb()
		{
			InitDataContext();
		}

		public WaldwunderDb(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		public WaldwunderDb(DataOptions<WaldwunderDb> options)
			: base(options.Options)
		{
			InitDataContext();
		}

		partial void InitDataContext();

		public ITable<Bilder>     Bilders     => this.GetTable<Bilder>();
		public ITable<Waldwunder> Waldwunders => this.GetTable<Waldwunder>();
	}

	public static partial class ExtensionMethods
	{
		#region Table Extensions
		public static Bilder? Find(this ITable<Bilder> table, long id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Bilder?> FindAsync(this ITable<Bilder> table, long id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Waldwunder? Find(this ITable<Waldwunder> table, long id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Waldwunder?> FindAsync(this ITable<Waldwunder> table, long id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}
		#endregion
	}
}
