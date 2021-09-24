using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeaon
{
    class TextBox : Component
    {
        private List<String> dialog;
        private Vector2 position;
        private Game1 game;

        public TextBox(Game1 game, Vector2 position, List<String> dialog)
        {
            this.game = game;
            this.position = position;
            this.dialog = dialog;
        }

        public TextBox(Game1 game, Vector2 position, String dialog)
        {
            List<String> dialogList = new List<string>() {dialog};

            this.game = game;
            this.position = position;
            this.dialog = dialogList;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(game.textBoxSprite, position, null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0);
            spriteBatch.Draw(game.playerHead, position + new Vector2(15, 10), null, Color.White, 0f, Vector2.Zero, 2.18f, SpriteEffects.None, 0);
            spriteBatch.DrawString(game.font, dialog[0], position + new Vector2(180, 10), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(game.font, "Press 'E' to continue!", position + new Vector2(590, 132), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
