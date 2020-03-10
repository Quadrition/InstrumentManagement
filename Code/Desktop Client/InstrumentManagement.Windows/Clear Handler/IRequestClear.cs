namespace InstrumentManagement.Windows.ClearHandler
{
    using System;

    /// <summary>
    /// An interface containing an event on clear request
    /// </summary>
    public interface IRequestClear
    {
        event EventHandler<ClearRequestedEventArgs> ClearRequested;
    }
}
