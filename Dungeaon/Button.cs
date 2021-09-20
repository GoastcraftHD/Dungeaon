using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon
{
    class Button : Component
    {
        private MouseState currentMouse;
        private SpriteFont font;
        private bool isHovering;
        private MouseState previousMouse;
        private Texture2D texture;

        public event EventHandler click;
        public bool clicked;
        public Color textColor;
        public Vector2 position;

        public Rectangle rectangle
        {
            get
            {
                return new Rectangle((int) position.X, (int) position.Y, texture.Width, texture.Height);
            }
        }

        public string text;

        public Button(Texture2D texture, SpriteFont font)
        {
            this.texture = texture;
            this.font = font;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;

            if (isHovering)
                color = Color.Gray;

            spriteBatch.Draw(texture, rectangle, color);

            if (!string.IsNullOrEmpty(text))
            {
                float x = (rectangle.X + (rectangle.Width / 2)) - (font.MeasureString(text).X / 2);
                float y = (rectangle.Y + (rectangle.Height / 2)) - (font.MeasureString(text).Y / 2);

                spriteBatch.DrawString(font, text, new Vector2(x, y), textColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            isHovering = false;

            if (mouseRectangle.Intersects(rectangle))
            {
                isHovering = true;

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
