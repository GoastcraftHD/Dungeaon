using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon.States
{
    class StartState : State
    {
        private List<Component> components;
        private Player player;
        private Rectangle door = new Rectangle(1250, 0, 250, 180);
        private TextBox activeTextBox;
        private MainGameState mainGameState;

        public StartState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            player = new Player(game.player, new Vector2(350, 700), game, graphicsDeviceManager, false);
            mainGameState = new MainGameState(game, graphicsDeviceManager, content, previousState);

            components = new List<Component>()
            {
                player
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            spriteBatch.Draw(game.graveyard, new Rectangle(0, 0, 1920, 1080), new Rectangle(0, 0, game.graveyard.Width, game.graveyard.Height), Color.White);

            spriteBatch.Draw(game.necromancer, new Vector2(120, 580), null, Color.White, 0f, Vector2.Zero, 3.8f, SpriteEffects.None, 0f);

            if (game.options.debugMode)
            {
                spriteBatch.Draw(game.whiteTexture, door, Color.White);
                spriteBatch.DrawString(game.font, "X: " + Mouse.GetState().X + " Y: " + Mouse.GetState().Y, new Vector2(0, 0), Color.Black, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);
            }

            foreach (Component component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (activeTextBox == null)
            {
                List<string> dialog = new List<string>() { "Test1", "Test2", "Test3" };
                activeTextBox = new TextBox(game, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - game.textBoxSprite.Width * 10 / 2, graphicsDeviceManager.PreferredBackBufferHeight - game.textBoxSprite.Height * 10), dialog, game.necromancerHead);
                components.Add(activeTextBox);
            }

            if (activeTextBox.finished)
                components.Remove(activeTextBox);

            foreach (Component component in components)
            {
                component.Update(gameTime);
            }

            if (player.hitBox.Intersects(door))
                game.ChangeState(mainGameState);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ChangeState(previousState);
            }
        }
    }
}
