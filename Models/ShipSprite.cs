using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace App05MonoGame.Models
{
    public class ShipSprite : PlayerSprite
    {
        public int Health { get; set; }
        public int Score { get; set; }

        public ShipSprite() : base()
        {
            Health = 100;
            Score = 0;
        }

        public ShipSprite(Texture2D image, int x, int y) : this()
        {
            Image = image;
            Position = new Vector2(x, y);
        }
    }

}
