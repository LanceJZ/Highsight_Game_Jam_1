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
        List<Explode> TheExplosions = new List<Explode>();
        Camera CameraRef;
        GameLogic LogicRef;
        SoundEffect BlockHitSound;
        SoundEffect BlockEatSound;

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
            BlockHitSound = Helper.LoadSoundEffect("BlockExplode");
            BlockEatSound = Helper.LoadSoundEffect("BlockEat");
        }

        public override void BeginRun()
        {
            base.BeginRun();

            AddAsChildOf(LogicRef.EnemyRef.PO);
            SetupShield();
            Reset();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public void Reset()
        {
            foreach(ModelEntity block in TheBlocks)
            {
                block.Enabled = true;
                block.EmissiveColor = new Vector3(0.2f, 0, 0);
                block.DefuseColor = new Vector3(0.6f, 0, 0.1f);
            }
        }

        public bool CheckColusion(ModelEntity otherEntity)
        {
            ModelEntity block = DidEntityCollide(otherEntity);

            if (block == null)
                return false;

            MakeExplosion(block.PO.WorldPosition + block.Position);
            BlockHitSound.Play();
            LogicRef.AddPoints(69);
            block.Enabled = false;
            otherEntity.Enabled = false;
            return true;
        }

        public bool CheckEating()
        {
            ModelEntity block = DidEntityCollide(LogicRef.PlayerRef);

            if (block == null)
                return false;

            LogicRef.PlayerRef.Velocity =
                VelocityFromVectorsZ(LogicRef.PlayerRef.Position,
                block.Position, 120);

            if (Helper.RandomMinMax(0, 10) == 10)
            {
                BlockEatSound.Play();
                block.Enabled = false;
                LogicRef.AddPoints(160);
                return true;
            }

            return false;
        }

        void MakeExplosion(Vector3 position)
        {
            foreach (Explode boom in TheExplosions)
            {
                if (!boom.Enabled)
                {
                    SetExplode(boom, position);
                    return;
                }
            }

            TheExplosions.Add(new Explode(Game, CameraRef));
            SetExplode(TheExplosions.Last(), position);
        }

        void SetExplode(Explode boom, Vector3 position)
        {
            float rad = Helper.RandomMinMax(0.1f, 1);
            int mc = Helper.RandomMinMax(6, 10);
            float speed = Helper.RandomMinMax(10, 50);
            Vector3 color = new Vector3(Helper.RandomMinMax(0.1f, 1),
                Helper.RandomMinMax(0.1f, 1), Helper.RandomMinMax(0.1f, 1));
            Vector3 lightcolor = new Vector3(Helper.RandomMinMax(0.01f, 0.2f),
                Helper.RandomMinMax(0.01f, 0.1f), Helper.RandomMinMax(0.01f, 0.2f));

            boom.Setup(color, lightcolor);
            boom.Spawn(position, rad, mc, speed, 0.1f, 2);
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
