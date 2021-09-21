﻿using System;
using Dungeaon.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeaon
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private State currentState;
        private State nextState;

        public Texture2D buttonTexture;
        public Texture2D room1;
        public Texture2D player;
        public Texture2D button;
        public SpriteFont font;

        public Options options = new Options()
        {
            backgroundColor = Color.Gray
        };

        public struct Options
        {
            public Color backgroundColor;
        }

        public void ChangeState(State state)
        {
            nextState = state;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
           // _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D buttonBack = new Texture2D(GraphicsDevice, 1, 1);
            Color[] color = new Color[buttonBack.Width * buttonBack.Height];

            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.White;
            }
            buttonBack.SetData(color);
            buttonTexture = buttonBack;
            font = Content.Load<SpriteFont>("font");
            room1 = Content.Load<Texture2D>("rooms/room1");
            player = Content.Load<Texture2D>("player");
            button = Content.Load<Texture2D>("button");

            currentState = new MainMenuState(this, _graphics, Content, null);
        }

        protected override void Update(GameTime gameTime)
        {
            if (nextState != null)
            {
                currentState = nextState;

                nextState = null;
            }

            currentState.Update(gameTime);
            currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(options.backgroundColor);
            currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
