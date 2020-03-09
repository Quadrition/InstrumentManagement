namespace InstrumentManagement.Data
{
    using InstrumentManagement.Data.Accounts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Encapsulates business rules when accessing the data layer
    /// </summary>
    public sealed class BusinessContext : IDisposable
    {
        private readonly DataContext context;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessContext"/> class
        /// </summary>
        public BusinessContext()
        {
            context = new DataContext();
        }

        /// <summary>
        /// Gets the underlying <see cref="DataContext"/>
        /// </summary>
        public DataContext DataContext
        {
            get
            {
                return context;
            }
        }

        public ICollection<Account> GetAccounts()
        {
            return context.Accounts.OrderBy(prop => prop.Id).ToArray();
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed || !disposing)
            {
                return;
            }

            if (context != null)
            {
                context.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
