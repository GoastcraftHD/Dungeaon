using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Dungeaon.States
{
    class MainMenuState : State
    {
        private List<Component> components;

        private State optionsState;
        private State mainGameState;

        public MainMenuState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            optionsState = new OptionsState(game, graphicsDeviceManager, content, this);
            mainGameState = new MainGame(game, graphicsDeviceManager, content, this);

            int buttonScale = 8;

            Button startGameButton = new Button(game.button, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.button.Width * buttonScale) / 2, 270))
            {
                font = game.font,
                text = "Start Game",
                textScale = 2,
                spriteScale = buttonScale
            };
            startGameButton.click += startGameButton_Click;

            Button optionsButton = new Button(game.button, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.button.Width * buttonScale) / 2, graphicsDeviceManager.PreferredBackBufferHeight / 2 - (game.button.Height * buttonScale) / 2))
            {
                font = game.font,
                text = "Options",
                textScale = 2,
                spriteScale = buttonScale
            };
            optionsButton.click += optionsButton_Click;

            Button exitButton = new Button(game.button, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.button.Width * buttonScale) / 2, 650))
            {
                font = game.font,
                text = "Exit Game",
                textScale = 2,
                spriteScale = buttonScale
            };
            exitButton.click += exitButton_Click;

            components = new List<Component>()
            {
                startGameButton,
                optionsButton,
                exitButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            foreach (Component component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in components)
            {
                component.Update(gameTime);
            }
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(mainGameState);
        }

        private void optionsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(optionsState);
        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }
    }
}
