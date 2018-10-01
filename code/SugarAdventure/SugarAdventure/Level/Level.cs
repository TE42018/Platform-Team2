using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SugarAdventure
{
    public class Level
    {
        private TmxMap levelData;
        private TmxTileset[] tileSets;


        private Vector2 startPosition;
        public Vector2 StartPosition
        {
            get
            {
                return startPosition;
            }
        }

        private List<Layer> layers = new List<Layer>();
        public List<Layer> Layers
        {
            get
            {
                return layers;
            }
        }

        private static int width, height; //in chunks

        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Level(string _levelSource, string _levelFolderName, string _levelFileName, int _levelNumber)
        {
            levelData = GetLevelFile(_levelSource, _levelFolderName, _levelFileName);
            
            //Console.WriteLine(levelData.ImageLayers);
            width = levelData.Width * levelData.TileWidth; 
            height = levelData.Height * levelData.TileHeight; // size in pixels    (Maybe get in layer instead)

            startPosition = new Vector2(70 * 0, height / 2);

            //Populate array of tilesets
            int tileSetCount = levelData.Tilesets.Count;
            tileSets = new TmxTileset[tileSetCount];
            for(int i = 0; i < tileSetCount; i++)
            {
                tileSets[i] = levelData.Tilesets[i];
            }

            int layerCount = levelData.Layers.Count();
            for (int i = 0; i < layerCount; i++)
            {
                TmxLayer currentLayer = levelData.Layers[i];                
                layers.Add(new Layer(currentLayer, tileSets, width, height, _levelNumber));
            }
        }

        private TmxMap GetLevelFile(string _levelSource, string _levelFolderName, string _levelName)
        {
            string levelFilePath = String.Format("{0}{1}\\{2}.tmx", _levelSource, _levelFolderName, _levelName);
            if (File.Exists(levelFilePath))
            {
                return new TmxMap(levelFilePath);
            }
            else
            {
                throw new FileNotFoundException("Couldn't find level file");
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach(Layer l in layers)
            {
                l.Draw(_spriteBatch);
            }
        }

        public Layer GetLayer(string _layerName)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if (levelData.Layers[i].Name == _layerName)
                    return layers[i];
            }
            return null;
        }
    }
}
