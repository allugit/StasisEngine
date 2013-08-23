using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
        private bool _paused;
        private bool _singleStep;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Circuit; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public CircuitSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update(GameTime gameTime)
        {
            if (!_paused || _singleStep)
            {
                string levelUid = LevelSystem.currentLevelUid;
                EventSystem eventSystem = _systemManager.getSystem(SystemType.Event) as EventSystem;
                List<int> circuitEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Circuit);

                for (int i = 0; i < circuitEntities.Count; i++)
                {
                    CircuitComponent circuitComponent = _entityManager.getComponent(levelUid, circuitEntities[i], ComponentType.Circuit) as CircuitComponent;

                    foreach (Gate gate in circuitComponent.circuit.gates)
                    {
                        if (gate.type == "output")
                        {
                            OutputGate outputGate = gate as OutputGate;
                            if (outputGate.postEvent)
                            {
                                GateOutputComponent outputComponent = _entityManager.getComponent(levelUid, outputGate.entityId, ComponentType.GateOutput) as GateOutputComponent;
                                GameEventType eventType = outputGate.state ? outputComponent.onEnabledEvent : outputComponent.onDisabledEvent;
                                eventSystem.postEvent(new GameEvent(eventType, outputGate.entityId));
                            }
                            outputGate.postEvent = false;
                        }
                    }
                }
            }
            _singleStep = false;
        }
    }
}
