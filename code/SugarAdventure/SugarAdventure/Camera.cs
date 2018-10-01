﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    class Camera
    {
        //DEBUG VARIABLES
        public static float ZoomStep = 0.25f;
        public static float Speed = 10;

        private int originalWidth;
        private int originalHeight;
        private static float mScaleX;
        private static float mScaleY;

        private Rectangle Target;
        private Vector2 pos;
        public Vector2 Pos
        {
            get
            {
                return pos;
            }
        }
        private static Rectangle bounds;
        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }
        }

        public static Vector2 Offset { get; set; }
        public static Level BoundingLevel { get; set; }
        public static int ViewportWidth { get; set; }
        public static int ViewportHeight { get; set; }
        public Matrix ViewMatrix { get; set; }
        static int boundWidth;
        static int boundHeight;



        public static float ZoomFactor { get; set; }

        public Camera(Vector2 _pos, int _viewportWidth, int _viewportHeight, float _zoomFactor = 1.0f)
        {
            pos = _pos;
            ZoomFactor = _zoomFactor;


            originalHeight = (int)(_viewportHeight / _zoomFactor);
            originalWidth = (int)(_viewportWidth / _zoomFactor);

            originalHeight = (int)(480 / _zoomFactor);
            originalWidth = (int)(800 / _zoomFactor);

            //ViewportWidth = (int)(_viewportWidth / ZoomFactor);
            //ViewportHeight = (int)(_viewportHeight / ZoomFactor);
            ViewportWidth = (int)Math.Ceiling(_viewportWidth / ZoomFactor);
            ViewportHeight = (int)Math.Ceiling(_viewportHeight / ZoomFactor);

            //mScaleX = (float)SugarGame.graphics.PreferredBackBufferWidth / originalWidth;
            //mScaleY = (float)SugarGame.graphics.PreferredBackBufferHeight / originalHeight;
            mScaleX = (float)SugarGame.graphics.PreferredBackBufferWidth / ViewportWidth;
            mScaleY = (float)SugarGame.graphics.PreferredBackBufferHeight / ViewportHeight;

            Console.WriteLine($"{mScaleX}, {mScaleY}");
        }

        public static void SetBoundingLevel(Level _boundingLevel)
        {
            BoundingLevel = _boundingLevel;

            boundWidth = _boundingLevel.GetLayer("Ground").LayerWidth;
            boundHeight = _boundingLevel.GetLayer("Ground").LayerHeight;

            bounds = new Rectangle(Point.Zero, new Point(boundWidth - ViewportWidth, boundHeight - ViewportHeight));

            mScaleX = (float)SugarGame.graphics.PreferredBackBufferWidth / ViewportWidth;
            mScaleY = (float)SugarGame.graphics.PreferredBackBufferHeight / ViewportHeight;
        }

        public void UpdateViewport(Rectangle _viewport)
        {
            ZoomFactor = (float)_viewport.Width / originalWidth;
            Console.WriteLine(ZoomFactor);

            ViewportWidth = (int)Math.Ceiling(_viewport.Width/ ZoomFactor);
            ViewportHeight = (int)Math.Ceiling(_viewport.Height/ ZoomFactor);
            bounds = new Rectangle(Point.Zero, new Point(boundWidth - ViewportWidth, boundHeight - ViewportHeight));

            mScaleX = (float)SugarGame.graphics.PreferredBackBufferWidth / ViewportWidth;
            mScaleY = (float)SugarGame.graphics.PreferredBackBufferHeight / ViewportHeight;
        }

        public void SetTarget(Rectangle _target)
        {
            Target = _target;
        }

        public void Update(GameTime gameTime)
        {
            pos.X = Target.Location.X - ViewportWidth / 2 + Target.Size.X / 2;
            pos.Y = Target.Location.Y - ViewportHeight /  2 + Target.Size.Y / 2;

            if (bounds != null)
                pos = Vector2.Clamp(pos, new Vector2(0, 0), bounds.Size.ToVector2());

            float mOffestX = -Pos.X * ZoomFactor;
            float mOffestY = -Pos.Y * ZoomFactor;

            //Console.WriteLine(bounds.Width + ", " + bounds.Height);

            ViewMatrix = new Matrix(
                mScaleX, 0, 0, 0,
                0, mScaleY, 0, 0,
                0, 0, 1, 0,
                mOffestX, mOffestY, 0, 1
                );
            //Console.WriteLine(Target.Location);
        }
    }
}
