using System;
using System.Collections.Generic;
using Dungeaon.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon.States
{
    class MainGameState : State
    {
        private List<Component> components;

        struct Room
        {
            public Texture2D texture;
            public Enemie enemie;
            public bool isBoss;
            public bool isShop;
        }

        private Room[,] rooms = new Room[3, 3];

        public Texture2D playerHealthbar;

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

        private Enemie knight;

        public MainGameState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            roomPos = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.room1.Width * roomScale) / 2, 0);
            roomRectangle = new Rectangle((int)roomPos.X + 5, (int)roomPos.Y + 10, (int)(game.room1.Width * roomScale) - 10, (int)(game.room1.Height * roomScale) - 18);
            player = new Player(game.player, new Vector2(700, 500), game);

            playerHealthbar = new Texture2D(graphicsDeviceManager.GraphicsDevice, 286, 32);
            Color[] healthbarColor = new Color[playerHealthbar.Width * playerHealthbar.Height];

            for (int i = 0; i < healthbarColor.Length; i++)
            {
                healthbarColor[i] = Color.Red;
            }
            playerHealthbar.SetData(healthbarColor);

            upperDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width / 2) - 15, (int)roomPos.Y + 1, 50, 10);
            rightDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width), (int)(roomPos.Y + roomRectangle.Height / 2), 10, 50);
            lowerDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width / 2) - 15, (int)(roomPos.Y + roomRectangle.Height) + 9, 50, 10);
            leftDoor = new Rectangle((int)(roomPos.X), (int)(roomPos.Y + roomRectangle.Height / 2), 10, 50);

            roomTextures = new Texture2D[3] { game.room1, game.room2, game.bossRoom};
            roomTexture = roomTextures[0];

            knight = new KnightEnemie(game, new Vector2(900, 400));
            knight.scale = 3.5f;

            Room r1 = new Room();
            r1.texture = roomTextures[0];
            r1.enemie = null;

            Room r2 = new Room();
            r2.texture = roomTextures[1];
            r2.enemie = knight;

            Room r3 = new Room();
            r3.texture = roomTextures[0];
            r3.enemie = knight;

            Room r4 = new Room();
            r4.texture = roomTextures[1];
            r4.enemie = null;

            Room r5 = new Room();
            r5.texture = roomTextures[2];
            r5.enemie = null;
            r5.isBoss = true;

            rooms = new Room[3, 3]
            {
                { r1, r2, r3 },
                { r4, r5, r1 },
                { r3, r4, r2 }
            };

            components = new List<Component>()
            {
                player
            };
        }

        private int mouseX;
        private int mouseY;
        private Room room;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            spriteBatch.Draw(room.texture, roomPos, null, Color.White, 0f, Vector2.Zero, roomScale, SpriteEffects.None, 0);

            room.enemie?.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(playerHealthbar, new Vector2(255, 860),Color.White);

            if (game.options.debugMode)
            {
                spriteBatch.Draw(game.whiteTexture, upperDoor, Color.White);
                spriteBatch.Draw(game.whiteTexture, rightDoor, Color.White);
                spriteBatch.Draw(game.whiteTexture, lowerDoor, Color.White);
                spriteBatch.Draw(game.whiteTexture, leftDoor, Color.White);         
                spriteBatch.DrawString(game.font, "X: " + mouseX + " Y: " + mouseY, new Vector2(0, 0), Color.Black, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);

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

            room = rooms[(int)playerRoomPos.Y, (int)playerRoomPos.X];

            mouseX = Mouse.GetState().X;
            mouseY = Mouse.GetState().Y;

            foreach (Component component in components)
            {
                component.Update(gameTime);
            }

            CheckForDoorColission();

            if (room.enemie != null)
            {
                room.enemie.position = Vector2.Lerp(room.enemie.position, player.position, 0.01f);

                if (room.enemie.hitBox.Intersects(player.hitBox))
                    game.ChangeState(new FightState(game, graphicsDeviceManager, content, this));
            }

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
            if (player.hitBox.Intersects(upperDoor))
            {
                playerRoomPos.Y--;
                Vector2 cache = Vector2.Clamp(playerRoomPos, Vector2.Zero, new Vector2(rooms.GetLength(1)-1, rooms.GetLength(0)-1));
                
                if (playerRoomPos == cache)
                    player.position = new Vector2(roomRectangle.X + roomRectangle.Width / 2, roomRectangle.Y + roomRectangle.Height - player.hitBox.Height - 10);

                playerRoomPos = cache;
            }

            if (player.hitBox.Intersects(rightDoor))
            {
                playerRoomPos.X++;
                Vector2 cache = Vector2.Clamp(playerRoomPos, Vector2.Zero, new Vector2(rooms.GetLength(1)-1, rooms.GetLength(0)-1));
                
                if (playerRoomPos == cache)
                    player.position = new Vector2(roomRectangle.X + 10, roomRectangle.Y + roomRectangle.Height / 2);

                playerRoomPos = cache;
            }

            if (player.hitBox.Intersects(lowerDoor))
            {
                playerRoomPos.Y++;
                Vector2 cache = Vector2.Clamp(playerRoomPos, Vector2.Zero, new Vector2(rooms.GetLength(1)-1, rooms.GetLength(0)-1));

                if (playerRoomPos == cache)
                    player.position = new Vector2(roomRectangle.X + roomRectangle.Width / 2, roomRectangle.Y + 10);

                playerRoomPos = cache;
            }

            if (player.hitBox.Intersects(leftDoor))
            {
                playerRoomPos.X--;
                Vector2 cache = Vector2.Clamp(playerRoomPos, Vector2.Zero, new Vector2(rooms.GetLength(1)-1, rooms.GetLength(0)-1));

                if (playerRoomPos == cache)
                    player.position = new Vector2(roomRectangle.X + roomRectangle.Width - player.hitBox.Width - 10, roomRectangle.Y + roomRectangle.Height / 2);

                playerRoomPos = cache;
            }
        }
    }
}