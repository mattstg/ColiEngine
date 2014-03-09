using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ColiSys
{
    class TestContent
    {
        public ContentManager Content;
        public SpriteFont font_small;
        public Texture2D dirt; 
        /// <summary>
        /// Basic White Sqaure, cast to any color you want
        /// </summary>
        public Texture2D sqr; 
        
        //singleton set//////////////////////////
        private static TestContent instance;
        private TestContent() {
            
        }
        public static TestContent Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TestContent();
                }
                return instance;
            }
        }
        //////////////////////


        public void LoadContent(ContentManager c)
        {
            Content = c;
            Content.RootDirectory = "ColiEngine/Content";


            dirt = Content.Load<Texture2D>("dirt");
            font_small = Content.Load<SpriteFont>("font800");
            sqr = Content.Load<Texture2D>("BasicBox");


        }










    }
}
