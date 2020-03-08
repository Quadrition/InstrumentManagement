namespace InstrumentManagement.Data.Accounts
{
    using InstrumentManagement.Data.Scales;
    using System.Collections.Generic;

    /// <summary>
    /// Repressents a user-type <see cref="Account"/> with limited functionalities
    /// </summary>
    public class User : Account
    {
        /// <summary>
        /// Gets or sets a list of <see cref="Scale"/> that can be accessed
        /// </summary>
        public virtual ICollection<Scale> Scales { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="User"/> class
        /// </summary>
        public User() : base()
        {
            Scales = new HashSet<Scale>();
        }
    }
}
