using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisGame.Components
{
    public class CircuitComponent : IComponent
    {
        private Circuit _circuit;

        public ComponentType componentType { get { return ComponentType.Circuit; } }

        public CircuitComponent(Circuit circuit)
        {
            _circuit = circuit;
        }
    }
}
