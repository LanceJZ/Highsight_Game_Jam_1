using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Highsight_Game_Jam_1
{
    class Skybox : DrawableGameComponent
    {
        #region Fields
        Camera TheCamera;
        BasicEffect Effect;
        Texture2D Texture;
        VertexBuffer CubeVertexBuffer;
        List<VertexPositionTexture> Vertices = new List<VertexPositionTexture>();

        Matrix Center;
        Matrix Scale;
        Matrix Translate;
        Matrix Rotate;

        string TextureFileName;
        float TheRotation;

        public Vector3 Position;
        #endregion
        #region Properties
        public float Rotation
        {
            get => TheRotation;
            set => TheRotation = MathHelper.WrapAngle(value);
        }

        #endregion
        #region Constructor
        public Skybox(Game game, Camera camera, string textureFileName) : base(game)
        {
            TheCamera = camera;
            Effect = new BasicEffect(GraphicsDevice);
            TextureFileName = textureFileName;
            Position = new Vector3(-0.5f, -0.5f, -0.5f);

            game.Components.Add(this);
        }
        #endregion
        protected override void LoadContent()
        {
            Texture = Helper.LoadTexture(TextureFileName);

            base.LoadContent();
            BeginRun();
        }

        protected override void Dispose(bool disposing)
        {
            Texture = null;
            Vertices.Clear();
            Vertices.TrimExcess();
            Vertices = null;
            base.Dispose(disposing);
        }

        public void Remove()
        {
            Game.Components.Remove(this);
            Dispose();
        }

        public void BeginRun()
        {
            CreateCube();
        }

        void CreateCube()
        {
            // Create the cube’s vertical faces
            BuildFace(new Vector3(0, 0, 0), new Vector3(0, 1, 1),
                new Vector2(0, 0.25f)); // West face
            BuildFace(new Vector3(0, 0, 1), new Vector3(1, 1, 1),
                new Vector2(0.75f, 0.25f)); // South face
            BuildFace(new Vector3(1, 0, 1), new Vector3(1, 1, 0),
                new Vector2(0.5f, 0.25f)); // East face
            BuildFace(new Vector3(1, 0, 0), new Vector3(0, 1, 0),
                new Vector2(0.25f, 0.25f)); // North face

            // Create the cube’s horizontal faces
            BuildFaceHorizontal(new Vector3(1, 1, 0), new Vector3(0, 1, 1),
                new Vector2(0.25f, 0)); // Top face
            BuildFaceHorizontal(new Vector3(1, 0, 1), new Vector3(0, 0, 0),
                new Vector2(0.25f, 0.5f)); // Bottom face

            CubeVertexBuffer = new VertexBuffer(GraphicsDevice,
                VertexPositionTexture.VertexDeclaration, Vertices.Count, BufferUsage.WriteOnly);

            CubeVertexBuffer.SetData(Vertices.ToArray());
        }
        #region Helper Methods
        void BuildFace(Vector3 p1, Vector3 p2, Vector2 txCoord)
        {
            Vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, txCoord.X + 0.25f, txCoord.Y + 0.25f));
            Vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, txCoord.X, txCoord.Y));
            Vertices.Add(BuildVertex(p1.X, p2.Y, p1.Z, txCoord.X + 0.25f, txCoord.Y));

            Vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, txCoord.X + 0.25f, txCoord.Y + 0.25f));
            Vertices.Add(BuildVertex(p2.X, p1.Y, p2.Z, txCoord.X, txCoord.Y + 0.25f));
            Vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, txCoord.X, txCoord.Y));
        }

        void BuildFaceHorizontal(Vector3 p1, Vector3 p2, Vector2 txCoord)
        {
            Vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, txCoord.X, txCoord.Y + 0.25f));
            Vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, txCoord.X + 0.25f, txCoord.Y));
            Vertices.Add(BuildVertex(p2.X, p1.Y, p1.Z, txCoord.X + 0.25f, txCoord.Y + 0.25f));

            Vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, txCoord.X, txCoord.Y + 0.25f));
            Vertices.Add(BuildVertex(p1.X, p1.Y, p2.Z, txCoord.X, txCoord.Y));
            Vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, txCoord.X + 0.25f, txCoord.Y));
        }

        VertexPositionTexture BuildVertex(float x, float y, float z, float u, float v)
        {
            return new VertexPositionTexture(new Vector3(x, y, z), new Vector2(u, v));
        }
        #endregion
        public override void Update(GameTime gameTime)
        {
            Center = Matrix.CreateTranslation(Position);
            Scale = Matrix.CreateScale(200f);
            Translate = Matrix.CreateTranslation(TheCamera.Position);
            Rotate = Matrix.CreateRotationY(Rotation);

            base.Update(gameTime);
        }
        #region Draw
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Effect.VertexColorEnabled = false;
            Effect.TextureEnabled = true;
            Effect.Texture = Texture;
            Effect.LightingEnabled = false;
            Effect.World = Center * Rotate * Scale * Translate;
            Effect.View = TheCamera.View;
            Effect.Projection = TheCamera.WideProjection;

            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.SetVertexBuffer(CubeVertexBuffer);
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0,
                    CubeVertexBuffer.VertexCount / 3);
            }
        }
        #endregion

    }
}
