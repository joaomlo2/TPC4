using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace TPC4
{
    class TextureStrip
    {
        public BasicEffect effect;
        public Matrix worldMatrix;
        public Texture2D textura;
        public VertexBuffer vertexBuffer1;
        public IndexBuffer indexBuffer1;
        public VertexPositionNormalTexture[] vertices;
        public int[] Indices;
        public float CamX;
        public float CamY;
        public float CamZ;

        public TextureStrip(GraphicsDevice device,Texture2D tex)
        {
            worldMatrix = Matrix.Identity;
            Indices = new int[4];
            effect = new BasicEffect(device);
            float aspectRatio = (float)device.Viewport.Width /
            device.Viewport.Height;
            CamX = 0.0f;
            CamY = 2.0f;
            CamZ = 2.0f;
            effect.View = Matrix.CreateLookAt(new Vector3(CamX, CamY, CamZ),Vector3.Zero, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10.0f);
            effect.VertexColorEnabled = false;
            textura = tex;
            effect.Texture = this.textura;
            effect.TextureEnabled = true;

            CreateVertices();
            

            Indices[0]=0;
            Indices[1]=1;
            Indices[2]=2;
            Indices[3]=3;

            effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
            effect.DirectionalLight0.Direction = new Vector3(0, -1, 0);
            effect.DirectionalLight0.SpecularColor = new Vector3(0, 0, 0);

            vertexBuffer1 = new VertexBuffer(device, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.None);
            vertexBuffer1.SetData<VertexPositionNormalTexture>(vertices);

            indexBuffer1 = new IndexBuffer(device, typeof(int), Indices.Length, BufferUsage.None);
            indexBuffer1.SetData<int>(Indices);
        }

        public void CreateVertices()
        {
            int vertexCount = 4;
            vertices = new VertexPositionNormalTexture[vertexCount];

            vertices[0] = new VertexPositionNormalTexture(new Vector3(-1.0f, 0.0f, +1.0f), new Vector3(0,1,0), new Vector2(0.0f, 1.0f));
            vertices[1] = new VertexPositionNormalTexture(new Vector3(-1.0f, 0.0f, -1.0f), new Vector3(0,1,0), new Vector2(0.0f, 0.0f));
            vertices[2] = new VertexPositionNormalTexture(new Vector3(+1.0f, 0.0f, +1.0f), new Vector3(0,1,0), new Vector2(1.0f, 1.0f));
            vertices[3] = new VertexPositionNormalTexture(new Vector3(+1.0f, 0.0f, -1.0f), new Vector3(0,1,0), new Vector2(1.0f, 0.0f));

        }

        public void Draw(GraphicsDevice device,Matrix ViewCam)
        {
            effect.View = ViewCam;
            effect.World = worldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            device.SetVertexBuffer(this.vertexBuffer1);
            device.Indices = this.indexBuffer1;

            device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, 4, 0, 2);
        }
    }
}
