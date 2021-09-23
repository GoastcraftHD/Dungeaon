using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Dungeaon.States
{
    class OptionsState : State
    {
        private List<Component> components;

        private int colorIndex = 0;
        private List<Color> bgColors = new List<Color>()
        {
            Color.Gray,
            Color.Black,
            Color.White,
            Color.Red,
            Color.Green,
            Color.Yellow
        };

        private List<String> bgColorsName = new List<String>()
        {
            "Gray",
            "Black",
            "White",
            "Red",
            "Green",
            "Yellow"
        };

        int buttonScale = 8;
        private int colorButtonPosY = 200;
        private Button debugModeButton;
        private Button fullscreenButton;


        public OptionsState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            debugModeButton = new Button(game.button, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.button.Width * buttonScale) / 2, 400))
            {
                font = game.font,
                text = "Debug Mode",
                textScale = 2,
                spriteScale = buttonScale
            };
            
            debugModeButton.click += debugModeButton_Click;

            fullscreenButton = new Button(game.button, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.button.Width * buttonScale) / 2, 600))
            {
                font = game.font,
                text = "Fullscreen",
                textScale = 2,
                spriteScale = buttonScale,
                buttonStayPressed = true
            };

            fullscreenButton.click += fullscreenButton_Click;

            Button colorAddButton = new Button(game.whiteTexture, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 + (game.button.Width * buttonScale) / 2 - 30, colorButtonPosY + game.button.Height * buttonScale / 2))
            {
                font = game.font,
                text = "+",
                textScale = 2,
                spriteScale = 10,
                hitBoxSizeY = game.button.Height * buttonScale,
                hitBoxSizeX = 50
            };

            colorAddButton.click += colorAddButton_Click;

            Button colorSubButton = new Button(game.whiteTexture, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.button.Width * buttonScale) / 2 + 20, colorButtonPosY + game.button.Height * buttonScale / 2))
            {
                font = game.font,
                text = "-",
                textScale = 2,
                spriteScale = 10,
                hitBoxSizeY = game.button.Height * buttonScale,
                hitBoxSizeX = 50
            };

            colorSubButton.click += colorSubButton_Click;

            Button backButton = new Button(game.button, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.button.Width * buttonScale) / 2, 850))
            {
                font = game.font,
                text = "Back",
                textScale = 2,
                spriteScale = buttonScale
            };
            backButton.click += backButton_Click;
            
            components = new List<Component>()
            {
                colorSubButton,
                colorAddButton,
                backButton,
                debugModeButton,
                fullscreenButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            spriteBatch.Draw(game.button, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.button.Width * buttonScale) / 2, colorButtonPosY), null, Color.White, 0f, Vector2.Zero, buttonScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(game.font, bgColorsName[colorIndex], new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.font.MeasureString(bgColorsName[colorIndex]).X * 2) / 2, colorButtonPosY + game.button.Height * buttonScale / 2 - 20), Color.Black, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);

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
            foreach (Component component in components)
            {
                component.Update(gameTime);
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(previousState);
        }

        private void colorAddButton_Click(object sender, EventArgs e)
        {
            if (colorIndex == bgColors.Count - 1)
                colorIndex = 0;
            else
                colorIndex++;
            game.options.backgroundColor = bgColors[colorIndex];
        }

        private void colorSubButton_Click(object sender, EventArgs e)
        {
            if (colorIndex == 0)
                colorIndex = bgColors.Count - 1;
            else
                colorIndex--;

            game.options.backgroundColor = bgColors[colorIndex];
        }

        private void debugModeButton_Click(object sender, EventArgs e)
        {
            game.options.debugMode = !game.options.debugMode;
            debugModeButton.buttonStayPressed = !debugModeButton.buttonStayPressed;
        }

        private void fullscreenButton_Click(object sender, EventArgs e)
        {
            fullscreenButton.buttonStayPressed = !fullscreenButton.buttonStayPressed;
            graphicsDeviceManager.IsFullScreen = !graphicsDeviceManager.IsFullScreen;
            graphicsDeviceManager.ApplyChanges();
        }
    }
}
