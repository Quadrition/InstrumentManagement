namespace InstrumentManagement.Data
{
    using InstrumentManagement.Data.Accounts;
    using InstrumentManagement.Data.Scales;
    using System.Data.Entity;

    /// <summary>
    /// Encapsulates database access
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Gets a collection of <see cref="Scale"/> entities
        /// </summary>
        public DbSet<Scale> Scales { get; set; }

        /// <summary>
        /// Gets a collection of <see cref="Account"/> entities
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Gets a collection of <see cref="Weight"/> entities
        /// </summary>
        public DbSet<Weight> ScaleWeights { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class that will use a default named connection string of 'Default' from the application's configuration file
        /// </summary>
        public DataContext() : base("Default")
        {

        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<DataContext>(modelBuilder);
        //    Database.SetInitializer(sqliteConnectionInitializer);
        //}
    }
}
