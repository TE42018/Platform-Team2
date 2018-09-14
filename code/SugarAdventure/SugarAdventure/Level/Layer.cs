using Microsoft.Xna.Framework;
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
    class Layer
    {
        Chunk[,] chunks;
        Tile[,] Tiles;
        List<IEntity> entities;
        Texture2D[] layerImageBuffer;
        int[] tileDrawingBlacklist = { };//{ 406, 407, 199, 401, 402, 302 };
        int[] tileBoxBlacklist = { 406, 407, 199, 401, 402, 302 };
        int[] tintedTiles = { 145 };

        private static int layerWidth;
        public int LayerWidth
        {
            get
            {
                return layerWidth;
            }
        }
        private static int layerHeight;
        public int LayerHeight
        {
            get
            {
                return layerHeight;
            }
        }
        private static int layerWidthInTiles, layerHeightInTiles, layerWidthInChunks, layerHeightInChunks;
        private static int tileWidth, tileHeight;

        private TmxLayer layerData;
        private TmxTileset[] tilesets;

        public Layer(TmxLayer _layer, TmxTileset[] _tilesets, int _width, int _height)
        {
            layerData = _layer;
            tilesets = _tilesets;

            layerWidth = _width;
            layerHeight = _height;

            tileWidth = tilesets[0].TileWidth;
            tileHeight = tilesets[0].TileHeight;

            layerWidthInTiles = layerWidth / tileWidth;
            layerHeightInTiles = layerHeight / tileHeight;

            Tiles = new Tile[layerWidthInTiles, layerHeightInTiles];
            //layerWidthInChunks = layerWidth / Chunk.width / tileWidth;
            //layerHeightInChunks = layerHeight / Chunk.height / tileHeight;

            layerImageBuffer = BuildLayerImages(layerData.Tiles.ToArray(), out layerImageBuffer);

            if (layerData.Name == "Ground")
            {
                PopulateTiles(layerData.Tiles.ToArray());
            }
        }

        private Texture2D[] BuildLayerImages(TmxLayerTile[] _layerTiles, out Texture2D[] _layerImageBuffer)
        {
            GraphicsDevice tempGraphicsDevice = Game1.graphics.GraphicsDevice;
            SpriteBatch tempSpriteBatch = new SpriteBatch(tempGraphicsDevice);

            RenderTarget2D leftTempTarget = new RenderTarget2D(tempGraphicsDevice, tileWidth * (layerWidthInTiles / 2), layerHeight);
            RenderTarget2D rightTempTarget = new RenderTarget2D(tempGraphicsDevice, tileWidth * (layerWidthInTiles / 2), layerHeight);

            _layerImageBuffer = new Texture2D[2];
            _layerImageBuffer[0] = new Texture2D(tempGraphicsDevice, leftTempTarget.Width, leftTempTarget.Height);
            _layerImageBuffer[1] = new Texture2D(tempGraphicsDevice, rightTempTarget.Width, rightTempTarget.Height);

            //Left half
            tempGraphicsDevice.SetRenderTarget(leftTempTarget);
            tempGraphicsDevice.Clear(Color.Transparent);

            tempSpriteBatch.Begin();
            for (int x = 0; x < layerWidth / tileWidth / 2; x++) //Width in tiles
            {
                for (int y = 0; y < layerHeight / tileHeight; y++) //Height in tiles
                {
                    int index = x + (y * (layerWidth / tileWidth));
                    int tileGid = layerData.Tiles[index].Gid;

                    if (tileGid == 0 || tileDrawingBlacklist.Contains(tileGid))
                        continue;

                    TmxTileset tileset = FindTileset(tileGid);
                    Texture2D tilesheet = GetTilesheet(tileset, out tilesheet);
                    Texture2D tileTexture = GetTileTexture(tileGid, tileset, tilesheet, out tileTexture);

                    int tileX = x * tileWidth;
                    int tileY = y * tileHeight;

                    if (layerData.Name != "Ground" && tintedTiles.Contains(tileGid))
                        tempSpriteBatch.Draw(tileTexture, new Vector2(tileX, tileY), Color.Brown);
                    else
                        tempSpriteBatch.Draw(tileTexture, new Vector2(tileX, tileY), Color.White);
                }
            }
            tempSpriteBatch.End();

            tempGraphicsDevice.SetRenderTarget(null);

            Color[] leftData = new Color[leftTempTarget.Width * leftTempTarget.Height];
            leftTempTarget.GetData(leftData);
            _layerImageBuffer[0].SetData(leftData);

            //Right half
            tempGraphicsDevice.SetRenderTarget(rightTempTarget);
            tempGraphicsDevice.Clear(Color.Transparent);
            tempSpriteBatch.Begin();
            for (int x = layerWidth / tileWidth / 2; x < layerWidth / tileWidth; x++) //Width in tiles
            {
                for (int y = 0; y < layerHeight / tileHeight; y++) //Height in tiles
                {
                    int index = x + (y * (layerWidth / tileWidth));
                    int tileGid = layerData.Tiles[index].Gid;

                    if (tileGid == 0 || tileDrawingBlacklist.Contains(tileGid))
                        continue;

                    TmxTileset tileset = FindTileset(tileGid);
                    Texture2D tilesheet = GetTilesheet(tileset, out tilesheet);
                    Texture2D tileTexture = GetTileTexture(tileGid, tileset, tilesheet, out tileTexture);

                    int tileX = x % (layerWidth / tileWidth / 2) * tileWidth;
                    int tileY = y * tileHeight;

                    if (layerData.Name != "Ground" && tintedTiles.Contains(tileGid))
                        tempSpriteBatch.Draw(tileTexture, new Vector2(tileX, tileY), Color.Brown);
                    else
                        tempSpriteBatch.Draw(tileTexture, new Vector2(tileX, tileY), Color.White);
                }
            }
            tempSpriteBatch.End();
            tempGraphicsDevice.SetRenderTarget(null);

            Color[] rightData = new Color[rightTempTarget.Width * rightTempTarget.Height];
            rightTempTarget.GetData(rightData);
            _layerImageBuffer[1].SetData(rightData);

            return _layerImageBuffer;
        }

        private Texture2D GetTileTexture(int _tileGid, TmxTileset _tileset, Texture2D _tilesheet, out Texture2D _outputTexture)
        {
            int tileIndex = _tileGid - _tileset.FirstGid;

            int cols = _tilesheet.Width / _tileset.TileWidth;
            int rows = _tilesheet.Height / _tileset.TileHeight;

            int indexX = tileIndex % cols;
            int indexY = tileIndex / cols;

            int tileX = indexX * _tileset.TileWidth;
            int tileY = indexY * _tileset.TileHeight;

            Rectangle tileRect = new Rectangle(tileX, tileY, _tileset.TileWidth, _tileset.TileHeight);

            Color[] rawData = new Color[tileRect.Width * tileRect.Height];
            _tilesheet.GetData(0, tileRect, rawData, 0, tileRect.Width * tileRect.Height);

            _outputTexture = new Texture2D(Game1.graphics.GraphicsDevice, tileRect.Width, tileRect.Height);
            _outputTexture.SetData(rawData);

            return _outputTexture;
        }

        private Texture2D GetTilesheet(TmxTileset tileset, out Texture2D _texture)
        {
            string tilesheetImageSource = tileset.Image.Source.Split('.')[0];

            _texture = Game1.contentManager.Load<Texture2D>(@"." + tilesheetImageSource);

            return _texture;
        }

        private TmxTileset FindTileset(int gid)
        {
            TmxTileset tilesetResult = null;

            if (gid == 0)
                return null;

            for (int i = 0; i < tilesets.Length; i++)
            {
                TmxTileset currentTileset = tilesets[i];
                if (gid >= currentTileset.FirstGid)
                {
                    tilesetResult = currentTileset;
                }
            }

            return tilesetResult;
        }

        private void PopulateTiles(TmxLayerTile[] _tiles)
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                TmxLayerTile tile = _tiles[i];

                Vector2 tilePos;
                tilePos.X = tile.X * tileWidth;
                tilePos.Y = tile.Y * tileHeight;

                Tiles[tile.X, tile.Y] = new Tile(tilePos, tileWidth, tile.Gid);
            }
        }
        //private void PopulateChunks(TmxLayerTile[] _tiles)
        //{
        //    chunks = new Chunk[layerWidthInChunks, layerHeightInChunks];

        //    for (int x = 0; x < chunks.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < chunks.GetLength(1); y++)
        //        {
        //            chunks[x, y] = new Chunk(new Vector2(x * Chunk.width * tileWidth, y * Chunk.height * tileHeight), tileWidth);
        //        }
        //    }

        //    for (int i = 0; i < _tiles.Length; i++)
        //    {
        //        TmxLayerTile tile = _tiles[i];

        //        if (tile.Gid == 0)
        //            continue;

        //        Vector2 chunkIndex;
        //        chunkIndex.X = (int)(tile.X / (float)layerWidthInTiles * layerWidthInChunks);
        //        chunkIndex.Y = (int)(tile.Y / (float)layerHeightInTiles * layerHeightInChunks);

        //        Vector2 tilePos;
        //        tilePos.X = tile.X * tileWidth;
        //        tilePos.Y = tile.Y * tileHeight;
        //        //Console.WriteLine(tilePos);

        //        chunks[(int)chunkIndex.X, (int)chunkIndex.Y].AddTile(tilePos, tileWidth, tile.Gid);
        //    }
        //    //Console.WriteLine(chunks[2, 0]);
        //    //Console.WriteLine(chunks.GetLength(0) + ", " + chunks.GetLength(1));
        //}

        public Tile[,] GetTiles()
        {
            return Tiles;
        }

        public int[] GetTileRows(Rectangle _hitbox)
        {
            int y1 = _hitbox.Top / tileHeight;
            int y2 = _hitbox.Bottom / tileHeight;

            int[] rows = new int[y2 - y1 + 1];

            for (int i = 0; i < y2-y1 + 1; i++)
            {
                rows[i] = y1 + i;
            }

            return rows;
        }

        //public Tile[] GetAllTiles()
        //{
        //    if (chunks.Length == 0)
        //        return null;

        //    int tileArraySize = 0;
        //    for (int i = 0; i < chunks.Length; i++)
        //    {
        //        Vector2 chunkIndex;
        //        chunkIndex.X = i % layerWidthInChunks;
        //        chunkIndex.Y = i / layerWidthInChunks;
        //        Chunk c = chunks[(int)chunkIndex.X, (int)chunkIndex.Y];
        //        tileArraySize += c.GetTiles().Length;
        //    }

        //    Tile[] tiles = new Tile[tileArraySize];

        //    for (int i = 0; i < tiles.Length; i++)
        //    {

        //    }

        //    int n = 0;
        //    for (int i = 0; i < chunks.Length; i++)
        //    {
        //        Vector2 chunkIndex;
        //        chunkIndex.X = i % layerWidthInChunks;
        //        chunkIndex.Y = i / layerWidthInChunks;

        //        Chunk chunk = chunks[(int)chunkIndex.X, (int)chunkIndex.Y];

        //        Tile[] chunkTiles = chunk.GetTiles();

        //        for (int j = 0; j < chunkTiles.Length; j++)
        //        {
        //            Tile tile = chunkTiles[j];
        //            if (!tileBoxBlacklist.Contains(tile.Gid))
        //                tiles[n] = tile;
        //            n++;
        //        }
        //    }
        //    return tiles;
        //}

        //public void GetTiles(Chunk _chunk)
        //{
        //    Tile[] tile = _chunk.GetTiles();
        //}

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(layerImageBuffer[0], Vector2.Zero, Color.White);
            spriteBatch.Draw(layerImageBuffer[1], new Vector2(layerImageBuffer[0].Width, 0), Color.White);
            //Draw entities

        }
    }
}
