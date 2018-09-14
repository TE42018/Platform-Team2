using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    class Player
    {
        private Level level;
        private Texture2D texture;
        private Rectangle Bounds;
        private Rectangle hitbox;
        public Rectangle Hitbox
        {
            get
            {
                return hitbox;
            }
        }

        Vector2 position;
        Vector2 velocity;
        Vector2 gravity;

        Tile[,] tilesToCheck;

        bool hasJumped;
        bool canJump = false;
        bool isClimbing;
        bool isOnGround;
        bool CanClimb;


        public Action CurrentAction { get; set; }

        public Player(Vector2 _position, Level _level)
        {
            position = _position;
            velocity = Vector2.Zero;
            gravity = new Vector2(0, 9.81f*300);
            hasJumped = true;
            level = _level;
            tilesToCheck = level.GetLayer("Ground").GetTiles();
        }

        public void LoadContent()
        {
            texture = Game1.contentManager.Load<Texture2D>(@"Platformer assets (1330 assets)/Extra animations and enemies (165 assets)/Alien sprites/alienPink");
            hitbox = new Rectangle(position.ToPoint(), new Point(texture.Width, texture.Height + 1));
        }

        public void Update(GameTime gameTime)
        {
            ///Move player on X axis
            //position.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            ////Console.WriteLine(position);
            //hitbox.Location = position.ToPoint();

            /////Check if there is a collision
            /////Move the player on the X axis

            ////Check collision on Left and Right sides
            //for (int i = 0; i < tilesToCheck.Length; i++)
            //{
            //    Tile t = tilesToCheck[i];
            //    if (t == null)
            //        continue;
            //    Rectangle tileBox = t.Hitbox;
            //    string tileType = t.Type;
            //    if (hitbox.Intersects(tileBox))
            //    {
            //        if (tileType == "block")
            //        {
            //            if (velocity.X > 0)
            //            {
            //                if (position.X + hitbox.Width > tileBox.Left)
            //                {
            //                    position.X = tileBox.Left - hitbox.Width;
            //                    velocity.X = 0;
            //                    //canJump = true;
            //                }
            //            }
            //            else if (velocity.X < 0)
            //            {
            //                if (position.X < tileBox.Right)
            //                {
            //                    position.X = tileBox.Right;
            //                    velocity.X = 0;
            //                }
            //            }
            //        }
            //        else if(tileType == "slope_up")
            //        {
            //            //y1 = y + (x1 - x)
            //            position.Y = tileBox.Bottom - (hitbox.Right - tileBox.Left) - hitbox.Height;
            //            Console.WriteLine(hitbox.Right - tileBox.Left);
            //        }
            //        hitbox.Location = position.ToPoint();
            //    }
            //}

            CollideX(gameTime);
            CollideY(gameTime);
            level.GetLayer("Ground").GetTileRows(hitbox);
            ///Move player on the Y axis
            //position.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //hitbox.Location = position.ToPoint();
            ////Console.WriteLine(position);
            /////Check if there is a collision
            /////Move the player on the Y axis

            ////Check collision on Top and Bottom sides
            //for (int i = 0; i < tilesToCheck.Length; i++)
            //{
            //    Tile t = tilesToCheck[i];
            //    if (t == null)
            //        continue;
            //    Rectangle tileBox = t.Hitbox;
            //    string tileType = t.Type;
            //    if (hitbox.Intersects(tileBox))
            //    {
            //        if (tileType == "block")
            //        {
            //            if (velocity.Y > 0)
            //            {
            //                if (position.Y + velocity.Y + hitbox.Height > tileBox.Top)
            //                {
            //                    position.Y = tileBox.Top - hitbox.Height;
            //                    velocity.Y = 0;
            //                    canJump = true;
            //                }
            //            }
            //            else if (velocity.Y < 0)
            //            {
            //                if (position.Y < tileBox.Bottom)
            //                {
            //                    position.Y = tileBox.Bottom;
            //                    velocity.Y = 0;
            //                }
            //            }
            //        }
            //        else if(tileType == "slope_up")
            //        { 
            //            position.Y = tileBox.Bottom - (hitbox.Right - tileBox.Left) - hitbox.Height;
            //            velocity.Y = 0;
            //        }
            //        hitbox.Location = position.ToPoint();
            //    }
            //}

            position = Vector2.Clamp(position, Vector2.Zero, new Vector2(Bounds.Width, Bounds.Height));

            velocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void CollideX(GameTime gameTime)
        {
            position.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            hitbox.Location = position.ToPoint();

            //int checkX;
            //int checkDirection = 0;
            //if (velocity.X > 0)
            //{
            //    checkX = hitbox.Right;
            //    checkDirection = 1;
            //}
            //else if (velocity.X < 0)
            //{
            //    checkX = hitbox.Left;
            //    checkDirection = -1;
            //}

            //int[] tileRows = level.GetLayer("Ground").GetTileRows(hitbox);

            ////int 
            for (int y = 0; y < tilesToCheck.GetLength(1); y++)
            {
                for (int x = 0; x < tilesToCheck.GetLength(0); x++)
                {
                    Tile t = tilesToCheck[x, y];
                    if (t == null)
                        continue;
                    string tileType = t.Type;
                    Rectangle tileBox = t.Hitbox;

                    if (hitbox.Intersects(tileBox))
                    {
                        if (tileType == "block")
                        {
                            if (velocity.X > 0)
                            {
                                    if (hitbox.Right > tileBox.Left)
                                    {
                                        position.X = tileBox.Left - hitbox.Width;
                                        velocity.X = 0;
                                    }
                                
                                    
                            }
                            else if (velocity.X < 0)
                            {
                                if (hitbox.Left < tileBox.Right)
                                {
                                    position.X = tileBox.Right;
                                    velocity.X = 0;
                                }
                            }

                        }
                        else if (tileType == "slope_up")
                        {
                            //position.Y = tileBox.Bottom - (hitbox.Right - tileBox.Left) - hitbox.Height;
                            Point collisionPoint = new Point(hitbox.Right - hitbox.Width / 2, hitbox.Bottom - 2);
                            //Console.WriteLine (PointInRect(collisionPoint, tileBox)) ;

                            //y1 = y + (x1 - x)
                            float y1 = tileBox.Bottom - (collisionPoint.X - tileBox.Left);
                            position.Y = y1 - hitbox.Height;
                            //Console.WriteLine(y1);
                        }
                    }
                }
            }

            hitbox.Location = position.ToPoint();
        }

        private void CollideY(GameTime gameTime)
        {
            position.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            hitbox.Location = position.ToPoint();

            for (int y = 0; y < tilesToCheck.GetLength(1); y++)
            {
                for (int x = 0; x < tilesToCheck.GetLength(0); x++)
                {
                    Tile t = tilesToCheck[x, y];
                    if (t == null || t.Hitbox == null)
                        continue;
                    

                    string tileType = t.Type;
                    Rectangle tileBox = t.Hitbox;
                    if (hitbox.Intersects(tileBox))
                    {
                        //velocity.Y = 0;
                        if (tileType == "block")
                        {
                                if (velocity.Y > 0)
                                {
                                    if (hitbox.Bottom > tileBox.Top)
                                    {
                                        position.Y = tileBox.Top - hitbox.Height;
                                        velocity.Y = 0;
                                        canJump = true;
                                    }
                                }
                                else if (velocity.Y < 0)
                                {
                                    if (hitbox.Top < tileBox.Bottom)
                                    {
                                        position.Y = tileBox.Bottom;
                                        velocity.Y = 0;
                                    }
                                }

                        }
                        else if (tileType == "slope_up")
                        {
                            //position.Y = tileBox.Bottom - (hitbox.Right - tileBox.Left) - hitbox.Height;
                            Point collisionPoint = new Point(hitbox.Right - hitbox.Width / 2, hitbox.Bottom - 2);
                            //Console.WriteLine (PointInRect(collisionPoint, tileBox)) ;

                            //y1 = y + (x1 - x)
                            if (PointInRect(collisionPoint, tileBox))
                            {
                                Console.WriteLine("HItting");
                                float y1 = tileBox.Bottom - (collisionPoint.X - tileBox.Left);
                                if (collisionPoint.Y < y1)
                                {
                                    position.Y = y1 - hitbox.Height - 2;
                                    velocity.Y = 0;
                                }
                            }
                            canJump = true;
                            //velocity.Y = 0;
                            //Console.WriteLine(y1);
                        }
                    }
                }
            }

            hitbox.Location = position.ToPoint();
        }

        public bool PointInRect(Point point, Rectangle rectangle)
        {
            return point.X > rectangle.Left && point.X < rectangle.Right &&
                   point.Y > rectangle.Top && point.Y < rectangle.Bottom;
        }

        public void SetBoundingLevel(Level _boundingLevel)
        {
            int boundWidth = _boundingLevel.GetLayer("Ground").LayerWidth;
            int boundHeight = _boundingLevel.GetLayer("Ground").LayerHeight;
            Bounds = new Rectangle(Point.Zero, new Point(boundWidth - hitbox.Width, boundHeight - hitbox.Height));
        }

        public void Jump()
        {
            if (canJump)
                velocity.Y = -1000;
            canJump = false;
        }

        public void MoveLeft()
        {
            velocity.X = -300;
        }

        public void MoveRight()
        {
            velocity.X = 300;
        }

        public void Stop()
        {
            velocity.X = 0;
        }

        public void ClimbUp()
        {
            if (CanClimb)
            {

            }
        }
        public void ClimbDown()
        {
            if (CanClimb && !isOnGround)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
