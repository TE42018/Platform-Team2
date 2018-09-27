using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SugarAdventure
{
    public class Tile
    {
        private string collideable = "false";
        public string Collideable {
            get
            {
                return collideable;
            }
        }

        private PropertyDict properties;
        public PropertyDict Properties
        {
            get
            {
                return properties;
            }
        }
        private Rectangle hitbox;
        public Rectangle Hitbox
        {
            get
            {
                return hitbox;
            }
        }
        private string type;
        public string Type
        {
            get
            {
                return type;
            }
        }
        private Vector2 pos;
        public Vector2 Pos
        {
            get
            {
                return pos;
            }
        }
        private Texture2D texture;
        //public Texture2D Texture
        //{
        //    get
        //    {
        //        return texture;
        //    }
        //}

        public Tile(Tile original, Tile newTile)
        { 
            pos = original.Pos;
            texture = newTile.texture;
            type = newTile.Type;
            hitbox = original.Hitbox;
        }

        public Tile(Vector2 _pos, Texture2D _texture, int _tileSize, PropertyDict _properties, int _levelNumber)
        {
            pos = _pos;
            texture = _texture;
            type = "none";
            hitbox = new Rectangle(Point.Zero, Point.Zero);

            if (_properties != null)
            {
                _properties.TryGetValue("Type", out type);
                _properties.TryGetValue("Collideable", out collideable);
                
                switch (type.ToLower())
                {
                    case "block":
                        hitbox = new Rectangle(pos.ToPoint(), new Point(texture.Width, texture.Height));
                        break;
                    case "platform":
                        hitbox = new Rectangle(pos.ToPoint(), new Point(texture.Width, texture.Height / 2));
                        break;
                    case "ladder":
                        hitbox = new Rectangle(pos.ToPoint(), new Point(texture.Width, texture.Height));
                        break;
                    case "slope_up":
                        hitbox = new Rectangle(pos.ToPoint(), new Point(texture.Width, texture.Height));
                        break;
                    case "slope_down":
                        hitbox = new Rectangle(pos.ToPoint(), new Point(texture.Width, texture.Height));
                        break;
                    case "lock_green":
                        hitbox = new Rectangle(pos.ToPoint(), new Point(texture.Width, texture.Height));
                        break;
                    case "portal":
                        hitbox = new Rectangle(pos.ToPoint(), new Point(texture.Width, texture.Height));
                        break;
                    case "spikes":
                        hitbox = new Rectangle(pos.ToPoint() + new Point(0, texture.Height/2), new Point(texture.Width, texture.Height/2));
                        break;


                    case "coin_bronze":
                        texture = null;
                        SugarGame.entityManager.Add(new Coin(pos, "bronze"));
                        break;
                    case "coin_silver":
                        texture = null;
                        SugarGame.entityManager.Add(new Coin(pos, "silver"));
                        break;
                    case "coin_gold":
                        texture = null;
                        SugarGame.entityManager.Add(new Coin(pos, "gold"));
                        break;
                    case "key_green":
                        texture = null;
                        SugarGame.entityManager.Add(new Key(pos, "green"));
                        break;



                    case "enemy_slime":
                        texture = null;
                        SugarGame.entityManager.Add(new Enemy(pos, "slime"));
                        break;
                    case "enemy_snail":
                        texture = null;
                        SugarGame.entityManager.Add(new Enemy(pos, "snail"));
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color tintColor)
        {
            if(texture != null)
                spriteBatch.Draw(texture, pos, tintColor);
        }
    }
}
