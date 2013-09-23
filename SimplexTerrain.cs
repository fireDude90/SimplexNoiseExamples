using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Window;
using SFML.Graphics;
using SimplexNoise;

namespace NoiseGenerator {
    public class SimplexTerrain : Drawable {
        private readonly Color Brown = new Color(92, 64, 51);
        private readonly Color DarkGreen = new Color(0, 200, 0);

        VertexArray ground;
        VertexArray groundFill;
        Random random;
        /// <summary>
        /// Constructs the default terrain with green and brown colors.
        /// Also randomizes the SimplexNoise engine
        /// </summary>
        public SimplexTerrain() {
            Random random = new Random();
            byte[] seed = new byte[512];
            random.NextBytes(seed);
            Noise.perm = seed;

            ground = GetNoiseCurve(5, 100);
            groundFill = GetPointsUnderCurve(ground);
        }

        private VertexArray GetNoiseCurve(float spacing, float height) {
            VertexArray noiseCurve = new VertexArray(PrimitiveType.LinesStrip);
            for (int i = 0; i <= 500; i++) {
                //float y = PerlinNoiseGenerator.PerlinNoise(i);
                float x = i * spacing;
                float y = SimplexNoise.Noise.Generate(i / 100f);
                noiseCurve.Append(new Vertex(new Vector2f(x, y * height + 500), DarkGreen));
            }
            return noiseCurve;
        }

        private VertexArray GetPointsUnderCurve(VertexArray array, bool randomizeColor = false) {
            if (random == null) random = new Random();
            VertexArray result = new VertexArray(PrimitiveType.TrianglesStrip);
            for (uint i = 0; i < array.VertexCount; i++) {
                Vertex vertex = array[i];
                result.Append(vertex); // top vertex
                result.Append(new Vertex(new Vector2f(vertex.Position.X, 600), randomizeColor ? new Color((byte)(random.Next(256)), (byte)(random.Next(256)), (byte)(random.Next(256))) : Brown)); // vertex at bottom of screen 
            }
            return result;
        }

        /*private List<VertexArray> GetAnimatedNoiseCurve(float spacing, float height, uint frames) { // Used to create moving terrain
            int size = 500;
            List<VertexArray> animation = new List<VertexArray>();
            for (int y = 0; y <= frames; y++) {
                animation.Add(new VertexArray(PrimitiveType.LinesStrip));
                for (int x = 0; x <= size; x++) {
                    float value = SimplexNoise.Noise.Generate(x / 20f, y / 20f);
                    animation[y].Append(new Vertex(new Vector2f(x * spacing, value * height + 500), DarkGreen));
                }
            }

            return animation;
        }*/

        void Drawable.Draw(RenderTarget target, RenderStates states) {
            target.Draw(ground);
            target.Draw(groundFill);
        }
    }
}
