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
        private Effect _fogEffect;
        private Texture2D _antiFogBrush;
        private Vector2 _antiFogBrushOrigin;
        private RenderTarget2D _fogRT;
        private RenderTarget2D _antiFogRT;
        private Vector2 _levelSelectPosition;
        private Texture2D _levelSelectIcon;
        private Vector2 _levelSelectIconHalfSize;
        private float _levelSelectAngle;
        private Color _levelSelectIconColor;
        private Color _levelSelectIconSelectedColor;
        private Color _levelSelectIconDeselectedColor;
        private LevelIcon _selectedLevelIcon;
        private bool _allowNewLevelSelection = true;
        private SpriteFont _levelSelectTitleFont;
        private SpriteFont _levelSelectDescriptionFont;

        public WorldMapScreen(LoderGame game) : base(ScreenType.WorldMap)
        {
            _game = game;
            _spriteBatch = _game.spriteBatch;
            _scale = 1f;
            
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _fogEffect = _content.Load<Effect>("fog_effect");
            _pathTexture = _content.Load<Texture2D>("world_map\\path");
            _pathTextureOrigin = new Vector2(_pathTexture.Width, _pathTexture.Height) / 2f;
            _antiFogBrush = _content.Load<Texture2D>("world_map\\anti_fog_brush");
            _antiFogBrushOrigin = new Vector2(_antiFogBrush.Width, _antiFogBrush.Height) / 2f;
            _levelSelectIcon = _content.Load<Texture2D>("level_select_icon");
            _levelSelectIconHalfSize = new Vector2(_levelSelectIcon.Width, _levelSelectIcon.Height) / 2f;
            _levelSelectIconSelectedColor = Color.Yellow;
            _levelSelectIconDeselectedColor = Color.White * 0.8f;
            _levelSelectIconColor = _levelSelectIconDeselectedColor;
            _levelSelectTitleFont = _content.Load<SpriteFont>("world_map\\level_select_title");
            _levelSelectDescriptionFont = _content.Load<SpriteFont>("world_map\\level_select_description");
            _halfScreenSize = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height) / 2f;
            _fogRT = new RenderTarget2D(_spriteBatch.GraphicsDevice, _spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height);
            _antiFogRT = new RenderTarget2D(_spriteBatch.GraphicsDevice, _spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height);
        }

        ~WorldMapScreen()
        {
            _content.Unload();
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

        private LevelIcon hitTestLevelIcons(Vector2 mouseWorld, float tolerance)
        {
            float shortest = 9999999f;
            LevelIcon result = null;

            foreach (LevelIcon levelIcon in _worldMap.levelIcons)
            {
                if (levelIcon.state != LevelIconState.Undiscovered)
                {
                    float distance = (mouseWorld - levelIcon.position).Length();

                    if (distance <= tolerance)
                    {
                        shortest = distance;
                        result = levelIcon;
                    }
                }
            }
            return result;
        }

        public override void update()
        {
            Vector2 mouseDelta;
            Vector2 mousePosition;
            Vector2 viewOffset = _halfScreenSize - _currentScreenCenter;
            bool wasLevelIconPreviouslySelected = _selectedLevelIcon != null;

            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newGamepadState = GamePad.GetState(PlayerIndex.One);
            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();

            mouseDelta = new Vector2(_newMouseState.X - _oldMouseState.X, _newMouseState.Y - _oldMouseState.Y);
            mousePosition = new Vector2(_newMouseState.X, _newMouseState.Y) - viewOffset;
            _allowNewLevelSelection = mouseDelta.Length() > 2f;
            _levelSelectAngle = MathHelper.WrapAngle(_levelSelectAngle + 0.05f);

            if (_allowNewLevelSelection)
                _selectedLevelIcon = hitTestLevelIcons(mousePosition, 100f);

            if (_selectedLevelIcon == null && wasLevelIconPreviouslySelected)
            {
                _levelSelectPosition = new Vector2(_oldMouseState.X, _oldMouseState.Y);
                _levelSelectIconColor = _levelSelectIconDeselectedColor;
            }
            else if (_selectedLevelIcon != null)
            {
                _levelSelectPosition = _selectedLevelIcon.position + viewOffset;
                _levelSelectIconColor = _levelSelectIconSelectedColor;
                _targetScreenCenter += (_selectedLevelIcon.position - _currentScreenCenter) / 100f;
            }
            
            if (_selectedLevelIcon == null)
            {
                _levelSelectPosition += mouseDelta;
            }

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

            if (_newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            {
                if (_selectedLevelIcon != null)
                {
                    _game.closeWorldMap();
                    _game.loadLevel(_selectedLevelIcon.levelUID);
                }
            }

            //_levelSelectPosition += mouseDelta;

            //_targetScreenCenter = Vector2.Max(_topLeft, Vector2.Min(_bottomRight, _targetScreenCenter));
            _currentScreenCenter += (_targetScreenCenter - _currentScreenCenter) / 11f;

            base.update();
        }

        // Pre process (occurs before Draw())
        public void preProcess()
        {
            Vector2 viewOffset = -_currentScreenCenter + _halfScreenSize;
            float antiFogTextureScale = _scale * 0.5f;

            // Draw anti fog points (points where the anti fog brush texture will be drawn
            _spriteBatch.GraphicsDevice.SetRenderTarget(_antiFogRT);
            _spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            foreach (WorldPath worldPath in _worldMap.worldPaths)
            {
                if (worldPath.state == WorldPathState.Discovered)
                {
                    float increment = 0.1f;
                    for (float i = 0f; i < 1f; i += increment)
                    {
                        Vector2 point = Vector2.CatmullRom(worldPath.controlA.position, worldPath.pointA.position, worldPath.pointB.position, worldPath.controlB.position, i);
                        _spriteBatch.Draw(_antiFogBrush, viewOffset + point, _antiFogBrush.Bounds, Color.White, 0f, _antiFogBrushOrigin, antiFogTextureScale, SpriteEffects.None, 0f);
                    }
                }
            }
            foreach (LevelIcon levelIcon in _worldMap.levelIcons)
            {
                if (levelIcon.state != LevelIconState.Undiscovered)
                {
                    _spriteBatch.Draw(_antiFogBrush, viewOffset + levelIcon.position, _antiFogBrush.Bounds, Color.White, 0f, _antiFogBrushOrigin, antiFogTextureScale, SpriteEffects.None, 0f);
                }
            }
            _spriteBatch.End();
            _spriteBatch.GraphicsDevice.SetRenderTarget(_fogRT);
            _spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, _fogEffect);
            _spriteBatch.Draw(_antiFogRT, _antiFogRT.Bounds, Color.White);
            _spriteBatch.End();
            _spriteBatch.GraphicsDevice.SetRenderTarget(null);
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
                        _spriteBatch.Draw(_pathTexture, viewOffset + point, _pathTexture.Bounds, Color.Yellow, 0f, _pathTextureOrigin, _scale, SpriteEffects.None, 0.3f);
                    }
                }
            }

            // Level icons
            foreach (LevelIcon levelIcon in _worldMap.levelIcons)
            {
                if (levelIcon.state != LevelIconState.Undiscovered)
                {
                    Texture2D texture = levelIcon.state == LevelIconState.Unfinished ? levelIcon.unfinishedIcon : levelIcon.finishedIcon;
                    _spriteBatch.Draw(texture, viewOffset + levelIcon.position, texture.Bounds, Color.White, 0f, new Vector2(texture.Width, texture.Height) / 2f, _scale, SpriteEffects.None, 0.2f);
                }
            }

            // Draw fog render target
            _spriteBatch.Draw(_fogRT, Vector2.Zero, _fogRT.Bounds, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0.1f);

            // Draw level select icon
            _spriteBatch.Draw(_levelSelectIcon, _levelSelectPosition, _levelSelectIcon.Bounds, _levelSelectIconColor, _levelSelectAngle, _levelSelectIconHalfSize, _scale, SpriteEffects.None, 0f);

            // Draw title text
            if (_selectedLevelIcon != null)
                _spriteBatch.DrawString(_levelSelectTitleFont, _selectedLevelIcon.title, new Vector2(32, 32), Color.White);

            // Draw description text
            if (_selectedLevelIcon != null)
                _spriteBatch.DrawString(_levelSelectDescriptionFont, _selectedLevelIcon.description, new Vector2(32, 96), Color.LightGray);

            base.draw();
        }
    }
}
