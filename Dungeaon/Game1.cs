using Dungeaon.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeaon
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private State currentState;
        private State nextState;

        public Texture2D whiteTexture;
        public Texture2D player;
        public Texture2D button;
        public Texture2D playerCard;
        public Texture2D inventoryCard;
        public Texture2D textBoxSprite;
        public Texture2D toolTip;
        public SpriteFont font;

        public Texture2D fightScreen;
        public Texture2D room1;
        public Texture2D room2;
        public Texture2D bossRoom;
        public Texture2D shoproom;
        public Texture2D door;
        public Texture2D smallDoor;
        public Texture2D graveyard;

        public Texture2D knightEnemy;
        public Texture2D financeEnemy;
        public Texture2D ghostEnemy;
        public Texture2D cyborgNinjaEnemy;
        public Texture2D boss1;
        public Texture2D devil;
        public Texture2D devilShop;
        public Texture2D necromancer;

        public Texture2D playerHead;
        public Texture2D boss1Head;
        public Texture2D devilHead;
        public Texture2D necromancerHead;

        public Texture2D healthPostion;
        public Texture2D speedPotion;

        public Texture2D sword1;
        public Texture2D sword2;
        public Texture2D sword3;
        public Texture2D sword4;
        public Texture2D sword5;
        public Texture2D sword6;

        public Texture2D shield1;
        public Texture2D shield2;
        public Texture2D shield3;
        public Texture2D shield4;
        public Texture2D shield5;

        public Options options = new Options()
        {
            backgroundColor = Color.Gray,
            debugMode = false
        };

        public struct Options
        {
            public Color backgroundColor;
            public bool debugMode;
        }

        public void ChangeState(State state)
        {
            nextState = state;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D buttonBack = new Texture2D(GraphicsDevice, 1, 1);
            Color[] color = new Color[buttonBack.Width * buttonBack.Height];

            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.White;
            }
            buttonBack.SetData(color);

            whiteTexture = buttonBack;
            font = Content.Load<SpriteFont>("font");
            room1 = Content.Load<Texture2D>("rooms/room1");
            room2 = Content.Load<Texture2D>("rooms/room2");
            bossRoom = Content.Load<Texture2D>("rooms/bossroom");
            player = Content.Load<Texture2D>("Player");
            button = Content.Load<Texture2D>("button");
            knightEnemy = Content.Load<Texture2D>("enemies/versuchsperson");
            financeEnemy = Content.Load<Texture2D>("enemies/Finanzmitarbeiter");
            ghostEnemy = Content.Load<Texture2D>("enemies/Geist");
            fightScreen = Content.Load<Texture2D>("Fightscreen");
            playerCard = Content.Load<Texture2D>("Board");
            playerHead = Content.Load<Texture2D>("heads/player-head");
            textBoxSprite = Content.Load<Texture2D>("Textbox");
            shoproom = Content.Load<Texture2D>("rooms/shoproom");
            boss1Head = Content.Load<Texture2D>("heads/boss1-head");
            boss1 = Content.Load<Texture2D>("enemies/boss1");
            devilHead = Content.Load<Texture2D>("heads/devil-Head");
            devil = Content.Load<Texture2D>("devil");
            devilShop = Content.Load<Texture2D>("devilshop");
            necromancerHead = Content.Load<Texture2D>("heads/Necromancer-Head");
            necromancer = Content.Load<Texture2D>("enemies/Necromancer");
            smallDoor = Content.Load<Texture2D>("rooms/no-door-schmol");
            door = Content.Load<Texture2D>("rooms/no-door");
            healthPostion = Content.Load<Texture2D>("items/potions/Hpotion");
            speedPotion = Content.Load<Texture2D>("items/potions/Speed");
            sword1 = Content.Load<Texture2D>("items/weapons/schwerts");
            sword2 = Content.Load<Texture2D>("items/weapons/schwertu1");
            sword3 = Content.Load<Texture2D>("items/weapons/schwertu2");
            sword4 = Content.Load<Texture2D>("items/weapons/schwertu3");
            sword5 = Content.Load<Texture2D>("items/weapons/schwertu4");
            sword6 = Content.Load<Texture2D>("items/weapons/schwertu5");
            shield1 = Content.Load<Texture2D>("items/defense/shield");
            shield2 = Content.Load<Texture2D>("items/defense/shieldu1");
            shield3 = Content.Load<Texture2D>("items/defense/shieldu2");
            shield4 = Content.Load<Texture2D>("items/defense/shieldu3");
            shield5 = Content.Load<Texture2D>("items/defense/shieldu4");
            cyborgNinjaEnemy= Content.Load<Texture2D>("enemies/Cyborg-Ninja-Demon");
            inventoryCard = Content.Load<Texture2D>("Inventar-Board");
            toolTip = Content.Load<Texture2D>("tooltip");
            graveyard = Content.Load<Texture2D>("rooms/Friedhof");

            // = Content.Load<Texture2D>("");

            currentState = new MainMenuState(this, _graphics, Content, null);
        }

        protected override void Update(GameTime gameTime)
        {
            if (nextState != null)
            {
                currentState = nextState;

                nextState = null;
            }

            currentState.Update(gameTime);
            currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(options.backgroundColor);
            currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
