using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace StasisGame
{
    public enum GameState
    {
        Intro,
        MainMenu,
        Options,
        CreatePlayer,
        WorldMap,
        Level
    };

    public class LoderGame : Game
    {
        private string[] _args;
        private string _argsDebug = "args:";
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _arial;
        private GameState _gameState;
        private Level _level;

        public LoderGame(string[] args)
        {
            _args = args;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _gameState = GameState.Intro;
        }

        protected override void Initialize()
        {
            base.Initialize();

            handleArgs();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _arial = Content.Load<SpriteFont>("arial");
        }

        protected override void UnloadContent()
        {
        }

        private void handleArgs()
        {
            foreach (string arg in _args)
                _argsDebug += arg;

            if (_args.Length > 0)
            {
                string flag = _args[0];
                switch (flag)
                {
                    case "-l":
                        quickLoadLevel(_args[1]);
                        break;
                }
            }
        }

        private void quickLoadLevel(string filePath)
        {
            // Load player data
            // ...

            // Character flags
            // ...

            // Load level
            _gameState = GameState.Level;
            _level = new Level(filePath);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_arial, _argsDebug, new Vector2(16, 16), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
