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
    class Swirl : ModelEntity
    {
        #region Fields
        GameLogic LogicRef;
        Explode TheExplosion;
        Timer FireTimer;
        SoundEffect SpinupSound;
        SoundEffect LaunchSound;
        SoundEffect ExplodeSound;
        State TheState;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Swirl(Game game, Camera camera, GameLogic gameLogic) : base(game, camera)
        {
            LogicRef = gameLogic;
            TheExplosion = new Explode(game, camera);
            FireTimer = new Timer(game, 10);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            EmissiveColor = new Vector3(0.1f, 0.1f, 0.1f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadModel("Swirl");
            SpinupSound = Helper.LoadSoundEffect("SwirlSpinup");
            LaunchSound = Helper.LoadSoundEffect("SwirlLaunch");
            ExplodeSound = Helper.LoadSoundEffect("SwirlExplode");

            base.LoadContent();
        }

        public override void BeginRun()
        {
            base.BeginRun();
            Enabled = false;
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            switch (TheState)
            {
                case State.Standby:
                    Standby();
                    break;
                case State.Launched:
                    CheckCollusion();
                    break;
            }


            base.Update(gameTime);
        }
        #endregion
        public override void Spawn(Vector3 position, Vector3 color)
        {
            base.Spawn(position);

            SpinupSound.Play();
            DefuseColor = color;
            TheExplosion.Setup(EmissiveColor, color);
            FireTimer.Reset(Helper.RandomMinMax(2, 10));
            TheState = State.Standby;
            Velocity = Vector3.Zero;
            RotationVelocity = Vector3.Zero;
            RotationAcceleration = Vector3.Zero;
            PO.RotationAcceleration.Z = -MathHelper.Pi;
        }

        public void HitByMissile()
        {
            TheExplosion.Spawn(Position, 1.5f, 50, 14, 0.2f, 1);
            ExplodeSound.Play();
            Reset();

            switch (TheState)
            {
                case State.Standby:
                    LogicRef.AddPoints(2000);
                    break;
                case State.Launched:
                    LogicRef.AddPoints(6000);
                    LogicRef.AddLife();
                    break;
            }
        }

        void Fire()
        {
            LaunchSound.Play();
            TheState = State.Launched;
            Velocity = PO.VelocityFromVectorsZ(LogicRef.PlayerRef.Position, 100);
        }

        void Standby()
        {
            PO.Position.Y = LogicRef.EnemyRef.Position.Y;

            if (FireTimer.Elapsed)
                Fire();
        }

        void CheckCollusion()
        {
            if (PO.CheckPlayBorders(new Vector2(110, 83), Vector2.One))
            {
                Reset();
                return;
            }

            if (LogicRef.PlayerRef.Enabled)
            {
                if (Sphere.Intersects(LogicRef.PlayerRef.Sphere))
                {
                    Reset();
                    LogicRef.LoseLife();
                }
            }
        }

        void Reset()
        {
            Enabled = false;
            LogicRef.EnemyRef.ResetSwirlTimer();
        }
    }
}
