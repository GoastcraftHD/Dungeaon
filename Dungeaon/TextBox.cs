using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon
{
    class TextBox : Component
    {
        private List<String> dialog;
        private Vector2 position;
        private Game1 game;
        private Texture2D headTexture;

        private int dialogIndex = 0;
        private int dialogLetterIndex = 0;
        private bool textComplete = false;
        private string displayedDialog = "";

        public bool finished = false;

        public TextBox(Game1 game, Vector2 position, List<String> dialog, Texture2D headTexture)
        {
            this.game = game;
            this.position = position;
            this.dialog = dialog;
            this.headTexture = headTexture;
        }

        public TextBox(Game1 game, Vector2 position, String dialog, Texture2D headTexture)
        {
            List<String> dialogList = new List<string>() { dialog };

            this.game = game;
            this.position = position;
            this.dialog = dialogList;
            this.headTexture = headTexture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(game.textBoxSprite, position, null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None,
                0);
            spriteBatch.Draw(headTexture, position + new Vector2(15, 10), null, Color.White, 0f, Vector2.Zero, 2.18f,
                SpriteEffects.None, 0);
            spriteBatch.DrawString(game.font, displayedDialog, position + new Vector2(180, 10), Color.White, 0f, Vector2.Zero,
                2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(game.font, "Press 'E' to continue!", position + new Vector2(590, 132), Color.Black,
                0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        private double timeSinceLastLetter = 0D;

        public override void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            timeSinceLastLetter += deltaTime;

            if (timeSinceLastLetter > .05D)
            {
                if (dialogLetterIndex < dialog[dialogIndex].Length)
                {
                    displayedDialog += dialog[dialogIndex][dialogLetterIndex];
                    dialogLetterIndex++;
                    timeSinceLastLetter = 0D;
                }
                else
                    textComplete = true;
            }

            if (textComplete && Keyboard.GetState().IsKeyDown(Keys.E))
            {
                if (dialogIndex + 1 < dialog.Count)
                {
                    dialogIndex++;
                    dialogLetterIndex = 0;
                    displayedDialog = "";
                    textComplete = false;
                }
                else
                {
                    finished = true;
                }
            }
        }
    }
}