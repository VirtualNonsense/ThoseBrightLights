using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Components.Sprites.Actors.PowerUps;
using SE_Praktikum.Components.Sprites.Actors.Weapons;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public abstract class Spaceship : Actor
    {
        public List<SpaceshipAddOn> Components;
        protected int IndexOfWeaponsOfTheSameType;
        protected float MaxSpeed;
        protected readonly float Acceleration;
        protected readonly float RotationAcceleration;
        protected readonly float MaxRotationSpeed;
        protected float DeltaRotation;
        private Logger _logger;
        protected Polygon _impactPolygon;
        private int _componentIndex;
        private readonly Dictionary<string, CastTimeAbility> _statusChangeResetTimer;
        private List<Weapon> _currentWeapons;

        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        public Spaceship(AnimationHandler animationHandler,
                         float maxSpeed = 3,
                         float acceleration = 5,
                         float rotationAcceleration = .1f,
                         float maxRotationSpeed = 10,
                         float health = 100,
                         float? maxHealth = null,
                         float impactDamage = 5,
                         SoundEffect impactSound = null) : base(
            animationHandler, impactSound, health: health, maxHealth: maxHealth, impactDamage: impactDamage)
        {
            _logger = LogManager.GetCurrentClassLogger();
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            RotationAcceleration = rotationAcceleration;
            MaxRotationSpeed = maxRotationSpeed;
            Components = new List<SpaceshipAddOn>();
            _statusChangeResetTimer = new Dictionary<string, CastTimeAbility>();
            AllWeapons = new List<Weapon>();
            _currentWeapons = new List<Weapon>();
        }
        
        // #############################################################################################################
        // Events
        // #############################################################################################################
        #region Events
        public event EventHandler OnShoot;
        public event EventHandler OnWeaponChanged;

        #endregion
        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        

        protected List<Weapon> AllWeapons { get; set; }

        protected List<Weapon> CurrentWeapons { get; set; } 
            

        public int ComponentIndex 
        {
            get => _componentIndex;
            set
            {
                if (value < 0)
                {
                    _componentIndex = 0;
                    InvokeOnWeaponChanged();
                    return;
                }

                if (value >= AllWeapons.Count)
                {
                    _componentIndex = AllWeapons.Count - 1;
                    InvokeOnWeaponChanged();
                    return;
                }
                _componentIndex = value;
                InvokeOnWeaponChanged();
            }
        }
        public override float Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = value;
                InvokeOnRotationChanged();
                if (Rotation < 3 * Math.PI/2 && Rotation > Math.PI/2)
                {
                    if (FlippedHorizontal) return;
                    _animationHandler.SpriteEffects = SpriteEffects.FlipVertically;
                    InvokeOnFlippedChange();
                    
                }
                else if (FlippedHorizontal)
                {
                    _animationHandler.SpriteEffects = SpriteEffects.None;
                    InvokeOnFlippedChange();
                }
            }
        }

        // #############################################################################################################
        // public Methods
        // #############################################################################################################
        public override void Update(GameTime gameTime)
        {
            foreach (var weapon in AllWeapons)
            {
                weapon.Update(gameTime);
            }

            foreach (var component in Components)
            {
                component.Update(gameTime);
            }

            for (int i = 0; i < _statusChangeResetTimer.Count;)
            {
                var timer = _statusChangeResetTimer.ElementAt(i).Value;
                var c = _statusChangeResetTimer.Count;
                timer.Update(gameTime);
                if (c == _statusChangeResetTimer.Count)
                    i++;
            }

            for (var i = 0; i < AllWeapons.Count;)
            {
                var w = AllWeapons[i];
                if (!w.IsRemoveAble)
                {
                    i++;
                    continue;
                }
                Components.Remove(w);
                if (ComponentIndex >= AllWeapons.Count)
                    ComponentIndex = AllWeapons.Count - 1;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var comp in Components)
            {
                comp.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        public virtual void AddWeapon(Weapon weapon)
        {
            weapon.Parent = this;
            Components.Add(weapon);
            ComponentIndex = Components.Count - 1;
            weapon.OnEmitBullet += EmitBulletToOnShot;
            switch (weapon)
            {
                case SingleShotWeapon ssw:
                    ssw.OnDeath += (sender, args) => RemoveWeapon((Weapon) sender);
                    ssw.OnClipEmpty += (sender, args) => ReloadWeapon((ClipWeapon)sender);
                    ssw.OnWeaponEmpty += (sender, args) => RemoveWeapon((Weapon)sender);
                    ssw.OnReloadProgressUpdate += (sender, args) => _logger.Debug(args.Progress * 100);
                    ssw.OnWeaponEmpty += (sender, args) => _logger.Debug("weapon empty!");
                    break;
            }

            AllWeapons = Components.OfType<Weapon>().ToList();
            CurrentWeapons =
                (from w in AllWeapons where w.NameTag == AllWeapons[^1].NameTag select w)
                .ToList();
        }

        public virtual void RemoveWeapon(Weapon weapon)
        {
            if (!AllWeapons.Contains(weapon)) return;
            weapon.OnEmitBullet -= EmitBulletToOnShot;
            Components.Remove(weapon);
            ComponentIndex = ComponentIndex;
            AllWeapons = Components.OfType<Weapon>().ToList();
            CurrentWeapons =
                (from w in AllWeapons where w.NameTag == AllWeapons[^1].NameTag select w)
                .ToList();
        }

        // #############################################################################################################
        // protected / private Methods
        // #############################################################################################################
        protected virtual void ShootCurrentWeapon()
        {
            if (AllWeapons.Count == 0) return;
            var previousWeapon = ((IndexOfWeaponsOfTheSameType - 1) +CurrentWeapons.Count)% CurrentWeapons.Count;
            if (!CurrentWeapons[previousWeapon].CanShoot) return;
            CurrentWeapons[IndexOfWeaponsOfTheSameType].Fire();
            IndexOfWeaponsOfTheSameType = (IndexOfWeaponsOfTheSameType +1) % CurrentWeapons.Count;
        }
        
        protected virtual void ShootAllWeapons()
        {
            if (AllWeapons.Count == 0) return;
            foreach (var weapon in AllWeapons)
                weapon.Fire();
        }
        
        protected virtual void InvokeOnWeaponChanged()
        {
            OnWeaponChanged?.Invoke(this, EventArgs.Empty);
        }
        
        protected virtual void InvokeOnShoot(Bullet b)
        {
            if (b is null)
                return;
            var e = new LevelEventArgs.ShotBulletEventArgs {Bullet = b};
            b.Layer = Layer;
            OnShoot?.Invoke(this,e);
        }

        private void EmitBulletToOnShot(object sender, Weapon.EmitBulletEventArgs args)
        {
            _logger.Debug("emiting bullet");
            InvokeOnShoot(args.Bullet);
        }

        private void ReloadWeapon(ClipWeapon w)
        {
            _logger.Debug(w);
            w.Reload();
        }

        protected override bool Collide(Actor other)
        {
            if ( this == other || 
                 Math.Abs(Layer - other.Layer) > float.Epsilon || 
                 !(CollisionEnabled && IsCollideAble && other.CollisionEnabled && other.IsCollideAble)) 
                return false;
            foreach (var polygon in HitBox)
            {
                _impactPolygon = polygon;
                if (other.HitBox.Any(polygon1 => polygon.Overlap(polygon1)))
                {
                    return true;
                }
            }
            _impactPolygon = null;
            return false;
        }

        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                case Bullet b:
                    // bullet shouldn't damage it's parent
                    if (this == b.Parent) return;
                    Health -= b.Damage;
                    _impactSound?.Play();
                    break;
                case Tile t :
                    ApproachDestination(t, 100);
                    Health -= Velocity.Length()/MaxSpeed * t.Damage;
                    _impactSound?.Play();
                    break;
                case PowerUp p:
                    ProcessPowerUp(p);
                    break;
                case Spaceship s:
                    Health -= s.Damage;
                    _impactSound?.Play();
                    break;

            }

            _impactPolygon = null;
        }

        public override void InterAct(Actor other)
        {
            base.InterAct(other);
            foreach (var comp in Components)
            {
                comp.InterAct(other);
            }
        }
        

        private void ProcessPowerUp(PowerUp powerup)
        {
            switch(powerup)
            {
                case HealthPowerUp h:
                    Health += h.HealthBonus;
                    break;
                case FullHealthPowerUp fh:
                    Health = MaxHealth;
                    break;

                case InstaDeathPowerUp i:
                    Health = 0;
                    break;
                
                case InfAmmoPowerUp ra:
                    //Weapons[CurrentWeapon].Ammo += ra.AmmoBonus;
                    break;
                
                case WeaponPowerUp r:
                    AddWeapon(r.Weapon);
                    break;
                case StarPowerUp s:
                    var key = "starpowerup";
                    Indestructible = true;
                    if (_statusChangeResetTimer.ContainsKey(key))
                    {
                        _statusChangeResetTimer[key].TargetTime += (int) s.Duration;
                        _logger.Debug($"indestructable for {_statusChangeResetTimer[key].TargetTime} ms!");

                        return;
                    }
                    _logger.Debug($"indestructable for {s.Duration} ms!");

                    var castTime = new CastTimeAbility((int)s.Duration, () => { });
                    castTime.Ability = () =>
                    {
                        Indestructible = false;
                        _statusChangeResetTimer.Remove(key);
                    };
                    castTime.Fire();
                    _statusChangeResetTimer.Add(key,castTime);
                    break;
                    
            }
        }
        
    }
}