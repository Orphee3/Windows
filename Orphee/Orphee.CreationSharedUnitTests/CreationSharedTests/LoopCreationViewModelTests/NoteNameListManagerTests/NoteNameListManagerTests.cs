using NUnit.Framework;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationSharedUnitTests.CreationSharedTests.LoopCreationViewModelTests.NoteNameListManagerTests
{
    public class WhenNoteNameListManagerIsCreated
    {
        protected INoteNameListManager NoteNameListManager;

        public WhenNoteNameListManagerIsCreated()
        {
            this.NoteNameListManager = new NoteNameListManager();
        }
    }

    [TestFixture]
    public class NoteNameListShouldBeFull : WhenNoteNameListManagerIsCreated
    {
        [Test]
        public void NoteNameListShouldNotBeNull()
        {
            Assert.IsNotNull(this.NoteNameListManager.NoteNameList);
        }

        [Test]
        public void NoteNameListSouldNotBeEmpty()
        {
            Assert.IsNotEmpty(this.NoteNameListManager.NoteNameList);
        }

        [Test]
        public void NoteNameListShouldBeComposedOfTwelveNotes()
        {
            Assert.AreEqual(12, this.NoteNameListManager.NoteNameList.Count);
        }
    }
}
