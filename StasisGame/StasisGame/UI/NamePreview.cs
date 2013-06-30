using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class NamePreview : IUIComponent
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private UIComponentAlignment _alignment;
        private int _x;
        private int _y;
        private string _name = "";
        private int _maxLetters;
        private int _letterSpacing = 32;
        private int _width;
        private Texture2D _letterLine;

        public string name { get { return _name; } set { _name = value; } }
        public bool selectable { get { return false; } }
        public float layerDepth { get { return 0f; } }

        public NamePreview(SpriteBatch spriteBatch, SpriteFont font, UIComponentAlignment alignment, int x, int y, int maxLetters)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _alignment = alignment;
            _x = x;
            _y = y;
            _maxLetters = maxLetters;
            _width = _maxLetters * _letterSpacing;
            _letterLine = ResourceManager.getTexture("line_indicator");
        }

        public void UIUpdate()
        {
        }

        public void UIDraw()
        {
            int realX = _x;

            switch (_alignment)
            {
                case UIComponentAlignment.TopCenter:
                    realX = _x + (_spriteBatch.GraphicsDevice.Viewport.Width / 2) - (_width / 2);
                    break;
            }

            // Draw spaces
            for (int i = 0; i < _maxLetters; i++)
            {
                _spriteBatch.Draw(_letterLine, new Vector2(realX + i * _letterSpacing, _y + _letterSpacing), _letterLine.Bounds, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            }

            // Draw letters
            for (int i = 0; i < _name.Length; i++)
            {
                Vector2 letterPosition = new Vector2(realX + (i * _letterSpacing) + (_letterSpacing / 2f) - 2, _y);
                string letter = _name[i].ToString();
                Vector2 letterSize = _font.MeasureString(letter);
                _spriteBatch.DrawString(_font, letter, letterPosition, Color.White, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
