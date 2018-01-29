using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    public class Words : GameComponent
    {
        Camera TheCamera;
        Model[] WordXNAModels = new Model[27];
        List<ModelEntity> WordEntityModels = new List<ModelEntity>();
        float Scale;
        int TextSize;
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;

        public Words(Game game) : base(game)
        {
            TheCamera = new Camera(game, new Vector3(0, 0, 1000),
                new Vector3(0, MathHelper.Pi, 0), 0, 900, 1010);

            game.Components.Add(this);
        }

        public override void Initialize()
        {
            TheCamera.MakeOrthGraphic();
            base.Initialize();
            LoadContent();
        }

        public void LoadContent()
        {
            for (int i = 0; i < 26; i++)
            {
                char letter = (char)(i + 65);

                WordXNAModels[i] = Helper.LoadModel("Core/" + letter.ToString());
            }

            WordXNAModels[26] = Helper.LoadModel("Core/UnderLine");
        }

        public void ProcessWords(string words, Vector3 locationStart, float scale)
        {
            Position = locationStart;
            Scale = scale;

            ChangeWords(words);
        }

        public void ChangeWords(string words, Vector3 defuseColor)
        {
            ChangeWords(words);
            ChangeColor(defuseColor);
        }

        public void ChangeWords(string words)
        {
            TextSize = words.Length;
            DeleteWords();

            int[] charCodes = new int[TextSize];

            for (int i = 0; i < TextSize; i++)
            {
                charCodes[i] = words[i];
            }

            for (int i = 0; i < TextSize; i++)
            {

                if (charCodes[i] > 96 && charCodes[i] < 123)
                {
                    charCodes[i] -= 32;
                }
            }

            foreach (int code in charCodes)
            {
                if (code > 64 && code < 91 || code == 95)
                {
                    int adjustedCode = code - 65;

                    if (code == 95)
                        adjustedCode = 26;

                    if (adjustedCode >= 0 && adjustedCode <= 26)
                    {
                        WordEntityModels.Add(InitiateLetter(adjustedCode));
                    }

                }

                if (code == 32)
                {
                    WordEntityModels.Add(new ModelEntity(Game, TheCamera, ""));
                }
            }

            ChangePosition();
            ChangeRotation();
        }

        public void Change(Vector3 position, Vector3 rotation)
        {
            ChangePosition(position);
            ChangeRotation(rotation);
        }

        public void ChangeRotation()
        {
            foreach (ModelEntity number in WordEntityModels)
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

        public void ChangePosition()
        {
            float space = 0;

            foreach (ModelEntity word in WordEntityModels)
            {
                word.Position = Position - new Vector3(space, 0, 0);
                word.MatrixUpdate();
                space -= Scale * 11.5f;
            }
        }

        public void ChangePosition(Vector3 position)
        {
            Position = position;
            ChangePosition();
        }

        public void ChangeColor(Vector3 defuseColor)
        {
            foreach (ModelEntity word in WordEntityModels)
            {
                word.DefuseColor = defuseColor;
            }
        }

        public void DeleteWords()
        {
            foreach (ModelEntity word in WordEntityModels)
            {
                word.Remove();
            }

            WordEntityModels.Clear();
        }

        public void ShowWords(bool show)
        {
            if (WordEntityModels != null)
            {
                foreach (ModelEntity word in WordEntityModels)
                {
                    word.Enabled = show;
                }
            }
        }

        ModelEntity InitiateLetter(int letter)
        {
            ModelEntity letterModel = new ModelEntity(Game, TheCamera, WordXNAModels[letter]);
            letterModel.Moveable = false;
            letterModel.ModelScale = new Vector3(Scale);

            return letterModel;
        }
    }
}
