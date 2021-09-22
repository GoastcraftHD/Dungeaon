using Dungeaon.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon
{
    class Player : Component
    {
        private Texture2D playerTexture;
        public Vector2 playerPosition;
        private SpriteEffects playerDir = SpriteEffects.None;
        private Game1 game;

        public Rectangle playerHitBox
        {
            get
            {
                return new Rectangle((int)playerPosition.X, (int)playerPosition.Y, (int)(playerTexture.Width * MainGame.roomScale), (int)(playerTexture.Height * MainGame.roomScale));
            }
        }

        public Player(Texture2D playerTexture, Vector2 spawnPosition, Game1 game)
        {
            this.playerTexture = playerTexture;
            this.playerPosition = spawnPosition;
            this.game = game;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, playerPosition, null, Color.White, 0f, Vector2.Zero, MainGame.roomScale, playerDir, 0);

            if (game.options.debugMode)
                spriteBatch.Draw(game.whiteTexture, playerPosition, playerHitBox, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (playerPosition.Y > MainGame.roomRectangle.Y)
                    playerPosition.Y -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerDir = SpriteEffects.FlipHorizontally;
                if (playerPosition.X > MainGame.roomRectangle.X)
                    playerPosition.X -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (playerPosition.Y < MainGame.roomRectangle.Height + MainGame.roomRectangle.Y - playerTexture.Height * MainGame.roomScale)
                    playerPosition.Y += 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerDir = SpriteEffects.None;
                if (playerPosition.X < MainGame.roomRectangle.Width + MainGame.roomRectangle.X - playerTexture.Width * MainGame.roomScale)
                    playerPosition.X += 2;
            }
        }
    }
}
