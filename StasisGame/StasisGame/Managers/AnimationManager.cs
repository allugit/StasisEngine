using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.Managers
{
    public class AnimationManager
    {
        private LoderGame _game;
        private ContentManager _content;
        private Dictionary<string, Dictionary<string, List<Texture2D>>> _animations;
        private Dictionary<string, Dictionary<string, List<Vector2>>> _offsets;
        private Texture2D _default;

        public AnimationManager(LoderGame game)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _default = _content.Load<Texture2D>("animations/no_texture");
            _animations = new Dictionary<string, Dictionary<string, List<Texture2D>>>();
            _offsets = new Dictionary<string, Dictionary<string, List<Vector2>>>();
            loadAnimation("main_character", "walk_left", "0", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "walk_left", "1", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "walk_left", "2", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "walk_left", "3", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "idle", "0", new Vector2(0.5f, 0.65f));
        }

        public void loadAnimation(string character, string animation, string assetName, Vector2 offset)
        {
            Texture2D texture = _content.Load<Texture2D>(string.Format("animations/{0}/{1}/{2}", character, animation, assetName));
            if (!_animations.ContainsKey(character))
            {
                _animations.Add(character, new Dictionary<string, List<Texture2D>>());
                _offsets.Add(character, new Dictionary<string, List<Vector2>>());
            }
            if (!_animations[character].ContainsKey(animation))
            {
                _animations[character].Add(animation, new List<Texture2D>());
                _offsets[character].Add(animation, new List<Vector2>());
            }
            _animations[character][animation].Add(texture);
            _offsets[character][animation].Add(new Vector2(offset.X * texture.Width, offset.Y * texture.Height));
        }

        public Texture2D getTexture(string character, string animation, int tick, out Vector2 offset)
        {
            Dictionary<string, List<Texture2D>> characterAnimations;
            List<Texture2D> frames = null;
            int index = -1;

            offset = Vector2.Zero;

            if (_animations.TryGetValue(character, out characterAnimations))
            {
                if (characterAnimations.TryGetValue(animation, out frames))
                {
                    frames = _animations[character][animation];
                    index = tick % frames.Count;
                    offset = _offsets[character][animation][index];
                }
            }
            if (frames == null)
            {
                offset = new Vector2(_default.Width * 0.5f, 0.6f);
                return _default;
            }
            else
            {
                return frames[index];
            }
        }
    }
}
