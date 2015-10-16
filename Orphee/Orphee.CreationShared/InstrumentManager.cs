using System;
using System.Collections.Generic;
using System.Linq;
using Midi;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    /// <summary>
    /// Class managing the instruments
    /// </summary>
    public class InstrumentManager : IInstrumentManager
    {
        /// <summary>List of all the instrument available </summary>
        public Instrument CurrentInstrument { get; set; }
        /// <summary>Current instrument </summary>
        public List<Instrument> InstrumentList { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public InstrumentManager()
        {
            this.InstrumentList = Enum.GetValues(typeof(Instrument)).Cast<Instrument>().ToList();
        }
    }
}
