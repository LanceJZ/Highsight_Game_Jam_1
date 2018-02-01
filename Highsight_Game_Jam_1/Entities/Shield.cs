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
        GameLogic LogicRef;

        public Shield(Game game, Camera camera, GameLogic gameLogic) : base(game)
        {
            CameraRef = camera;
            LogicRef = gameLogic;
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

            AddAsChildOf(LogicRef.EnemyRef.PO);
            SetupShield();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public bool CheckColusion(ModelEntity otherEntity)
        {
            if (DidEntityCollide(otherEntity) == null)
                return false;

            DidEntityCollide(otherEntity).Enabled = false;
            otherEntity.Enabled = false;
            return true;
        }

        public bool CheckEating()
        {
            if (DidEntityCollide(LogicRef.PlayerRef) == null)
                return false;

            ModelEntity block = DidEntityCollide(LogicRef.PlayerRef);

            if (Helper.RandomMinMax(0, 10) == 10)
            {
                block.Enabled = false;
                return true;
            }

            LogicRef.PlayerRef.Velocity =
                VelocityFromVectorsZ(LogicRef.PlayerRef.Position,
                block.Position, 100);

            return false;
        }

        ModelEntity DidEntityCollide(ModelEntity otherEntity)
        {
            foreach (ModelEntity block in TheBlocks)
            {
                if (block.Enabled)
                {
                    if (block.Sphere.Intersects(otherEntity.Sphere))
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        void SetupShield()
        {
            float sidestart = 50;
            float outsidestart = 8;
            int thickness = 6;

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < thickness; x++)
                {
                    AddBlock(new Vector3((x * -4), sidestart + outsidestart + (y * -4), 0));
                }
            }

            for (int r = 0; r < 5; r++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < thickness; x++)
                    {
                        AddBlock(new Vector3((x * -4) + (r * -8), sidestart + ((y * -4) + (r * -8)), 0));
                    }
                }
            }

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < thickness; x++)
                {
                    AddBlock(new Vector3((x * -4) + -38, -10 + (y * 4), 0));
                }
            }

            for (int r = 0; r < 5; r++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < thickness; x++)
                    {
                        AddBlock(new Vector3((x * -4) + (r * -8), -sidestart + ((y * 4) + (r * 8)), 0));
                    }
                }
            }

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < thickness; x++)
                {
                    AddBlock(new Vector3((x * -4), -sidestart + -outsidestart + (y * 4), 0));
                }
            }
        }

        void AddBlock (Vector3 position)
        {
            TheBlocks.Add(new Cube(Game, CameraRef));
            TheBlocks.Last().ModelScale = new Vector3(2.75f);
            TheBlocks.Last().PO.AddAsChildOf(this);
            TheBlocks.Last().Spawn(position);
        }
    }
}
