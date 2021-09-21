using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeaon.States
{
    public abstract class State
    {
        protected ContentManager content;
        protected GraphicsDeviceManager graphicsDeviceManager;
        protected Game1 game;
        protected State previousState;

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void PostUpdate(GameTime gameTime);

        public State(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState)
        {
            this.game = game;
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.content = content;
            this.previousState = previousState;
        }

        public abstract void Update(GameTime gameTime);
    }
}
