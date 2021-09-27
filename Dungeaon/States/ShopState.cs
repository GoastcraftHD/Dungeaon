using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon.States
{
    class ShopState : State
    {
        private List<Component> components;

        public ShopState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            Button item1Button = new Button(game.speedPotion, new Vector2(570, 570))
            {
                spriteScale = 7
            };

            Button item2Button = new Button(game.healthPostion, new Vector2(700, 570))
            {
                spriteScale = 7
            };

            Button item3Button = new Button(game.healthPostion, new Vector2(720, 200))
            {
                spriteScale = 6
            };

            components = new List<Component>()
            {
                item1Button,
                item2Button,
                item3Button
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            spriteBatch.Draw(game.devilShop, MainGameState.roomPos, null, Color.White, 0f, Vector2.Zero, 14f, SpriteEffects.None, 0);
            MainGameState.DrawUI(spriteBatch, game);

            if (game.options.debugMode)
            {
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

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                game.ChangeState(previousState);

            foreach (Component component in components)
            {
                component.Update(gameTime);
            }
        }
    }
}
