using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.Window;

namespace NoiseGenerator {
    public class Enviroment : Drawable {
        private SimplexTerrain terrian;
        private CircleShape sun;
        
        public Enviroment() {
            terrian = new SimplexTerrain();
            sun = new CircleShape(75);
            sun.FillColor = Color.Yellow;
            sun.Position = new Vector2f(75, 75);
        }

        void Drawable.Draw(RenderTarget target, RenderStates states) {
            target.Draw(terrian);
            target.Draw(sun);
        }
    }
}
