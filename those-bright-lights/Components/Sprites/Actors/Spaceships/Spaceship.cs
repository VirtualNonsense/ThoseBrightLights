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

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public abstract class Spaceship : Actor
    {
        public List<SpaceshipAddOn> Components;
        protected int CurrentWeapon;
        protected int IndexOfWeaponsOfTheSameType;
        protected float MaxSpeed;
        protected readonly float Acceleration;
        private Logger _logger;
        protected Polygon _impactPolygon;
        public Propulsion Propulsion;
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        public Spaceship(AnimationHandler animationHandler, float maxSpeed = 3, float acceleration = 5, float health = 100, SoundEffect impactSound = null) : base(
            animationHandler, impactSound)
        {
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            Health = health;
            Components = new List<SpaceshipAddOn>();
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        // #############################################################################################################
        // Events
        // #############################################################################################################
        #region Events
        public event EventHandler OnShoot;
        //TODO: Consider moving this into sprite
        

        #endregion
        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        

        protected List<Weapon> AllWeaponsList => Components.OfType<Weapon>().ToList();
        protected List<Weapon> CurrentWeapons => (from w in AllWeaponsList where w.NameTag == AllWeaponsList[IndexOfWeaponsOfTheSameType].NameTag select w).ToList(); 

        public override float Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = value;
                if (Rotation < 3 * Math.PI/2 && Rotation > Math.PI/2)
                {
                    if (FlippedHorizontal) return;
                    _animationHandler.SpriteEffects = SpriteEffects.FlipVertically;
                }
                else if (FlippedHorizontal)
                {
                    _animationHandler.SpriteEffects = SpriteEffects.None;
                }
        
            }
        }

        

        // #############################################################################################################
        // public Methods
        // #############################################################################################################
        public override void Update(GameTime gameTime)
        {
            foreach (var weapon in AllWeaponsList)
            {
                weapon.Update(gameTime);
            }

            foreach (var component in Components)
            {
                component.Update(gameTime);
            }

            for (var i = 0; i < AllWeaponsList.Count;)
            {
                var w = AllWeaponsList[i];
                if (!w.IsRemoveAble)
                {
                    i++;
                    continue;
                }
                Components.Remove(w);
                if (CurrentWeapon >= AllWeaponsList.Count)
                    CurrentWeapon = AllWeaponsList.Count - 1;
            }

            if (Propulsion != null)
            {
                // TODO: Move into separate setter
                Propulsion.Position = Position;
                Propulsion.Rotation = Rotation;
                Propulsion.Layer = Layer;
                Propulsion.Update(gameTime);
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
            CurrentWeapon = Components.Count - 1;
            weapon.OnEmitBullet += EmitBulletToOnShot;
            switch (weapon)
            {
                case SingleShotWeapon ssw:
                    ssw.OnClipEmpty += (sender, args) => ReloadWeapon((ClipWeapon)sender);
                    ssw.OnWeaponEmpty += (sender, args) => RemoveWeapon((Weapon)sender);
                    ssw.OnReloadProgressUpdate += (sender, args) => _logger.Debug(args.Progress * 100);
                    ssw.OnWeaponEmpty += (sender, args) => _logger.Debug("weapon empty!");
                    break;
            }
        }

        public virtual void RemoveWeapon(Weapon weapon)
        {
            if (!AllWeaponsList.Contains(weapon)) return;
            weapon.OnEmitBullet -= EmitBulletToOnShot;
            // Weapons.Remove(weapon);
            weapon.IsRemoveAble = true;
        }

        // #############################################################################################################
        // protected / private Methods
        // #############################################################################################################
        protected virtual void ShootCurrentWeapon()
        {
            if (AllWeaponsList.Count == 0) return;
            var previousWeapon = ((IndexOfWeaponsOfTheSameType - 1) +CurrentWeapons.Count)% CurrentWeapons.Count;
            if (!CurrentWeapons[previousWeapon].CanShoot) return;
            CurrentWeapons[IndexOfWeaponsOfTheSameType].Fire();
            IndexOfWeaponsOfTheSameType = (IndexOfWeaponsOfTheSameType +1) % CurrentWeapons.Count;
        }

        protected virtual void ShootAllWeapons()
        {
            if (AllWeaponsList.Count == 0) return;
            foreach (var weapon in AllWeaponsList)
                weapon.Fire();
        }

        protected virtual void InvokeOnShoot(Bullet b)
        {
            if (b is null)
                return;
            var e = new LevelEvent.ShootBullet {Bullet = b};
            b.Layer = Layer;
            OnShoot?.Invoke(this,e);
        }

        private void EmitBulletToOnShot(object sender, Weapon.EmitBulletEventArgs args)
        {
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

            if ((from component in Components 
                where component.HitBox != null 
                from poly in component.HitBox 
                from otherPoly in other.HitBox 
                where poly.Overlap(otherPoly) 
                select poly).Any())
            {
                return true;
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
                    break;
                case PowerUp p:
                    ProcessPowerUp(p);
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
                case InstaDeathPowerUp i:
                    Health -= Health;
                    break;
                
                case AmmoPowerUp ra:
                    //Weapons[CurrentWeapon].Ammo += ra.AmmoBonus;
                    break;
                
                case WeaponPowerUp r:
                    AddWeapon(r.Weapon);
                    break;
                
                case ScoreBonusPowerUp sb:
                    //score+= sb.bonusScore;
                    break;
                
                    
            }
        }
        protected void ApproachDestination(Actor other, int maxIteration, int iteration=0)
        {
            if (iteration >= maxIteration)
            {
                _logger.Debug($"Approachdestination after {iteration} abborted");
                return;
            }
            if (DeltaPosition.Length() <= 10 * float.Epsilon)
            {
                var v = _impactPolygon.Position - other.Position;
                v /= v.Length();
                Position += 10 * v;
            }
            else
            {
                Position -= DeltaPosition;
                DeltaPosition /= 2;
                Position += DeltaPosition;
            }

            if (!Collide(other))
                return;
            iteration++;
            ApproachDestination(other, maxIteration, iteration);
        }
    }
}