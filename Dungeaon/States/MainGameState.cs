using Dungeaon.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Dungeaon.States
{
    class MainGameState : State
    {
        private List<Component> components;

        struct Room
        {
            public Texture2D texture;
            public Enemie enemie;
            public bool[] walls;
            public bool isBoss;
            public bool isShop;
        }

        private Room[,] rooms = new Room[5, 5];

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
            player = new Player(game.player, new Vector2(700, 500), game,graphicsDeviceManager);

            upperDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width / 2) - 20, (int)roomPos.Y + 10, 50, 35);
            rightDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width) - 10, (int)(roomPos.Y + roomRectangle.Height / 2) + 12, 15, 50);
            lowerDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width / 2) - 20, (int)(roomPos.Y + roomRectangle.Height) - 5, 50, 20);
            leftDoor = new Rectangle((int)(roomPos.X), (int)(roomPos.Y + roomRectangle.Height / 2) + 12, 30, 50);

            roomTextures = new Texture2D[2] { game.room1, game.room2 };
            rooms = new Room[5, 5];

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

            if (room.walls[0])
                spriteBatch.Draw(game.door, new Vector2(upperDoor.X, upperDoor.Y), null, Color.White, 0f, Vector2.Zero, roomScale, SpriteEffects.None, 0);

            if (room.walls[1])
                spriteBatch.Draw(game.smallDoor, new Vector2(rightDoor.X + 10, rightDoor.Y), null, Color.White, 0f, Vector2.Zero, roomScale, SpriteEffects.None, 0);

            if (room.walls[2])
                spriteBatch.Draw(game.smallDoor, new Vector2(lowerDoor.X, lowerDoor.Y + 22), null, Color.White, -(float)(Math.PI / 2), Vector2.Zero, roomScale, SpriteEffects.None, 0);

            if (room.walls[3])
                spriteBatch.Draw(game.smallDoor, new Vector2(leftDoor.X, leftDoor.Y), null, Color.White, 0f, Vector2.Zero, roomScale, SpriteEffects.None, 0);


            if (room.enemie != null && room.enemie.isAlive)
                room.enemie.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(game.playerCard, new Vector2(roomPos.X / 2 - game.playerCard.Width * 3.4f / 2, 10), null, Color.White, 0f, Vector2.Zero, 3.4f, SpriteEffects.None, 0);
            spriteBatch.Draw(Player.playerHealthBar, position: new Vector2(roomPos.X / 2 - Player.playerHealthBar.Width / 2 + 6, 819),Player.playerHealthBarRect, color: Color.White);
            spriteBatch.Draw(game.playerHead, new Vector2(roomPos.X / 2 - game.playerHead.Width * 4f / 2, 114), null, Color.White);

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
                    game.ChangeState(new FightState(game, graphicsDeviceManager, content, this, room.enemie));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ChangeState(previousState);
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
                    room.walls = new bool[4];

                    if (x - 1 == -1)
                        room.walls[3] = true;

                    if (x + 1 == sizeX)
                        room.walls[1] = true;

                    if (y - 1 == -1)
                        room.walls[0] = true;

                    if (y + 1 == sizeY)
                        room.walls[2] = true;

                    if (rand.Next(101) <= 66)
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
            startRoom.walls = new bool[4] { true, false, false, true };
            rooms[0, 0] = startRoom;

            Room shopRoom = new Room();
            shopRoom.texture = game.shoproom;
            rooms[rand.Next(1, sizeY), rand.Next(1, sizeX)] = shopRoom;

            Room bossRoom = new Room();
            bossRoom.texture = game.bossRoom;
            rooms[rand.Next(1, sizeY), rand.Next(1, sizeX)] = bossRoom;


            return rooms;
        }

        private void CheckForDoorColission()
        {
            if (player.hitBox.Intersects(upperDoor) && !room.walls[0])
            {
                playerRoomPos.Y--;
                player.position = new Vector2(roomRectangle.X + roomRectangle.Width / 2, roomRectangle.Y + roomRectangle.Height - player.hitBox.Height - 40);
            }

            if (player.hitBox.Intersects(rightDoor) && !room.walls[1])
            {
                playerRoomPos.X++;
                player.position = new Vector2(roomRectangle.X + 10, roomRectangle.Y + roomRectangle.Height / 2);
            }

            if (player.hitBox.Intersects(lowerDoor) && !room.walls[2])
            {
                playerRoomPos.Y++;
                player.position = new Vector2(roomRectangle.X + roomRectangle.Width / 2, roomRectangle.Y + 20);

            }

            if (player.hitBox.Intersects(leftDoor) && !room.walls[3])
            {
                playerRoomPos.X--;
                player.position = new Vector2(roomRectangle.X + roomRectangle.Width - player.hitBox.Width - 50, roomRectangle.Y + roomRectangle.Height / 2);
            }
        }
    }
}