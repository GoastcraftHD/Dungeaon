using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static SpriteFont font;

        private Texture2D character;
        private Texture2D whiteRectangle;

        private List<Component> gameComponents;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

        private TextBox textBox = new TextBox();

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            whiteRectangle = new Texture2D(GraphicsDevice, 800, 150);
            Color[] color = new Color[whiteRectangle.Height * whiteRectangle.Width];

            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.White;
            }

            whiteRectangle.SetData(color);
            textBox.textBoxbackground = whiteRectangle; //Content.Load<Texture2D>("TextBack");

            Button button = new Button(whiteRectangle, font)
            {
                position = new Vector2(100, 100),
                text = "Exit Game",
                textColor = Color.Black
            };

            button.click += button_Click;

            gameComponents = new List<Component>()
            {
                button
            };



            character = Content.Load<Texture2D>("Bub");

            
        }

        private void button_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private Vector2 playerPos = new Vector2(100, 100);
        private float fps = 0f;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Component component in gameComponents)
            {
                component.Update(gameTime);
            }

            fps = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        private bool a = true;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float deltTime = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null,
                null);

            foreach (Component component in gameComponents)
            {
                component.Draw(gameTime, _spriteBatch);
            }

            // _spriteBatch.Draw(character, playerPos, Color.White);

            _spriteBatch.DrawString(font, fps.ToString(), new Vector2(1800, 1), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

            List<String> dialog = new List<String>()
            {
                "asdadasdasdagsdafgafasdcasgagdsgyfsdgysadsg\ndfgdsgfsgdfgdfgsgfdgdfgdfgfdgdfgdfgfdgfdgf",
                "plp llnkokovojicvcdfjidffidjfi iijfiefiefie\nasdaasdadssdaf?"
            };

            if (a && textBox.DrawTextBox(_spriteBatch, deltTime, character, "Name", dialog))
            {
                textBox.Reset();
                a = false;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
