using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.Window;

namespace NoiseGenerator {
    class Game {
        public static readonly Color CornflowerBlue = new Color(154, 206, 235);

        private Color SkyColor = CornflowerBlue;
        Enviroment enviroment;
        public Game() {
            enviroment = new Enviroment();
        }

        public void Run() {
            #region Window/View
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "My Window");
            window.Closed += ((s, e) => window.Close());
            window.SetFramerateLimit(60);
            View view = new View();
            view.Reset(new FloatRect(0, 0, 800, 600));
            #endregion

            while (window.IsOpen()) {
                window.DispatchEvents();
                window.SetView(view);


                window.Clear(SkyColor);
                #region Take Screenshot
                bool takeScreenshot = false;
                if (Keyboard.IsKeyPressed(Keyboard.Key.O)) {
                    takeScreenshot = true;
                }
                #endregion

                #region Move Viewport
                if (Keyboard.IsKeyPressed(Keyboard.Key.A)) {
                    view.Move(new Vector2f(-5, 0));
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D)) {
                    view.Move(new Vector2f(5, 0));
                }
                #endregion

                window.Draw(enviroment);

                if (view.Center.X < 400) { // do not scroll off the screen
                    view.Center = new Vector2f(400, 300);
                }

                window.Display();
                if (takeScreenshot) window.Capture().SaveToFile(@"..\..\screenshot.jpg");
            }
        }
    }
}