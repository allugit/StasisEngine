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
    public class NameInputPane : BluePane
    {
        private SpriteFont _font;
        private int _letterSpacing = 48;
        private Texture2D _lineIndicator;
        private StringBuilder _sb;
        private int _maxLetters;
        private List<TextButton> _letterButtons;
        private TextButton _selectedButton;
        private MouseState _newMouseState;
        private MouseState _oldMouseState;
        private KeyboardState _newKeyState;
        private KeyboardState _oldKeyState;

        public string name { get { return _sb.ToString(); } }
        public List<TextButton> letterButtons { get { return _letterButtons; } }

        public NameInputPane(Screen screen, SpriteFont font, UIAlignment alignment, int x, int y, int width, int height, int maxLetters)
            : base(screen, alignment, x, y, width, height)
        {
            _font = font;
            _maxLetters = maxLetters;
            _lineIndicator = ResourceManager.getTexture("line_indicator");
            _sb = new StringBuilder();
            _letterButtons = new List<TextButton>();

            string[] letters = new string[]
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", " ", "-", "_", ".", ","
            };
            int gridCellWidth = (int)Math.Floor((float)_width / (float)_letterSpacing);
            int calculatedX = base.x;
            int calculatedY = base.y;
            Point letterOffset = new Point(32, 24);

            for (int i = 0; i < letters.Length; i++)
            {
                string letter = letters[i];
                int letterCellX = i % gridCellWidth;
                int letterCellY = (int)Math.Floor((float)i / (float)gridCellWidth);

                _letterButtons.Add(new TextButton(
                    _screen,
                    _font,
                    _alignment,
                    letterOffset.X + x + letterCellX * _letterSpacing,
                    letterOffset.Y + y + letterCellY * _letterSpacing,
                    16,
                    TextAlignment.Center,
                    letter,
                    1,
                    Color.White,
                    new Color(0.8f, 0.8f, 0.8f),
                    () => { addLetter(letter); }));
            }
        }

        private bool isKeyPressed(Keys key)
        {
            return _newKeyState.IsKeyDown(key) && _oldKeyState.IsKeyUp(key);
        }

        private void addLetter(string letter)
        {
            if (_sb.Length < _maxLetters)
                _sb.Append(letter);
        }

        private void removeLetter()
        {
            if (_sb.Length > 0)
                _sb.Remove(_sb.Length - 1, 1);
        }

        public void reset()
        {
            _sb.Clear();
        }

        public override void update()
        {
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;
            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();

            bool shift = _newKeyState.IsKeyDown(Keys.LeftShift);
            Vector2 mouse = new Vector2(_newMouseState.X, _newMouseState.Y);
            bool mouseOverTextButton = false;

            // Handle keyboard input
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

            // Handle mouse input
            foreach (TextButton letterButton in _letterButtons)
            {
                if (letterButton.hitTest(mouse))
                {
                    if (_selectedButton != letterButton)
                    {
                        if (_selectedButton != null)
                            _selectedButton.mouseOut();

                        letterButton.mouseOver();
                    }

                    mouseOverTextButton = true;
                    _selectedButton = letterButton;

                    if (_newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                        letterButton.activate();

                    break;
                }
            }
            if (!mouseOverTextButton)
            {
                if (_selectedButton != null)
                    _selectedButton.mouseOut();

                _selectedButton = null;
            }

            base.update();
        }

        public override void draw()
        {
            int gridWidth = (int)Math.Floor((float)_width / (float)_letterSpacing);

            // Draw pane
            base.draw();

            // Draw letters
            for (int i = 0; i < _letterButtons.Count; i++)
            {
                _letterButtons[i].draw();
            }

            // Draw line indicator
            if (_selectedButton != null)
            {
                _spriteBatch.Draw(_lineIndicator, new Vector2(_selectedButton.x, _selectedButton.y), _lineIndicator.Bounds, Color.White, 0f, new Vector2(19, -32), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
