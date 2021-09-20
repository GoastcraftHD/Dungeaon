using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon
{
    static class PlayerUtil
    {
        public static Vector2 Movement(Vector2 currPos)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                currPos.X -= 1;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                currPos.X += 1;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                currPos.Y -= 1;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                currPos.Y += 1;

            return currPos;
        }
    }
}
