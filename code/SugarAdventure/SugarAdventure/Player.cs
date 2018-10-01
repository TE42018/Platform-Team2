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
        AnimationManager animator;
        Animation standAnim;
        Animation walkAnim;
        Animation jumpAnim;
        Animation climbAnim;

        private SoundEffect jumpEffect;
        private HashSet<IPickupable> inventory;
        private Texture2D texture;
        private Vector2 originalPosition;
        private float damageCounter = 0;
        private Rectangle bounds;
        private Rectangle hitbox;
        public Rectangle Hitbox
        {
            get
            {
                return hitbox;
            }
        }
        private Level level;
        public Level Level
        {
            get
            {
                return level;
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

        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Money { get; set; }
        public Actions CurrentAction { get; set; }

        public Player(Level _level, int _maxHealth)
        {
            level = _level;
            MaxHealth = _maxHealth;
            Health = MaxHealth;
            position = level.StartPosition;
            originalPosition = position;
            velocity = Vector2.Zero;
            gravity = new Vector2(0, 9.81f*300);
            inventory = new HashSet<IPickupable>();
            //hasJumped = true;
            tilesToCheck = level.GetLayer("Ground").GetTiles();
        }

        public void LoadContent()
        {
            standAnim = new Animation(SugarGame.contentManager.Load<Texture2D>("standPink"), 1);
            animator = new AnimationManager(standAnim);

            walkAnim = new Animation(SugarGame.contentManager.Load<Texture2D>("walkPink"), 10);
            walkAnim.FrameSpeed = 0.04f;

            climbAnim = new Animation(SugarGame.contentManager.Load<Texture2D>("climbPink"), 2);
            jumpAnim = new Animation(SugarGame.contentManager.Load<Texture2D>("alienPink_jump"), 1);

            texture = SugarGame.contentManager.Load<Texture2D>(@".\Platformer_assets\Aliens\alienPink");
            texture = standAnim.Texture;
            jumpEffect = SugarGame.contentManager.Load<SoundEffect>("jump");
            hitbox = new Rectangle(position.ToPoint(), new Point(texture.Width, texture.Height));
        }

        public void Update(GameTime gameTime)
        {
            damageCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;




            if (isClimbing)
            {
                animator.Play(climbAnim);
                switch (CurrentAction)
                {
                    case Actions.Up: velocity.Y = -300f; animator.Update(gameTime); break;
                    case Actions.Down: velocity.Y = 300f; animator.Update(gameTime); break;
                    default: velocity.Y = 0; break;
                }
            }
            else if (!canJump)
            {
                animator.Play(jumpAnim);
            }
            else
            {
                animator.Play(walkAnim);
                switch (CurrentAction)
                {
                    case Actions.Left: velocity.X = -300f; animator.Update(gameTime); break;
                    case Actions.Right: velocity.X = 300f; animator.Update(gameTime); break;
                    default: velocity.X = 0f; animator.Play(standAnim); animator.Stop(); break;
                }
            }

            CollideX(gameTime);
            CollideY(gameTime);

            position = Vector2.Clamp(position, Vector2.Zero, new Vector2(bounds.Width, bounds.Height));
            position = new Vector2((float)Math.Floor(position.X), (float)Math.Floor(position.Y));
            animator.Position = position;

            //Console.WriteLine($"{position}, {animator.Position}");

            if (!isClimbing)
                velocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            CurrentAction = Actions.None;
        }

        public void Pickup(IPickupable item)
        {
            inventory.Add(item);
            if(item is Coin)
            {
                Money += (item as Coin).Value;
            }
            Console.WriteLine(item.GetType());
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

        public void RemoveItem(string itemName)
        {
            for (int i = inventory.Count - 1; i >= 0; i--)
            {
                IEntity item = (inventory.ElementAt(i) as IEntity);
                if (item.Type == itemName)
                {
                    RemoveItem(item as IPickupable);
                }
            }
        }

        public void Damage(int attackDamage)
        {
            if (damageCounter <= 0)
            {
                Health -= attackDamage;
                if (Health <= 0)
                {
                    Kill();
                }
                damageCounter = 1;
            }
        }

        public void Kill()
        {
            SugarGame.Instance.GSM.Gamescreens.Peek().Quit = true;
            SugarGame.Instance.Components.Add(new MenuComponent(SugarGame.Instance));
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
                                RemoveItem("key_green");
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
                        else if(tileType == "spikes")
                        {
                            Kill();
                        }

                        if (tileType == "ladder")
                        {
                            canClimb = true;
                        }
                        if(tileType == "portal" && CurrentAction == Actions.Activate)
                        {
                            level = SugarGame.levelManager.LoadNextLevel();
                            SetBoundingLevel(level);
                            Camera.SetBoundingLevel(level);
                            tilesToCheck = level.GetLayer("Ground").GetTiles();
                            position = level.StartPosition;
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
            CurrentAction = Actions.Left;
            velocity.X = -300;
        }

        public void MoveRight()
        {
            CurrentAction = Actions.Right;
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
                CurrentAction = Actions.Up;
            }
        }
        public void ClimbDown()
        {
            if (isClimbing)
            {
                CurrentAction = Actions.Down;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animator.Draw(spriteBatch, velocity.X < 0);
        }
    }
}
