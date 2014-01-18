using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input; 

namespace Msquared.ColiEngine
{
    class UI
    {
        private bool D_test1Active;


        private static UI instance;
        private UI() { }
        public static UI Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UI();
                }
                return instance;
            }
        }






        public void Draw_D_test1()
        {



        }


    }
}
