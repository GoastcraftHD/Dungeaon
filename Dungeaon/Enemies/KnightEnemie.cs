using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Dungeaon.Enemies
{
    class KnightEnemie : Enemie
    {
        public override string name => "Test Subject";
        public override int maxHealth => 100;
        public override int money => 5;
        public override int health { get => _health; set => _health = value; }
        public override int damage => 10;
        public override Texture2D texture => game.knightEnemy;
        public override Texture2D headTexture => game.knightHead;
        public override Rectangle hitBox => new Rectangle((int)position.X + 10, (int)position.Y + 10, (int)(texture.Width * scale) - 20, (int)(texture.Height * scale) - 20);
        public override float scale { get => _scale; set => _scale = value; }
        public override Vector2 position { get => _position; set => _position = value; }
        public override bool isAlive { get => _isAlive; set => _isAlive = value; }
        public override List<string> Dialog => new List<string>() { "Ich werde mir deine Seele holen!", "*Als das Lict richtig lag hast\ndu das innere des einstigen Ritters\ngesehen nichts als verdorbenes Fleisch\nund Schmerz*", "Wo ist er!\nWo ist er!\nWer hat mir das angetan?!\n","*Der Ritter laesst sein Schwert direkt\nin der ersten Sekunde fallen\n(anscheinend ist er Rechtshaender)*" };

        public KnightEnemie(Game1 game, Vector2 position) : base(game, position)
        {
            health = maxHealth;
            isAlive = true;
            scale = 3.5f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);

            if (game.options.debugMode)
            {
                spriteBatch.Draw(game.whiteTexture, hitBox, Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Attack(bool block, bool dodge)
        {
            Random random = new Random();
            int misschance = random.Next(1, 10);
            if (misschance >= 6)
            {
                if (block == false && dodge == false)
                {
                    Player.player_Health -= damage;
                    double playerhealth = ((double)Player.player_Health / (double)Player.player_maxHealth) * Player.constant_PlayerHealthBarWidth;
                    Player.playerHealthBarWidth = (int)playerhealth;
                }
                else if (dodge == true)
                {
                    Random rdm = new Random();
                    int dodgechance = rdm.Next(1, 4);
                    if (dodgechance >= 3)
                    {
                        Player.player_Health -= damage;
                        double playerhealth = ((double)Player.player_Health / (double)Player.player_maxHealth) * Player.constant_PlayerHealthBarWidth;
                        Player.playerHealthBarWidth = (int)playerhealth;
                    }

                }
                else if (block == true)
                {
                    int blockdamage = 40 * damage / 100;
                    Player.player_Health -= blockdamage;
                    double playerhealth = ((double)Player.player_Health / (double)Player.player_maxHealth) * Player.constant_PlayerHealthBarWidth;
                    Player.playerHealthBarWidth = (int)playerhealth;
                }
            }
        }
    }
}
