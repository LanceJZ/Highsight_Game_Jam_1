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
    class Missile : ModelEntity
    {
        #region Fields
        GameLogic LogicRef;
        SoundEffect ReadySound;
        State TheState;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Missile(Game game, Camera camera, GameLogic gameLogic) : base(game, camera)
        {
            LogicRef = gameLogic;

        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadModel("Missile");
            ReadySound = Helper.LoadSoundEffect("MissileReady");
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
                    Aiming();
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
            ReadySound.Play();
            base.Spawn(position);
            TheState = State.Standby;
            Velocity = Vector3.Zero;
        }

        public void Fire()
        {
            if (TheState == State.Standby)
            {
                TheState = State.Launched;
                PO.Velocity.X = 150;
            }
        }

        void Aiming()
        {
            PO.Position.Y = LogicRef.PlayerRef.Position.Y;
        }

        void CheckCollusion()
        {
            LogicRef.ShieldRef.CheckColusion(this);

            if (LogicRef.PlayerRef.Enabled)
            {
                if (LogicRef.PlayerRef.MissileCollusion())
                {
                    Enabled = false;
                    LogicRef.LoseLife();
                }
            }

            if (Sphere.Intersects(LogicRef.EnemyRef.SwirlRef.Sphere))
            {
                Enabled = false;
                LogicRef.EnemyRef.SwirlRef.HitByMissile();
                return;
            }

            if (LogicRef.EnemyRef.Enabled && !LogicRef.EnemyRef.SwirlRef.Enabled)
            {
                if (Sphere.Intersects(LogicRef.EnemyRef.Sphere))
                {
                    Enabled = false;
                    LogicRef.EnemyRef.Explode();
                    return;
                }
            }

            if (Position.X > 120)
                Enabled = false;
        }
    }
}
