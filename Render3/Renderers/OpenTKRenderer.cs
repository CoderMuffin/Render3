//#define TKRenderer //Comment out to disable OpenTKRenderer

#if TKRenderer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Render3.Core;

namespace Render3.Renderers
{
    public class OpenTKWindow : GameWindow
    {
        public OpenTKWindow(int w, int h, string title) : base(w, h, GraphicsMode.Default, title) { }
    }
    public struct Triangle
    {
        public Point2[] vertices;
        public Color c;
    }
    public class OpenTKRenderer : Renderer
    {
        public List<Triangle> triangles=new List<Triangle>();
        public bool arrayRendering = true;
        public bool tkRendering = false;
        public OpenTKWindow window;
        public OpenTKRenderer(Dimensions2 size,string title)
        {


            //Run takes a double, which is how many frames per second it should strive to reach.
            //You can leave that out and it'll just update as fast as the hardware will allow it.
            new System.Threading.Thread(()=> {
                window = new OpenTKWindow((int)size.width,(int)size.height,title);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();

                GL.MatrixMode(MatrixMode.Projection);
                GL.Ortho(0, 480, 320, 0, 0, 1000);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
                GL.Translate(0, 0, -10);
                window.RenderFrame += Window_UpdateFrame; window.Run(60.0,60.0);
            }).Start();
        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            while (arrayRendering) { } //don't do work while triangles are being drawn!
            tkRendering = true;
                GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit| ClearBufferMask.DepthBufferBit);
            foreach (Triangle t in triangles)
            {
                float[] simpleVertices = { *t.vertices[0].ToOpenTKFloatArr().join, t.vertices[1].ToOpenTKVector3(), t.vertices[2].ToOpenTKVector3() };
                GLuint vertexbuffer;
                // Generate 1 buffer, put the resulting identifier in vertexbuffer
                glGenBuffers(1, &vertexbuffer);
                // The following commands will talk about our 'vertexbuffer' buffer
                glBindBuffer(GL_ARRAY_BUFFER, vertexbuffer);
                // Give our vertices to OpenGL.
                glBufferData(GL_ARRAY_BUFFER, sizeof(g_vertex_buffer_data), g_vertex_buffer_data, GL_STATIC_DRAW);
            }
            tkRendering = false;
        }


        public override void DrawTriangle(Point2[] vertices, Color c)
        {
            triangles.Add(new Triangle() { vertices=vertices, c=c });
        }

        public override void StartDrawing(Dimensions2 screenSize)
        {
            while (tkRendering) { }
            arrayRendering = true;
            triangles.Clear();
        }

        public override void StopDrawing()
        {
            arrayRendering = false;
        }
    }
}
#endif