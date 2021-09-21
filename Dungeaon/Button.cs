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
        private MouseState previousMouse;
        private bool isHovering;
        private Texture2D texture;

        public event EventHandler click;
        public bool clicked;
        public Vector2 position;

        public Color spriteColor = Color.White;
        public Color spriteColorHover = Color.Gray;
        public float spriteRotation = 0f;
        public Vector2 spriteOrigin = Vector2.Zero;
        public int spriteScale = 1;
        public int hitBoxScaleX = 1;
        public int hitBoxScaleY = 1;
        public SpriteEffects spriteEffect = SpriteEffects.None;

        public SpriteFont font;
        public Color textColor = Color.Black;
        public float textRotation = 0f;
        public Vector2 textOrigin = Vector2.Zero;
        public float textScale = 1f;
        public SpriteEffects textEffect = SpriteEffects.None;

        public Rectangle rectangle
        {
            get
            {
                return new Rectangle((int) position.X, (int) position.Y, texture.Width * spriteScale, texture.Height * spriteScale);
            }
        }

        public string text;

        public Button(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = spriteColor;

            if (isHovering)
                color = spriteColorHover;

            spriteBatch.Draw(texture, position, null, color, spriteRotation, spriteOrigin, spriteScale, spriteEffect, 0);

            if (!string.IsNullOrEmpty(text))
            {
                float x = (rectangle.X + (rectangle.Width / 2)) - (font.MeasureString(text).X * textScale / 2);
                float y = (rectangle.Y + (rectangle.Height / 2)) - (font.MeasureString(text).Y * textScale / 2);

                spriteBatch.DrawString(font, text, new Vector2(x, y), textColor, textRotation, textOrigin, textScale, textEffect, 0);
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
