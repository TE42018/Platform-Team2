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
        public Point Size { get; set; }

        public Coin(Vector2 _pos, string _type)
        {
            Type = "coin_" + _type;
            Position = _pos;
            Size = new Point(47, 47);

            switch (Type.ToLower())
            {
                case "coin_bronze":
                    Value = 1;
                    Texture = SugarGame.entityManager.Textures[(int)EntityTexture.Coin_bronze];
                    break;
                case "coin_silver":
                    Value = 3;
                    Texture = SugarGame.entityManager.Textures[(int)EntityTexture.Coin_silver];
                    break;
                case "coin_gold":
                    Value = 5;
                    Texture = SugarGame.entityManager.Textures[(int)EntityTexture.Coin_gold];
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
