using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    class Tile
    {
        Dictionary<int, string> specialTiles = new Dictionary<int, string>
        {
            { 190, "Platform" },
            { 109, "Platform" },
            { 110, "Platform" },
            { 111, "Platform" },
            { 112, "Platform" },
            { 123, "Platform" },
            { 124, "Platform" },
            { 125, "Platform" },
            { 126, "Platform" },
            { 108, "Slope_up" },
            { 107, "Slope_down" },
            { 302, "Ladder" },
        };

        Dictionary<int, string> entityTiles = new Dictionary<int, string>
        {
            { 199, "Coin_gold" },
            { 401, "Coin_silver" },
            { 402, "Coin_bronze" },
            { 403, "Key_green" },
        };

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
        private int gid;
        public int Gid
        {
            get
            {
                return gid;
            }
        }

        public Tile(Vector2 _pos, int _tileSize, int _gid)
        {
            gid = _gid;
            pos = _pos;

            if (specialTiles.ContainsKey(gid))
            {
                string tileType;
                specialTiles.TryGetValue(gid, out tileType);

                switch (tileType.ToLower())
                {
                    case "platform":
                        type = "block";
                        hitbox = new Rectangle(_pos.ToPoint(), new Point(_tileSize, _tileSize/2));
                        break;
                    case "slope_up":
                        type = "slope_up";
                        hitbox = new Rectangle(_pos.ToPoint(), new Point(_tileSize, _tileSize));
                        break;
                    case "slope_down":
                        type = "slope_down";
                        hitbox = new Rectangle(_pos.ToPoint(), new Point(_tileSize, _tileSize));
                        break;
                    case "ladder":
                        type = "ladder";
                        hitbox = new Rectangle(_pos.ToPoint(), new Point(_tileSize, _tileSize));
                        break;
                }
            }
            else if (entityTiles.ContainsKey(gid))
            {
                string entityType;
                entityTiles.TryGetValue(gid, out entityType);

                switch (entityType.ToLower())
                {
                    case "coin_bronze":
                        Console.WriteLine("COIN");
                        break;
                }
            }
            else
            {
                if (gid != 0)
                {
                    type = "block";
                    hitbox = new Rectangle(_pos.ToPoint(), new Point(_tileSize, _tileSize));
                }
                else
                {
                    type = "none";
                    hitbox = new Rectangle(Point.Zero, Point.Zero);
                }
                
            }
        }
    }
}
