using Dungeaon.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon
{
    class Player : Component
    {
        public static int player_Health = 100;
        public static int player_maxHealth = 100;
        public static int playerHealthBarWidth = 286;
        public Vector2 position;
        private Texture2D texture;
        private SpriteEffects direction = SpriteEffects.None;
        private Game1 game;

        public Rectangle hitBox => new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * MainGameState.roomScale), (int)(texture.Height * MainGameState.roomScale));

        public Player(Texture2D playerTexture, Vector2 spawnPosition, Game1 game)
        {
            this.texture = playerTexture;
            this.position = spawnPosition;
            this.game = game;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, MainGameState.roomScale, direction, 0);

            if (game.options.debugMode)
                spriteBatch.Draw(game.whiteTexture, position, hitBox, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (position.Y > MainGameState.roomRectangle.Y)
                    position.Y -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                direction = SpriteEffects.FlipHorizontally;
                if (position.X > MainGameState.roomRectangle.X)
                    position.X -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (position.Y < MainGameState.roomRectangle.Height + MainGameState.roomRectangle.Y - texture.Height * MainGameState.roomScale)
                    position.Y += 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                direction = SpriteEffects.None;
                if (position.X < MainGameState.roomRectangle.Width + MainGameState.roomRectangle.X - texture.Width * MainGameState.roomScale)
                    position.X += 2;
            }
        }
    }
}
