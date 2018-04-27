﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Engine.Managers
{
    public sealed class AssetManager
    {
        private readonly Dictionary<Type, Dictionary<string, object>> contentDictionary;

        #region Thread-safe singleton - use "AssetManager.Instance" to access
        private static readonly Lazy<AssetManager> lazy = new Lazy<AssetManager>(() => new AssetManager());

        private AssetManager()
        {
            contentDictionary = new Dictionary<Type, Dictionary<string, object>>();
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
                contentDictionary.Add(typeof(T), new Dictionary<string, object>());
            }
            contentDictionary[typeof(T)].Add(contentName, Content.Load<T>(contentName));
        }

        public T GetContent<T>(String contentName) where T : class
        {
            return contentDictionary[typeof(T)][contentName] as T;
        }

        public Texture2D CreateTexture(Color filthyPeasantSpellingColor, GraphicsDevice graphicsDevice)
        {
            if (!contentDictionary.ContainsKey(typeof(Texture2D)))
            {
                contentDictionary.Add(typeof(Texture2D), new Dictionary<string, object>());
            }
            if (!contentDictionary[typeof(Texture2D)].ContainsKey(filthyPeasantSpellingColor.ToString()))
            {
                contentDictionary[typeof(Texture2D)].Add(filthyPeasantSpellingColor.ToString(), CreateTexture(graphicsDevice, 1, 1, pixel => filthyPeasantSpellingColor));
            }
            return contentDictionary[typeof(Texture2D)][filthyPeasantSpellingColor.ToString()] as Texture2D;

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