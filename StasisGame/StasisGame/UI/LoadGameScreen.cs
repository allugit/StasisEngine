using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisGame.Managers;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class LoadGameScreen : Screen
    {
        private LoderGame _game;
        private Texture2D _logo;
        private ContentManager _content;
        private SpriteFont _savedGameFont;
        private BluePane _container;
        private List<StonePane> _bars;
        private List<TextButton> _savedGameButtons;
        private TextButton _selectedButton;

        public LoadGameScreen(LoderGame game)
            : base(game.spriteBatch, ScreenType.LoadGameMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _logo = _content.Load<Texture2D>("logo");
            _savedGameFont = _content.Load<SpriteFont>("load_game_menu/saved_game_font");
            _savedGameButtons = new List<TextButton>();
            _bars = new List<StonePane>();

            List<XElement> playerSaves = DataManager.loadPlayerSaves();
            Vector2 initialPosition = new Vector2(-230, -180);

            _container = new BluePane(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                -250,
                -200,
                500,
                400);

            foreach (XElement playerSave in playerSaves)
            {
                int slot = int.Parse(playerSave.Attribute("slot").Value);
                string text = slot.ToString() + " - " + playerSave.Attribute("name").Value;

                _bars.Add(new StonePane(
                    _spriteBatch,
                    UIAlignment.MiddleCenter,
                    (int)initialPosition.X - 10,
                    (int)(initialPosition.Y) + _bars.Count * 48 - 10,
                    480,
                    40));

                _savedGameButtons.Add(new TextButton(
                    _game.spriteBatch,
                    _savedGameFont,
                    UIAlignment.MiddleCenter,
                    (int)initialPosition.X,
                    (int)(initialPosition.Y) + _savedGameButtons.Count * 48,
                    4,
                    TextAlignment.Left,
                    text,
                    1,
                    Color.White,
                    new Color(0.7f, 0.7f, 0.7f),
                    () =>
                    {
                        _game.closeLoadGameMenu();
                        _game.loadGame(slot);
                    }));
            }
        }

        ~LoadGameScreen()
        {
            _content.Unload();
        }

        public override void update()
        {
            bool mouseOverTextButton = false;
            Vector2 mouse = new Vector2(_newMouseState.X, _newMouseState.Y);

            // Update input
            base.update();

            // Handle button mouse input
            foreach (TextButton button in _savedGameButtons)
            {
                if (button.hitTest(mouse))
                {
                    if (_selectedButton != button)
                    {
                        if (_selectedButton != null)
                            _selectedButton.mouseOut();

                        button.mouseOver();
                    }

                    mouseOverTextButton = true;
                    _selectedButton = button;

                    if (_newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                        button.activate();

                    break;
                }
            }
            if (!mouseOverTextButton)
            {
                if (_selectedButton != null)
                    _selectedButton.mouseOut();

                _selectedButton = null;
            }

            // Background renderer
            _game.menuBackgroundRenderer.update(35f, _game.menuBackgroundScreenOffset);

            base.update();
        }

        public override void draw()
        {
            // Draw background
            _game.menuBackgroundRenderer.draw();

            // Draw logo
            _spriteBatch.Draw(_logo, new Vector2((int)(_game.GraphicsDevice.Viewport.Width - _logo.Width), 0), _logo.Bounds, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            // Draw container
            _container.draw();

            // Draw bars
            foreach (StonePane bar in _bars)
                bar.draw();

            // Draw buttons
            for (int i = 0; i < _savedGameButtons.Count; i++)
            {
                _savedGameButtons[i].draw();
            }

            base.draw();
        }
    }
}
