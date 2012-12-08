using System;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public interface ISubControllable
    {
        ILevelController getLevelController();

        Vector2 getPosition();
        void setPosition(Vector2 position);

        void selectSubController(ActorSubController subController);
        void deselectSubController(ActorSubController subController);
    }
}
