using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class WeaponFactory
    {
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly ParticleFactory _particleFactory;

        public WeaponFactory(AnimationHandlerFactory animationHandlerFactory, ParticleFactory particleFactory)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _particleFactory = particleFactory;
        }

        public MissileLauncher GetMissileLauncher(ContentManager contentManager)
        {
            Texture2D texture = contentManager.Load<Texture2D>("Artwork/projectiles/missile");
            TileSet textureTileSet = new TileSet(texture);
            Texture2D propulsion = contentManager.Load<Texture2D>("Artwork/projectiles/missile_propulsion_15_15");
            TileSet propulsionTileSet = new TileSet(propulsion,15,15, null);
            SoundEffect flightEffect = null; //contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Flight_plane_c");
            SoundEffect impactSound = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/Big_Explo");
            var m = new MissileLauncher(_animationHandlerFactory,textureTileSet, propulsionTileSet,
                _particleFactory, null, flightEffect, impactSound);
            return m;
        }

        public Lasergun GetLasergun(ContentManager contentManager)
        {
            Texture2D texture = contentManager.Load<Texture2D>("Artwork/projectiles/laser");
            TileSet textureTileSet = new TileSet(texture, new []
            {
                new Polygon(Vector2.Zero, new Vector2(8, 2), 0, new List<Vector2>
                {
                    new Vector2(-8, 2),
                    new Vector2(8, 2),
                    new Vector2(8, -2),
                    new Vector2(-8, -2),
                }), 
            });
            SoundEffect soundEffect = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/Laser_Short");
            SoundEffect flightEffect = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Wobble_test");
            SoundEffect impactSound = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/Clink");
            var l = new Lasergun(_animationHandlerFactory, textureTileSet, _particleFactory, soundEffect, flightEffect, impactSound);
            return l;
        }
    }
}