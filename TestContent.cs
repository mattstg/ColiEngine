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

        public Texture2D dirt;
        
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


        }










    }
}
