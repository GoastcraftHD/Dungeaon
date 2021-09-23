using System;
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

        public Texture2D whiteTexture;
        public Texture2D player;
        public Texture2D button;
        public SpriteFont font;

        public Texture2D fightScreen;
        public Texture2D room1;
        public Texture2D room2;
        public Texture2D bossRoom;

        public Texture2D knightEnemy;
        public Texture2D financeEnemy;
        public Texture2D ghostEnemy;


        public Options options = new Options()
        {
            backgroundColor = Color.Gray,
            debugMode = false
        };

        public struct Options
        {
            public Color backgroundColor;
            public bool debugMode;
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
            _graphics.IsFullScreen = true;
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
            
            whiteTexture = buttonBack;
            font = Content.Load<SpriteFont>("font");
            room1 = Content.Load<Texture2D>("rooms/room1");
            room2 = Content.Load<Texture2D>("rooms/room2");
            bossRoom = Content.Load<Texture2D>("rooms/bossroom");
            player = Content.Load<Texture2D>("Player");
            button = Content.Load<Texture2D>("button");
            knightEnemy = Content.Load<Texture2D>("enemies/versuchsperson");
            financeEnemy = Content.Load<Texture2D>("enemies/Finanzmitarbeiter");
            ghostEnemy = Content.Load<Texture2D>("enemies/Geist");
            fightScreen = Content.Load<Texture2D>("Fightscreen");

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
