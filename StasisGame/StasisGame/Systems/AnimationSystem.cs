using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class AnimationSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private AnimationManager _animationManager;
        private bool _paused;
        private bool _singleStep;

        public SystemType systemType { get { return SystemType.Animation; } }
        public int defaultPriority { get { return 35; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public AnimationSystem(SystemManager systemManager, EntityManager entityManager, AnimationManager animationManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _animationManager = animationManager;
        }

        public void setAnimation(CharacterRenderComponent component, string animation)
        {
            component.animation = animation;
            component.time = 0f;
            component.currentFrame = 0;
        }

        public void update(GameTime gameTime)
        {
            LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;
            string levelUid = LevelSystem.currentLevelUid;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (levelSystem.finalized)
            {
                if (!_paused || _singleStep)
                {
                    List<int> characterRenderEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.CharacterRender);

                    for (int i = 0; i < characterRenderEntities.Count; i++)
                    {
                        CharacterRenderComponent characterRenderComponent = _entityManager.getComponent(levelUid, characterRenderEntities[i], ComponentType.CharacterRender) as CharacterRenderComponent;
                        CharacterMovementComponent characterMovementComponent = _entityManager.getComponent(levelUid, characterRenderEntities[i], ComponentType.CharacterMovement) as CharacterMovementComponent;
                        float changeRate = 1f / _animationManager.getFPS(characterRenderComponent.character, characterRenderComponent.animation);

                        // Handle time
                        characterRenderComponent.time += dt;
                        if (characterRenderComponent.time >= changeRate)
                        {
                            int currentFrame = characterRenderComponent.currentFrame;
                            int totalFrames = _animationManager.getFrameCount(characterRenderComponent.character, characterRenderComponent.animation);
                            int nextFrame = currentFrame + 1;

                            characterRenderComponent.currentFrame = nextFrame == totalFrames ? 0 : nextFrame;
                            characterRenderComponent.time -= changeRate;
                        }

                        // Update animation
                        if (characterMovementComponent.walkLeft)
                        {
                            if (characterRenderComponent.animation != "walk_left")
                            {
                                setAnimation(characterRenderComponent, "walk_left");
                            }
                        }
                        else if (characterMovementComponent.walkRight)
                        {
                            if (characterRenderComponent.animation != "walk_right")
                            {
                                setAnimation(characterRenderComponent, "walk_right");
                            }
                        }
                        else
                        {
                            if (characterRenderComponent.animation != "idle")
                            {
                                setAnimation(characterRenderComponent, "idle");
                            }
                        }
                    }
                }
                _singleStep = false;
            }
        }
    }
}
