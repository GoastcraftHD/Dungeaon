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
        private List<Component> components;
        private Enemie enemie;
        private const int enemieHealthBarWitdhB = 286;
        private int enemieHealthBarWitdh = 286;
        private TextBox activeTextBox;
        private Rectangle enemySourceRectangle;
        private Rectangle enemySizeRectangle;

        private Rectangle ememieHealthBarRect => new Rectangle(0, 0, enemieHealthBarWitdh, 32);

        public FightState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState, Enemie enemie) : base(game, graphicsDeviceManager, content, previousState)
        {
            this.enemie = enemie;

            enemyHealthbar = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);
            enemySourceRectangle = new Rectangle(0, 0, enemie.texture.Width, enemie.texture.Height);
            enemySizeRectangle = new Rectangle(graphicsDeviceManager.PreferredBackBufferWidth / 2 - 400 / 2, 130, 400, 500);
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

            Button attackButton = new Button(new Vector2(600, 730))
            {
                texture = buttonAttackTexture,
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
            Button blockButton = new Button(new Vector2(868, 730))
            {
                texture = buttonAttackTextureB,
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
            Button dodgeButton = new Button(new Vector2(1138, 730))
            {
                texture = buttonAttackTextureC,
                font = game.font,
                text = "DODGE",
                textScale = 4,
                spriteScale = 3,
            };

            dodgeButton.click += dge_Button;
            #endregion

            //List
            components = new List<Component>()
            {
                attackButton,
                blockButton,
                dodgeButton
            };

            components.AddRange(Player.inventorySlots);
        }

        //Fightscreen wird erstellt
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            spriteBatch.DrawString(game.font, "X: " + mouseX + " Y: " + mouseY, new Vector2(0, 0), Color.Black, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);
            spriteBatch.Draw(game.fightScreen, roomPos, null, Color.White, 0f, Vector2.Zero, 3.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(enemie.texture, enemySizeRectangle, enemySourceRectangle, Color.White);
            spriteBatch.Draw(enemyHealthbar, new Vector2(roomPos.X + game.fightScreen.Width * 3.4f / 2 - ememieHealthBarRect.Width / 2, 70), ememieHealthBarRect, Color.White);
            spriteBatch.DrawString(game.font, enemie.name, new Vector2(roomPos.X + game.fightScreen.Width * 3.5f / 2 - ememieHealthBarRect.Width / 2, 35), Color.Yellow, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);

            MainGameState.DrawUI(spriteBatch, game, graphicsDeviceManager);

            foreach (Component component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            foreach (Button slot in Player.inventorySlots)
            {
                if (mouseRectangle.Intersects(slot.HitBoxRectangle) && Player.inventory[slot].sprite != null)
                    MainGameState.DrawToolTip(spriteBatch, Player.inventory[slot], game);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        private Rectangle mouseRectangle;

        public override void Update(GameTime gameTime)
        {
            mouseRectangle = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);

            foreach (Component component in components)
            {
                component.Update(gameTime);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                game.ChangeState(previousState);
            }

            if (activeTextBox == null)
            {
                if(enemie.headTexture == game.boss1Head)
                {
                    activeTextBox = new TextBox(game, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - game.textBoxSprite.Width * 10 / 2, graphicsDeviceManager.PreferredBackBufferHeight - game.textBoxSprite.Height * 10), enemie.Dialog, enemie.headTexture);
                    components.Add(activeTextBox);
                }
                else {
                    Random random = new Random();
                    int textAuswahl = random.Next(4);
                    activeTextBox = new TextBox(game, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - game.textBoxSprite.Width * 10 / 2, graphicsDeviceManager.PreferredBackBufferHeight - game.textBoxSprite.Height * 10), enemie.Dialog[textAuswahl], enemie.headTexture);
                    components.Add(activeTextBox);
                }
            }

            if (activeTextBox.finished)
                components.Remove(activeTextBox);
        }

        #region button funktionen
        private void atk_Button(object sender, EventArgs e)
        {
            bool dodge = false;
            bool block = false;
            enemie.health -= Player.player_Damage;
            double enemieHealth = ((double)enemie.health / (double)enemie.maxHealth) * enemieHealthBarWitdhB;
            enemieHealthBarWitdh = (int)enemieHealth;
            if (enemie.health <= 0)
            {
                Player.player_Money += enemie.money;
                enemie.isAlive = false;
                game.ChangeState(previousState);
            }
            if (Player.player_Health <= 0)
            {
                game.ChangeState(new MainMenuState(game, graphicsDeviceManager, content, null));
                Player.player_Health = 100;
                Player.playerHealthBarWidth = 286;
            }
            enemie.Attack(block, dodge);

        }

        private void blk_Button(object sender, EventArgs e)
        {
            bool dodge = false;
            bool block = true;
            enemie.Attack(block, dodge);
            enemie.health -= 60 * Player.player_Damage / 100;
            double enemieHealth = ((double)enemie.health / (double)enemie.maxHealth) * enemieHealthBarWitdhB;
            enemieHealthBarWitdh = (int)enemieHealth;
            if (enemie.health <= 0)
            {
                Player.player_Money += enemie.money;
                enemie.isAlive = false;
                game.ChangeState(previousState);
            }
            if (Player.player_Health <= 0)
            {
                game.ChangeState(new MainMenuState(game, graphicsDeviceManager, content, null));
                Player.player_Health = 100;
                Player.playerHealthBarWidth = 286;
            }
        }

        private void dge_Button(object sender, EventArgs e)
        {
            bool dodge = true;
            bool block = false;
            enemie.Attack(block, dodge);
            enemie.health -= 40 * Player.player_Damage / 100;
            double enemieHealth = ((double)enemie.health / (double)enemie.maxHealth) * enemieHealthBarWitdhB;
            enemieHealthBarWitdh = (int)enemieHealth;
            if (enemie.health <= 0)
            {
                Player.player_Money += enemie.money;
                enemie.isAlive = false;
                game.ChangeState(previousState);
            }
            if (Player.player_Health <= 0)
            {
                game.ChangeState(new MainMenuState(game, graphicsDeviceManager, content, null));
                Player.player_Health = 100;
                Player.playerHealthBarWidth = 286;
            }
        }
        #endregion
    }
}
