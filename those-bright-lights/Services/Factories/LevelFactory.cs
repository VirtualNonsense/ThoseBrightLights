using System;
using Microsoft.Xna.Framework.Media;
using SE_Praktikum.Core;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class LevelFactory
    {
        private readonly MapFactory _mapFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly EnemyFactory _enemyFactory;
        private readonly PowerUpFactory _powerUpFactory;
        private readonly IScreen _screen;
        private readonly IGameEngine _gameEngine;
        private readonly HUDFactory _hUdFactory;

        // #############################################################################################################
        // constructor
        // #############################################################################################################
        public LevelFactory(
            MapFactory mapFactory,
            PlayerFactory playerFactory,
            ParticleFactory particleFactory,
            EnemyFactory enemyFactory,
            PowerUpFactory powerUpFactory,
            IScreen screen,
            IGameEngine gameEngine,
            HUDFactory hUDFactory)
        {
            _mapFactory = mapFactory;
            _playerFactory = playerFactory;
            _particleFactory = particleFactory;
            _enemyFactory = enemyFactory;
            _powerUpFactory = powerUpFactory;
            _screen = screen;
            _gameEngine = gameEngine;
            _hUdFactory = hUDFactory;
        }
        // #############################################################################################################
        // public methods
        // #############################################################################################################
        /// <summary>
        /// returns the level of a given path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="levelNumber"></param>
        /// <param name="song"></param>
        /// <returns></returns>
        public Level GetInstance(string path, int levelNumber, Song song = null)
        {
            return new Level(path,
                levelNumber,
                _mapFactory,
                _playerFactory,
                _particleFactory,
                _enemyFactory,
                _powerUpFactory,
                _screen,
                _gameEngine,
                _hUdFactory,
                song);
        }
    }
}