using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    class Numbers : PositionedObject
    {
        Camera TheCamera;
        Model[] NumberModels = new Model[10];
        List<ModelEntity> NumberEntities = new List<ModelEntity>();
        //public Vector3 Position = Vector3.Zero;
        //public Vector3 Rotation = Vector3.Zero;
        //float Scale;

        public Numbers(Game game) : base(game)
        {
            TheCamera = new Camera(game, new Vector3(0, 0, 1000),
                new Vector3(0, MathHelper.Pi, 0), 0, 900, 1010);

            //game.Components.Add(this);
        }

        public override void Initialize()
        {
            TheCamera.MakeOrthGraphic();
            base.Initialize();
            LoadContent();
        }

        public void LoadContent()
        {
            for (int i = 0; i < 10; i++)
            {
                NumberModels[i] = Helper.LoadModel("Core/" + i.ToString());
            }
        }

        public void ProcessNumber(int number, Vector3 locationStart, Vector3 rotation, float scale)
        {
            Rotation = rotation;
            ProcessNumber(number, locationStart, scale);
        }

        public void ProcessNumber(int number, Vector3 locationStart, float scale)
        {
            Position = locationStart;
            Scale = scale;

            ChangeNumber(number);
        }

        public void ChangeNumber(int number, Vector3 defuseColor)
        {
            ChangeNumber(number);
            ChangeColor(defuseColor);
        }

        public void ChangeNumber(int number)
        {
            int numberIn = number;

            ClearNumbers();

            do
            {
                //Make digit the modulus of 10 from number.
                int digit = numberIn % 10;
                //This sends a digit to the draw function with the location and size.
                NumberEntities.Add(InitiateNumber(digit));
                // Dividing the int by 10, we discard the digit that was derived from the modulus operation.
                numberIn /= 10;
                // Move the location for the next digit location to the left. We start on the right hand side
                // with the lowest digit.
            } while (numberIn > 0);

            ChangePosition();
            ChangeRotation();
        }

        public void ChangePosition()
        {
            float space = 0;

            for (int i = NumberEntities.Count - 1; i > -1; i--)
            {
                NumberEntities[i].Position = new Vector3(space, 0, 0);
                NumberEntities[i].MatrixUpdate();
                space += (Scale * 11);
            }
        }

        public void Change(Vector3 position, Vector3 rotation)
        {
            ChangePosition(position);
            ChangeRotation(rotation);
        }

        public void ChangeRotation()
        {
            foreach (ModelEntity number in NumberEntities)
            {
                number.Rotation = Rotation;
                number.MatrixUpdate();
            }
        }

        public void ChangeRotation(Vector3 rotation)
        {
            Rotation = rotation;
            ChangeRotation();
        }

        public void ChangePosition(Vector3 position)
        {
            Position = position;
            ChangePosition();
        }

        public void ChangeColor(Vector3 defuseColor)
        {
            foreach (ModelEntity number in NumberEntities)
            {
                number.DefuseColor = defuseColor;
            }
        }

        public void ShowNumbers(bool show)
        {
            if (NumberEntities != null)
            {
                foreach (ModelEntity number in NumberEntities)
                {
                    number.Enabled = show;
                }
            }
        }

        void RemoveNumber(ModelEntity numberE)
        {
            NumberEntities.Remove(numberE);
        }

        void ClearNumbers()
        {
            foreach (ModelEntity digit in NumberEntities)
            {
                digit.Remove();
            }

            NumberEntities.Clear();
        }

        ModelEntity InitiateNumber(int number)
        {
            if (number < 0)
                number = 0;

            ModelEntity digit = new ModelEntity(Game, TheCamera, NumberModels[number]);

            digit.Moveable = false;
            digit.ModelScale = new Vector3(Scale);
            digit.PO.AddAsChildOf(this);

            return digit;
        }
    }
}
