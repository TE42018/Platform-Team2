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
    public class Player
    {
        private SoundEffect jumpEffect;
        private HashSet<IPickupable> inventory;
        private Level level;
        private Texture2D texture;
        private Vector2 originalPosition;
        private Rectangle bounds;
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

        //bool hasJumped;
        bool canJump = false;
        bool isClimbing;
        //bool isOnGround;
        bool canClimb;


        public Actions CurrentAction { get; set; }

        public Player(Vector2 _position, Level _level)
        {
            position = _position;
            originalPosition = position;
            velocity = Vector2.Zero;
            gravity = new Vector2(0, 9.81f*300);
            inventory = new HashSet<IPickupable>();
            //hasJumped = true;
            level = _level;
            tilesToCheck = level.GetLayer("Ground").GetTiles();
        }

        public void LoadContent()
        {
            texture = SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Aliens\alienPink");
            jumpEffect = SugarGame.contentManager.Load<SoundEffect>("jump");
            hitbox = new Rectangle(position.ToPoint(), new Point(texture.Width, texture.Height));
        }

        public void Update(GameTime gameTime)
        {

            CollideX(gameTime);
            CollideY(gameTime);

            position = Vector2.Clamp(position, Vector2.Zero, new Vector2(bounds.Width, bounds.Height));

            if (!isClimbing)
                velocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Pickup(IPickupable item)
        {
            inventory.Add(item);
        }

        public bool HasItem(IPickupable item)
        {
            foreach (IPickupable i in inventory)
            {
                if (i == item)
                {

                    return true;
                }
            }
            return false;
        }
        public bool HasItem(string itemName)
        {
            foreach (IPickupable i in inventory)
            {
                if ((i as IEntity).Type == itemName)
                {

                    return true;
                }
            }
            return false;
        }

        public void RemoveItem(IPickupable item)
        {
            inventory.Remove(item);
        }

        public void Reset()
        {
            position = new Vector2(position.X, position.Y - 420);
        }

        private void CollideX(GameTime gameTime)
        {
            position.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            hitbox.Location = position.ToPoint();
            
            for (int y = 0; y < tilesToCheck.GetLength(1); y++)
            {
                for (int x = 0; x < tilesToCheck.GetLength(0); x++)
                {
                    Tile t = tilesToCheck[x, y];
                    if (t == null)
                        continue;
                    string tileType = t.Type.ToLower();
                    Rectangle tileBox = t.Hitbox;

                    if (hitbox.Intersects(tileBox))
                    {
                        if (tileType == "block" || tileType == "platform")
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
                            int deltaX = hitbox.Right - tileBox.Left;
                            int deltaY = Math.Min(deltaX, tileBox.Height);

                            if (velocity.X > 0)
                            {
                                if (hitbox.Bottom > tileBox.Bottom - deltaY)
                                {
                                    position.Y = tileBox.Bottom - deltaY - hitbox.Height;
                                }
                            }
                        }
                        else if(tileType == "slope_down")
                        {
                            int deltaX = tileBox.Right - hitbox.Left;
                            int deltaY = Math.Min(deltaX, tileBox.Height);

                            if (hitbox.Bottom > tileBox.Bottom - deltaY)
                            {
                                position.Y = tileBox.Bottom - deltaY - hitbox.Height;
                            }
                        }

                        if(tileType == "ladder")
                        {
                            canClimb = true;
                        }
                    }
                }
            }
            //Console.WriteLine(canClimb);
            hitbox.Location = position.ToPoint();
        }

        private void CollideY(GameTime gameTime)
        {
            position.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            hitbox.Location = position.ToPoint();

            canClimb = false;

            for (int y = 0; y < tilesToCheck.GetLength(1); y++)
            {
                for (int x = 0; x < tilesToCheck.GetLength(0); x++)
                {
                    Tile t = tilesToCheck[x, y];
                    if (t == null || t.Hitbox == null)
                        continue;
                    
                    string tileType = t.Type.ToLower();
                    Rectangle tileBox = t.Hitbox;
                    if (hitbox.Intersects(tileBox))
                    {
                        //Console.WriteLine(tileType);
                        if (tileType == "block" || tileType == "platform")
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
                            int deltaX = hitbox.Right - tileBox.Left;
                            int deltaY = Math.Min(deltaX, tileBox.Height);

                            if (hitbox.Bottom > tileBox.Bottom - deltaY)
                            {
                                position.Y = tileBox.Bottom - deltaY - hitbox.Height;
                                velocity.Y = 0;
                                canJump = true;
                            }

                        }
                        else if (tileType == "slope_down")
                        {
                            int deltaX = tileBox.Right - hitbox.Left;
                            int deltaY = Math.Min(deltaX, tileBox.Height);

                            if (hitbox.Bottom > tileBox.Bottom - deltaY)
                            {
                                position.Y = tileBox.Bottom - deltaY - hitbox.Height;
                                velocity.Y = 0;
                                canJump = true;
                            }
                        }
                        else if(tileType == "lock_green")
                        {
                            if (HasItem("key_green")){
                                tilesToCheck[x, y] = new Tile(tilesToCheck[x, y], tilesToCheck[x, y + 1]);
                            }
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

                        if (tileType == "ladder")
                        {
                            canClimb = true;
                        }
                    }
                }
            }

            if (!canClimb)
                isClimbing = false;

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
            bounds = new Rectangle(Point.Zero, new Point(boundWidth - hitbox.Width, boundHeight - hitbox.Height));
        }

        public void Jump()
        {
            if (isClimbing)
            {
                isClimbing = false;
                velocity.Y = -700;
            }
            if (canJump)
            {
                jumpEffect.Play();
                velocity.Y = -1000;
            }
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
            if (canClimb)
            {
                isClimbing = true;
                //Console.WriteLine("Climbing up");
                velocity.Y = -300;
            }
        }
        public void ClimbDown()
        {
            if (isClimbing)
            {
                velocity.Y = 300;
            }
        }
        public void StopClimbing()
        {
            if(isClimbing)
                velocity.Y = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
