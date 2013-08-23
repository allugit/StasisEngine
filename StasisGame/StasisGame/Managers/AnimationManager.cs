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
        private Dictionary<string, Dictionary<string, float>> _fps;
        private Texture2D _default;

        public AnimationManager(LoderGame game)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _default = _content.Load<Texture2D>("animations/no_texture");
            _animations = new Dictionary<string, Dictionary<string, List<Texture2D>>>();
            _offsets = new Dictionary<string, Dictionary<string, List<Vector2>>>();
            _fps = new Dictionary<string, Dictionary<string, float>>();
            loadAnimation("main_character", "walk_left", "0", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "walk_left", "1", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "walk_left", "2", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "walk_left", "3", new Vector2(0.5f, 0.65f));
            setFPS("main_character", "walk_left", 24);
            loadAnimation("main_character", "walk_right", "0", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "walk_right", "1", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "walk_right", "2", new Vector2(0.5f, 0.65f));
            loadAnimation("main_character", "walk_right", "3", new Vector2(0.5f, 0.65f));
            setFPS("main_character", "walk_right", 24);
            loadAnimation("main_character", "idle", "0", new Vector2(0.5f, 0.65f));
            setFPS("main_character", "idle", 24);
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

        public Texture2D getTexture(string character, string animation, int index, out Vector2 offset)
        {
            Dictionary<string, List<Texture2D>> characterAnimations;
            List<Texture2D> frames = null;

            offset = Vector2.Zero;

            if (_animations.TryGetValue(character, out characterAnimations))
            {
                if (characterAnimations.TryGetValue(animation, out frames))
                {
                    frames = _animations[character][animation];
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

        public void setFPS(string character, string animation, float framesPerSecond)
        {
            if (!_fps.ContainsKey(character))
            {
                _fps.Add(character, new Dictionary<string, float>());
            }
            if (!_fps[character].ContainsKey(animation))
            {
                _fps[character].Add(animation, framesPerSecond);
            }
            else
            {
                _fps[character][animation] = framesPerSecond;
            }
        }

        public float getFPS(string character, string animation)
        {
            Dictionary<string, float> animationFps = null;
            float fps = 24f;

            if (_fps.TryGetValue(character, out animationFps))
            {
                if (animationFps.TryGetValue(animation, out fps))
                {
                    return fps;
                }
            }
            return fps;
        }

        public int getFrameCount(string character, string animation)
        {
            if (!_animations.ContainsKey(character))
            {
                return 1;
            }
            if (!_animations[character].ContainsKey(animation))
            {
                return 1;
            }
            else
            {
                return _animations[character][animation].Count;
            }
        }
    }
}
