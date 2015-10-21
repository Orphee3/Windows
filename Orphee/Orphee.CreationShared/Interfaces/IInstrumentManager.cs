using System.Collections.Generic;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    /// <summary>
    /// InstrumentManager interface
    /// </summary>
    public interface IInstrumentManager
    {
        // Properties
        /// <summary>List of all the instrument available </summary>
        List<MyInstrument> InstrumentList { get; }

        /// <summary>Current instrument </summary>

        // Methods
        int GetInstrumentIndex(Instrument instrument);
    }
}
