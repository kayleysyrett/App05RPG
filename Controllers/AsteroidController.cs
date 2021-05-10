using App05MonoGame.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;




namespace App05MonoGame.Controllers
{


    /// <summary>
    /// This class creates a list of coins which
    /// can be updated and drawn and checked for
    /// collisions with the player sprite
    /// </summary>
    /// <authors>
    /// Derek Peacock & Andrei Cruceru
    /// </authors>
    public class AsteroidController
    {
        private SoundEffect asteroidEffect;

        private readonly List<Sprite> asteroids;

        private Texture2D[] images;

        private Random generator;

        private double timer;

        private double maxTime;

        public AsteroidController()
        {
            asteroids = new List<Sprite>();
            images = new Texture2D[3];
            generator = new Random();
            maxTime = 1.0;

        }
        /// <summary>
        /// Loads three different asteroid images
        /// and puts them in the array
        /// </summary>
        public void LoadImages(ContentManager content)
        {
            asteroidEffect = SoundController.GetSoundEffect("Flame");
            images [0] = content.Load<Texture2D>("Actors/Stones2Filled_01");
            images [1] = content.Load<Texture2D>("Actors/Stones2Filled_01");
            images [2] = content.Load<Texture2D>("Actors/Stones2Filled_01");

        }

        public void CreateAsteroid()
        {
            int imageNumber = generator.Next(3);

            Texture2D asteroidImage = images[imageNumber];

            int y = generator.Next(900) + 100;

            Sprite asteroidSprite = new Sprite(asteroidImage, 1900, y)
            {
                Direction = new Vector2(-1, 0),
                Speed = 100,

                Rotation = MathHelper.ToRadians(3),
                RotationSpeed = 2f,
            };

            asteroids.Add(asteroidSprite);
        }

        public void HasCollided(PlayerSprite player)
        {
            foreach (Sprite asteroid in asteroids)
            {
                if (asteroid.HasCollided(player) && asteroid.IsAlive)
                {
                    asteroidEffect.Play();

                    asteroid.IsActive = false;
                    asteroid.IsAlive = false;
                    asteroid.IsVisible = false;
                }
            }           
        }

        public void Update(GameTime gameTime)
        {
            timer = timer - gameTime.ElapsedGameTime.TotalSeconds;
            if(timer <= 0 ){

                CreateAsteroid();
                timer = maxTime;
            }

            foreach(Sprite asteroid in asteroids)
            {
                asteroid.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite asteroid in asteroids)
            {
                asteroid.Draw(spriteBatch);
            }
        }
    }
}
