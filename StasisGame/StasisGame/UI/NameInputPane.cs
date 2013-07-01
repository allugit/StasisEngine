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
        private int _maxLetters;
        private bool _firstUpdate = true;

        public string name { get { return _sb.ToString(); } }

        public NameInputPane(SpriteBatch spriteBatch, SpriteFont font, UIComponentAlignment alignment, int x, int y, int maxLetters)
            : base(spriteBatch, alignment, x, y, 648, 320)
        {
            _font = font;
            _maxLetters = maxLetters;
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

        private void addLetter(string letter)
        {
            if (_sb.Length < _maxLetters)
                _sb.Append(letter);

            _selectedIndex = _letters.IndexOf(letter);
        }

        private void removeLetter()
        {
            if (_sb.Length > 0)
                _sb.Remove(_sb.Length - 1, 1);
        }

        private void selectIndexWithMouse(Vector2 position)
        {
            int gridWidth = (int)Math.Floor((float)_width / (float)_letterSpacing);

            for (int i = 0; i < _letters.Count; i++)
            {
                int x = i % gridWidth;
                int y = (int)Math.Floor((float)i / (float)gridWidth);
                Vector2 lowerLetterPosition = new Vector2(_destRect.X, _destRect.Y) + new Vector2(x, y) * _letterSpacing + new Vector2(12, 12);
                Vector2 upperLetterPosition = lowerLetterPosition + new Vector2(_letterSpacing, _letterSpacing);
                Vector2 d1, d2;

                d1 = position - lowerLetterPosition;
                d2 = upperLetterPosition - position;

                if (d1.X < 0 || d1.Y < 0)
                {
                    continue;
                }
                else if (d2.X < 0 || d2.Y < 0)
                {
                    continue;
                }
                else
                {
                    _selectedIndex = i;
                }
            }
        }

        public override void UIUpdate()
        {
            if (!_firstUpdate)
            {
                bool shift = InputSystem.newKeyState.IsKeyDown(Keys.LeftShift);

                // Mouse input
                if (InputSystem.oldMouseState.X - InputSystem.newMouseState.X != 0 ||
                    InputSystem.oldMouseState.Y - InputSystem.newMouseState.Y != 0)
                {
                    selectIndexWithMouse(new Vector2(InputSystem.newMouseState.X, InputSystem.newMouseState.Y));
                }
                if (InputSystem.newMouseState.LeftButton == ButtonState.Pressed && InputSystem.oldMouseState.LeftButton == ButtonState.Released)
                {
                    addLetter(_letters[_selectedIndex]);
                }

                // Keyboard input
                if (isKeyPressed(Keys.A))
                    addLetter(shift ? "A" : "a");
                if (isKeyPressed(Keys.B))
                    addLetter(shift ? "B" : "b");
                if (isKeyPressed(Keys.C))
                    addLetter(shift ? "C" : "c");
                if (isKeyPressed(Keys.D))
                    addLetter(shift ? "D" : "d");
                if (isKeyPressed(Keys.E))
                    addLetter(shift ? "E" : "e");
                if (isKeyPressed(Keys.F))
                    addLetter(shift ? "F" : "f");
                if (isKeyPressed(Keys.G))
                    addLetter(shift ? "G" : "g");
                if (isKeyPressed(Keys.H))
                    addLetter(shift ? "H" : "h");
                if (isKeyPressed(Keys.I))
                    addLetter(shift ? "I" : "i");
                if (isKeyPressed(Keys.J))
                    addLetter(shift ? "J" : "j");
                if (isKeyPressed(Keys.K))
                    addLetter(shift ? "K" : "k");
                if (isKeyPressed(Keys.L))
                    addLetter(shift ? "L" : "l");
                if (isKeyPressed(Keys.M))
                    addLetter(shift ? "M" : "m");
                if (isKeyPressed(Keys.N))
                    addLetter(shift ? "N" : "n");
                if (isKeyPressed(Keys.O))
                    addLetter(shift ? "O" : "o");
                if (isKeyPressed(Keys.P))
                    addLetter(shift ? "P" : "p");
                if (isKeyPressed(Keys.Q))
                    addLetter(shift ? "Q" : "q");
                if (isKeyPressed(Keys.R))
                    addLetter(shift ? "R" : "r");
                if (isKeyPressed(Keys.S))
                    addLetter(shift ? "S" : "s");
                if (isKeyPressed(Keys.T))
                    addLetter(shift ? "T" : "t");
                if (isKeyPressed(Keys.U))
                    addLetter(shift ? "U" : "u");
                if (isKeyPressed(Keys.V))
                    addLetter(shift ? "V" : "v");
                if (isKeyPressed(Keys.W))
                    addLetter(shift ? "W" : "w");
                if (isKeyPressed(Keys.X))
                    addLetter(shift ? "X" : "x");
                if (isKeyPressed(Keys.Y))
                    addLetter(shift ? "Y" : "y");
                if (isKeyPressed(Keys.Z))
                    addLetter(shift ? "Z" : "z");
                if (isKeyPressed(Keys.D0))
                    addLetter("0");
                if (isKeyPressed(Keys.D1))
                    addLetter("1");
                if (isKeyPressed(Keys.D2))
                    addLetter("2");
                if (isKeyPressed(Keys.D3))
                    addLetter("3");
                if (isKeyPressed(Keys.D4))
                    addLetter("4");
                if (isKeyPressed(Keys.D5))
                    addLetter("5");
                if (isKeyPressed(Keys.D6))
                    addLetter("6");
                if (isKeyPressed(Keys.D7))
                    addLetter("7");
                if (isKeyPressed(Keys.D8))
                    addLetter("8");
                if (isKeyPressed(Keys.D9))
                    addLetter("9");
                if (isKeyPressed(Keys.Space))
                    addLetter(" ");
                if (isKeyPressed(Keys.OemMinus))
                    addLetter(shift ? "_" : "-");
                if (isKeyPressed(Keys.OemPeriod))
                    addLetter(".");
                if (isKeyPressed(Keys.OemComma))
                    addLetter(",");
                if (isKeyPressed(Keys.Back))
                    removeLetter();

                base.UIUpdate();
            }

            _firstUpdate = false;
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

                if (i == _selectedIndex)
                {
                    Vector2 indicatorPosition = new Vector2(_destRect.X, _destRect.Y) + new Vector2(x, y) * _letterSpacing + new Vector2(32, 52);
                    _spriteBatch.Draw(_lineIndicator, indicatorPosition, _lineIndicator.Bounds, Color.White, 0f, new Vector2(_lineIndicator.Width, 0) / 2f, 1f, SpriteEffects.None, 0f);
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
