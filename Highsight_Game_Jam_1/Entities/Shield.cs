using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    class Shield : PositionedObject
    {
        List<Cube> TheBlocks = new List<Cube>();
        Camera CameraRef;
        Player PlayerRef;

        public Shield(Game game, Camera camera, GameLogic gameLogic) : base(game)
        {
            CameraRef = camera;
            PlayerRef = gameLogic.PlayerRef;
        }

        public override void Initialize()
        {
            base.Initialize();

            LoadContent();
        }

        public void LoadContent()
        {

        }

        public override void BeginRun()
        {
            base.BeginRun();

            Position.X = 100;
            Velocity.Y = 5;
            SetupShield();

        }

        public override void Update(GameTime gameTime)
        {
            Animate();

            base.Update(gameTime);
        }

        public bool CheckColusion(ModelEntity otherEntity)
        {
            foreach(ModelEntity block in TheBlocks)
            {
                if (block.Enabled)
                {
                    if (block.Sphere.Intersects(otherEntity.Sphere))
                    {
                        block.Enabled = false;
                        otherEntity.Enabled = false;
                        return true;
                    }
                }
            }

            return false;
        }

        void SetupShield()
        {
            for (int r = 0; r < 5; r++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        AddBlock(new Vector3((x * -4) + (r * -4), 60 + ((y * -4) + (r * -8)), 0));
                    }
                }
            }
        }

        void AddBlock (Vector3 position)
        {
            TheBlocks.Add(new Cube(Game, CameraRef));
            TheBlocks.Last().ModelScale = new Vector3(4);
            TheBlocks.Last().PO.AddAsChildOf(this);
            TheBlocks.Last().Spawn(position);
        }

        void Animate()
        {
            if (Position.Y > 20)
                Velocity.Y = -5;

            if (Position.Y < -20)
                Velocity.Y = 5;
        }
    }
}
