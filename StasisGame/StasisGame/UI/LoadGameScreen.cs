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
        private List<TextButton> _savedGameButtons;

        public LoadGameScreen(LoderGame game)
            : base(game.spriteBatch, ScreenType.LoadGameMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _logo = _content.Load<Texture2D>("logo");
            _savedGameButtons = new List<TextButton>();

            createUIComponents();
        }

        ~LoadGameScreen()
        {
            _content.Unload();
        }

        private void createUIComponents()
        {
            List<XElement> playerSaves = DataManager.loadPlayerSaves();
            Vector2 initialPosition = new Vector2(-200, 300);

            /*
            foreach (XElement playerSave in playerSaves)
            {
                int slot = int.Parse(playerSave.Attribute("slot").Value);
                string text = slot.ToString() + " - " + playerSave.Attribute("name").Value;
                TextButton button = new TextButton(
                    _game.spriteBatch,
                    _arial,
                    Color.White,
                    (int)initialPosition.X,
                    (int)(initialPosition.Y) + _saveButtons.Count * 24,
                    text,
                    UIAlignment.TopCenter,
                    () =>
                    {
                        _game.closeLoadGameMenu();
                        _game.loadGame(slot);
                    });
                _saveButtons.Add(button);
            }*/
        }

        public override void update()
        {
            base.update();

            // Handle button mouse input
            for (int i = 0; i < _savedGameButtons.Count; i++)
            {
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

            // Draw buttons
            for (int i = 0; i < _savedGameButtons.Count; i++)
            {
                _savedGameButtons[i].draw();
            }

            base.draw();
        }
    }
}
