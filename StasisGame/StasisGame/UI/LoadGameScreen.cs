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
        private SpriteFont _confirmationFont;
        private BluePane _container;
        private List<LabelTextureButton> _savedGameButtons;
        private List<TextureButton> _deleteGameButtons;
        private TextureButton _cancelButton;
        private ConfirmationOverlay _confirmationOverlay;

        public LoadGameScreen(LoderGame game)
            : base(game.spriteBatch, ScreenType.LoadGameMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _logo = _content.Load<Texture2D>("logo");
            _savedGameFont = _content.Load<SpriteFont>("load_game_menu/saved_game_font");
            _confirmationFont = _content.Load<SpriteFont>("shared_ui/confirmation_font");
            _savedGameButtons = new List<LabelTextureButton>();
            _deleteGameButtons = new List<TextureButton>();

            List<XElement> playerSaves = DataManager.loadPlayerSaves();
            Vector2 initialPosition = new Vector2(-240, -190);

            _container = new BluePane(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                -250,
                -200,
                545,
                400);

            _cancelButton = new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                155,
                180,
                _content.Load<Texture2D>("shared_ui/cancel_button_over"),
                _content.Load<Texture2D>("shared_ui/cancel_button"),
                new Rectangle(0, 0, 152, 33),
                () =>
                {
                    _game.closeLoadGameMenu();
                    _game.openMainMenu();
                });

            foreach (XElement playerSave in playerSaves)
            {
                int slot = int.Parse(playerSave.Attribute("slot").Value);
                string text = slot.ToString() + " - " + playerSave.Attribute("name").Value;

                _deleteGameButtons.Add(new TextureButton(
                    _spriteBatch,
                    UIAlignment.MiddleCenter,
                    (int)initialPosition.X + 485,
                    (int)(initialPosition.Y) + _savedGameButtons.Count * 45,
                    _content.Load<Texture2D>("shared_ui/x_button_over"),
                    _content.Load<Texture2D>("shared_ui/x_button"),
                    new Rectangle(0, 0, 40, 40),
                    () =>
                    {
                        openConfirmation(
                            "Are you sure you want to delete this saved game?",
                            () => { closeConfirmation(); },
                            () =>
                            {
                                DataManager.deletePlayerData(slot);
                                closeConfirmation();
                                _game.closeLoadGameMenu();
                                _game.openLoadGameMenu();
                            });
                    }));

                _savedGameButtons.Add(new LabelTextureButton(
                    _game.spriteBatch,
                    UIAlignment.MiddleCenter,
                    (int)initialPosition.X,
                    (int)(initialPosition.Y) + _savedGameButtons.Count * 45,
                    _content.Load<Texture2D>("load_game_menu/saved_game_button_over"),
                    _content.Load<Texture2D>("load_game_menu/saved_game_button"),
                    new Rectangle(0, 0, 480, 40),
                    _savedGameFont,
                    TextAlignment.Left,
                    text,
                    16,
                    12,
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

        private void openConfirmation(string text, Action onCancel, Action onOkay)
        {
            _confirmationOverlay = new ConfirmationOverlay(
                _spriteBatch,
                _confirmationFont,
                text,
                onCancel,
                onOkay);
        }

        private void closeConfirmation()
        {
            _confirmationOverlay = null;
        }

        private bool hitTestTextureButton(TextureButton button)
        {
            if (button.hitTest(new Vector2(_newMouseState.X, _newMouseState.Y)))
            {
                if (!button.selected)
                    button.mouseOver();

                if (_newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                    button.activate();

                return true;
            }
            else if (button.selected)
            {
                button.mouseOut();
            }

            return false;
        }

        public override void update()
        {
            Vector2 mouse = new Vector2(_newMouseState.X, _newMouseState.Y);

            // Only update confirmation window if one exists
            if (_confirmationOverlay != null)
            {
                _confirmationOverlay.update();
            }
            else
            {
                // Update input
                base.update();

                // Handle mouse input for saved game buttons
                foreach (LabelTextureButton button in _savedGameButtons)
                    hitTestTextureButton(button);

                // Handle mouse input for delete game buttons
                foreach (TextureButton button in _deleteGameButtons)
                    hitTestTextureButton(button);

                // Handle mouse input for cancel button
                hitTestTextureButton(_cancelButton);
            }

            // Update background renderer
            _game.menuBackgroundRenderer.update(35f, _game.menuBackgroundScreenOffset);
        }

        public override void draw()
        {
            // Draw background
            _game.menuBackgroundRenderer.draw();

            // Draw logo
            _spriteBatch.Draw(_logo, new Vector2((int)(_game.GraphicsDevice.Viewport.Width - _logo.Width), 0), _logo.Bounds, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            // Draw container
            _container.draw();

            // Draw buttons
            for (int i = 0; i < _savedGameButtons.Count; i++)
                _savedGameButtons[i].draw();
            for (int i = 0; i < _deleteGameButtons.Count; i++)
                _deleteGameButtons[i].draw();

            // Draw cancel button
            _cancelButton.draw();

            // Draw confirmation window if it exists
            if (_confirmationOverlay != null)
                _confirmationOverlay.draw();

            base.draw();
        }
    }
}
