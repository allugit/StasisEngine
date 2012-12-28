using System;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public interface ISubControllable
    {
        LevelController getLevelController();

        void selectSubController(ActorSubController subController);
        void deselectSubController(ActorSubController subController);
        bool shift { get; }
        bool ctrl { get; }

        void delete();
    }
}
