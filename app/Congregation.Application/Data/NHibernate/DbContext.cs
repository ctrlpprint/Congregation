using System;
using Congregation.Core.Common;
using NHibernate;

namespace Congregation.Application.Data.NHibernate
{
	public class DbContext : IDbContext
	{
		public DbContext(ISessionFactory sessionFactory) {
			Check.NotNull(sessionFactory, "sessionFactory");
			this.sessionFactory = sessionFactory;
		}

		public virtual IDisposable BeginTransaction() {
			return sessionFactory.GetCurrentSession().BeginTransaction();
		}

		/// <summary>
		/// This isn't specific to any one DAO and flushes everything that has been
		/// changed since the last commit.
		/// </summary>
		public virtual void CommitChanges() {
			sessionFactory.GetCurrentSession().Flush();
		}

		public virtual void CommitTransaction() {
			sessionFactory.GetCurrentSession().Transaction.Commit();
		}

		public virtual void RollbackTransaction() {
			sessionFactory.GetCurrentSession().Transaction.Rollback();
		}

		private readonly ISessionFactory sessionFactory;
	}

	/// <summary>
	/// Note that outside of CommitChanges(), you shouldn't have to invoke this object very often.
	/// If you're using the NHibernateSessionModule HttpModule, then the transaction 
	/// opening/committing will be taken care of for you.
	/// </summary>
	public interface IDbContext
	{
		IDisposable BeginTransaction();
		void CommitChanges();
		void CommitTransaction();
		void RollbackTransaction();		
	}
}