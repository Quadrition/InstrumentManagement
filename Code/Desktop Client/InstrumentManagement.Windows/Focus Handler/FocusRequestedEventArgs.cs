namespace InstrumentManagement.Windows.FocusHandler
{
    using System;

    /// <summary>
    /// An event arguments on focus request for property
    /// </summary>
    public class FocusRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusRequestedEventArgs"/> class
        /// </summary>
        /// <param name="propertyName">A name of a property to be focused</param>
        public FocusRequestedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; private set; }
    }
}
