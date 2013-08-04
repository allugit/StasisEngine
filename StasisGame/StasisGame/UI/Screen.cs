using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public enum UIAlignment
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    };
    public enum TextAlignment
    {
        Left,
        Center,
        Right
    };
    public enum ScreenType
    {
        MainMenu,
        OptionsMenu,
        WorldMap,
        Level,
        LoadGameMenu,
        PlayerCreation,
        Confirmation
    };

    abstract public class Screen : ITranslatable
    {
        protected ScreenSystem _screenSystem;
        protected SpriteBatch _spriteBatch;
        protected GamePadState _newGamepadState;
        protected GamePadState _oldGamepadState;
        protected KeyboardState _newKeyState;
        protected KeyboardState _oldKeyState;
        protected MouseState _newMouseState;
        protected MouseState _oldMouseState;
        protected ScreenType _screenType;
        protected float _translationX;
        protected float _translationY;
        protected List<Transition> _transitions;
        protected List<Transition> _transitionsToRemove;
        private Action _onFinished;

        public ScreenType screenType { get { return _screenType; } set { _screenType = value; } }
        public ScreenSystem screenSystem { get { return _screenSystem; } }
        public KeyboardState newKeyState { get { return _newKeyState; } }
        public KeyboardState oldKeyState { get { return _oldKeyState; } }
        public MouseState newMouseState { get { return _newMouseState; } }
        public MouseState oldMouseState { get { return _oldMouseState; } }
        public float translationX { get { return _translationX; } set { _translationX = value; } }
        public float translationY { get { return _translationY; } set { _translationY = value; } }
        public Screen screen { get { return this; } }

        public Screen(ScreenSystem screenSystem, ScreenType screenType)
        {
            _screenSystem = screenSystem;
            _spriteBatch = screenSystem.spriteBatch;
            _screenType = screenType;
            _transitions = new List<Transition>();
            _transitionsToRemove = new List<Transition>();
        }

        public string wrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ', '\n');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }

        virtual public void applyIntroTransitions()
        {
        }

        virtual public void applyOutroTransitions(Action onFinished = null)
        {
            _onFinished = onFinished;
        }

        virtual public void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newGamepadState = GamePad.GetState(PlayerIndex.One);
            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();

            // Remove finished transitions
            for (int i = 0; i < _transitionsToRemove.Count; i++)
            {
                _transitions.Remove(_transitionsToRemove[i]);
            }
            _transitionsToRemove.Clear();

            // Attempt to call onFinished callback
            if (_transitions.Count == 0 && _onFinished != null)
            {
                _onFinished();
                _onFinished = null;
            }

            // Update transitions
            for (int i = 0; i < _transitions.Count; i++)
            {
                Transition transition = _transitions[i];

                // Break if this is a queued transition that's not ready to be processed.
                if (transition.queued && i != 0)
                {
                    break;
                }

                // Handle starting/finished transitions
                if (transition.finished)
                {
                    transition.end();
                    _transitionsToRemove.Add(transition);
                    continue;
                }
                else if (transition.starting)
                {
                    transition.begin();
                }

                // Update transitions
                if ((transition.queued && i == 0) || !transition.queued)
                {
                    transition.update();
                }
            }
        }

        virtual public void draw()
        {
        }

    }
}
