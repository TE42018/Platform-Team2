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
    class Chunk
    {
        private List<Tile> tiles;
        private Vector2 offset;
        private Rectangle hitBox;

        private Vector2 pos;

        public const int width = 16;
        public const int height = 16; //in tiles
        public const int area = width * height;

        public Chunk(Vector2 _pos, int _tileSize)
        {
            pos = _pos;
            tiles = new List<Tile>();
            hitBox = new Rectangle(_pos.ToPoint(), new Point(width * _tileSize, height * _tileSize));
        }

        public Tile[] GetTiles()
        {
            return tiles.ToArray();
        }

        public void AddTile(Vector2 _pos, int _tileSize, int _gid)
        {
            tiles.Add(new Tile(_pos, _tileSize, _gid));
        }
        
    }
}
