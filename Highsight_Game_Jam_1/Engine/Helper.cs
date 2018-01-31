using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Highsight_Game_Jam_1
{
    public static class Helper
    {
        #region Fields
        static GraphicsDeviceManager TheGraphicsDM;
        static GraphicsDevice TheGraphicsD;
        static Random RandomNumberGenerator = new Random(DateTime.Now.Millisecond);
        static Game GameRef;
        #endregion
        #region Properties
        public static Random Rand { get => RandomNumberGenerator; }
        public static GraphicsDeviceManager GraphicsDM { get => TheGraphicsDM; }
        public static GraphicsDevice Graphics { get => TheGraphicsD; }
        public static Game TheGame { get=>GameRef; }
        /// <summary>
        /// Returns the window size in pixels, of the height.
        /// </summary>
        /// <returns>int</returns>
        public static int WindowHeight { get => TheGraphicsDM.PreferredBackBufferHeight; }
        /// <summary>
        /// Returns the window size in pixels, of the width.
        /// </summary>
        /// <returns>int</returns>
        public static int WindowWidth { get => TheGraphicsDM.PreferredBackBufferWidth; }
        /// <summary>
        /// Returns The Windows size in pixels as a Vector2.
        /// </summary>
        public static Vector2 WindowSize
        {
            get => new Vector2(TheGraphicsDM.PreferredBackBufferWidth,
                TheGraphicsDM.PreferredBackBufferHeight);
        }
        #endregion
        #region Initialize
        public static void Initialize(Game game, GraphicsDeviceManager graphicsDeviceManager,
            GraphicsDevice graphicsDevice)
        {
            GameRef = game;
            TheGraphicsDM = graphicsDeviceManager;
            TheGraphicsD = graphicsDevice;
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Get a random float between min and max
        /// </summary>
        /// <param name="min">the minimum random value</param>
        /// <param name="max">the maximum random value</param>
        /// <returns>float</returns>
        public static float RandomMinMax(float min, float max)
        {
            return min + (float)RandomNumberGenerator.NextDouble() * (max - min);
        }
        /// <summary>
        /// Get a random int between min and max
        /// </summary>
        /// <param name="min">the minimum random value</param>
        /// <param name="max">the maximum random value</param>
        /// <returns>int</returns>
        public static int RandomMinMax(int min, int max)
        {
            return min + (int)(RandomNumberGenerator.NextDouble() * ((max + 1) - min));
        }
        /// <summary>
        /// Loads XNA Model from file using the filename. Stored in Content/Models/
        /// </summary>
        /// <param name="modelFileName">File name of model to load.</param>
        /// <returns>XNA Model</returns>
        public static Model LoadModel(string modelFileName)
        {
            if (modelFileName != "")
            {
                if (File.Exists("Content/Models/" + modelFileName + ".xnb"))
                    return TheGame.Content.Load<Model>("Models/" + modelFileName);

                System.Diagnostics.Debug.WriteLine("The Model File " + modelFileName + " was not found.");
            }
            else
                System.Diagnostics.Debug.WriteLine("The Model File Name was empty");

            return null;
        }
        /// <summary>
        /// Loads Sound Effect from file using filename. Stored in Content/Sounds
        /// </summary>
        /// <param name="soundFileName">File Name of the sound.</param>
        /// <returns>SoundEffect</returns>
        public static SoundEffect LoadSoundEffect(string soundFileName)
        {
            if (soundFileName != "")
            {
                if (File.Exists("Content/Sounds/" + soundFileName + ".xnb"))
                    return TheGame.Content.Load<SoundEffect>("Sounds/" + soundFileName);
            }

            System.Diagnostics.Debug.WriteLine("The Sound File " + soundFileName + " was not found.");
            return null;
        }
        /// <summary>
        /// Loads Texture2D from file using the filename. Stored in Content/Textures
        /// </summary>
        /// <param name="textureFileName">File Name of the texture.</param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string textureFileName)
        {
            if (textureFileName != "")
            {
                if (File.Exists("Content/Textures/" + textureFileName + ".xnb"))
                    return TheGame.Content.Load<Texture2D>("Textures/" + textureFileName);
            }

            System.Diagnostics.Debug.WriteLine("The Texture File " + textureFileName + " was not found.");
            return null;
        }
        #endregion
    }
}
