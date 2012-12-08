using System;

namespace StasisEditor.Controllers.Actors
{
    public interface ISubControllable
    {
        ILevelController getLevelController();
        void selectSubController(ActorSubController subController);
        void deselectSubController(ActorSubController subController);
    }
}
