using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class ParticleFactory
    {
        // Fields
        private readonly IScreen _screen;
        private readonly AnimationHandlerFactory _factory;
        private readonly ContentManager _contentManager;
        private readonly TileSetFactory _tileSetFactory;

        // Constructor
        public ParticleFactory(IScreen screen, AnimationHandlerFactory factory, ContentManager contentManager, TileSetFactory tileSetFactory)
        {
            _screen = screen;
            this._factory = factory;
            _contentManager = contentManager;
           _tileSetFactory = tileSetFactory;
        }
        
        // Build each particle animation with desired animation settings and json file
        public ExplosionsParticle BuildExplosionParticle(AnimationSettings settings = null)
        {
            var animationSettings = settings ?? new AnimationSettings(7, 50f, 1);
            var explosionTileSet = 
                _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\explosion_45_45.json", 0);
            return new ExplosionsParticle(
                _factory.GetAnimationHandler(explosionTileSet,
                    new List<AnimationSettings>(new[] {animationSettings})),
                _screen);
        }

        public ExplosionsParticle BuildLaserExplosionParticle(AnimationSettings settings = null)
        {
            var animationSettings = settings ?? new AnimationSettings(6, 50f, 1);
            //TileSet explosion = new TileSet(_contentManager.Load<Texture2D>("Artwork/effects/laesr_explosion_6_6_6"),6,6, null);
            var laserexpTileSet = 
                _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\laesr_explosion_6_6_6.json", 0);
            return new ExplosionsParticle(
                _factory.GetAnimationHandler(laserexpTileSet, 
                    new List<AnimationSettings>(new[] {animationSettings})),
                _screen);
        }

        public ExplosionsParticle BuildProjectileExplosionParticle(AnimationSettings settings = null)
        {
            var animationSettings = settings ?? new AnimationSettings(4, 50f, 1);
            var explosionTileSet = 
                _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\projectile_5_6_4.json", 0);
            return new ExplosionsParticle(
                _factory.GetAnimationHandler(explosionTileSet,
                    settings: new List<AnimationSettings>(new[] {animationSettings})), _screen);
        }

        public ExplosionsParticle BuildMinigunFireParticle(AnimationSettings settings = null)
        {
            var animationSettings = settings ?? new AnimationSettings(5, 50f, 1);
            var explosionTileSet = 
                _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\minigunexplosions_8_10_5.json", 0);
            return new ExplosionsParticle(
                _factory.GetAnimationHandler(explosionTileSet, 
                    new List<AnimationSettings>(new[] {animationSettings})),
                _screen);
        }

        public StatusEffectParticle BuildStarEffectParticle(int onScreenTime)
        {
            var animationSettings = new AnimationSettings(10, 50, isLooping: true);
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\star_21_21_10.json", 0);
            return new StatusEffectParticle(
                _factory.GetAnimationHandler(tileSet, 
                    new List<AnimationSettings>(new[] {animationSettings})), onScreenTime);
        }

        public ExplosionsParticle BuildMineExplosionsParticle(AnimationSettings settings = null)
        {
            var animationSettings = settings ?? new AnimationSettings(updateList: new List<(int, float)>
            {
                (8, 50f),
                (9, 50f),
                (10, 50f),
                (11, 50f),
                (12, 50f),
                (13, 50f),
                (14, 50f),
                (15, 50f),
            },scale:2);
            var explosionTileSet = 
                _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\mine_35_35_8_2x.json", 0);
            return new ExplosionsParticle(
                _factory.GetAnimationHandler(explosionTileSet,
                    settings: new List<AnimationSettings>(new[] {animationSettings})), _screen);
        }
    }
}