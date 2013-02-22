using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisGame.Components
{
    public class GateOutputComponent : IComponent
    {
        private OutputGate _outputGate;

        public ComponentType componentType { get { return ComponentType.GateOutput; } }
        public OutputGate outputGate { get { return _outputGate; } set { _outputGate = value; } }

        public GateOutputComponent()
        {
        }
    }
}
