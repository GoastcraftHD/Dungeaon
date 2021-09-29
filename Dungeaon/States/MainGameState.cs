using Dungeaon.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Dungeaon.States
{
    enum ItemType
    {
        Potion,
        Weapon,
        Defense
    }

    class MainGameState : State
    {
        private List<Component> components;
        public static List<Button> inventorySlots;

        public static List<Item> postionList;
        public static List<Item> weaponList;
        public static List<Item> defenseList;

        #region ItemInitilisation
        private void InitPotionList()
        {
            Item healthPotion = new Item()
            {
                name = "Healing Potion",
                sprite = game.healthPostion,
                cost = 10,
                type = ItemType.Potion
            };

            Item speedPotion = new Item()
            {
                name = "Speed Potion",
                sprite = game.speedPotion,
                cost = 7,
                type = ItemType.Potion
            };

            postionList = new List<Item>()
            {
                healthPotion,
                speedPotion
            };
        }

        private void InitWeaponList()
        {
            Item rustySword = new Item()
            {
                name = "Rusty Sword",
                sprite = game.sword1,
                cost = 1,
                type = ItemType.Weapon,
                damage = 10
            };

            Item ironSword = new Item()
            {
                name = "Iron Sword",
                sprite = game.sword2,
                cost = 5,
                type = ItemType.Weapon,
                damage = 15
            };

            Item Sword = new Item()
            {
                name = "Sword",
                sprite = game.sword3,
                cost = 10,
                type = ItemType.Weapon,
                damage = 20
            };

            Item crystalSword = new Item()
            {
                name = "Crystal Sword",
                sprite = game.sword4,
                cost = 30,
                type = ItemType.Weapon,
                damage = 25
            };

            Item razorSword = new Item()
            {
                name = "Razor Blade",
                sprite = game.sword5,
                cost = 40,
                type = ItemType.Weapon,
                damage = 30
            };

            Item flamingSword = new Item()
            {
                name = "Flaming Sword",
                sprite = game.sword6,
                cost = 50,
                type = ItemType.Weapon,
                damage = 35
            };

            weaponList = new List<Item>()
            {
                rustySword,
                ironSword,
                Sword,
                crystalSword,
                razorSword,
                flamingSword
            };
        }

        private void InitDefenseList()
        {
            Item basicShield = new Item()
            {
                name = "Basic Shield",
                sprite = game.shield1,
                cost = 10,
                type = ItemType.Defense
            };

            Item advancedShield = new Item()
            {
                name = "Advanced Shield",
                sprite = game.shield2,
                cost = 20,
                type = ItemType.Defense
            };

            Item proShield = new Item()
            {
                name = "Professional Shield",
                sprite = game.shield3,
                cost = 40,
                type = ItemType.Defense
            };

            Item woodShield = new Item()
            {
                name = "Wooden Shield",
                sprite = game.shield4,
                cost = 35,
                type = ItemType.Defense
            };

            Item wolfShield = new Item()
            {
                name = "Wolf Clan Shield",
                sprite = game.shield5,
                cost = 500,
                type = ItemType.Defense
            };

            defenseList = new List<Item>()
            {
                basicShield,
                advancedShield,
                proShield,
                woodShield,
                wolfShield
            };
        }
        #endregion
        public struct Item
        {
            public string name;
            public Texture2D sprite;
            public int cost;
            public ItemType type;
            public int damage;
            public int defense;
        }

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

        public static Vector2 roomPos;
        private Player player;
        private Vector2 playerRoomPos = new Vector2(0, 0);
        private Texture2D[] roomTextures;
        public static Vector2 inventoryCardPos;
        private Item emptyItem = new Item();

        private Rectangle shopHitbox;

        public MainGameState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            roomPos = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.room1.Width * roomScale) / 2, 0);
            roomRectangle = new Rectangle((int)roomPos.X + 5, (int)roomPos.Y + 10, (int)(game.room1.Width * roomScale) - 10, (int)(game.room1.Height * roomScale) - 18);
            player = new Player(game.player, new Vector2(700, 500), game, graphicsDeviceManager);
            shopHitbox = new Rectangle((int)roomPos.X, (int)roomPos.Y, 350, 150);
            inventoryCardPos = new Vector2((roomPos.X + game.room1.Width * roomScale) + (graphicsDeviceManager.PreferredBackBufferWidth - (roomPos.X + game.room1.Width * roomScale)) / 2 - game.inventoryCard.Width * 3.4f / 2, 10);

            upperDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width / 2) - 20, (int)roomPos.Y + 10, 50, 35);
            rightDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width) - 10, (int)(roomPos.Y + roomRectangle.Height / 2) + 12, 15, 50);
            lowerDoor = new Rectangle((int)(roomPos.X + roomRectangle.Width / 2) - 20, (int)(roomPos.Y + roomRectangle.Height) - 5, 50, 20);
            leftDoor = new Rectangle((int)(roomPos.X), (int)(roomPos.Y + roomRectangle.Height / 2) + 12, 30, 50);

            roomTextures = new Texture2D[2] { game.room1, game.room2 };

            rooms = GenerateDungeon(5, 5);

            InitPotionList();
            InitWeaponList();
            InitDefenseList();

            components = new List<Component>()
            {
                player
            };

            InitInventory();
        }

        private void InitInventory()
        {
            int slotScale = 100;
            inventorySlots = new List<Button>();

            for (int i = 0; i < 15; i++)
            {
                int yPos = i / 3;
                int xPos = i % 3;
                Vector2 slotPos = inventoryCardPos + new Vector2(slotScale * xPos + 15 * xPos, yPos * slotScale + yPos * 100 - 15 * yPos) + new Vector2(50, 30);

                Button slot = new Button(slotPos)
                {
                    texture = game.sword1,
                    spriteScaleX = slotScale,
                    spriteScaleY = slotScale
                };

                inventorySlots.Add(slot);
                components.Add(slot);
                Player.inventory.Add(slot, emptyItem);
            }
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

            if (room.isShop)
                spriteBatch.Draw(game.devil, new Vector2(680, 75), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

            if (game.options.debugMode)
            {
                spriteBatch.Draw(game.whiteTexture, upperDoor, Color.White);
                spriteBatch.Draw(game.whiteTexture, rightDoor, Color.White);
                spriteBatch.Draw(game.whiteTexture, lowerDoor, Color.White);
                spriteBatch.Draw(game.whiteTexture, leftDoor, Color.White);

                if (room.isShop)
                    spriteBatch.Draw(game.whiteTexture, shopHitbox, Color.White);

                spriteBatch.DrawString(game.font, "X: " + mouseX + " Y: " + mouseY, new Vector2(0, 0), Color.Black, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);
            }

            DrawUI(spriteBatch, game, graphicsDeviceManager);

            foreach (Component component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        private double time = 0D;

        public override void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            time += deltaTime;

            if (time > .2D)


            mouseX = Mouse.GetState().X;
            mouseY = Mouse.GetState().Y;

            foreach (Component component in components)
            {
                component.Update(gameTime);
            }

            CheckForDoorColission();

            if (room.isShop && player.hitBox.Intersects(shopHitbox))
                game.ChangeState(new ShopState(game, graphicsDeviceManager, content, this));

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

        public static void DrawUI(SpriteBatch spriteBatch, Game1 game, GraphicsDeviceManager graphicsDeviceManager)
        {
            spriteBatch.Draw(game.playerCard, new Vector2(roomPos.X / 2 - game.playerCard.Width * 3.4f / 2, 10), null, Color.White, 0f, Vector2.Zero, 3.4f, SpriteEffects.None, 0);
            spriteBatch.Draw(Player.playerHealthBar, new Vector2(roomPos.X / 2 - Player.constant_PlayerHealthBarWidth / 2 + 6, 819), Player.playerHealthBarRect, Color.White);
            spriteBatch.Draw(game.playerHead, new Vector2(roomPos.X / 2 - game.playerHead.Width * 4f / 2, 114), null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0);

            spriteBatch.Draw(game.inventoryCard, inventoryCardPos, null, Color.White, 0f, Vector2.Zero, 3.4f, SpriteEffects.None, 0);
            spriteBatch.DrawString(game.font, Player.player_MoneyAnzeige, new Vector2(roomPos.X / 2 - game.playerCard.Width  / 2, 730), Color.Yellow, 0f, Vector2.Zero, 3.4f, SpriteEffects.None, 0);

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
                    room.walls = setDoors(y, x, sizeY, sizeX);

                    if (rand.Next(101) <= 66)
                    {
                        int enemieIndex = rand.Next(3);
                        Enemie enemie = new KnightEnemie(game, new Vector2(900, 400));

                        if (enemieIndex == 0)
                        {
                            enemie = new KnightEnemie(game, new Vector2(900, 400));
                        }
                        else if (enemieIndex == 1)
                        {
                            enemie = new GhostEnemie(game, new Vector2(900, 400));
                        }
                        else if(enemieIndex == 2)
                        {
                            enemie = new CyborgNinjaEnemie(game, new Vector2(900, 400));
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

            int shopX = rand.Next(1, sizeX);
            int shopY = rand.Next(1, sizeY);

            Room shopRoom = new Room();
            shopRoom.texture = game.shoproom;
            shopRoom.isShop = true;
            shopRoom.walls = setDoors(shopY, shopX, sizeY, sizeX);
            rooms[shopY, shopX] = shopRoom;

            int bossX = rand.Next(1, sizeX);
            int bossY = rand.Next(1, sizeY);

            Room bossRoom = new Room();
            bossRoom.texture = game.bossRoom;
            bossRoom.isBoss = true;
            bossRoom.walls = setDoors(bossY, bossX, sizeY, sizeX);
            rooms[bossY, bossX] = bossRoom;


            return rooms;
        }

        private bool[] setDoors(int y, int x, int sizeY, int sizeX)
        {
            bool[] array = new bool[4];

            if (x - 1 == -1)
                array[3] = true;

            if (x + 1 == sizeX)
                array[1] = true;

            if (y - 1 == -1)
                array[0] = true;

            if (y + 1 == sizeY)
                array[2] = true;

            return array;
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