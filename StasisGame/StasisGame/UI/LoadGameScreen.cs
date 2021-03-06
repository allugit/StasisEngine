﻿using System;
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
        private ContentManager _content;
        private SpriteFont _savedGameFont;
        private SpriteFont _confirmationFont;
        private BluePane _container;
        private List<LabelTextureButton> _savedGameButtons;
        private List<TextureButton> _deleteGameButtons;
        private TextureButton _cancelButton;
        private ConfirmationScreen _confirmationScreen;

        public LoadGameScreen(LoderGame game)
            : base(game.screenSystem, ScreenType.LoadGameMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _savedGameFont = _content.Load<SpriteFont>("load_game_menu/saved_game_font");
            _confirmationFont = _content.Load<SpriteFont>("shared_ui/confirmation_font");
            _savedGameButtons = new List<LabelTextureButton>();
            _deleteGameButtons = new List<TextureButton>();

            _container = new BluePane(
                this,
                UIAlignment.MiddleCenter,
                0,
                0,
                545,
                400);

            _cancelButton = new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                135,
                180,
                _content.Load<Texture2D>("shared_ui/cancel_button_over"),
                _content.Load<Texture2D>("shared_ui/cancel_button"),
                new Rectangle(0, 0, 152, 33),
                () =>
                {
                    _game.closeLoadGameMenu();
                    _game.openMainMenu();
                });
        }

        ~LoadGameScreen()
        {
            _content.Unload();
        }

        public void loadPlayerSaves()
        {
            List<XElement> playerSaves = DataManager.loadPlayerSaves();
            Vector2 initialPosition = new Vector2(-262, -190);

            _savedGameButtons.Clear();
            _deleteGameButtons.Clear();

            foreach (XElement playerSave in playerSaves)
            {
                int slot = int.Parse(playerSave.Attribute("slot").Value);
                string text = slot.ToString() + " - " + playerSave.Attribute("name").Value;

                _deleteGameButtons.Add(new TextureButton(
                    this,
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
                    this,
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
                        _screenSystem.addTransition(new ScreenFadeOutTransition(this, Color.Black, true, 0.05f, null, () =>
                        {
                            _game.startPersistentSystems();
                            _game.playerSystem.createPlayer();
                            DataManager.loadPlayerData(slot);
                            _game.openWorldMap();
                        }));
                    }));
            }
        }

        public override void applyIntroTransitions()
        {
            for (int i = 0; i < _savedGameButtons.Count; i++)
            {
                _savedGameButtons[i].translationX = _spriteBatch.GraphicsDevice.Viewport.Width;
                _deleteGameButtons[i].translationX = _spriteBatch.GraphicsDevice.Viewport.Width;
            }
            _cancelButton.translationX = _spriteBatch.GraphicsDevice.Viewport.Width;
            _transitions.Clear();
            _transitions.Add(new ScaleTransition(_container, 0f, 1f));
            _transitions.Add(new TranslateTransition(_cancelButton, _spriteBatch.GraphicsDevice.Viewport.Width, 0, 0, 0, false));
            for (int i = 0; i < _savedGameButtons.Count; i++)
            {
                _transitions.Add(new TranslateTransition(_savedGameButtons[i], _spriteBatch.GraphicsDevice.Viewport.Width, 0, 0, 0, true, 0.1f));
                _transitions.Add(new TranslateTransition(_deleteGameButtons[i], _spriteBatch.GraphicsDevice.Viewport.Width, 0, 0, 0, false, 0.1f));
            }
            base.applyIntroTransitions();
        }

        public override void applyOutroTransitions(Action onFinished = null)
        {
            _transitions.Clear();
            for (int i = 0; i < _savedGameButtons.Count; i++)
            {
                _transitions.Add(new TranslateTransition(_savedGameButtons[i], 0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, 0, true, 0.2f));
                _transitions.Add(new TranslateTransition(_deleteGameButtons[i], 0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, 0, false, 0.2f));
            }
            _transitions.Add(new ScaleTransition(_container, 1f, 0f));
            _transitions.Add(new TranslateTransition(_cancelButton, 0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, 0, false, 0.2f));
            base.applyOutroTransitions(onFinished);
        }

        private void openConfirmation(string text, Action onCancel, Action onOkay)
        {
            _confirmationScreen = new ConfirmationScreen(
                _screenSystem,
                _confirmationFont,
                text,
                onCancel,
                onOkay);
            _confirmationScreen.applyIntroTransitions();
            _screenSystem.addScreen(_confirmationScreen);
        }

        private void closeConfirmation()
        {
            _confirmationScreen.applyOutroTransitions(() =>
            {
                _screenSystem.removeScreen(_confirmationScreen);
                _confirmationScreen = null;
            });
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
            if (_confirmationScreen == null)
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
        }

        public override void draw()
        {
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
            if (_confirmationScreen != null)
                _confirmationScreen.draw();

            base.draw();
        }
    }
}
