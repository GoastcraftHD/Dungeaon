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

        public static Texture2D playerHealthbar;

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

            roomTextures = new Texture2D[2] { game.room1, game.room2};
            rooms = new Room[3, 3];

            rooms = GenerateDungeon(3, 3);

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

            room = rooms[(int)playerRoomPos.Y, (int)playerRoomPos.X];

            spriteBatch.Draw(room.texture, roomPos, null, Color.White, 0f, Vector2.Zero, roomScale, SpriteEffects.None, 0);

            if (room.enemie != null && room.enemie.isAlive)
                room.enemie.Draw(gameTime, spriteBatch);

            #region PlayerCard

            spriteBatch.Draw(game.playerCard,new Vector2(roomPos.X / 2 - game.playerCard.Width * 3.4f / 2, 10), null, Color.White, 0f, Vector2.Zero, 3.4f, SpriteEffects.None, 0);
            spriteBatch.Draw(playerHealthbar, new Vector2(roomPos.X / 2 - playerHealthbar.Width / 2, 820),Color.White);
            spriteBatch.Draw(game.playerHead, new Vector2(roomPos.X / 2 - game.playerHead.Width * 4f / 2, 114), null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0);
            
            #endregion
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

            mouseX = Mouse.GetState().X;
            mouseY = Mouse.GetState().Y;

            foreach (Component component in components)
            {
                component.Update(gameTime);
            }

            CheckForDoorColission();

            if (room.enemie != null && room.enemie.isAlive)
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

        private Room[,] GenerateDungeon(int sizeX, int sizeY)
        {
            Random rand = new Random();

            Room[,] rooms = new Room[sizeY, sizeX];

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    Room room = new Room();
                    int index = rand.Next(roomTextures.Length);
                    room.texture = roomTextures[index];

                    if (rand.Next(2) == 1)
                    {
                        int enemieIndex = rand.Next(1);
                        Enemie enemie = new KnightEnemie(game, new Vector2(900, 400));
                        
                        if (enemieIndex == 0)
                        {
                            enemie = new KnightEnemie(game, new Vector2(900, 400));
                        }

                        room.enemie = enemie;
                    }

                    rooms[y, x] = room;
                }
            }

            Room startRoom = new Room();
            startRoom.texture = roomTextures[0];
            rooms[0, 0] = startRoom;
            
            Room estartRoom = new Room();
            estartRoom.texture = roomTextures[1];
            estartRoom.enemie = new KnightEnemie(game, new Vector2(900, 400));
            rooms[0, 0] = estartRoom;

            return rooms;
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