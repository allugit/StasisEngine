using System;

namespace StasisEditor.Controllers.Actors
{
    public interface IActorController
    {
        ILevelController getLevelController();
    }
}
