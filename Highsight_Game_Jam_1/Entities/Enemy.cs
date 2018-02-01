﻿#region Using
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
            PO.Position.X = 100;
            PO.Velocity.Y = 5;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadModel("Enemy");
            base.LoadContent();
        }

        public override void BeginRun()
        {

            base.BeginRun();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            Animate();

            if (ArmTimer.Elapsed)
            {
                ArmSwirl();
            }

            base.Update(gameTime);
        }
        #endregion
        public void ResetSwirlTimer()
        {
            ArmTimer.Reset(Helper.RandomMinMax(4, 20));
        }

        void ArmSwirl()
        {
            if (!TheSwirl.Enabled)
            {
                TheSwirl.Spawn(new Vector3(Position.X, Position.Y, 4));
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
