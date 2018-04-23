using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Concurrent;

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
    }
}