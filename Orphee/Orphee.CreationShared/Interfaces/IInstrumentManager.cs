using System.Collections.Generic;
using Midi;

namespace Orphee.CreationShared.Interfaces
{
    public interface IInstrumentManager
    {
        // Properties
        List<Instrument> InstrumentList { get; }
        Instrument CurrentInstrument { get; set; }

        // Methods
    }
}
