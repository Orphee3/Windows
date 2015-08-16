﻿using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Midi;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.ImportModule.Interfaces;
using Moq;
using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.CreationSharedUnitTests.CreationSharedTests.LoopCreationViewModelTests.InstrumentMangerTests
{
    public class InstrumentManagerTestsBase
    {
        protected ILoopCreationPageViewModel LoopCreationViewModel;
        protected IInstrumentManager InstrumentManager;
        protected Instrument CurrentInstrument;
        protected Mock<ISoundPlayer> SoundPlayerMock;
        protected Mock<IOrpheeFileExporter> OrpheeFileExporterMock;
        protected Mock<IOrpheeFileImporter> OrpheeFileImporterMock;

        public InstrumentManagerTestsBase()
        {
            this.OrpheeFileExporterMock = new Mock<IOrpheeFileExporter>();
            this.SoundPlayerMock = new Mock<ISoundPlayer>();
            this.OrpheeFileImporterMock = new Mock<IOrpheeFileImporter>();
            this.InstrumentManager = new InstrumentManager();
            this.LoopCreationViewModel = new LoopCreationPageViewModel(this.SoundPlayerMock.Object, this.InstrumentManager, this.OrpheeFileExporterMock.Object, this.OrpheeFileImporterMock.Object);
        }
    }

    public class WhenTheProgramLoads : InstrumentManagerTestsBase
    {
        
    }

    [TestFixture]
    public class TheInstrumentManagerShouldBeFilled : WhenTheProgramLoads
    {
        [Test]
        public void TheInstrumentManagerShouldNotBeNull()
        {
            Assert.IsNotNull(this.InstrumentManager.InstrumentList);
        }

        [Test]
        public void TheInstrumentManagerShouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this.InstrumentManager.InstrumentList);
        }

        [Test]
        public void TheFirstInstrumentShouldBeTheAccousticGrandPiano()
        {
            Assert.AreEqual(Instrument.AcousticGrandPiano, this.InstrumentManager.InstrumentList[0]);
        }
    }
    [TestFixture]
    public class TheCurrentInstrumentShouldBeTheFirstInInstrumentList : WhenTheProgramLoads
    {
        [SetUp]
        public void Init()
        {
            this.CurrentInstrument = this.InstrumentManager.CurrentInstrument;
        }

        [Test]
        public void TheResultShouldBeEqual()
        {
            Assert.AreEqual(this.CurrentInstrument, this.InstrumentManager.InstrumentList[0]);
        }
    }

    public class WhenYouSelectAnOtherInstrumentInTheCombobox : InstrumentManagerTestsBase
    {
        
    }

    [TestFixture]
    public class TheCurrentInstrumentShouldChangeAsWell : WhenYouSelectAnOtherInstrumentInTheCombobox
    {
        [SetUp]
        public void Init()
        {
            this.CurrentInstrument = this.InstrumentManager.CurrentInstrument;
            this.LoopCreationViewModel.CurrentInstrumentIndex = 4;
        }

        [Test]
        public void TheResultShouldNotbeEqual()
        {
            Assert.AreNotEqual(this.CurrentInstrument, this.InstrumentManager.CurrentInstrument);
        }
    }
}
