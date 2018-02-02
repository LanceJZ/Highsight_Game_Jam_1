using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Highsight_Game_Jam_1
{
    public class Letters : PositionedObject
    {
        Camera TheCamera;
        Model[] WordXNAModels = new Model[27];
        List<ModelEntity> WordEntityModels = new List<ModelEntity>();

        public Letters(Game game) : base(game)
        {
            TheCamera = new Camera(game, new Vector3(0, 0, 1000),
                new Vector3(0, MathHelper.Pi, 0), 0, 900, 1010);
        }

        public override void Initialize()
        {
            TheCamera.MakeOrthGraphic();
            base.Initialize();
            LoadContent();
        }

        public void Setup(Vector3 position, float scale)
        {
            Position = position;
            Scale = scale;
        }

        public void Setup(Vector3 position, Vector3 rotation, float scale)
        {
            Rotation = rotation;
            Setup(position, scale);
        }

        public void Setup(string words, Vector3 position, float scale)
        {
            Setup(position, scale);
            SetWords(words);
        }

        public void SetWords(string words, Vector3 defuseColor)
        {
            SetWords(words);
            ChangeColor(defuseColor);
        }

        public void SetWords(string words)
        {
            int TotalSize = words.Length;
            DeleteWords();

            int[] charCodes = new int[TotalSize];

            for (int i = 0; i < TotalSize; i++)
            {
                charCodes[i] = words[i];
            }

            for (int i = 0; i < TotalSize; i++)
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
                    WordEntityModels.Add(new ModelEntity(Game, TheCamera));
                }
            }

            SetPosition();
        }

        public void SetPosition()
        {
            float space = 0;

            foreach (ModelEntity word in WordEntityModels)
            {
                word.Position = new Vector3(space, 0, 0);
                word.MatrixUpdate();
                space += Scale * 11.5f;
            }
        }

        public void ChangeEachRotation(Vector3 rotation)
        {
            foreach (ModelEntity number in WordEntityModels)
            {
                number.Rotation = rotation;
                number.MatrixUpdate();
            }
        }

        public void ChangeColor(Vector3 defuseColor)
        {
            foreach (ModelEntity word in WordEntityModels)
            {
                word.DefuseColor = defuseColor;
            }
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

        void DeleteWords()
        {
            foreach (ModelEntity word in WordEntityModels)
            {
                word.Remove();
            }

            WordEntityModels.Clear();
        }

        void LoadContent()
        {
            for (int i = 0; i < 26; i++)
            {
                char letter = (char)(i + 65);

                WordXNAModels[i] = Helper.LoadModel("Core/" + letter.ToString());
            }

            WordXNAModels[26] = Helper.LoadModel("Core/UnderLine");
        }

        ModelEntity InitiateLetter(int letter)
        {
            ModelEntity letterModel = new ModelEntity(Game, TheCamera, WordXNAModels[letter]);

            letterModel.Moveable = false;
            letterModel.ModelScale = new Vector3(Scale);
            letterModel.PO.AddAsChildOf(this);

            return letterModel;
        }
    }
}
