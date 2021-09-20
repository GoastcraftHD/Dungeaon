using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon
{
    class TextBox
    {
        public Texture2D textBoxbackground;
        private float timeSinceLastIncrement = 0;
        private int dialogIndex = 0;
        int wordIndex = 0;
        private String text = "";

        public bool DrawTextBox(SpriteBatch _spriteBatch, float deltaTime, Texture2D characterHead, String characteName, List<String> dialog)
        {
            _spriteBatch.Draw(textBoxbackground, new Vector2(560, 910), Color.White);
            _spriteBatch.Draw(characterHead, new Vector2(560, 910), Color.White);


            String dia = dialog[dialogIndex];

            if (wordIndex < dia.Length)
            {
                timeSinceLastIncrement += deltaTime;

                if (timeSinceLastIncrement >= .5)
                {
                    text += dia[wordIndex];
                    wordIndex++;
                    timeSinceLastIncrement = 0;
                }
            }
            else
            {
                _spriteBatch.DrawString(Game1.font, "Press 'E' to continue.", new Vector2(1095, 1020), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

                if (dialogIndex + 1 < dialog.Count)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.E))
                    {
                        if (dialogIndex + 1 < dialog.Count)
                        {
                            dialogIndex++;
                            int diaIndex = dialogIndex;
                            Reset();
                            dialogIndex = diaIndex;
                        }
                        else
                        {
                            return true;
                        }
                        
                    }
                }
            }
            _spriteBatch.DrawString(Game1.font, text, new Vector2(700, 910), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            return false;
        }

        public void DrawTextBox(SpriteBatch _spriteBatch, float deltaTime, Texture2D characterHead, String characteName, String dialog)
        {
            List<String> dialogList = new List<string>() { dialog };
            DrawTextBox(_spriteBatch, deltaTime, characterHead, characteName, dialogList);
        }

        public void Reset()
        {
            wordIndex = 0;
            dialogIndex = 0;
            timeSinceLastIncrement = 0;
            text = "";
        }
    }
}
