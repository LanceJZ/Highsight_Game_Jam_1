#region Using
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
#endregion
namespace Highsight_Game_Jam_1
{
    class Player : ModelEntity
    {
        #region Fields
        GameLogic LogicRef;
        Shot TheShot;
        Missile TheMissile;
        SoundEffect ExplodeSound;
        SoundEffect FlapSound;
        SoundEffect IdleSound;
        SoundEffect FireSound;
        SoundEffect FireMissileSound;
        Timer InZoneTimer;
        SoundEffect InZoneSound;
        Timer FlapSoundTimer;
        Timer IdleSoundTimer;
        KeyboardState OldKeyState;
        float MovementSpeed = 50;
        int PlayAreaHeight = 83;
        bool InZone;
        #endregion
        #region Properties
        public Shot ShotRef { get => TheShot; }
        public Missile MissileRef { get => TheMissile; }
        #endregion
        #region Constructor
        public Player(Game game, Camera camera, GameLogic gameLogic) : base(game, camera)
        {
            LogicRef = gameLogic;
            TheShot = new Shot(game, camera, gameLogic);
            TheMissile = new Missile(game, camera, gameLogic);
            FlapSoundTimer = new Timer(game);
            IdleSoundTimer = new Timer(game);
            InZoneTimer = new Timer(game);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            ExplodeSound = Helper.LoadSoundEffect("PlayerExplode");
            FlapSound = Helper.LoadSoundEffect("PlayerFlap");
            IdleSound = Helper.LoadSoundEffect("PlayerIdle");
            FireSound = Helper.LoadSoundEffect("PlayerFire");
            FireMissileSound = Helper.LoadSoundEffect("PlayerFireMissile");
            InZoneSound = Helper.LoadSoundEffect("NZone");

            LoadModel("Player");
            base.LoadContent();
        }

        public override void BeginRun()
        {
            IdleSoundTimer.Amount = (float)IdleSound.Duration.TotalSeconds;
            FlapSoundTimer.Amount = (float)FlapSound.Duration.TotalSeconds;
            InZoneTimer.Amount = (float)InZoneSound.Duration.TotalSeconds;

            base.BeginRun();
            Reset();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            PO.WrapTopBottom(PlayAreaHeight);
            Input();
            BlockCollusion();
            ZoneCollusion();

            if (Sphere.Intersects(LogicRef.EnemyRef.Sphere))
            {
                SpawnMissile();
            }

            base.Update(gameTime);
        }
        #endregion
        public bool MissileCollusion()
        {
            if (Sphere.Intersects(TheMissile.Sphere))
            {
                return true;
            }

            return false;
        }

        public void Reset()
        {
            TheMissile.Enabled = false;
            Spawn(new Vector3(-100, 0, 0));
        }

        public void Explode()
        {
            ExplodeSound.Play();
            Reset();
        }

        public void EndGame()
        {
            Enabled = false;
            TheMissile.Enabled = false;
        }

        void ZoneCollusion()
        {
            if (LogicRef.NzoneRef.CheckCollide(Sphere))
            {
                InZone = true;

                if (InZoneTimer.Elapsed)
                {
                    InZoneTimer.Reset();
                    InZoneSound.Play();
                }

                return;
            }
            else
            {
                InZone = false;
            }
        }

        void BlockCollusion()
        {
            if (LogicRef.ShieldRef.CheckEating())
            {
                SpawnMissile();
            }
        }

        void SpawnMissile()
        {
            if (!TheMissile.Enabled)
                TheMissile.Spawn(new Vector3(-100, PO.Position.Y, 0));
        }

        void Input()
        {
            KeyboardState KBS = Keyboard.GetState();

            if (KBS != OldKeyState)
            {
                if (KBS.IsKeyDown(Keys.LeftControl))
                {
                    if (!InZone)
                    {
                        if (!TheShot.Enabled)
                        {
                            FireSound.Play();
                            TheShot.Spawn(Position + PO.VelocityFromAngleZ(Rotation.Z, 7),
                                Rotation, PO.VelocityFromAngleZ(Rotation.Z, 100));
                        }

                        if (TheMissile.Enabled)
                        {
                            FireMissileSound.Play();
                            TheMissile.Fire();
                        }
                    }
                }
            }

            Velocity = Vector3.Zero;

            if (KBS.IsKeyDown(Keys.Up))
            {
                Rotation = Vector3.UnitZ * MathHelper.PiOver2;
                Velocity = Vector3.UnitY * MovementSpeed;
            }
            else if (KBS.IsKeyDown(Keys.Down))
            {
                Rotation = Vector3.UnitZ * (MathHelper.Pi + MathHelper.PiOver2);
                Velocity = Vector3.UnitY * -MovementSpeed;
            }
            else if (KBS.IsKeyDown(Keys.Right))
            {
                if (Position.X > 100)
                    return;

                Rotation = Vector3.UnitZ * 0;
                Velocity = Vector3.UnitX * MovementSpeed;
            }
            else if (KBS.IsKeyDown(Keys.Left))
            {
                if (Position.X < -105)
                    return;

                Rotation = Vector3.UnitZ * MathHelper.Pi;
                Velocity = Vector3.UnitX * -MovementSpeed;
            }

            if (Velocity.X != 0 || Velocity.Y != 0)
            {
                if (FlapSoundTimer.Elapsed)
                {
                    FlapSoundTimer.Reset();
                    FlapSound.Play();
                }
            }
            else
            {
                if (IdleSoundTimer.Elapsed)
                {
                    IdleSoundTimer.Reset();
                    IdleSound.Play();
                }
            }

            OldKeyState = Keyboard.GetState();
        }
    }
}
