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
    class ModelEntity : DrawableGameComponent
    {
        #region Fields
        string ModelFileName;
        List<ModelEntity> Children = new List<ModelEntity>();
        protected Camera TheCamera;
        protected PositionedObject ThePO;
        protected Model TheModel;
        protected GraphicsDevice GD;
        protected Dictionary<string, Matrix> BaseTransforms = new Dictionary<string, Matrix>();
        protected Dictionary<string, Matrix> CurrentTransforms = new Dictionary<string, Matrix>();
        protected Matrix[] BoneTransforms;
        protected Matrix BaseWorld = Matrix.Identity;
        protected Vector3 MinVector;
        protected Vector3 MaxVector;
        protected Vector3 DrawOffset = Vector3.Zero;
        public Vector3 DefuseColor = Vector3.One;
        public Vector3 EmissiveColor = Vector3.Zero;
        public Vector3 ModelScale = Vector3.One;
        public Vector3 ModelScaleVelocity = Vector3.Zero;
        public Vector3 ModelScaleAcceleration = Vector3.Zero;
        public float Alpha = 1;
        #endregion
        #region Properties
        public Camera CameraRef { get => TheCamera; }

        public virtual Vector3 Position
        {
            get => ThePO.Position;
            set => ThePO.Position = value;
        }

        public virtual Vector3 Velocity
        {
            get => ThePO.Velocity;
            set => ThePO.Velocity = value;
        }

        public virtual Vector3 Acceleration
        {
            get => ThePO.Acceleration;
            set => ThePO.Acceleration = value;
        }

        public virtual Vector3 Rotation
        {
            get => ThePO.Rotation;
            set => ThePO.Rotation = value;
        }

        public virtual Vector3 RotationVelocity
        {
            get => ThePO.RotationVelocity;
            set => ThePO.RotationVelocity = value;
        }

        public virtual Vector3 RotationAcceleration
        {
            get => ThePO.RotationAcceleration;
            set => ThePO.RotationAcceleration = value;
        }

        public PositionedObject PO { get => ThePO; }

        public Model ModelRef { get => TheModel; }

        public Matrix WorldMatrixRef { get => BaseWorld; }

        public BoundingBox Bounds
        {
            get
            {
                Matrix scaleMatrix = Matrix.CreateScale(Scale);
                Matrix rotate = RotateMatrix(Rotation);
                Matrix translate = Matrix.CreateTranslation(Position);

                Matrix transform = scaleMatrix * rotate * translate;

                Vector3 v1 = Vector3.Transform(MinVector, transform);
                Vector3 v2 = Vector3.Transform(MaxVector, transform);
                Vector3 boxMin = Vector3.Min(v1, v2);
                Vector3 boxMax = Vector3.Max(v1, v2);

                return new BoundingBox(boxMin, boxMax);
            }
        }

        public BoundingSphere Sphere
        {
            get
            {
                BoundingSphere sphere = TheModel.Meshes[0].BoundingSphere;
                sphere.Radius *= 0.75f;
                sphere = sphere.Transform(BaseWorld);
                return sphere;
            }
        }

        public float Scale
        {
            get => (ModelScale.X + ModelScale.Y + ModelScale.Z) / 3;
            set => ModelScale = new Vector3(value);
        }

        public bool Hit
        {
            get => ThePO.Hit;
            set => ThePO.Hit = value;
        }

        public bool Moveable
        {
            get => ThePO.Moveable;
            set => ThePO.Moveable = value;
        }

        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                Visible = value;

                foreach (ModelEntity me in Children)
                {
                    me.Visible = value;
                }

                ThePO.Enabled = value;
            }
        }
        #endregion
        #region Constructor
        public ModelEntity(Game game, Camera camera) : base(game)
        {
            TheCamera = camera;
            ThePO = new PositionedObject(game);

            game.Components.Add(this);
        }

        public ModelEntity(Game game, Camera camera, Model model) : base(game)
        {
            TheCamera = camera;
            ThePO = new PositionedObject(game);
            SetModel(model);

            game.Components.Add(this);
        }

        public ModelEntity(Game game, Camera camera, string modelFileName) : base(game)
        {
            TheCamera = camera;
            ThePO = new PositionedObject(game);
            ModelFileName = modelFileName;

            game.Components.Add(this);
        }

        public ModelEntity(Game game, Camera camera, Model model, Vector3 position) : base(game)
        {
            TheCamera = camera;
            ThePO = new PositionedObject(game);
            ThePO.Position = position;
            SetModel(model);

            game.Components.Add(this);
        }

        public ModelEntity(Game game, Camera camera, string modelFileName, Vector3 position) : base(game)
        {
            TheCamera = camera;
            ThePO = new PositionedObject(game);
            ThePO.Position = position;
            ModelFileName = modelFileName;

            game.Components.Add(this);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            Visible = false;
            base.Initialize();
            LoadContent();
            BeginRun();
        }

        protected override void LoadContent()
        {
            if (TheModel == null)
                LoadModel(ModelFileName);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Dispose(bool disposing)
        {
            TheModel = null;

            base.Dispose(disposing);
        }

        public void Remove()
        {
            Enabled = false;
            Game.Components.Remove(this);
            Dispose();
        }

        public virtual void BeginRun()
        {
            Enabled = false;

            if (TheModel != null)
            {
                BoneTransforms = new Matrix[TheModel.Bones.Count];

                for (int i = 1; i < TheModel.Bones.Count; i++)
                {
                    BaseTransforms[TheModel.Bones[i].Name] = TheModel.Bones[i].Transform;
                    CurrentTransforms[TheModel.Bones[i].Name] = TheModel.Bones[i].Transform;
                }

                MinVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                MaxVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                foreach (ModelMesh mesh in TheModel.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        VertexPositionNormalTexture[] vertexData = new
                        VertexPositionNormalTexture[part.VertexBuffer.VertexCount];
                        part.VertexBuffer.GetData<VertexPositionNormalTexture>(vertexData);

                        for (int i = 0; i < part.VertexBuffer.VertexCount; i++)
                        {
                            MinVector = Vector3.Min(MinVector, vertexData[i].Position);
                            MaxVector = Vector3.Max(MaxVector, vertexData[i].Position);
                        }
                    }
                }

                Game.SuppressDraw();
                Enabled = true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("The Model was null for this Entity. " + this);
            }
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MatrixUpdate();
        }

        public void MatrixUpdate()
        {
            if (TheModel != null && BoneTransforms != null)
            {
                TheModel.Root.Transform = Matrix.CreateScale(ModelScale) *
                    RotateMatrix(Rotation) * Matrix.CreateTranslation(Position + DrawOffset);

                if (ThePO.Child)
                {
                    foreach (PositionedObject po in ThePO.ParentPOs)
                    {
                        TheModel.Root.Transform *=
                            RotateMatrix(po.Rotation) * Matrix.CreateTranslation(po.Position);
                    }
                }

                foreach (string keys in CurrentTransforms.Keys)
                {
                    TheModel.Bones[keys].Transform = CurrentTransforms[keys];
                }

                TheModel.CopyAbsoluteBoneTransformsTo(BoneTransforms);
                BaseWorld = TheModel.Root.Transform;
            }
        }
        #endregion
        #region Draw
        public override void Draw(GameTime gameTime)
        {
            if (TheModel == null)
            {
                return;
            }

            if (TheCamera == null)
            {
                System.Diagnostics.Debug.WriteLine(
                    "The Camera is not setup (null) on the class. " + this);

                return;
            }

            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (ModelMesh mesh in TheModel.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                    basicEffect.World = BoneTransforms[mesh.ParentBone.Index];
                    basicEffect.View = TheCamera.View;
                    basicEffect.Projection = TheCamera.Projection;
                    basicEffect.DiffuseColor = DefuseColor;
                    basicEffect.EmissiveColor = EmissiveColor;
                    basicEffect.Alpha = Alpha;
                    basicEffect.EnableDefaultLighting();
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
        #endregion
        #region Spawn
        /// <summary>
        /// If position, rotation and velocity are used.
        /// </summary>
        /// <param name="position">Position to spawn at.</param>
        /// <param name="rotation">Rotation to spawn at.</param>
        /// <param name="velocity">Initial Velocity to spawn with.</param>
        public virtual void Spawn(Vector3 position, Vector3 rotation, Vector3 velocity)
        {
            Velocity = velocity;
            Spawn(position, rotation);
        }
        /// <summary>
        /// If only position and rotation are used.
        /// </summary>
        /// <param name="position">Position to spawn at.</param>
        /// <param name="rotation">Rotation to spawn at.</param>
        public virtual void Spawn(Vector3 position, Vector3 rotation)
        {
            Rotation = rotation;
            Spawn(position);
        }
        /// <summary>
        /// If only position is used.
        /// </summary>
        /// <param name="position">Position to spawn at.</param>
        public virtual void Spawn(Vector3 position)
        {
            Enabled = true;
            ThePO.Hit = false;
            Position = position;
            MatrixUpdate();
        }
        #endregion
        #region Helper Methods
        public void AddAsChildOf(ModelEntity parent)
        {
            AddAsChildOf(parent, true);
        }

        public void AddAsChildOf(ModelEntity parent, bool activeDepedent)
        {
            parent.Children.Add(this);
            ThePO.AddAsChildOf(parent.ThePO, activeDepedent);
        }
        /// <summary>
        /// Sets the model from a loaded XNA Model.
        /// </summary>
        /// <param name="model">Loaded model.</param>
        public void SetModel(Model model)
        {
            if (model != null)
            {
                TheModel = model;
            }
        }
        /// <summary>
        /// Loads and sets the model from the file name.
        /// </summary>
        /// <param name="modelFileName">The file name located at content/Models</param>
        public void LoadModel(string modelFileName)
        {
            TheModel = Helper.LoadModel(modelFileName);
            SetModel(TheModel);
        }
        /// <summary>
        /// Return a Model that is loaded from the filename.
        /// </summary>
        /// <param name="modelFileName">The file name located at content/Models</param>
        /// <returns></returns>
        public Model Load(string modelFileName)
        {
            return Helper.LoadModel(modelFileName);
        }

        public Matrix RotateMatrix(Vector3 rotation)
        {
            return Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
        }

        public bool IsCollision(Model model1, Matrix world1, Model model2, Matrix world2)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);

                for (int meshIndex2 = 0; meshIndex2 < model2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = model2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(world2);

                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }
            return false;
        }
        #endregion
    }
}
