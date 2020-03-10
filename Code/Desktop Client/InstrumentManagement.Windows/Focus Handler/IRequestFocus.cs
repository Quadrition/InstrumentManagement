namespace InstrumentManagement.Windows.FocusHandler
{
    using System;

    /// <summary>
    /// An interface containing an event on focus request
    /// </summary>
    public interface IRequestFocus
    {
        event EventHandler<FocusRequestedEventArgs> FocusRequested;
    }
}
