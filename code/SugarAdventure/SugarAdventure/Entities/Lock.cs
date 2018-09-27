using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SugarAdventure
{
    public class Lock : IEntity, IInteractable
    {
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Point Size { get; set; }
        public Rectangle Hitbox { get; set; }

        public IPickupable Key { get; set; }
        public string KeyName { get; set; }

        public Lock(Vector2 _pos, string _type)
        {
            Position = _pos;
            Type = "lock_" + _type;
            KeyName = "key_" + _type;
            Key = Game1.entityManager.GetEntity(KeyName);

            switch (Type.ToLower())
            {
                case "lock_green":
                    Texture = Game1.entityManager.Textures[(int)EntityTexture.Lock_green];
                    break;
            }

            Hitbox = new Rectangle(Position.ToPoint(), new Point(Texture.Width, Texture.Height));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
