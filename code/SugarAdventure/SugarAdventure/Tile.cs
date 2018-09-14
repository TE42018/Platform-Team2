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
        int[] halfTiles = {  };

        Dictionary<int, string> specialTiles = new Dictionary<int, string>
        {
            { 101, "Platform" },
            { 201, "Slope_up" },
            { 202, "Slope_down" },
            { 301, "Coin_bronze" },
        };

        private Rectangle hitBox;
        public Rectangle HitBox
        {
            get
            {
                return hitBox;
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
            hitBox = new Rectangle(_pos.ToPoint(), new Point(_tileSize, _tileSize));
        }
    }
}
