namespace InstrumentManagement.Data.Accounts
{
    using InstrumentManagement.Windows;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a simlpe account with username and password
    /// </summary>
    public abstract class Account : Model
    {
        private string username;
        private string password;

        /// <summary>
        /// Gets or sets a username for the <see cref="Account"/>
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Korisničko ime je obavezno"), Index("IX_Username", IsUnique = true)]
        [StringLength(32, ErrorMessage = "Korisničko ime može imati najviše 32 karaktera")]
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        /// <summary>
        /// Gets or sets a password for the <see cref="Account"/>
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Lozinka je obavezna i ne može biti prazna")]
        [StringLength(32, ErrorMessage = "Korisničko ime može imati najviše 32 karaktera")]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                NotifyPropertyChanged(nameof(Password));
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Account"/> class
        /// </summary>
        public Account()
        {

        }

        static readonly string[] ValidatedProperties = { "Username", "Password" };

        /// <summary>
        /// Checks if all <see cref="Account"/>'s properties are valid
        /// </summary>
        /// <returns>True if all properties are valid, otherwise false</returns>
        public override bool IsValid
        {
            get
            {
                return ValidatedProperties.FirstOrDefault(perp => OnValidate(perp) != null) == null;
            }
        }
    }
}
