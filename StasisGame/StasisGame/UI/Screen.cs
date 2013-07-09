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
    public enum ScreenType
    {
        MainMenu,
        OptionsMenu,
        WorldMap,
        Level,
        LoadGameMenu,
        PlayerCreation
    };

    abstract public class Screen
    {
        protected SpriteBatch _spriteBatch;
        protected GamePadState _newGamepadState;
        protected GamePadState _oldGamepadState;
        protected KeyboardState _newKeyState;
        protected KeyboardState _oldKeyState;
        protected MouseState _newMouseState;
        protected MouseState _oldMouseState;
        protected ScreenType _screenType;

        public ScreenType screenType { get { return _screenType; } set { _screenType = value; } }
        public SpriteBatch spriteBatch { get { return _spriteBatch; } }
        public KeyboardState newKeyState { get { return _newKeyState; } }
        public KeyboardState oldKeyState { get { return _oldKeyState; } }
        public MouseState newMouseState { get { return _newMouseState; } }
        public MouseState oldMouseState { get { return _oldMouseState; } }

        public Screen(SpriteBatch spriteBatch, ScreenType screenType)
        {
            _spriteBatch = spriteBatch;
            _screenType = screenType;
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

        virtual public void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newGamepadState = GamePad.GetState(PlayerIndex.One);
            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();
        }

        virtual public void draw()
        {
        }

    }
}
