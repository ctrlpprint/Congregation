using System.Linq;
using Congregation.Application.Data.NHibernate;
using NHibernate;

namespace Congregation.Application.Data
{
	public interface IRepository {
		ISession Session { get; }
		IDbContext DbContext { get; }
		T Get<T>(int id);
		IQueryable<T> GetAll<T>();
		T SaveOrUpdate<T>(T entity);

		/// <summary>
		/// This deletes the object and commits the deletion immediately.  We don't want to delay deletion
		/// until a transaction commits, as it may throw a foreign key constraint exception which we could
		/// likely handle and inform the user about.  Accordingly, this tries to delete right away; if there
		/// is a foreign key constraint preventing the deletion, an exception will be thrown.
		/// </summary>
		void Delete<T>(T entity);
	}
}