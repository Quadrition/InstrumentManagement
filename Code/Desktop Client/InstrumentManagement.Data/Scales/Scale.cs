namespace InstrumentManagement.Data.Scales
{
    using InstrumentManagement.Data.Accounts;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a scale as a type of the <see cref="Instrument"/>
    /// </summary>
    public class Scale : Instrument
    {
        /// <summary>
        /// Gets or sets a list of <see cref="ScaleRange"/> that are related to the <see cref="Scale"/>
        /// </summary>
        public virtual ICollection<ScaleRange> Ranges { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="User"/> who have an access to the <see cref="Scale"/>
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        public Scale()
        {
            Ranges = new HashSet<ScaleRange>();
            Users = new HashSet<User>();
        }

        /// <summary>
        /// Checks if all <see cref="Scale"/>'s properties are valid
        /// </summary>
        /// <returns>True if all properties is valid, otherwise false</returns>
        public override bool IsValid
        {
            get
            {
                if (Ranges.Count == 0)
                    return false;

                return base.IsValid;
            }
        }
    }
}
