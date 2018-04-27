using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Game_Engine.Managers
{
    /*
     * The AssetManager centralises storage of game content.
     * Though in most cases content would be loaded at startup only and should be quick enough not to warrant multithreading,
     * the AssetManager has still been made thread safe to enable concurrency in instances where content would be modified by multiple threads.
     */
    public sealed class AssetManager
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> contentDictionary;

        #region Thread-safe singleton - use "AssetManager.Instance" to access
        private static readonly Lazy<AssetManager> lazy = new Lazy<AssetManager>(() => new AssetManager());

        private AssetManager()
        {
            contentDictionary = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();
        }

        public static AssetManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        #endregion

        public void AddContent<T>(ContentManager Content, String contentName)
        {
            if (!contentDictionary.ContainsKey(typeof(T)))
            {
                contentDictionary.TryAdd(typeof(T), new ConcurrentDictionary<string, object>());
            }
            contentDictionary[typeof(T)].TryAdd(contentName, Content.Load<T>(contentName));
        }

        public T GetContent<T>(String contentName) where T : class
        {
            return contentDictionary[typeof(T)][contentName] as T;
        }

        public static Texture2D CreateTexture(Color color, GraphicsDevice graphicsDevice)
        {
            return CreateTexture(graphicsDevice, 1, 1, pixel => color);
        }
        private static Texture2D CreateTexture(GraphicsDevice device, int width, int height, System.Func<int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] colorArray = new Color[width * height];
            for (int pixel = 0; pixel < colorArray.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                colorArray[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(colorArray);

            return texture;
        }
    }
}