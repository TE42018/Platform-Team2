using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SugarAdventure
{
    class Key : IEntity, IPickupable
    {
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Hitbox { get; set; }
        public Point Size { get; set; }

        public Key(Vector2 _pos, string _type)
        {
            Position = _pos;
            Type = "key_" + _type;
            Size = new Point(44, 40);

            switch (Type)
            {
                case "key_green":
                    Texture = SugarGame.entityManager.Textures[(int)EntityTexture.Key_green];
                    break;
            }

            int posX = (int)Position.X + (Texture.Width / 2) - (Size.X / 2);
            int posY = (int)Position.Y + (Texture.Height / 2) - (Size.Y / 2);

            Hitbox = new Rectangle(new Point(posX, posY), Size);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
