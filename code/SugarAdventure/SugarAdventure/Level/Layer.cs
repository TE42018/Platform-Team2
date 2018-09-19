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
    public class Layer
    {
        Tile[,] Tiles;
        Texture2D[] layerImageBuffer;
        int[] tileDrawingBlacklist = { 403, 406, 407, 199, 401, 402 };
        int[] tileBoxBlacklist = { 406, 407, 199, 401, 402, 302 };
        int[] tintedTiles = { 145 };

        private int layerWidth;
        public int LayerWidth
        {
            get
            {
                return layerWidth;
            }
        }
        private int layerHeight;
        public int LayerHeight
        {
            get
            {
                return layerHeight;
            }
        }
        private static int layerWidthInTiles, layerHeightInTiles;
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

            //layerImageBuffer = BuildLayerImages(layerData.Tiles.ToArray(), out layerImageBuffer);

            //if (layerData.Name == "Ground")
            //{
                PopulateTiles(layerData.Tiles.ToArray());
            //}
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

                Texture2D tileTexture = null;
                TmxTileset tileTileset = FindTileset(tile.Gid);
                if (tileTileset != null)
                {
                    Texture2D tileTilesheet = GetTilesheet(tileTileset, out tileTilesheet);
                    tileTexture = GetTileTexture(tile.Gid, tileTileset, tileTilesheet, out tileTexture);
                }

                Tiles[tile.X, tile.Y] = new Tile(tilePos, tileTexture, tileWidth, tile.Gid);
            }
        }

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

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                for (int x = 0; x < Tiles.GetLength(0); x++)
                {
                    Tile t = Tiles[x, y];
                    
                    if (t == null || t.Texture == null)
                        continue;

                    if (!tileDrawingBlacklist.Contains(t.Gid))
                    {
                        if (layerData.Name != "Ground" && tintedTiles.Contains(t.Gid))
                            t.Draw(spriteBatch, Color.Brown);
                        else
                            t.Draw(spriteBatch, Color.White);
                    }
                }
            }
            //spriteBatch.Draw(layerImageBuffer[0], Vector2.Zero, Color.White);
            //spriteBatch.Draw(layerImageBuffer[1], new Vector2(layerImageBuffer[0].Width, 0), Color.White);
        }
    }
}
