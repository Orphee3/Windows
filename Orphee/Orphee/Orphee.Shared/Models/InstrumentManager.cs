﻿using System;
using System.Collections.Generic;
using System.Linq;
using Midi;
using Orphee.Models.Interfaces;

namespace Orphee.Models
{
    public class InstrumentManager : IInstrumentManager
    {
        public Instrument CurrentInstrument { get; set; }
        public List<Instrument> InstrumentList { get; private set; }

        public InstrumentManager()
        {
            this.InstrumentList = Enum.GetValues(typeof(Instrument)).Cast<Instrument>().ToList();
        }
    }
}
