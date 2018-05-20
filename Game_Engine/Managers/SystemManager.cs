using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Managers
{
    /*
     * Maintains all current Update and Draw systems.
     * Note that the queues are not thread safe meaning that only one thread should write to them at any one time.
     * This limitation is considered acceptable as systems should be added at game initialization and are normally registered throughout the rest of runtime.
     */
    public class SystemManager
    {
        public Queue<IUpdateableSystem> UpdateableSystems { get; set; }
        public Queue<IDrawableSystem> DrawableSystems { get; set; }

        #region Thread-safe singleton - use "SystemManager.Instance" to access
        private static readonly Lazy<SystemManager> lazy = new Lazy<SystemManager>(() => new SystemManager());

        private SystemManager()
        {
            UpdateableSystems = new Queue<IUpdateableSystem>();
            DrawableSystems = new Queue<IDrawableSystem>();
        }
        public static SystemManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        #endregion

        public void AddToUpdateables(params IUpdateableSystem[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                UpdateableSystems.Enqueue(list[i]);
            }
        }

        public void AddToDrawables(params IDrawableSystem[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                DrawableSystems.Enqueue(list[i]);
            }
        }
        public void ClearDrawables()
        {
            DrawableSystems.Clear();
        }
        public void ClearUpdateables()
        {
            UpdateableSystems.Clear();
        }
        public void RemoveFromUpdateables(params IUpdateableSystem[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                UpdateableSystems.ToList().Remove(list[i]);
            }
        }
        public void RemoveFromDrawables(params IDrawableSystem[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                DrawableSystems.ToList().Remove(list[i]);
            }
        }

        public void Update(GameTime gameTime)
        {
            int size = UpdateableSystems.Count;
            for (int i = 0; i < size; i++)
            {
                IUpdateableSystem system = UpdateableSystems.Dequeue();
                system.Update(gameTime);
                UpdateableSystems.Enqueue(system);
            }
        }
        public void Draw(GameTime gameTime)
        {
            int size = DrawableSystems.Count;
            for (int i = 0; i < size; i++)
            {
                IDrawableSystem system = DrawableSystems.Dequeue();
                system.Draw(gameTime);
                DrawableSystems.Enqueue(system);
            }
        }
    }
}
