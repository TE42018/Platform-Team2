using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SugarAdventure
{
    class Coin : IEntity, IPickupable
    {
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Hitbox { get; set; }
        public Texture2D Texture { get; set; }

        public int Value { get; set; }

        public Coin(Vector2 _pos, int _value)
        {
            Type = "coin";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        } 
    }
}
