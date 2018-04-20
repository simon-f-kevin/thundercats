using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Game_Engine.Managers
{
    public class AssetManager
    {
        private static AssetManager instance;
        private Dictionary<Type, Dictionary<string, object>> contentDictionary;

        private AssetManager()
        {
            contentDictionary = new Dictionary<Type, Dictionary<string, object>>();
        }

        public static AssetManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AssetManager();
                }
                return instance;
            }
        }

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
    }
}