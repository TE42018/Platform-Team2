using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SugarAdventure
{
    public class Enemy : IEntity, IEnemy
    {
        public string Type { get; set; }
        public Level Level { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Hitbox { get; set; }
        public int AttackDamage { get; set; }
        public Texture2D Texture { get; set; }
        public Point Size { get; set; }

        public Vector2 Velocity { get; private set; }
        public int WalkSpeed { get; private set; }

        private int direction;

        public Enemy(Vector2 _pos, string _type)
        {
            Type = "enemy_" + _type;
            Position = _pos;
            direction = -1;

            switch (Type)
            {
                case "enemy_slime":
                    Texture = SugarGame.entityManager.Textures[(int)EntityTexture.Slime_walk1];
                    Size = new Point(Texture.Width, Texture.Height);
                    Hitbox = new Rectangle(Position.ToPoint(), Size);
                    AttackDamage = 1;
                    WalkSpeed = 200;
                    break;
                case "enemy_snail":
                    Texture = SugarGame.entityManager.Textures[(int)EntityTexture.Snail_walk1];
                    Size = new Point(Texture.Width, Texture.Height);
                    Hitbox = new Rectangle(Position.ToPoint(), Size);
                    AttackDamage = 1;
                    WalkSpeed = 100;
                    break;
            }
        }

        public void Update(GameTime gameTime, Level level)
        {

            Walk();
            CheckCollision(gameTime, level);
        }



        public void Walk()
        {
            Velocity = new Vector2(direction * WalkSpeed, 0);
            //Position += Velocity;
            Hitbox = new Rectangle(Position.ToPoint(), Size);
            //Update walk animation
        }

        private void CheckCollision(GameTime gameTime, Level level)
        {
            Velocity = Velocity + new Vector2(0, 9.81f * 300);

            Tile[,] tiles = level.GetLayer("Ground").GetTiles();
            CollideX(gameTime, tiles);
            CollideY(gameTime, tiles);
            
            int boundWidth = level.GetLayer("Ground").LayerWidth;
            int boundHeight = level.GetLayer("Ground").LayerHeight;

            if (Position.X + Size.X > boundWidth || Position.X < 0 )
                direction *= -1;
        }

        private void CollideX(GameTime gameTime, Tile[,] tilesToCheck)
        {
            Position = new Vector2(Position.X + Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
            Hitbox = new Rectangle(Position.ToPoint(), Size);

            for (int y = 0; y < tilesToCheck.GetLength(1); y++)
            {
                for (int x = 0; x < tilesToCheck.GetLength(0); x++)
                {
                    Tile t = tilesToCheck[x, y];
                    if (t == null)
                        continue;
                    string tileType = t.Type.ToLower();
                    Rectangle tileBox = t.Hitbox;

                    if (Hitbox.Intersects(tileBox))
                    {
                        if (tileType == "block" || tileType == "platform" || tileType == "slope_up" || tileType == "slope_down")
                        {
                            if (Velocity.X > 0)
                            {
                                if (Hitbox.Right > tileBox.Left)
                                {
                                    Position = new Vector2(tileBox.Left - Hitbox.Width, Position.Y);
                                    Velocity = new Vector2(0, Velocity.Y);
                                }


                            }
                            else if (Velocity.X < 0)
                            {
                                if (Hitbox.Left < tileBox.Right)
                                {
                                    Position = new Vector2(tileBox.Right, Position.Y);
                                    Velocity = new Vector2(0, Velocity.Y);
                                }
                            }
                            direction *= -1;
                        }
                    }
                }
            }
            Hitbox = new Rectangle(Position.ToPoint(), Size);
        }

        private void CollideY(GameTime gameTime, Tile[,] tilesToCheck)
        {
            Position = new Vector2(Position.X, Position.Y + Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            Hitbox = new Rectangle(Position.ToPoint(), Size);

            for (int y = 0; y < tilesToCheck.GetLength(1); y++)
            {
                for (int x = 0; x < tilesToCheck.GetLength(0); x++)
                {
                    Tile t = tilesToCheck[x, y];
                    if (t == null || t.Hitbox == null)
                        continue;

                    string tileType = t.Type.ToLower();
                    Rectangle tileBox = t.Hitbox;
                    if (Hitbox.Intersects(tileBox))
                    {
                        if (tileType == "block" || tileType == "platform" || tileType == "slope_up" || tileType == "slope_down")
                        {
                            if (Velocity.Y > 0)
                            {
                                if (Hitbox.Bottom > tileBox.Top)
                                {
                                    Position = new Vector2(Position.X, tileBox.Top - Hitbox.Height);
                                    Velocity = new Vector2(Velocity.X, 0);
                                }
                            }
                            else if (Velocity.Y < 0)
                            {
                                if (Hitbox.Top < tileBox.Bottom)
                                {
                                    Position = new Vector2(Position.X, tileBox.Bottom);
                                    Velocity = new Vector2(Velocity.X, 0);
                                }
                            }

                        }
                        else if(tileType == "ladder")
                        { 
                            direction *= -1;
                        }
                    }
                }
            }

            Hitbox = new Rectangle(Position.ToPoint(), Size);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Texture != null)
                spriteBatch.Draw(texture: Texture, position: Position, color: Color.White, effects: direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
    }
}
