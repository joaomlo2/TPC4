using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TPC4
{
    class Camera
    {
        //Nesta classe cria-se e controla-se a câmara
        Vector3 posC, dirC, targetC, baseVector;
        float  yaw, pitch;
        public Matrix viewC;
        public Matrix world, rotacao;
        static private MouseState originalMouseState;
        float strafeValue;
        float velocidade;
        float xDifference, auxX;
        float yDifference, auxY;
        public Camera(GraphicsDevice graphics)
        {
            Mouse.SetPosition(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2);
            originalMouseState = Mouse.GetState();
            this.posC = new Vector3(0,2,2);
            this.baseVector = new Vector3(1, 0, 0);
            this.dirC = new Vector3(1, 0, 0);
            this.strafeValue = 0.4f;
            this.yaw = 0.0f;
            this.pitch = 0.0f;
            this.world = Matrix.Identity;
            this.viewC = Matrix.CreateLookAt(new Vector3(0.0f, 2.0f, 2.0f), new Vector3(0f, 0f, 0f), Vector3.Up);
            this.xDifference = 0.0f;
            this.yDifference = 0.0f;
            this.auxX = originalMouseState.X;
            this.auxY = originalMouseState.Y;
            this.velocidade = 0.2f;
        }

        public void frente()
        {
            posC = posC + velocidade * dirC;
            targetC = posC + dirC;
        }

        public void tras()
        {
            posC = posC - velocidade * dirC;
            targetC = posC + dirC;
        }

        public void rodar(float yaw, float pitch)
        {
            rotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, pitch);
            world = rotacao;
            dirC = Vector3.Transform(baseVector, rotacao);
            targetC = posC + dirC;

        }

        public void strafeDireita(float strafeV)
        {
            strafeValue = strafeV * velocidade;
            posC = posC + velocidade * Vector3.Cross(dirC, Vector3.Up);
            targetC = posC + dirC;
        }

        public void strafeEsquerda(float strafeV)
        {
            strafeValue = strafeV * velocidade;
            posC = posC - velocidade * Vector3.Cross(dirC, Vector3.Up);
            targetC = posC + dirC;
        }

        public void Input(GraphicsDevice graphics)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();
            if (keyboardState.IsKeyDown(Keys.S))
            {
                tras();
                viewC = Matrix.CreateLookAt(posC, targetC, Vector3.Up);

            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                strafeEsquerda(strafeValue);
                viewC = Matrix.CreateLookAt(posC, targetC, Vector3.Up);

            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                strafeDireita(strafeValue);
                viewC = Matrix.CreateLookAt(posC, targetC, Vector3.Up);

            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                frente();
                viewC = Matrix.CreateLookAt(posC, targetC, Vector3.Up);

            }

            if (currentMouseState != originalMouseState)
            {
                xDifference = currentMouseState.X - originalMouseState.X;
                yDifference = currentMouseState.Y - originalMouseState.Y;

                if (xDifference < 0)
                {
                    if (yaw > -4)
                    {
                        yaw = yaw + (xDifference * (-0.01f));
                    }
                    yaw -= xDifference * 0.01f;
                }
                else if (xDifference > 0)
                {
                    if (yaw > 4)
                    {
                        yaw = -yaw + (xDifference * 0.01f);
                    }
                    yaw -= xDifference * 0.01f;
                }
                if (yDifference < 0)
                {
                    if (pitch > 1.4)
                    {
                        pitch = pitch - (yDifference * (-0.01f));
                        Console.WriteLine("Limitidador de Pitch : " + pitch + " Diferença " + (yDifference * (-0.01f)));
                    }
                    else
                    {
                        pitch -= yDifference * 0.01f;
                    }
                }
                else if (yDifference > 0)
                {
                    if (pitch < -1.4)
                    {
                        pitch = pitch + (yDifference * 0.01f);
                       Console.WriteLine("Limitidador de Pitch : " + pitch + " Diferença " + (yDifference * 0.01f));

                    }
                    else
                    {
                        pitch -= yDifference * 0.01f;
                    }
                }
                rodar(yaw, pitch);
                try { 
                Mouse.SetPosition(((int)graphics.Viewport.Width / 2), (int)(graphics.Viewport.Height / 2));
                } catch(Exception y){}
                viewC = Matrix.CreateLookAt(posC, targetC, Vector3.Up);
            }
        }
    }
}