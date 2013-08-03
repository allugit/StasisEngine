using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisGame.UI
{
    public class ConfirmationOverlay
    {
        private Screen _screen;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private Color _backgroundColor;
        private Label _label;
        private Pane _pane;
        private TextureButton _cancelButton;
        private TextureButton _okayButton;
        private Texture2D _pixel;
        private Action _onCancel;
        private Action _onOkay;
        private MouseState _newMouseState;
        private MouseState _oldMouseState;
        private KeyboardState _newKeyState;
        private KeyboardState _oldKeyState;
        private bool _firstUpdate = true;

        public ConfirmationOverlay(Screen screen, SpriteBatch spriteBatch, SpriteFont font, string text, Action onCancel, Action onOkay)
        {
            _screen = screen;
            _spriteBatch = spriteBatch;
            _font = font;
            _onCancel = onCancel;
            _onOkay = onOkay;
            _backgroundColor = Color.Black * 0.6f;
            _pixel = ResourceManager.getTexture("pixel");

            Vector2 titleSize = _font.MeasureString(text);
            int padding = 16;

            _pane = new StonePane(
                _screen,
                UIAlignment.MiddleCenter,
                -(int)(titleSize.X / 2f) - padding,
                -50 + -padding,
                (int)titleSize.X + padding * 2,
                100);


            _label = new Label(
                _spriteBatch,
                _font,
                UIAlignment.MiddleCenter,
                0,
                -50,
                TextAlignment.Center,
                text,
                2);

            _cancelButton = new TextureButton(
                _screen,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                (int)(titleSize.X / 2f) - 285,
                15,
                ResourceManager.getTexture("cancel_button_over"),
                ResourceManager.getTexture("cancel_button"),
                new Rectangle(0, 0, 152, 33),
                () => { _onCancel(); });

            _okayButton = new TextureButton(
                _screen,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                (int)(titleSize.X / 2f) - 125,
                15,
                ResourceManager.getTexture("okay_button_over"),
                ResourceManager.getTexture("okay_button"),
                new Rectangle(0, 0, 152, 33),
                () => { _onOkay(); });
        }

        private void hitTestTextureButton(TextureButton button, Vector2 point)
        {
            if (button.hitTest(point))
            {
                button.mouseOver();

                if (_newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                    button.activate();
            }
            else
            {
                if (button.selected)
                    button.mouseOut();
            }
        }

        public void update()
        {
            if (!_firstUpdate)
            {
                _oldMouseState = _newMouseState;
                _oldKeyState = _newKeyState;
                _newMouseState = Mouse.GetState();
                _newKeyState = Keyboard.GetState();

                Vector2 mouse = new Vector2(_newMouseState.X, _newMouseState.Y);

                hitTestTextureButton(_cancelButton, mouse);
                hitTestTextureButton(_okayButton, mouse);
            }
            _firstUpdate = false;
        }

        public void draw()
        {
            // Draw overlay
            _spriteBatch.Draw(_pixel, _spriteBatch.GraphicsDevice.Viewport.Bounds, _backgroundColor);

            // Draw components
            _pane.draw();
            _label.draw();
            _cancelButton.draw();
            _okayButton.draw();
        }
    }
}
