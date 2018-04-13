using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace Game_Engine.Managers
{
    public class AssetManager
    {
        private static AssetManager instance;

        private Dictionary<string, Texture2D> textureDict;
        private Dictionary<string, Model> modelDict;
        private Dictionary<string, SpriteFont> fontDict;


        private AssetManager()
        {
            textureDict = new Dictionary<string, Texture2D>();
            modelDict = new Dictionary<string, Model>();
            fontDict = new Dictionary<string, SpriteFont>();
        }

        public static AssetManager Instance()
        {
            if (instance == null)
            {
                instance = new AssetManager();
            }
            return instance;
        }

        public void AddContent<TypeT>(ContentManager Content, String contentName)
        {
            if (typeof(TypeT) == typeof(Model))
                modelDict.Add(contentName, Content.Load<Model>(contentName));
            else if (typeof(TypeT) == typeof(Texture2D))
                textureDict.Add(contentName, Content.Load<Texture2D>(contentName));
            else if (typeof(TypeT) == typeof(SpriteFont))
                fontDict.Add(contentName, Content.Load<SpriteFont>(contentName));
        }

        public TypeT GetContent<TypeT>(String contentName) where TypeT : class
        {
            if (typeof(TypeT) == typeof(Model))
                return modelDict[contentName] as TypeT;
            else if (typeof(TypeT) == typeof(Texture2D))
                return textureDict[contentName] as TypeT;
            else return null;
        }
    }
}
