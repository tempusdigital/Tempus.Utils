namespace Tempus.Utils.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;
    using Tempus.Utils.EntityFrameworkCore.Conventions;
    using System;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Data;
    using Microsoft.EntityFrameworkCore.Storage;

    public abstract class TransactionDbContext : DbContext
    {
        private IDbContextTransaction _dbContextTransaction;
        private IsolationLevel _dbContextTransactionIsolationLevel;

        public TransactionDbContext(DbContextOptions options) : base(options)
        {

        }

        protected TransactionDbContext() : base()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConvention(
                new StringRequiredConvention(),
                new UniqueIndexAttributeConvention());
        }

        public async virtual Task BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_dbContextTransaction != null)
            {
                if (_dbContextTransactionIsolationLevel != isolationLevel)
                    throw new InvalidOperationException($"It is not possible to create a transaction with isolation level '{isolationLevel}', because there is already a active transaction with a different transaction level: '{_dbContextTransactionIsolationLevel}'");

                return;
            }

            _dbContextTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            _dbContextTransactionIsolationLevel = isolationLevel;
        }

        public async virtual Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                _dbContextTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                _dbContextTransaction?.Dispose();
                _dbContextTransaction = null;
            }
        }

        public virtual void RollbackTransaction()
        {
            try
            {
                _dbContextTransaction?.Rollback();
            }
            finally
            {
                _dbContextTransaction?.Dispose();
                _dbContextTransaction = null;
            }
        }

        public override void Dispose()
        {
            RollbackTransaction();
            base.Dispose();
        }
    }
}
