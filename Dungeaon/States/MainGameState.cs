using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon.States
{
    class MainGameState : State
    {
        private List<Component> components;

        private int[,] rooms = new int[3, 3] // 0 = empty; 1 = enemy; 2 = dead enemy; 3 = chest; 4 = looted chest; 5 = shop; 6 = boss; 7 = dead boss;
        {                                    // 10 = room1; 20 = room2; 30 = boos room;
            { 10, 21, 10 },
            { 23, 36, 11 },
            { 15, 23, 15 }
        };

        public static Rectangle roomRectangle;
        public static float roomScale = 3.5f;
        public static Rectangle upperDoor;
        public static Rectangle rightDoor;
        public static Rectangle lowerDoor;
        public static Rectangle leftDoor;

        private Vector2 roomPos;
        private Player player;
        private Vector2 playerRoomPos = new Vector2(0, 0);
        private Texture2D[] roomTextures;
        private Texture2D roomTexture;

        public MainGameState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            roomPos = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.room1.Width * roomScale) / 2, 0);
            roomRectangle = new Rectangle((int)roomPos.X + 5, (int)roomPos.Y + 10, (int)(game.room1.Width * roomScale) - 10, (int)(game.room1.Height * roomScale) - 18);
            player = new Player(game.player, new Vector2(700, 500), game);

            upperDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width / 2), (int)roomPos.Y + 1, 50, 10);
            rightDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width), (int)(roomPos.Y + roomRectangle.Height / 2), 10, 50);
            lowerDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width / 2), (int)(roomPos.Y + roomRectangle.Height) + 9, 50, 10);
            leftDoor = new Rectangle((int)(roomPos.X), (int)(roomPos.Y + roomRectangle.Height / 2), 10, 50);

            roomTextures = new Texture2D[3] { game.room1, game.room2, game.bossRoom };
            roomTexture = roomTextures[0];

            components = new List<Component>()
            {
                player
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            spriteBatch.Draw(roomTexture, roomPos, null, Color.White, 0f, Vector2.Zero, roomScale, SpriteEffects.None, 0);

            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            spriteBatch.DrawString(game.font, "X: " + mouseX + " Y: " + mouseY, new Vector2(0, 0), Color.Black, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);

            if (game.options.debugMode)
            {
                spriteBatch.Draw(game.whiteTexture, upperDoor, Color.White);
                spriteBatch.Draw(game.whiteTexture, rightDoor, Color.White);
                spriteBatch.Draw(game.whiteTexture, lowerDoor, Color.White);
                spriteBatch.Draw(game.whiteTexture, leftDoor, Color.White);
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

        private double timeSinceLastDebugDraw = 0D;

        public override void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastDebugDraw += deltaTime;

            if (timeSinceLastDebugDraw > .5D)
            {
                timeSinceLastDebugDraw = 0D;
                Console.WriteLine(playerRoomPos);
            }

            foreach (Component component in components)
            {
                component.Update(gameTime);
            }

            CheckForDoorColission();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ChangeState(previousState);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                game.ChangeState(new FightState(game, graphicsDeviceManager,content,this));
            }
        }

        private void CheckForDoorColission()
        {
            if (player.playerHitBox.Intersects(upperDoor))
            {
                playerRoomPos.Y--;
                Vector2 cache = Vector2.Clamp(playerRoomPos, Vector2.Zero, new Vector2(rooms.GetLength(1)-1, rooms.GetLength(0)-1));
                
                if (playerRoomPos == cache)
                    player.playerPosition = new Vector2(roomRectangle.X + roomRectangle.Width / 2, roomRectangle.Y + roomRectangle.Height - player.playerHitBox.Height - 10);

                playerRoomPos = cache;
            }

            if (player.playerHitBox.Intersects(rightDoor))
            {
                playerRoomPos.X++;
                Vector2 cache = Vector2.Clamp(playerRoomPos, Vector2.Zero, new Vector2(rooms.GetLength(1)-1, rooms.GetLength(0)-1));
                
                if (playerRoomPos == cache)
                    player.playerPosition = new Vector2(roomRectangle.X + 10, roomRectangle.Y + roomRectangle.Height / 2);

                playerRoomPos = cache;
            }

            if (player.playerHitBox.Intersects(lowerDoor))
            {
                playerRoomPos.Y++;
                Vector2 cache = Vector2.Clamp(playerRoomPos, Vector2.Zero, new Vector2(rooms.GetLength(1)-1, rooms.GetLength(0)-1));

                if (playerRoomPos == cache)
                    player.playerPosition = new Vector2(roomRectangle.X + roomRectangle.Width / 2, roomRectangle.Y + 10);

                playerRoomPos = cache;
            }

            if (player.playerHitBox.Intersects(leftDoor))
            {
                playerRoomPos.X--;
                Vector2 cache = Vector2.Clamp(playerRoomPos, Vector2.Zero, new Vector2(rooms.GetLength(1)-1, rooms.GetLength(0)-1));

                if (playerRoomPos == cache)
                    player.playerPosition = new Vector2(roomRectangle.X + roomRectangle.Width - player.playerHitBox.Width - 10, roomRectangle.Y + roomRectangle.Height / 2);

                playerRoomPos = cache;
            }

            int room = Convert.ToInt32(rooms[(int)playerRoomPos.Y, (int)playerRoomPos.X].ToString().Substring(0, 1));
            int roomProperty = Convert.ToInt32(rooms[(int)playerRoomPos.Y, (int)playerRoomPos.X].ToString().Substring(1, 1));

            roomTexture = roomTextures[room - 1];
        }
    }
}