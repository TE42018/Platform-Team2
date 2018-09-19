using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SugarAdventure.Entities
{
    class Enemy : IEntity
    {
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Point Size { get; set; }

        public Enemy(Vector2 _pos, string _type)
        {
            Type = "enemy_" + _type;

            switch (Type)
            {
                case "enemy_slime":
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
