using System;
using Dungeaon.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Dungeaon
{
    class Player : Component
    {
        private Texture2D texture;
        public Vector2 position;
        private SpriteEffects direction = SpriteEffects.None;
        private Game1 game;

        public Rectangle hitBox => new Rectangle((int)position.X + 20, (int)position.Y + 20, (int)(texture.Width * MainGameState.roomScale) - 30, (int)(texture.Height * MainGameState.roomScale) - 30);

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
                spriteBatch.Draw(game.whiteTexture, new Vector2(hitBox.X, hitBox.Y), hitBox, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        private Vector2 currentVelocity = new Vector2(0, 0);
        private Vector2 maxVelocity = new Vector2(4, 4);
        private Vector2 accelerationSpeed = new Vector2(1, 1);
        private Vector2 slowDownSpeed = new Vector2(2, 2);

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (position.Y > MainGameState.roomRectangle.Y)
                {
                    if (currentVelocity.Y < -maxVelocity.Y)
                        currentVelocity.Y = -maxVelocity.Y;
                    else
                        currentVelocity.Y -= accelerationSpeed.Y;
                }
            }
            else
            {
                currentVelocity.Y += slowDownSpeed.Y;
                currentVelocity.Y = Math.Clamp(currentVelocity.Y, -maxVelocity.Y, 0f);
            }


            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                direction = SpriteEffects.FlipHorizontally;

                if (position.X > MainGameState.roomRectangle.X)
                {
                    if (currentVelocity.X < -maxVelocity.X)
                        currentVelocity.X = -maxVelocity.X;
                    else
                        currentVelocity.X -= accelerationSpeed.X;
                }
            }
            else
            {
                currentVelocity.X += slowDownSpeed.X;
                currentVelocity.X = Math.Clamp(currentVelocity.X, -maxVelocity.X, 0f);
            }


            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (position.Y < MainGameState.roomRectangle.Y + MainGameState.roomRectangle.Height - texture.Height * MainGameState.roomScale)
                {
                    if (currentVelocity.Y < maxVelocity.Y)
                        currentVelocity.Y = maxVelocity.Y;
                    else
                        currentVelocity.Y += accelerationSpeed.Y;
                }
            }
            else
            {
                if (currentVelocity.Y > 0)
                {
                    currentVelocity.Y -= slowDownSpeed.Y;
                    currentVelocity.Y = Math.Clamp(currentVelocity.Y, 0f, maxVelocity.Y);
                }
            }


            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                direction = SpriteEffects.None;

                if (position.X < MainGameState.roomRectangle.X + MainGameState.roomRectangle.Width - texture.Width * MainGameState.roomScale)
                {
                    if (currentVelocity.X < maxVelocity.X)
                        currentVelocity.X = maxVelocity.X;
                    else
                        currentVelocity.X += accelerationSpeed.X;
                }
            }
            else
            {
                if (currentVelocity.X > 0)
                {
                    currentVelocity.X -= slowDownSpeed.X;
                    currentVelocity.X = Math.Clamp(currentVelocity.X, 0f, maxVelocity.X);
                }
            }

            position += currentVelocity;
        }
    }
}
