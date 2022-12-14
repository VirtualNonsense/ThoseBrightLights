using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using ThoseBrightLights.Components;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services.Factories;
using ThoseBrightLights.Services.StateMachines;

namespace ThoseBrightLights.Core.GameStates
{
    public class SaveSelect : GameState
    {
        // Fields
        private readonly IGameEngine _engine;
        private readonly IScreen _screen;
        private readonly ControlElementFactory _factory;
        private readonly ISaveGameHandler _saveGameHandler;
        private ComponentGrid _components;
        private Logger _logger;

        // Constructor
        public SaveSelect(IGameEngine engine, IScreen screen, ControlElementFactory factory, ISaveGameHandler saveGameHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _engine = engine;
            _screen = screen;
            _factory = factory;
            _saveGameHandler = saveGameHandler;
        }
        
        // In LoadContent are the 3 different states with the representative buttons
        public override void LoadContent()
        {
            // Arrangement
            if (!(_components is null))
                return;
            var center = new Vector2(0, 0);

            var width = _screen.Camera.GetPerspectiveScreenWidth();
            var height = _screen.Camera.GetPerspectiveScreenHeight();
            
            _components = new ComponentGrid(center, width, height, 1);
            var bWidth = width / 3;
            var bHeight = height / 3;

            var slotExists = _saveGameHandler.SaveExists(SaveSlot.Slot1);

            // Button "one" - the print also changes when complete new game or a "Slot 1" when saving has been done
            var slots1 = _factory.GetButton(bWidth, bHeight, Vector2.Zero, slotExists ? "Slot 1" : "New Game", _screen.Camera);
            slots1.Click += (sender, args) => 
            { 

                _logger.Trace("slot one selected");
                _saveGameHandler.SaveSlot = SaveSlot.Slot1;

                if (_saveGameHandler.SaveExists(SaveSlot.Slot1))
                {
                    _saveGameHandler.Load();
                }
                else
                {
                    _saveGameHandler.CreateSave();
                    _saveGameHandler.Save();
                }
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.SaveSlotSelected);
            };

            slotExists = _saveGameHandler.SaveExists(SaveSlot.Slot2);

            // Button "two" - similiar idea to button "one"
            var slots2 = _factory.GetButton(bWidth, bHeight, Vector2.Zero, slotExists ? "Slot 2" : "New Game", _screen.Camera);
            slots2.Click += (sender, args) => 
            { 
                _logger.Trace("slot two selected");
                _saveGameHandler.SaveSlot = SaveSlot.Slot2;
                if (_saveGameHandler.SaveExists(SaveSlot.Slot2))
                {
                    _saveGameHandler.Load();
                }
                else
                {
                    _saveGameHandler.CreateSave();
                    _saveGameHandler.Save();
                }
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.SaveSlotSelected); 
            };

            slotExists = _saveGameHandler.SaveExists(SaveSlot.Slot3);

            // Button "three" - similiar idea to button "one"
            var slots3 = _factory.GetButton(bWidth, bHeight, Vector2.Zero, slotExists ? "Slot 3" : "New Game", _screen.Camera);
            slots3.Click += (sender, args) => 
            { 
                _logger.Trace("slot three selected");
                _saveGameHandler.SaveSlot = SaveSlot.Slot3;
                if (_saveGameHandler.SaveExists(SaveSlot.Slot3))
                {
                    _saveGameHandler.Load();
                }
                else
                {
                    _saveGameHandler.CreateSave();
                    _saveGameHandler.Save();
                }
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.SaveSlotSelected); 
            };

            _components.Add(slots1);
            _components.Add(slots2);
            _components.Add(slots3);

        }

        // Monogame functions
        public override void UnloadContent()
        {
            _components = null;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            _engine.Render(_components);
        }
    }
}