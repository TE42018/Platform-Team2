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
        private Texture2D texture;
        private Rectangle Bounds;
        private Rectangle hitBox;
        public Rectangle HitBox
        {
            get
            {
                return hitBox;
            }
        }

        Vector2 position;
        Vector2 velocity;
        Vector2 gravity;

        Tile[] tilesToCheck;

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
            tilesToCheck = _level.GetLayer("Ground").GetAllTiles();
        }

        public void LoadContent()
        {
            texture = Game1.contentManager.Load<Texture2D>(@"Platformer assets (1330 assets)/Extra animations and enemies (165 assets)/Alien sprites/alienPink");
            hitBox = new Rectangle(position.ToPoint(), new Point(texture.Width, texture.Height + 1));
        }

        public void Update(GameTime gameTime)
        {
            ///Move player on X axis
            position.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Console.WriteLine(position);
            hitBox.Location = position.ToPoint();

            ///Check if there is a collision
            ///Move the player on the X axis

            //Check collision on Left and Right sides
            for (int i = 0; i < tilesToCheck.Length; i++)
            {
                Tile t = tilesToCheck[i];
                if (t == null)
                    continue;
                Rectangle tileBox = t.HitBox;
                if (hitBox.Intersects(tileBox))
                {
                    if (velocity.X > 0)
                    {
                        if (position.X + hitBox.Width > tileBox.Left)
                        {
                            position.X = tileBox.Left - hitBox.Width;
                            velocity.X = 0;
                            //canJump = true;
                        }
                    }
                    else if (velocity.X < 0)
                    {
                        if (position.X < tileBox.Right)
                        {
                            position.X = tileBox.Right;
                            velocity.X = 0;
                        }
                    }
                    hitBox.Location = position.ToPoint();
                }
            }

            ///Move player on the Y axis
            position.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            hitBox.Location = position.ToPoint();
            //Console.WriteLine(position);
            ///Check if there is a collision
            ///Move the player on the Y axis

            //Check collision on Top and Bottom sides
            for (int i = 0; i < tilesToCheck.Length; i++)
            {
                Tile t = tilesToCheck[i];
                if (t == null)
                    continue;
                Rectangle tileBox = t.HitBox;
                if (hitBox.Intersects(tileBox))
                {
                    if (velocity.Y > 0)
                    {
                        if (position.Y + velocity.Y + hitBox.Height > tileBox.Top)
                        {
                            position.Y = tileBox.Top - hitBox.Height;
                            velocity.Y = 0;
                            canJump = true;
                        }
                    }
                    else if (velocity.Y < 0)
                    {
                        if (position.Y < tileBox.Bottom)
                        {
                            position.Y = tileBox.Bottom;
                            velocity.Y = 0;
                        }
                    }
                    hitBox.Location = position.ToPoint();
                }
            }

            position = Vector2.Clamp(position, Vector2.Zero, new Vector2(Bounds.Width, Bounds.Height));

            velocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void SetBoundingLevel(Level _boundingLevel)
        {
            int boundWidth = _boundingLevel.GetLayer("Ground").LayerWidth;
            int boundHeight = _boundingLevel.GetLayer("Ground").LayerHeight;
            Bounds = new Rectangle(Point.Zero, new Point(boundWidth - hitBox.Width, boundHeight - hitBox.Height));
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
