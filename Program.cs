using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Window;
using SFML.Graphics;

namespace NoiseGenerator {
    class Program {
        #region Constants
        public static readonly Color CornflowerBlue = new Color(154, 206, 235);
        #endregion

        private static VertexArray GetNoiseCurve(float spacing, float height) {
            VertexArray noiseCurve = new VertexArray(PrimitiveType.LinesStrip);
            for (int i = 0; i <= 500; i++) {
                //float y = PerlinNoiseGenerator.PerlinNoise(i);
                float x = i * spacing - 500;
                float y = SimplexNoise.Noise.Generate(i / 20f);
                noiseCurve.Append(new Vertex(new Vector2f(x, y * height + 500), new Color(0, 200, 0, 255)));
            }
            return noiseCurve;
        }

        private static VertexArray GetPointsUnderCurve(VertexArray array) {
            VertexArray result = new VertexArray(PrimitiveType.TrianglesStrip);
            for (uint i = 0; i < array.VertexCount; i++) {
                Vertex vertex = array[i];
                result.Append(vertex); // top vertex
                result.Append(new Vertex(new Vector2f(vertex.Position.X, 600), new Color(92, 64, 51))); // vertex at bottom of screen 
            }
            return result;
        }

        static void Main(string[] args) {
            float spacing = 5;
            int height = 50;

            VertexArray noiseCurve = GetNoiseCurve(spacing, height);
            VertexArray underNoiseCurve = GetPointsUnderCurve(noiseCurve);

            #region Window/View
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "My Window");
            window.Closed += ((s, e) => window.Close());
            window.SetFramerateLimit(60);
            View view = new View();
            view.Reset(new FloatRect(0, 0, 800, 600));
            #endregion


            #region Text
            Font font = new Font("font.otf");
            Text text = new Text(String.Format("Current Spacing: {0}\nCurrent Height: {1}", spacing, height), font, 20);
            text.Position = new Vector2f(200, 150);
            text.Color = Color.Black;

            RectangleShape textBackground = new RectangleShape(new Vector2f(300, 100));
            textBackground.FillColor = Color.White;
            textBackground.Position = new Vector2f(125, 125);
            #endregion

            bool wasRpressed = false;

            while (window.IsOpen()) {
                window.DispatchEvents();
                window.SetView(view);


                window.Clear(CornflowerBlue);

                #region Move Viewport
                if (Keyboard.IsKeyPressed(Keyboard.Key.A)) {
                    view.Move(new Vector2f(-5, 0));
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D)) {
                    view.Move(new Vector2f(5, 0));
                }
                #endregion

                #region Change Line
                bool changed = false; // does the noise need to be recalculated?
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) {
                    height += 1;
                    changed = true;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Down)) {
                    height -= 1;
                    changed = true;
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) {
                    spacing -= 0.25f;
                    changed = true;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) {
                    spacing += 0.25f;
                    changed = true;
                }
                #endregion

                #region Take Screenshot
                bool takeScreenshot = false;
                if (Keyboard.IsKeyPressed(Keyboard.Key.O)) {
                    takeScreenshot = true;
                }
                #endregion

                #region Recalculate Seed
                bool isRpressed = Keyboard.IsKeyPressed(Keyboard.Key.R);
                if (isRpressed && !wasRpressed) {
                    byte[] seed = new byte[512];
                    new Random().NextBytes(seed);
                    SimplexNoise.Noise.perm = seed;
                    changed = true;
                    wasRpressed = true;
                }
                else if (!isRpressed) {
                    wasRpressed = false;
                }
                #endregion

                if (changed) {
                    noiseCurve = GetNoiseCurve(spacing, height);
                    underNoiseCurve = GetPointsUnderCurve(noiseCurve);
                    text.DisplayedString = String.Format("Current Spacing: {0}\nCurrent Height: {1}", spacing, height);
                }

                window.Draw(noiseCurve);
                window.Draw(underNoiseCurve);

                window.SetView(window.DefaultView);
                window.Draw(textBackground);
                window.Draw(text);

                window.Display();
                if (takeScreenshot) window.Capture().SaveToFile(@"..\..\screenshot.jpg");
            }
        }
    }
}
