using System.Collections.Generic;
using Dungeaon.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon.States
{
    class StartState : State
    {
        private List<Component> components;
        private Player player;
        private Rectangle door = new Rectangle(1250, 0, 250, 180);
        private TextBox necroMancerTextBox;
        private TextBox financeTextBox;
        private TextBox aufregung;
        private MainGameState mainGameState;
        private Vector2 financePos;

        public StartState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            player = new Player(game.player, new Vector2(350, 700), game, graphicsDeviceManager, false);
            mainGameState = new MainGameState(game, graphicsDeviceManager, content, previousState);
            financePos = new Vector2(2000, graphicsDeviceManager.PreferredBackBufferHeight / 2 - 20);

            components = new List<Component>()
            {
                player
            };

            List<string> Ndialog = new List<string>() { "HAhahaHAHAHAHHAHAhaha\n*hust hust* haHAHAha.... haha\nEndlich bist du wach das war\nanstregend.....",
                "So mein ehmaliger Toter (Freund?)\ndu musst fuer mich was erledigen\nich habe womoeglich ein monster erschaffen\n",
                "Und ich habe Angst also musst du\nes Logischerweise loswerden",
                "Das Letzte mal als ich so viel\nAngst hatte war als das Finanzamt mich\nfast bei der steuerhinterziehung\ngeschnappt hat" };
            necroMancerTextBox = new TextBox(game, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - game.textBoxSprite.Width * 10 / 2, graphicsDeviceManager.PreferredBackBufferHeight - game.textBoxSprite.Height * 10), Ndialog, game.necromancerHead);
            components.Add(necroMancerTextBox);

            aufregung = new TextBox(game, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - game.textBoxSprite.Width * 10 / 2, graphicsDeviceManager.PreferredBackBufferHeight - game.textBoxSprite.Height * 10), "HAT JEMAND STEUERHINTERZIEHUNG GESAGT! ", null);

            List<string> Fdialog = new List<string>() { "Da haben wir sie endlich\nWir suchen sie schon seit fast\n600 Jahren",
                "???....\nwas soll das? ist das\nihr schlaeger oder was?\nsie wissen er kann mich nichts anhaben ",
                "Mord ist illegal also geht das nicht",
                "Halt warte nein geh weg von mir!" };
            financeTextBox = new TextBox(game, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2 - game.textBoxSprite.Width * 10 / 2, graphicsDeviceManager.PreferredBackBufferHeight - game.textBoxSprite.Height * 10), Fdialog, game.financeHead);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            spriteBatch.Draw(game.graveyard, new Rectangle(0, 0, 1920, 1080), new Rectangle(0, 0, game.graveyard.Width, game.graveyard.Height), Color.White);

            spriteBatch.Draw(game.necromancer, new Vector2(120, 580), null, Color.White, 0f, Vector2.Zero, 3.8f, SpriteEffects.None, 0f);

            if (necroMancerTextBox.finished)
            {
                spriteBatch.Draw(game.financeEnemy, financePos, null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0);
            }

            if (game.options.debugMode)
            {
                spriteBatch.Draw(game.whiteTexture, door, Color.White);
                spriteBatch.DrawString(game.font, "X: " + Mouse.GetState().X + " Y: " + Mouse.GetState().Y, new Vector2(0, 0), Color.Black, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);
            }

            foreach (Component component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        private bool aText = true;
        private bool fText = true;
       

        public override void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (necroMancerTextBox.finished)
            {

                components.Remove(necroMancerTextBox);

                if (aText)
                {
                    components.Add(aufregung);
                    aText = false;
                }
            }

            if (aufregung.finished)
            {

                components.Remove(aufregung);

                if (fText)
                {
                    components.Add(financeTextBox);
                    fText = false;
                }

                financePos = Vector2.Lerp(financePos, new Vector2(1000, financePos.Y), 0.01f);
            }

            if (financeTextBox.finished)
            {
                components.Remove(financeTextBox);
                game.ChangeState(new FightState(game, graphicsDeviceManager, content, mainGameState, new FinanzEnemie(game, Vector2.Zero)));
            }

            foreach (Component component in components)
            {
                component.Update(gameTime);
            }

            if (player.hitBox.Intersects(door))
                game.ChangeState(mainGameState);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ChangeState(previousState);
            }
        }
    }
}
