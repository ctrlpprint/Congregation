using System.Linq;
using Congregation.Core.Common;
using NHibernate;
using NHibernate.Linq;

namespace Congregation.Application.Data.NHibernate
{
	public class Repository : IRepository
	{
		private readonly ISessionFactory sessionFactory;

		public ISession Session {
			get { return sessionFactory.GetCurrentSession(); }
		}

		public virtual IDbContext DbContext {
			get { return new DbContext(sessionFactory); }
		}

		public Repository(ISessionFactory sessionFactory) {
			this.sessionFactory = sessionFactory;
			Check.NotNull(sessionFactory, "sessionFactory");
		}

		public virtual T Get<T>(int id) {
			return Session.Get<T>(id);
		}

		public virtual IQueryable<T> GetAll<T>() {
			return Session.Query<T>();
		}

		public virtual T SaveOrUpdate<T>(T entity) {
			Session.SaveOrUpdate(entity);
			return entity;
		}

		/// <summary>
		/// This deletes the object and commits the deletion immediately.  We don't want to delay deletion
		/// until a transaction commits, as it may throw a foreign key constraint exception which we could
		/// likely handle and inform the user about.  Accordingly, this tries to delete right away; if there
		/// is a foreign key constraint preventing the deletion, an exception will be thrown.
		/// </summary>
		public virtual void Delete<T>(T entity) {
			Session.Delete(entity);
			Session.Flush();
		}
 
	}
}