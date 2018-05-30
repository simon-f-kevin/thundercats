using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Components.Preformance;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;

namespace Game_Engine.Systems
{
    /// <summary>
    /// This systems calculates the frames per second, it has functionality to display fps directly in window title
    /// </summary>
    public class FrameCounterSystem : IUpdateableSystem, IDrawableSystem
    {
        private Queue<float> frameBuffer;
        private const int MAXIMUM_SAMPLES = 60;
        private FPSComponent fpsComponent;
        private bool showInTitle;
        private GameWindow window;

        public FrameCounterSystem(bool showInTitle = false, GameWindow window = null)
        {
            this.showInTitle = showInTitle;
            this.window = window;
            frameBuffer = new Queue<float>();
        }

        public void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            fpsComponent = ComponentManager.Instance.GetConcurrentDictionary<FPSComponent>().Values.First() as FPSComponent; //there shoudl only exist one fps-comp 
            fpsComponent.CurrentFramesPerSecond = 1.0f / elapsedTime;

            frameBuffer.Enqueue(fpsComponent.CurrentFramesPerSecond);

            if (frameBuffer.Count > MAXIMUM_SAMPLES)
            {
                frameBuffer.Dequeue();
                fpsComponent.AverageFramesPerSecond = frameBuffer.Average(i => i);
            }
            else
            {
                fpsComponent.AverageFramesPerSecond = fpsComponent.CurrentFramesPerSecond;
            }

            fpsComponent.TotalFrames++;
            fpsComponent.TotalSeconds += elapsedTime;

            UIComponent uIComponent = ComponentManager.Instance.GetConcurrentDictionary<UIComponent>().Values.First() as UIComponent;
            uIComponent.Text = "FPS: " + fpsComponent.CurrentFramesPerSecond.ToString();
        }

        public void Draw(GameTime gameTime)
        {
            if (showInTitle)
            {
                if(fpsComponent != null)
                {
                    window.Title = "Current FPS: " + fpsComponent.CurrentFramesPerSecond.ToString();
                }
            }
        }
    }
}
