using Dungeaon.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Dungeaon.States
{
    class FightState : State
    {
        private Texture2D enemyHealthbar;
        private Vector2 roomPos;
        private List<Component> componentList;
        private Enemie enemie;
        private const int enemieHealthBarWitdhB = 286;
        private int enemieHealthBarWitdh = 286;
        private Rectangle ememieHealthBarRect => new Rectangle(0, 0, enemieHealthBarWitdh, 32);

        public FightState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState, Enemie enemie) : base(game, graphicsDeviceManager, content, previousState)
        {
            this.enemie = enemie;

            enemyHealthbar = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);

            roomPos = MainGameState.roomPos;

            //Healthbar vom Gegner
            enemyHealthbar.SetData(new Color[] { Color.Red });

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

            attackButton.click += atk_Button;

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

            blockButton.click += blk_Button;

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

            dodgeButton.click += dge_Button;
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
            spriteBatch.Draw(enemie.texture, new Vector2(820, 260), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0);
            spriteBatch.Draw(enemyHealthbar, new Vector2(roomPos.X + game.fightScreen.Width * 3.4f / 2 - ememieHealthBarRect.Width / 2, 70), ememieHealthBarRect, Color.White);
            spriteBatch.DrawString(game.font, "Versuchsperson", new Vector2(roomPos.X + game.fightScreen.Width * 3.5f / 2 - ememieHealthBarRect.Width / 2, 35), Color.Yellow, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);
           
            MainGameState.DrawUI(spriteBatch, game);

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

        private void atk_Button(object sender, EventArgs e)
        {
            bool dodge = false;
            bool block = false;
            enemie.health -= 15;
            double enemieHealth = ((double)enemie.health / (double)enemie.maxHealth) * enemieHealthBarWitdhB;
            enemieHealthBarWitdh = (int)enemieHealth;
            if (enemie.health <= 0)
            {
                enemie.isAlive = false;
                game.ChangeState(previousState);
            }
            if (Player.player_Health <= 0)
            {
                game.ChangeState(new MainMenuState(game, graphicsDeviceManager, content, null));
            }
            enemie.Attack(block, dodge);

        }

        private void blk_Button(object sender, EventArgs e)
        {
            bool dodge = false;
            bool block = true;
            enemie.Attack(block, dodge);
        }

        private void dge_Button(object sender, EventArgs e)
        {
            bool dodge = true;
            bool block = false;
            enemie.Attack(block, dodge);
        }
    }
}
