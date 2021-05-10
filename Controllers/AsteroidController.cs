using App05MonoGame.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace App05MonoGame.Controllers
{
    public enum CoinColours
    {
        copper = 100,
        Silver = 200,
        Gold = 500
    }

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

        public AsteroidController()
        {
            asteroids = new List<Sprite>();
        }
        /// <summary>
        /// Create an animated sprite of a copper coin
        /// which could be collected by the player for a score
        /// </summary>
        public void CreateAsteroid(GraphicsDevice graphics, Texture2D asteroidImage)
        {
            asteroidEffect = SoundController.GetSoundEffect("Flame");

            Sprite asteroidSprite = new Sprite(asteroidImage, 1200, 500)
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
