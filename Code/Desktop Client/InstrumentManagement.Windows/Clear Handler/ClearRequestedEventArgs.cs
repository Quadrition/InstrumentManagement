namespace InstrumentManagement.Windows.ClearHandler
{
    using System;

    /// <summary>
    /// An <see cref="EventArgs"/> on clear request for a property
    /// </summary>
    public class ClearRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearRequestedEventArgs"/> class
        /// </summary>
        /// <param name="propertyName">A name of a property to be cleared</param>
        public ClearRequestedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; private set; }
    }
}
