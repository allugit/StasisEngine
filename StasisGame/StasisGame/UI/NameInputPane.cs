using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class NameInputPane : Pane
    {
        private SpriteFont _font;
        private List<string> _letters;
        private int _selectedIndex = 0;
        private int _letterSpacing = 48;
        private Texture2D _lineIndicator;
        private StringBuilder _sb;

        public NameInputPane(SpriteFont font, SpriteBatch spriteBatch, UIComponentAlignment alignment, int x, int y)
            : base(spriteBatch, alignment, x, y, 648, 320)
        {
            _font = font;
            _lineIndicator = ResourceManager.getTexture("line_indicator");
            _letters = new List<string>(new string[]
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", " ", "-", "_", ".", ","
            });
            _sb = new StringBuilder();
        }

        private bool isKeyPressed(Keys key)
        {
            return InputSystem.newKeyState.IsKeyDown(key) && InputSystem.oldKeyState.IsKeyUp(key);
        }

        public override void UIUpdate()
        {
            bool shift = InputSystem.newKeyState.IsKeyDown(Keys.LeftShift);

            if (isKeyPressed(Keys.A))
                _sb.Append(shift ? "A" : "a");
            if (isKeyPressed(Keys.B))
                _sb.Append(shift ? "B" : "b");
            if (isKeyPressed(Keys.C))
                _sb.Append(shift ? "C" : "c");
            if (isKeyPressed(Keys.D))
                _sb.Append(shift ? "D" : "d");
            if (isKeyPressed(Keys.E))
                _sb.Append(shift ? "E" : "e");
            if (isKeyPressed(Keys.F))
                _sb.Append(shift ? "F" : "f");
            if (isKeyPressed(Keys.G))
                _sb.Append(shift ? "G" : "g");
            if (isKeyPressed(Keys.H))
                _sb.Append(shift ? "H" : "h");
            if (isKeyPressed(Keys.I))
                _sb.Append(shift ? "I" : "i");
            if (isKeyPressed(Keys.J))
                _sb.Append(shift ? "J" : "j");
            if (isKeyPressed(Keys.K))
                _sb.Append(shift ? "K" : "k");
            if (isKeyPressed(Keys.L))
                _sb.Append(shift ? "L" : "l");
            if (isKeyPressed(Keys.M))
                _sb.Append(shift ? "M" : "m");
            if (isKeyPressed(Keys.N))
                _sb.Append(shift ? "N" : "n");
            if (isKeyPressed(Keys.O))
                _sb.Append(shift ? "O" : "o");
            if (isKeyPressed(Keys.P))
                _sb.Append(shift ? "P" : "p");
            if (isKeyPressed(Keys.Q))
                _sb.Append(shift ? "Q" : "q");
            if (isKeyPressed(Keys.R))
                _sb.Append(shift ? "R" : "r");
            if (isKeyPressed(Keys.S))
                _sb.Append(shift ? "S" : "s");
            if (isKeyPressed(Keys.T))
                _sb.Append(shift ? "T" : "t");
            if (isKeyPressed(Keys.U))
                _sb.Append(shift ? "U" : "u");
            if (isKeyPressed(Keys.V))
                _sb.Append(shift ? "V" : "v");
            if (isKeyPressed(Keys.W))
                _sb.Append(shift ? "W" : "w");
            if (isKeyPressed(Keys.X))
                _sb.Append(shift ? "X" : "x");
            if (isKeyPressed(Keys.Y))
                _sb.Append(shift ? "Y" : "y");
            if (isKeyPressed(Keys.Z))
                _sb.Append(shift ? "Z" : "z");
            if (isKeyPressed(Keys.D0))
                _sb.Append("0");
            if (isKeyPressed(Keys.D1))
                _sb.Append("1");
            if (isKeyPressed(Keys.D2))
                _sb.Append("2");
            if (isKeyPressed(Keys.D3))
                _sb.Append("3");
            if (isKeyPressed(Keys.D4))
                _sb.Append("4");
            if (isKeyPressed(Keys.D5))
                _sb.Append("5");
            if (isKeyPressed(Keys.D6))
                _sb.Append("6");
            if (isKeyPressed(Keys.D7))
                _sb.Append("7");
            if (isKeyPressed(Keys.D8))
                _sb.Append("8");
            if (isKeyPressed(Keys.D9))
                _sb.Append("9");
            if (isKeyPressed(Keys.Space))
                _sb.Append(" ");
            if (isKeyPressed(Keys.OemMinus))
                _sb.Append(shift ? "_" : "-");
            if (isKeyPressed(Keys.OemPeriod))
                _sb.Append(".");
            if (isKeyPressed(Keys.OemComma))
                _sb.Append(",");

            base.UIUpdate();

            Console.WriteLine("Name: {0}", _sb.ToString());
        }

        public override void UIDraw()
        {
            int gridWidth = (int)Math.Floor((float)_width / (float)_letterSpacing);

            // Draw pane
            base.UIDraw();

            // Draw letters
            for (int i = 0; i < _letters.Count; i++)
            {
                int x = i % gridWidth;
                int y = (int)Math.Floor((float)i / (float)gridWidth);
                Vector2 letterPosition = new Vector2(_destRect.X, _destRect.Y) + new Vector2(x, y) * _letterSpacing + new Vector2(24, 24);

                if (InputSystem.usingGamepad)
                {
                    if (i == _selectedIndex)
                    {
                        Vector2 indicatorPosition = new Vector2(_destRect.X, _destRect.Y) + new Vector2(x, y) * _letterSpacing + new Vector2(32, 64);
                        _spriteBatch.Draw(_lineIndicator, indicatorPosition, _lineIndicator.Bounds, Color.White, 0f, new Vector2(_lineIndicator.Width, 0) / 2f, 1f, SpriteEffects.None, 0f);
                    }
                }

                _spriteBatch.DrawString(_font, _letters[i], letterPosition + new Vector2(-2, -2), Color.Black);
                _spriteBatch.DrawString(_font, _letters[i], letterPosition + new Vector2(2, -2), Color.Black);
                _spriteBatch.DrawString(_font, _letters[i], letterPosition + new Vector2(2, 2), Color.Black);
                _spriteBatch.DrawString(_font, _letters[i], letterPosition + new Vector2(-2, 2), Color.Black);
                _spriteBatch.DrawString(_font, _letters[i], letterPosition, Color.White);
            }
        }
    }
}
