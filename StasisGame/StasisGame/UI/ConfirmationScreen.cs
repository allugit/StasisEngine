using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisCore;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class ConfirmationScreen : Screen
    {
        private SpriteFont _font;
        private Label _label;
        private Pane _pane;
        private Overlay _overlay;
        private TextureButton _cancelButton;
        private TextureButton _okayButton;
        private Action _onCancel;
        private Action _onOkay;

        public ConfirmationScreen(ScreenSystem screenSystem, SpriteFont font, string text, Action onCancel, Action onOkay)
            : base(screenSystem, ScreenType.Confirmation)
        {
            _spriteBatch = screenSystem.spriteBatch;
            _font = font;
            _onCancel = onCancel;
            _onOkay = onOkay;

            Vector2 titleSize = _font.MeasureString(text);
            int padding = 16;

            _overlay = new Overlay(this, Color.Black);

            _pane = new StonePane(
                this,
                UIAlignment.MiddleCenter,
                0,
                -padding,
                (int)titleSize.X + padding * 2,
                100);


            _label = new Label(
                this,
                _font,
                UIAlignment.MiddleCenter,
                0,
                -50,
                TextAlignment.Center,
                text,
                2);

            _cancelButton = new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                (int)(titleSize.X / 2f) - 285,
                15,
                ResourceManager.getTexture("cancel_button_over"),
                ResourceManager.getTexture("cancel_button"),
                new Rectangle(0, 0, 152, 33),
                () => { _onCancel(); });

            _okayButton = new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                (int)(titleSize.X / 2f) - 125,
                15,
                ResourceManager.getTexture("okay_button_over"),
                ResourceManager.getTexture("okay_button"),
                new Rectangle(0, 0, 152, 33),
                () => { _onOkay(); });
        }

        public override void applyIntroTransitions()
        {
            int fromX = _spriteBatch.GraphicsDevice.Viewport.Width;

            _overlay.alpha = 0f;
            _cancelButton.translationX = fromX;
            _okayButton.translationX = fromX;
            _transitions.Clear();
            _transitions.Add(new AlphaFadeTransition(_overlay, 0f, 0.6f));
            _transitions.Add(new ScaleTransition(_pane, 0f, 1f, false, 0.1f));
            _transitions.Add(new TranslateTransition(_cancelButton, fromX, 0, 0, 0, false));
            _transitions.Add(new TranslateTransition(_okayButton, fromX, 0, 0, 0, false));
            base.applyIntroTransitions();
        }

        public override void applyOutroTransitions(Action onFinished = null)
        {
            int toX = _spriteBatch.GraphicsDevice.Viewport.Width;
            _transitions.Add(new AlphaFadeTransition(_overlay, 0.6f, 0f));
            _transitions.Add(new ScaleTransition(_pane, 1f, 0f, false, 0.1f));
            _transitions.Add(new TranslateTransition(_cancelButton, 0, 0, toX, 0, false));
            _transitions.Add(new TranslateTransition(_okayButton, 0, 0, toX, 0, false));
            base.applyOutroTransitions(onFinished);
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

        public override void update()
        {
            Vector2 mouse = new Vector2(_newMouseState.X, _newMouseState.Y);

            base.update();
            hitTestTextureButton(_cancelButton, mouse);
            hitTestTextureButton(_okayButton, mouse);
        }

        public override void draw()
        {
            base.draw();

            // Draw components
            _overlay.draw();
            _pane.draw();
            _label.draw();
            _cancelButton.draw();
            _okayButton.draw();
        }
    }
}
