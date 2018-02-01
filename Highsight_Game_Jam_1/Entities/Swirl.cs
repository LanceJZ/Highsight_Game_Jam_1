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
        Timer FireTimer;
        State TheState;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Swirl(Game game, Camera camera, GameLogic gameLogic) : base(game, camera)
        {
            LogicRef = gameLogic;
            FireTimer = new Timer(game, 10);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadModel("Swirl");

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
        public override void Spawn(Vector3 position)
        {
            base.Spawn(position);

            FireTimer.Reset(Helper.RandomMinMax(2, 10));
            TheState = State.Standby;
            Velocity = Vector3.Zero;
            RotationVelocity = Vector3.Zero;
            RotationAcceleration = Vector3.Zero;
            PO.RotationAcceleration.Z = -MathHelper.Pi;
        }

        void Fire()
        {
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
            }

            if (LogicRef.PlayerRef.MissileRef.Enabled)
            {
                if (Sphere.Intersects(LogicRef.PlayerRef.MissileRef.Sphere))
                {
                    Reset();
                    LogicRef.PlayerRef.MissileRef.Enabled = false;
                }
            }

            if (LogicRef.PlayerRef.Enabled)
            {
                if (Sphere.Intersects(LogicRef.PlayerRef.Sphere))
                {
                    Reset();
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
