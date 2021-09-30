using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Dungeaon.Enemies
{
    class FinanzEnemie : Enemie
    {
        public override string name => "Das Pure Boese";

        public override int maxHealth => 100;

        public override int health { get => _health; set => _health = value; }

        public override int damage => 1;

        public override int money => 0;

        public override Texture2D texture => game.financeEnemy;

        public override Texture2D headTexture => game.financeHead;

        public override float scale { get => _scale; set => _scale = value; }

        public override Rectangle hitBox => new Rectangle((int)position.X + 10, (int)position.Y + 10, (int)(texture.Width * scale) - 20, (int)(texture.Height * scale) - 20);

        public override Vector2 position { get => _position; set => _position = value; }
        public override bool isAlive { get => _isAlive; set => _isAlive = value; }

        public override List<string> Dialog => new List<string>() { "Oh nein bitte drueck nicht \"ATTACK\"\num mir die volle menge schaden zu geben!",
            "Oh je nicht der \"BLOCK\" Knopf\num einen teil des schadens zu negieren\nund mir einen teil des schaden zuzufuegen",
            "bitte weich meinen angriff nicht mit dem\n\"DODGE\" button aus um eine chance zu haben\nkeinen schaden zu erleiden!\nund mich anzugreifen",
            "" };

        public FinanzEnemie(Game1 game, Vector2 position) : base(game, position)
        {
            health = maxHealth;
            isAlive = true;
            scale = 3.5f;
        }

        public override void Attack(bool block, bool dodge)
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
