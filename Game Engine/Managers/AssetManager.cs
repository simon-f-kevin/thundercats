using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace Game_Engine.Managers
{
    public class AssetManager
    {
        private static AssetManager instance;
        private Dictionary<Type, Dictionary<string, object>> ContentDict;


        private AssetManager()
        {
            ContentDict = new Dictionary<Type, Dictionary<string, object>>();
            ContentDict.Add(typeof(Texture2D), new Dictionary<string, object>());
            ContentDict.Add(typeof(Model), new Dictionary<string, object>());
            ContentDict.Add(typeof(SpriteFont), new Dictionary<string, object>());
            ContentDict.Add(typeof(Effect), new Dictionary<string, object>());
        }

        public static AssetManager Instance
        {
            get{
                if (instance == null)
                {
                    instance = new AssetManager();
                }
                return instance;
            }
        }

        public void AddContent<TypeT>(ContentManager Content, String contentName)
        {
            ContentDict[typeof(TypeT)].Add(contentName, Content.Load<TypeT>(contentName));
        }

        public TypeT GetContent<TypeT>(String contentName) where TypeT : class
        {
            return ContentDict[typeof(TypeT)][contentName] as TypeT;
        }
    }
}
