using System;
using System.Collections.Generic;

namespace StasisCore
{
    public interface IEventHandler
    {
        void trigger(GameEvent e);
    }
}
