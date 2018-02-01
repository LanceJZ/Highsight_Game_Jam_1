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
        KeyboardState OldKeyState;
        int PlayAreaHeight = 83;
        float MovementSpeed = 20;
        #endregion
        #region Properties
        public Shot ShotRef { get => TheShot; }
        public Missile MissileRef { get => TheMissile; }
        #endregion
        #region Constructor
        public Player(Game game, Camera camera, GameLogic gameLogic) : base(game, camera)
        {
            TheShot = new Shot(game, camera, gameLogic);
            TheMissile = new Missile(game, camera, gameLogic);
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
            LoadModel("Player");
            base.LoadContent();
        }

        public override void BeginRun()
        {
            base.BeginRun();

            Spawn(new Vector3(-100, 0, 0));
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            PO.WrapTopBottom(PlayAreaHeight);
            Input();
            BlockCollusion();

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
                    if (!TheShot.Enabled)
                    {
                        TheShot.Spawn(Position + PO.VelocityFromAngleZ(Rotation.Z, 7),
                            Rotation, PO.VelocityFromAngleZ(Rotation.Z, 100));
                    }

                    if (TheMissile.Enabled)
                    {
                        TheMissile.Fire();
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

            OldKeyState = Keyboard.GetState();
        }
    }
}
