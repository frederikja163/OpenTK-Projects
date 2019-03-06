using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Snake
{
    /// <summary>
    /// One component of the snake
    /// This class will manage drawing as well as movement of any particular parts of the snake
    /// </summary>
    public class SnakeComponent : DrawAble
    {
        /// <summary>
        /// The current position of the snakecomponent
        /// The position will be rounded to the nearest integer so whole numbers are guaranteed
        /// </summary>
        public Vector2 PositionInt
        {
            get
            {
                Vector2 pos = _transformMatrix.ExtractTranslation().Xy;
                return new Vector2((float)Math.Round((pos.X + 1f) * Config.GridSize.X + .5f), 
                    (float)Math.Round((pos.Y + 1f) * Config.GridSize.Y + .5f));
            }
            private set
            {
                _transformMatrix = _transformMatrix.ClearTranslation();
                _transformMatrix *= Matrix4.CreateTranslation((value.X-.5f) / Config.GridSize.X - 1f, (value.Y-.5f) / Config.GridSize.Y - 1f, 0);
            }
        }
        /// <summary>
        /// The current position of the snakecomponent
        /// The position will be a floating point number so precision is guaranteed
        /// </summary>
        public Vector2 PositionFloat
        {
            get
            {
                Vector2 pos = _transformMatrix.ExtractTranslation().Xy;
                return new Vector2((pos.X + 1f) * Config.GridSize.X + .5f, 
                    (pos.Y + 1f) * Config.GridSize.Y + .5f);
            }
            private set
            {
                _transformMatrix = _transformMatrix.ClearTranslation();
                _transformMatrix *= Matrix4.CreateTranslation((value.X-.5f) / Config.GridSize.X - 1f, (value.Y-.5f) / Config.GridSize.Y - 1f, 0);
            }
        }

        /// <summary>
        /// Field of the direction
        /// </summary>
        private Vector2 _direction;
        /// <summary>
        /// Direction of this part of the snake
        /// Used to make smooth movement
        /// </summary>
        public Vector2 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                PositionInt = PositionInt;
            }
        }

        /// <summary>
        /// Basic constructor to initialize the snake component with a given position
        /// </summary>
        /// <param name="positionInt">Position to initialize snake component at</param>
        public SnakeComponent(Vector2 positionInt)
            : base()
        {
            _transformMatrix *= Matrix4.CreateScale(.5f / Config.GridSize.X, .5f/ Config.GridSize.Y, 1);
            PositionInt = positionInt;
        }

        /// <summary>
        /// Define the layout of a vertex, this is called in the constructor from drawable
        /// </summary>
        protected override unsafe void VertexAttributeLayout()
        {
            //Define the layout of the VBO
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), 0);
            GL.EnableVertexAttribArray(0);
        }

        /// <inheritdoc />
        /// <summary>
        /// Generate the VB data for the VBO
        /// </summary>
        protected override Vertex[] VertexBuffer
        {
            get
            {
                const float minX = -1f, minY = -1f, maxX = 1f, maxY = 1f;
                return new Vertex[]
                {
                    new Vertex() {Position = new Vector2(minX, minY)},
                    new Vertex() {Position = new Vector2(maxX, minY)},
                    new Vertex() {Position = new Vector2(maxX, maxY)},
                    new Vertex() {Position = new Vector2(minX, maxY)}
                };
            }
        }

        /// <summary>
        /// Should be called once per update to update the position of the snake
        /// </summary>
        public void Update(float DeltaTime)
        {
            PositionFloat += Direction * DeltaTime / Config.TickSpeed;
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Generate the IB data for the IBO
        /// </summary>
        /// <returns>The newly generated IB</returns>
        protected override uint[] IndexBuffer =>
            new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };
    }
}
