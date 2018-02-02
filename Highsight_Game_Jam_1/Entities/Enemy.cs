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
    class Enemy : ModelEntity
    {
        #region Fields
        GameLogic LogicRef;
        Swirl TheSwirl;
        Timer ArmTimer;
        SoundEffect ExplodeSound;
        SoundEffect AlertSound;
        bool AlertSounded;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Enemy(Game game, Camera camera, GameLogic gameLogic) : base(game, camera)
        {
            LogicRef = gameLogic;
            TheSwirl = new Swirl(game, camera, gameLogic);
            ArmTimer = new Timer(game, Helper.RandomMinMax(4, 20));
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadModel("Enemy");
            ExplodeSound = Helper.LoadSoundEffect("EnemyExplode");
            AlertSound = Helper.LoadSoundEffect("SwirlAlert");

            base.LoadContent();
        }

        public override void BeginRun()
        {

            base.BeginRun();
            Reset();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            Animate();

            if (LogicRef.CurrentMode == GameState.InPlay)
            {
                if (ArmTimer.Amount - ArmTimer.Seconds < 2f && !AlertSounded)
                {
                    AlertSounded = true;
                    AlertSound.Play();
                }

                if (ArmTimer.Elapsed)
                {
                    ArmSwirl();
                }
            }

            base.Update(gameTime);
        }
        #endregion
        public void Reset()
        {
            PO.Position.X = 100;
            PO.Velocity.Y = 5;
            DefuseColor = new Vector3(0.5f, 0.1f, 0.8f);
            TheSwirl.Enabled = false;
            ResetSwirlTimer();
        }

        public void Explode()
        {
            ExplodeSound.Play();
            Reset();
        }

        public void EndGame()
        {
            TheSwirl.Enabled = false;
        }

        public void ResetSwirlTimer()
        {
            ArmTimer.Reset(Helper.RandomMinMax(4, 20));
            AlertSounded = false;
        }

        void ArmSwirl()
        {
            if (!TheSwirl.Enabled)
            {
                TheSwirl.Spawn(new Vector3(Position.X, Position.Y, 4), DefuseColor);
            }
        }

        void Animate()
        {
            float amount = 15;

            if (Position.Y > amount)
                PO.Velocity.Y = -5;

            if (Position.Y < -amount)
                PO.Velocity.Y = 5;
        }

    }
}
