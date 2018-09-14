using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SugarAdventure
{
    class GroundLayer : Layer
    {
        List<Chunk> chunks;

        public GroundLayer(TmxLayer _layer, TmxTileset[] _tilesets, int _width, int _height) : base(_layer, _tilesets, _width, _height)
        {
            //Console.WriteLine(_tilesets[3].FirstGid);
        }
    }
}
