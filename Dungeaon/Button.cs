using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon
{
    class Button : Component
    {
        private MouseState currentMouse;
        private MouseState previousMouse;
        private bool isHovering;

        public Texture2D texture;
        public event EventHandler click;
        public bool clicked;
        public Vector2 position;

        public bool buttonStayPressed = false;
        public Color spriteColor = Color.White;
        public Color spriteColorHover = Color.Gray;
        public float spriteRotation = 0f;
        public Vector2 spriteOrigin = Vector2.Zero;
        public int spriteScale = 1;
        public int hitBoxSizeX = 0;
        public int hitBoxSizeY = 0;
        public SpriteEffects spriteEffect = SpriteEffects.None;

        public SpriteFont font;
        public int spriteScaleX = 1;
        public int spriteScaleY = 1;
        public Color textColor = Color.Black;
        public float textRotation = 0f;
        public Vector2 textOrigin = Vector2.Zero;
        public float textScale = 1f;
        public SpriteEffects textEffect = SpriteEffects.None;

        public Rectangle HitBoxRectangle
        {
            get
            {
                if (texture != null)
                {
                    int scaleX = spriteScaleX != 1 ? spriteScaleX : texture.Width * spriteScale;
                    int scaleY = spriteScaleY != 1 ? spriteScaleY : texture.Height * spriteScale;

                    return new Rectangle((int)position.X, (int)position.Y, scaleX, scaleY);
                }

                return new Rectangle((int)position.X, (int)position.Y, hitBoxSizeX, hitBoxSizeY);
            }
        }

        public Rectangle sourceRectangle { get => new Rectangle(0, 0, texture.Width, texture.Height); }
        public Rectangle sizeRectangle { get => new Rectangle((int)position.X, (int)position.Y, spriteScaleX, spriteScaleY); }

        public string text;

        public Button(Vector2 position)
        {
            this.position = position;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = spriteColor;

            if (isHovering || buttonStayPressed)
                color = spriteColorHover;

            if  (texture != null && (spriteScaleX != 1 || spriteScaleY != 1))
                spriteBatch.Draw(texture, sizeRectangle, sourceRectangle, color);
            else if (texture != null)
                spriteBatch.Draw(texture, position, null, color, spriteRotation, spriteOrigin, spriteScale, spriteEffect, 0);


            if (!string.IsNullOrEmpty(text))
            {
                float width = spriteScaleX != 1 ? sizeRectangle.Width : texture.Width * spriteScale;
                float height = spriteScaleY != 1 ? sizeRectangle.Height : texture.Height * spriteScale;

                float x = (position.X + (width / 2)) - (font.MeasureString(text).X * textScale / 2);
                float y = (position.Y + (height / 2)) - (font.MeasureString(text).Y * textScale / 2);

                spriteBatch.DrawString(font, text, new Vector2(x, y), textColor, textRotation, textOrigin, textScale, textEffect, 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            isHovering = false;

            if (mouseRectangle.Intersects(HitBoxRectangle))
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
