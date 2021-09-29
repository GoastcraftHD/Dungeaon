using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dungeaon.Enemies
{

    abstract class Enemie : Component
    {
        public abstract String name { get; }
        public abstract int maxHealth { get; }
        protected int _health { get; set; }
        public abstract int health { get; set; }
        public abstract int damage { get; }
        public abstract int money { get; }
        public abstract Texture2D texture { get; }
        public abstract Texture2D headTexture { get; }
        protected float _scale { get; set; }
        public abstract float scale { get; set; }
        public abstract Rectangle hitBox { get; }
        protected Vector2 _position { get; set; }
        public abstract Vector2 position { get; set; }
        protected bool _isAlive { get; set; }
        public abstract bool isAlive { get; set; }

        public abstract void Attack(bool block, bool dodge);

        protected Game1 game;

        public Enemie(Game1 game, Vector2 spawnPosition)
        {
            this.game = game;
            _position = spawnPosition;
        }
    }
}
