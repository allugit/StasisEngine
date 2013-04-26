using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using StasisCore;
using StasisCore.Models;
using StasisGame.Managers;
using StasisGame.Data;

namespace StasisGame.UI
{
    public class WorldMapScreen : Screen
    {
        private LoderGame _game;
        private SpriteBatch _spriteBatch;
        private float _scale;
        private Vector2 _currentScreenCenter;
        private Vector2 _targetScreenCenter;
        private WorldMap _worldMap;
        private WorldMapData _worldMapData;
        private Vector2 _halfScreenSize;
        private Texture2D _pathTexture;
        private Vector2 _pathTextureOrigin;
        private ContentManager _content;

        public WorldMapScreen(LoderGame game) : base(ScreenType.WorldMap)
        {
            _game = game;
            _spriteBatch = _game.spriteBatch;
            _scale = 1f;
            
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _pathTexture = _content.Load<Texture2D>("world_map\\path");
            _pathTextureOrigin = new Vector2(_pathTexture.Width, _pathTexture.Height) / 2f;

            _halfScreenSize = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height) / 2f;
        }

        public void loadWorldMap(WorldMapData worldMapData)
        {
            // Create world map
            _worldMapData = worldMapData;
            _worldMap = new WorldMap(ResourceManager.getResource(worldMapData.worldMapUID));

            // Initialize states from stored world map data
            foreach (LevelIconData levelIconData in _worldMapData.levelIconData)
            {
                LevelIcon levelIcon = _worldMap.getLevelIcon(levelIconData.id);
                levelIcon.state = levelIconData.state;
            }
            foreach (WorldPathData worldPathData in _worldMapData.worldPathData)
            {
                WorldPath worldPath = _worldMap.getWorldPath(worldPathData.id);
                worldPath.state = worldPathData.state;
            }
        }

        public override void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newGamepadState = GamePad.GetState(PlayerIndex.One);
            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();

            if (_newGamepadState.IsConnected)
            {
                _targetScreenCenter += _newGamepadState.ThumbSticks.Left * 7 * new Vector2(1, -1);
                _targetScreenCenter += _newGamepadState.ThumbSticks.Right * 7 * new Vector2(1, -1);

                _scale = Math.Max(0.5f, _scale - _newGamepadState.Triggers.Left / 500f);
                _scale = Math.Min(1f, _scale + _newGamepadState.Triggers.Right / 500f);
            }

            if (_newKeyState.IsKeyDown(Keys.Left) || _newKeyState.IsKeyDown(Keys.A))
                _targetScreenCenter += new Vector2(-7, 0);
            if (_newKeyState.IsKeyDown(Keys.Right) || _newKeyState.IsKeyDown(Keys.D))
                _targetScreenCenter += new Vector2(7, 0);
            if (_newKeyState.IsKeyDown(Keys.Up) || _newKeyState.IsKeyDown(Keys.W))
                _targetScreenCenter += new Vector2(0, -7);
            if (_newKeyState.IsKeyDown(Keys.Down) || _newKeyState.IsKeyDown(Keys.S))
                _targetScreenCenter += new Vector2(0, 7);

            //_targetScreenCenter = Vector2.Max(_topLeft, Vector2.Min(_bottomRight, _targetScreenCenter));
            _currentScreenCenter += (_targetScreenCenter - _currentScreenCenter) / 11f;

            base.update();
        }

        public override void draw()
        {
            Vector2 viewOffset = -_currentScreenCenter + _halfScreenSize;

            // World map texture
            _spriteBatch.Draw(_worldMap.texture, viewOffset, _worldMap.texture.Bounds, Color.White, 0f, _worldMap.halfTextureSize, _scale, SpriteEffects.None, 1f);
            
            // World paths
            foreach (WorldPath worldPath in _worldMap.worldPaths)
            {
                if (worldPath.state == WorldPathState.Discovered)
                {
                    float increment = 0.001f;
                    for (float i = 0f; i < 1f; i += increment)
                    {
                        Vector2 point = Vector2.CatmullRom(worldPath.controlA.position, worldPath.pointA.position, worldPath.pointB.position, worldPath.controlB.position, i);
                        _spriteBatch.Draw(_pathTexture, viewOffset + point, _pathTexture.Bounds, Color.Yellow, 0f, _pathTextureOrigin, 1f, SpriteEffects.None, 0f);
                    }
                }
            }

            // Level icons
            foreach (LevelIcon levelIcon in _worldMap.levelIcons)
            {
                if (levelIcon.state != LevelIconState.Undiscovered)
                {
                    Texture2D texture = levelIcon.state == LevelIconState.Unfinished ? levelIcon.unfinishedIcon : levelIcon.finishedIcon;
                    _spriteBatch.Draw(texture, viewOffset + levelIcon.position, texture.Bounds, Color.White, 0f, new Vector2(texture.Width, texture.Height) / 2f, 1f, SpriteEffects.None, 0f);
                }
            }

            base.draw();
        }
    }
}
