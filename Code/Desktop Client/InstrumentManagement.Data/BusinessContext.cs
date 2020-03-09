namespace InstrumentManagement.Data
{
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales;
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

        #region Scale Scenarios

        /// <summary>
        /// Adds a new <see cref="Scale"/> entity to the data store
        /// </summary>
        /// <param name="scale">The <see cref="Scale"/> entity to be added</param>
        /// <exception cref="ArgumentException">When the <paramref name="scale"/> is invalid or the constraints are violated or number of existing <see cref="Scale"/> in the licence exceeded</exception>
        /// <exception cref="ArgumentNullException">When the <paramref name="scale"/> is null</exception>
        /// <exception cref="Exception">When the <paramref name="scale"/> serial number already exists in the data store</exception>
        public void AddNewScale(Scale scale)
        {
            if (scale == null)
            {
                throw new ArgumentNullException();
            }

            // Checks if user is valid
            if (!scale.IsValid)
            {
                throw new ArgumentException();
            }

            // Checks if an scale entity with passed scale's serial number exists in data store
            var entity = context.Scales.SingleOrDefault(perp => perp.SerialNumber == scale.SerialNumber);
            if (entity != null)
            {
                throw new Exception();
            }

            // Adds scale in data store
            context.Scales.Add(scale);
            context.SaveChanges();
        }

        /// <summary>
        /// Updates the specified <see cref="Scale"/> by applying the values passed in over the existing values from the data store
        /// </summary>
        /// <param name="scale">The <see cref="Scale"/> entity containins the new values to presist</param>
        /// <exception cref="ArgumentException">When the <see cref="Scale"/> is invalid or does not exists in data store</exception>
        /// <exception cref="ArgumentNullException">When the <paramref name="scale"/> is null </exception>
        public void UpdateScale(Scale scale)
        {
            if (scale == null)
            {
                throw new ArgumentNullException();
            }

            // Cheks if scale is valid
            if (!scale.IsValid)
            {
                throw new ArgumentException();
            }

            // Finds scale in data store
            var entity = context.Scales.Find(scale.Id);
            if (entity == null)
            {
                throw new ArgumentException();
            }

            // Updates the scale in data store
            context.Entry(entity).CurrentValues.SetValues(scale);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets a list of <see cref="Scale"/> from the data store
        /// </summary>
        /// <returns>A list of <see cref="Scale"/> entities ordered by id</returns>
        public ICollection<Scale> GetAllScales()
        {
            return context.Scales.OrderBy(prop => prop.Id).ToArray();
        }

        #region Weight Scenarios

        /// <summary>
        /// Adds a new <see cref="ScaleWeight"/> entity to the data store
        /// </summary>
        /// <param name="scaleWeight">The <see cref="ScaleWeight"/> entity to be added</param>
        /// <exception cref="ArgumentException">When the <paramref name="scaleWeight"/> is invalid or the constraints are violated</exception>
        /// <exception cref="ArgumentNullException">When the <paramref name="scaleWeight"/> is null</exception>
        /// <exception cref="Exception">When the <paramref name="scaleWeight"/> serial number already exists in the data store</exception>
        public void AddNewScaleWeight(ScaleWeight scaleWeight)
        {
            if (scaleWeight == null)
            {
                throw new ArgumentNullException();
            }

            // Checks if weight is valid
            if (!scaleWeight.IsValid)
            {
                throw new ArgumentException();
            }

            // Checks if a weight entity with passed weight's serial number exists in data store
            var entity = context.ScaleWeights.SingleOrDefault(perp => perp.SerialNumber == scaleWeight.SerialNumber);
            if (entity != null)
            {
                throw new Exception();
            }

            // Adds weight in data store
            context.ScaleWeights.Add(scaleWeight);
            context.SaveChanges();
        }

        /// <summary>
        /// Updates the specified <see cref="ScaleWeight"/> by applying the values passed in over the existing values from the data store
        /// </summary>
        /// <param name="user">The <see cref="ScaleWeight"/> entity containins the new values to presist</param>
        /// <exception cref="ArgumentException">When the <paramref name="scaleWeight"/> is invalid or does not exists in data store</exception>
        /// <exception cref="ArgumentNullException">When the <paramref name="scaleWeight"/> is null </exception>
        public void UpdateScaleWeight(ScaleWeight scaleWeight)
        {
            if (scaleWeight == null)
            {
                throw new ArgumentNullException();
            }

            // Cheks if weight is valid
            if (!scaleWeight.IsValid)
            {
                throw new ArgumentException();
            }

            // Finds weight in data store
            var entity = context.Scales.Find(scaleWeight.Id);
            if (entity == null)
            {
                throw new ArgumentException();
            }

            // Updates the weight in data store
            context.Entry(scaleWeight).CurrentValues.SetValues(scaleWeight);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets a list of <see cref="ScaleWeight"/> from the data store
        /// </summary>
        /// <returns>A list of <see cref="ScaleWeight"/> entities ordered by id</returns>
        public ICollection<ScaleWeight> GetAllScaleWeights()
        {
            return context.ScaleWeights.OrderBy(prop => prop.Id).ToArray();
        }

        #endregion

        #endregion

        #region Account Scenarios

        /// <summary>
        /// Adds a new <see cref="Account"/> entity to the data store
        /// </summary>
        /// <param name="account">The <see cref="Account"/> entity to be added</param>
        /// <exception cref="ArgumentException">When the <paramref name="account"/> is invalid or the constraints are violated</exception>
        /// <exception cref="ArgumentNullException">When the <paramref name="account"/> is null</exception>
        /// <exception cref="NotImplementedException">When the <see cref="Administrator"/> is passed</exception>
        public void AddNewAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException();
            }

            // Checks if the account is valid
            if (!account.IsValid)
            {
                throw new ArgumentException();
            }

            // Checks if the account is administrator
            //TODO vrati ovo posle
            /*if (account is Administrator)
            {
                throw new NotImplementedException();
            }*/

            // Checks if the account exists in database
            Account accountEntity = context.Accounts.SingleOrDefault(acc => acc.Username == account.Username);
            if (accountEntity != null)
            {
                throw new ArgumentException();
            }

            // Adds account to data store
            context.Accounts.Add(account);
            context.SaveChanges();
        }

        /// <summary>
        /// Updates the specified <see cref="Account"/> by applying the values passed in over the existing values from the data store
        /// </summary>
        /// <param name="account">The <see cref="Account"/> entity containing the new values to presist</param>
        /// <exception cref="ArgumentException">When the <paramref name="account"/> is invalid or does not exists in the data store</exception>
        /// <exception cref="ArgumentNullException">When the <paramref name="account"/> is null </exception>
        public void UpdateAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException();
            }

            // Checks if the account is valid
            if (!account.IsValid)
            {
                throw new ArgumentException();
            }

            // Checks if the account exists in database
            Account accountEntity = context.Accounts.Find(account.Id);
            if (accountEntity == null)
            {
                throw new ArgumentException();
            }

            // Updates tge account in the data store
            context.Entry(accountEntity).CurrentValues.SetValues(account);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets a list of <see cref="User"/> from the data store
        /// </summary>
        /// <returns>A list of <see cref="User"/> entities ordered by primary key</returns>
        public ICollection<User> GetUsers()
        {
            List<Account> accounts = context.Accounts.OrderBy(prop => prop.Id).ToList().FindAll(account => account is User);
            List<User> users = new List<User>();

            foreach (User user in accounts)
            {
                users.Add(user);
            }

            return users.ToArray();
        }

        /// <summary>
        /// Gets a list of all <see cref="Account"/> from the data store
        /// </summary>
        /// <returns>A list of all <see cref="Account"/> entities ordered by primary key</returns>
        public ICollection<Account> GetAccounts()
        {
            return context.Accounts.OrderBy(prop => prop.Id).ToArray();
        }

        #endregion

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
