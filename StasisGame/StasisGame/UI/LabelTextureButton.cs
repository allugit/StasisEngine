using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class LabelTextureButton : TextureButton
    {
        private SpriteFont _font;
        private TextAlignment _textAlignment;
        private string _text;
        private int _textXOffset;
        private int _textYOffset;
        private int _outline;
        private Color _selectedColor;
        private Color _deselectedColor;

        public LabelTextureButton(
            Screen screen,
            SpriteBatch spriteBatch,
            UIAlignment alignment,
            int x,
            int y,
            Texture2D selectedTexture,
            Texture2D deselectedTexture,
            Rectangle localHitBox,
            SpriteFont font,
            TextAlignment textAlignment,
            string text,
            int textXOffset,
            int textYOffset,
            int outline,
            Color selectedColor,
            Color deselectedColor,
            Action onActivate)
            : base(screen, spriteBatch, alignment, x, y, selectedTexture, deselectedTexture, localHitBox, onActivate)
        {
            _font = font;
            _textAlignment = textAlignment;
            _text = text;
            _textXOffset = textXOffset;
            _textYOffset = textYOffset;
            _outline = outline;
            _selectedColor = selectedColor;
            _deselectedColor = deselectedColor;
        }

        public LabelTextureButton(
            Screen screen,
            SpriteBatch spriteBatch,
            UIAlignment alignment,
            int x,
            int y,
            Texture2D selectedTexture,
            Texture2D deselectedTexture,
            Rectangle localHitBox,
            SpriteFont font,
            TextAlignment textAlignment,
            string text,
            int textXOffset,
            int textYOffset,
            int outline,
            Color selectedColor,
            Color deselectedColor,
            Action onActivate,
            Action onMouseOver,
            Action onMouseOut)
            : base(screen, spriteBatch, alignment, x, y, selectedTexture, deselectedTexture, localHitBox, onActivate, onMouseOver, onMouseOut)
        {
            _font = font;
            _textAlignment = textAlignment;
            _text = text;
            _textXOffset = textXOffset;
            _textYOffset = textYOffset;
            _outline = outline;
            _selectedColor = selectedColor;
            _deselectedColor = deselectedColor;
        }

        public override void draw()
        {
            Vector2 position = new Vector2(x + _textXOffset + (int)_screen.translationX + _translationX, y + _textYOffset + (int)_screen.translationY + _translationY);
            Vector2 origin = Vector2.Zero;
            Vector2 stringSize = _font.MeasureString(_text);

            base.draw();

            if (_textAlignment == TextAlignment.Center)
                origin = new Vector2(stringSize.X / 2f, 0);
            else if (_textAlignment == TextAlignment.Right)
                origin = new Vector2(stringSize.X, 0);

            if (_outline > 0)
            {
                _spriteBatch.DrawString(_font, _text, position + new Vector2(-_outline, -_outline), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, position + new Vector2(0, -_outline), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, position + new Vector2(_outline, -_outline), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, position + new Vector2(_outline, 0), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, position + new Vector2(_outline, _outline), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, position + new Vector2(0, _outline), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, position + new Vector2(-_outline, _outline), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, position + new Vector2(-_outline, 0), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
            }
            _spriteBatch.DrawString(_font, _text, position, selected ? _selectedColor : _deselectedColor, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
