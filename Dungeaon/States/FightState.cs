using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Dungeaon.States
{
    class FightState : State
    {
        private Texture2D enemyHealthbar;
        private Vector2 roomPos;
        List<Component> componentList;

        public FightState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            roomPos = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.room1.Width * 3.5f) / 2, 0);

            //Healthbar vom Gegner
            enemyHealthbar = new Texture2D(graphicsDeviceManager.GraphicsDevice, 286, 32);
            Color[] healthbarColor = new Color[enemyHealthbar.Width * enemyHealthbar.Height];

            for (int i = 0; i < healthbarColor.Length; i++)
            {
                healthbarColor[i] = Color.Red;
            }
            enemyHealthbar.SetData(healthbarColor);

            #region buttons

            //Attackbutton
            Texture2D buttonAttackTexture = new Texture2D(graphicsDeviceManager.GraphicsDevice, 64, 32);
            Color[] color = new Color[buttonAttackTexture.Width * buttonAttackTexture.Height];

            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.Red;
            }
            buttonAttackTexture.SetData(color);

            Button attackButton = new Button(buttonAttackTexture, new Vector2(600, 730))
            {
                font = game.font,
                text = "ATTACK",
                textScale = 4,
                spriteScale = 3,
            };

            //Block Button
            Texture2D buttonAttackTextureB = new Texture2D(graphicsDeviceManager.GraphicsDevice, 64, 32);
            Color[] colorB = new Color[buttonAttackTexture.Width * buttonAttackTexture.Height];

            for (int i = 0; i < color.Length; i++)
            {
                colorB[i] = Color.Blue;
            }

            buttonAttackTextureB.SetData(colorB);
            Button blockButton = new Button(buttonAttackTextureB, new Vector2(868, 730))
            {
                font = game.font,
                text = "BLOCK",
                textScale = 4,
                spriteScale = 3,
            };


            //DodgeButton
            Texture2D buttonAttackTextureC = new Texture2D(graphicsDeviceManager.GraphicsDevice, 64, 32);
            Color[] colorC = new Color[buttonAttackTexture.Width * buttonAttackTexture.Height];

            for (int i = 0; i < color.Length; i++)
            {
                colorC[i] = Color.Peru;
            }

            buttonAttackTextureC.SetData(colorC);
            Button dodgeButton = new Button(buttonAttackTextureC, new Vector2(1138, 730))
            {
                font = game.font,
                text = "DODGE",
                textScale = 4,
                spriteScale = 3,
            };

            #endregion


            //List
            componentList = new List<Component>()
            {
                attackButton,
                blockButton,
                dodgeButton
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            spriteBatch.DrawString(game.font, "X: " + mouseX + " Y: " + mouseY, new Vector2(0, 0), Color.Black, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);
            spriteBatch.Draw(game.fightScreen, roomPos, null, Color.White, 0f, Vector2.Zero, 3.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(game.knightEnemy, new Vector2(820, 260), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0);
            spriteBatch.Draw(enemyHealthbar, new Vector2(roomPos.X + game.fightScreen.Width * 3.4f / 2 - enemyHealthbar.Width / 2, 70), Color.White);
            spriteBatch.DrawString(game.font, "Versuchsperson", new Vector2(roomPos.X + game.fightScreen.Width * 3.5f / 2 - enemyHealthbar.Width / 2, 35), Color.Yellow, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);
            spriteBatch.Draw(game.playerCard, new Vector2(roomPos.X / 2 - game.playerCard.Width * 3.4f / 2, 10), null, Color.White, 0f, Vector2.Zero, 3.4f, SpriteEffects.None, 0);
            spriteBatch.Draw(MainGameState.playerHealthbar, new Vector2(roomPos.X / 2 - MainGameState.playerHealthbar.Width / 2 + 6, 819), Color.White);
            spriteBatch.Draw(game.playerHead, new Vector2(roomPos.X / 2 - game.playerHead.Width * 4f / 2, 114), null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0);

            foreach (Component component in componentList)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in componentList)
            {
                component.Update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                game.ChangeState(previousState);
            }
        }
    }
}
