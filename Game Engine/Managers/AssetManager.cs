using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

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
    }
}