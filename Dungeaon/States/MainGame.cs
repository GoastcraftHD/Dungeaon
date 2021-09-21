using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon.States
{
    class MainGame : State
    {
        private List<Component> components;

        private int[,] rooms = new int[3, 3]
        {
            { 0, 1, 2 },
            { 2, 0, 1 },
            { 1, 2, 0 }
        };

        private int[,] enemiesLayout = new int[3, 3] // 0 = empty; 10 = enemy; 20 = chest; 30 = shop; 40 = boss;
        {
            { 0, 10, 20 },
            { 20, 0, 10 },
            { 10, 20, 0 }
        };

        private Rectangle roomRectangle;
        private Vector2 playerPos = new Vector2(200, 200);
        private SpriteEffects playerDir = SpriteEffects.None;
        float scale = 3.5f;
        private Vector2 roomPos;

        public MainGame(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            roomPos = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.room1.Width * scale) / 2, 0);
            roomRectangle = new Rectangle((int)roomPos.X + 5, (int)roomPos.Y + 10, (int)(game.room1.Width * scale) - 10, (int)(game.room1.Height * scale) - 18);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);


            spriteBatch.Draw(game.room1, roomPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(game.player, playerPos, null, Color.White, 0f, Vector2.Zero, scale, playerDir, 0);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (playerPos.Y > roomRectangle.Y)
                    playerPos.Y -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerDir = SpriteEffects.FlipHorizontally;
                if (playerPos.X > roomRectangle.X)
                    playerPos.X -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (playerPos.Y < roomRectangle.Height + roomRectangle.Y - game.player.Height * scale)
                    playerPos.Y += 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerDir = SpriteEffects.None;
                if (playerPos.X < roomRectangle.Width + roomRectangle.X - game.player.Width * scale)
                    playerPos.X += 2;
            }
        }
    }
}