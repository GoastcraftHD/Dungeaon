﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeaon.Enemies
{

    abstract class Enemie : Component
    {
        public abstract String name { get; }
        public abstract int maxHealth { get; }
        protected int _health { get; set; }
        public abstract int health { get; set; }
        public abstract int damage { get; }
        public abstract Texture2D texture { get; }
        protected float _scale { get; set; }
        public abstract float scale { get; set; }
        public abstract Rectangle hitBox { get; }
        protected Vector2 _position { get; set; }
        public abstract Vector2 position { get; set; }
        protected  bool _isAlive { get; set; }
        public abstract bool isAlive { get; set; }


        protected Game1 game;

        public Enemie(Game1 game, Vector2 spawnPosition)
        {
            this.game = game;
            _position = spawnPosition;
        }
    }
}