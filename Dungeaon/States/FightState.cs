using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Dungeaon.Enemies;

namespace Dungeaon.States
{
    class FightState : State
    {
        private Vector2 roomPos;
        List<Component> componentList;

        public FightState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            Texture2D buttonAttackTexture = new Texture2D(graphicsDeviceManager.GraphicsDevice, 64, 32);
            Color[] color = new Color[buttonAttackTexture.Width * buttonAttackTexture.Height];

            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.Red;
            }

            //MediumPurple
            buttonAttackTexture.SetData(color);

            roomPos = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.room1.Width * 3.5f) / 2, 0);

            Button attackButton = new Button(buttonAttackTexture, new Vector2(600, 730))
            {
                font = game.font,
                text = "ATTACK",
                textScale = 2,
                spriteScale = 3,
            };

            Texture2D buttonAttackTextureB = new Texture2D(graphicsDeviceManager.GraphicsDevice, 64, 32);
            Color[] colorB = new Color[buttonAttackTexture.Width * buttonAttackTexture.Height];

            for (int i = 0; i < color.Length; i++)
            {
                colorB[i] = Color.Blue;
            }

            buttonAttackTextureB.SetData(colorB);
            Button blockButton = new Button(buttonAttackTextureB, new Vector2(838, 730))
            {
                font = game.font,
                text = "BLOCK",
                textScale = 2,
                spriteScale = 3,
            };
        
            Texture2D buttonAttackTextureC = new Texture2D(graphicsDeviceManager.GraphicsDevice, 64, 32);
            Color[] colorC = new Color[buttonAttackTexture.Width * buttonAttackTexture.Height];

            for (int i = 0; i < color.Length; i++)
            {
                colorC[i] = Color.Peru;
            }

            buttonAttackTextureC.SetData(colorC);
            Button dodgeButton = new Button(buttonAttackTextureC, new Vector2(1078, 730))
            {
                font = game.font,
                text = "DODGE",
                textScale = 2,
                spriteScale = 3,
            };
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
