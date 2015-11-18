using Orphee.CreationShared.Interfaces;

namespace Orphee.CreationShared
{
    public class OrpheeTrackBase
    {
        protected IColorManager _colorManager;

        public OrpheeTrackBase(IColorManager colorManager)
        {
            this._colorManager = colorManager;
        }
    }
}
