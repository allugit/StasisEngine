using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.Systems
{
    public enum SystemType
    {
        Render,
        Input,
        Physics,
        Fluid,
        Player,
        CharacterMovement,
        Camera,
        Tree,
        Event,
        Circuit,
        Screen,
        Equipment,
        Rope,
        Level,
        WorldMap
    };

    public interface ISystem
    {
        void update();
        SystemType systemType { get; }
        int defaultPriority { get; }
        bool paused { get; set; }
        bool singleStep { get; set; }
    }
}
