using System;
using System.Collections.Generic;
using StasisCore;
using StasisCore.Models;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class CircuitSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Circuit; } }

        public CircuitSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
        {
            EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
            List<int> circuitEntities = _entityManager.getEntitiesPosessing(ComponentType.Circuit);

            for (int i = 0; i < circuitEntities.Count; i++)
            {
                CircuitComponent circuitComponent = _entityManager.getComponent(circuitEntities[i], ComponentType.Circuit) as CircuitComponent;

                foreach (Gate gate in circuitComponent.circuit.gates)
                {
                    if (gate.type == "output")
                    {
                        OutputGate outputGate = gate as OutputGate;
                        if (outputGate.postEvent)
                        {
                            GateOutputComponent outputComponent = _entityManager.getComponent(outputGate.entityId, ComponentType.GateOutput) as GateOutputComponent;
                            GameEventType eventType = outputGate.state ? outputComponent.onEnabledEvent : outputComponent.onDisabledEvent;
                            eventSystem.postEvent(new GameEvent(eventType, outputGate.entityId));
                        }
                        outputGate.postEvent = false;
                    }
                }
            }
        }
    }
}
