using System;

namespace StasisEditor.Controllers.Actors
{
    public interface ISubControllable
    {
        ILevelController getLevelController();
    }
}
