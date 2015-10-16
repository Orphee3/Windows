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
        List<Instrument> InstrumentList { get; }
        /// <summary>Current instrument </summary>
        Instrument CurrentInstrument { get; set; }

        // Methods
    }
}
