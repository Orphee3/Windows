using System.Collections.Generic;
using Midi;

namespace Orphee.Models.Interfaces
{
    public interface IInstrumentManager
    {
        // Properties
        List<Instrument> InstrumentList { get; }
        Instrument CurrentInstrument { get; set; }

        // Methods
    }
}
