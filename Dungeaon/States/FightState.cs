using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeaon.States
{
    class FightState : State
    {
        private Vector2 roomPos;
        public FightState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            roomPos = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - (game.room1.Width * 3.5f) / 2, 0);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(game.fightScreen, roomPos, null, Color.White, 0f, Vector2.Zero, 3.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(game.knightEnemy, new Vector2(760, 300), null,Color.White,0f,Vector2.Zero,10f,SpriteEffects.None,0);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
