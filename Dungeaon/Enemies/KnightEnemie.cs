﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeaon.Enemies
{
    class KnightEnemie : Enemie
    {
        public override string name => "Versuchs Person";
        public override int maxHealth => 100;
        public override int health { get => _health; set => _health = value; }
        public override int damage => 1;
        public override Texture2D texture => game.knightEnemy;
        public override Texture2D headTexture => null;
        public override Rectangle hitBox => new Rectangle((int)position.X + 10, (int)position.Y + 10, (int)(texture.Width * scale) - 20, (int)(texture.Height * scale) - 20);
        public override float scale { get => _scale; set => _scale = value; }
        public override Vector2 position { get => _position; set => _position = value; }
        public override bool isAlive { get => _isAlive; set => _isAlive = value; }

        public KnightEnemie(Game1 game, Vector2 position) : base(game, position)
        {
            health = maxHealth;
            isAlive = true;
            scale = 3.5f;
        }

        public override void Attack()
        {

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
    }
}
