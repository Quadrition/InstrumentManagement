namespace InstrumentManagement.Data
{
    using InstrumentManagement.Windows;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a instrument base class
    /// </summary>
    public abstract class Instrument : Model
    {
        private string manufacturer;
        private string type;
        private string serialNumber;
        private string inventoryNumber;
        private int? theYearOfProduction;
        private string location;
        private string organizationalUnit;
        private string locationOfTheImage;

        /// <summary>
        /// Gets or sets a manufacturer for the <see cref="Instrument"/>
        /// </summary>
        [StringLength(32, ErrorMessage = "Proizvođač može imati najviše 32 karaktera")]
        public string Manufacturer
        {
            get
            {
                return manufacturer;
            }
            set
            {
                manufacturer = value;
                NotifyPropertyChanged(nameof(Manufacturer));
            }
        }

        /// <summary>
        /// Gets or sets a type for the <see cref="Instrument"/>
        /// </summary>
        [StringLength(32, ErrorMessage = "Tip može imati najviše 32 karaktera")]
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                NotifyPropertyChanged(nameof(Type));
            }
        }

        /// <summary>
        /// Gets or sets a serial number for the <see cref="Instrument"/>
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Serijski broj je obavezan"), Index("IX_SerialNumber", IsUnique = true)]
        [StringLength(32, ErrorMessage = "Serijski broj može imati najviše 32 karaktera")]
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }
            set
            {
                serialNumber = value;
                NotifyPropertyChanged(nameof(Type));
            }
        }

        /// <summary>
        /// Gets or sets an inventory number for the <see cref="Instrument"/>
        /// </summary>
        [StringLength(32, ErrorMessage = "Inventarni broj može imati najviše 32 karaktera")]
        public string InventoryNumber
        {
            get
            {
                return inventoryNumber;
            }
            set
            {
                inventoryNumber = value;
                NotifyPropertyChanged(nameof(InventoryNumber));
            }
        }

        /// <summary>
        /// Gets or sets a year of production for the <see cref="Instrument"/>
        /// </summary>
        [Range(1900, 2100, ErrorMessage = "Godina proizvodnje je neispravna")]
        public int? TheYearOfProduction
        {
            get
            {
                return theYearOfProduction;
            }
            set
            {
                theYearOfProduction = value;
                NotifyPropertyChanged(nameof(TheYearOfProduction));
            }
        }

        /// <summary>
        /// Gets or sets a location for the <see cref="Instrument"/>
        /// </summary>
        [StringLength(32, ErrorMessage = "Lokacija može imati najviše 32 karaktera")]
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                NotifyPropertyChanged(nameof(Location));
            }
        }

        /// <summary>
        /// Gets or sets an organizational unit for the <see cref="Instrument"/>
        /// </summary>
        [StringLength(32, ErrorMessage = "Organizaciona jedinica može imati najviše 32 karaktera")]
        public string OrganizationalUnit
        {
            get
            {
                return organizationalUnit;
            }
            set
            {
                organizationalUnit = value;
                NotifyPropertyChanged(nameof(OrganizationalUnit));
            }
        }

        /// <summary>
        /// Gets or sets a location of the image for the <see cref="Instrument"/>
        /// </summary>
        [StringLength(2048, ErrorMessage = "URL slike može imati najviše 2048 karaktera")]
        public string LocationOfTheImage
        {
            get
            {
                return locationOfTheImage;
            }
            set
            {
                locationOfTheImage = value;
                NotifyPropertyChanged(nameof(LocationOfTheImage));
            }
        }

        static readonly string[] ValidatedProperties = { "Manufacturer", "Type", "SerialNumber", "InventoryNumber", "TheYearOfProduction", "Location", "OrganizationalUnit", "LocationOfTheImage" };

        /// <summary>
        /// Initializes a new instance of <see cref="Instrument"/> class
        /// </summary>
        public Instrument()
        {
            
        }

        /// <summary>
        /// Checks if all <see cref="Instrument"/>'s properties are valid
        /// </summary>
        /// <returns>True if all properties is valid, otherwise false</returns>
        public override bool IsValid
        {
            get
            {
                return ValidatedProperties.FirstOrDefault(perp => OnValidate(perp) != null) == null;
            }
        }
    }
}
