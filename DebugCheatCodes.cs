using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input; 

namespace ColiSys
{
    class DebugCheatCodes
    {

        int scrollValue = 0;
        

        private static DebugCheatCodes instance;
        private DebugCheatCodes() { }
        public static DebugCheatCodes Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DebugCheatCodes();
                }
                return instance;
            }
        }



        public void Input()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keys = Keyboard.GetState();
            _InputMouse(mouse); //maybe check if not null?
            _InputKeyboard(keys);
        }
      
       
        private void _InputMouse(MouseState mouse)
        {
           
            if (scrollValue != mouse.ScrollWheelValue)
            {//if scroll value has been altered
                Console.Out.Write(mouse.ScrollWheelValue + "vs" + scrollValue);
                if (scrollValue < mouse.ScrollWheelValue)
                {
                    if (Consts.TopScope.BRUSH_SIZE <= Consts.TopScope.BRUSH_MAX_SIZE)
                        Consts.TopScope.BRUSH_SIZE++;
                }
                else
                {
                    if (Consts.TopScope.BRUSH_SIZE > Consts.TopScope.BRUSH_MIN_SIZE)
                        Consts.TopScope.BRUSH_SIZE--;

                }
                scrollValue = mouse.ScrollWheelValue;
            }
           
        }

        private void _InputKeyboard(KeyboardState keys)
        {
            if (keys.IsKeyDown(Keys.F))
            {
                Game1.graphics.IsFullScreen = !Game1.graphics.IsFullScreen;
                Game1.graphics.ApplyChanges();
            }



            if(keys.IsKeyDown(Keys.O))
                if (Consts.TopScope.BRUSH_SIZE <= Consts.TopScope.BRUSH_MAX_SIZE)
                    Consts.TopScope.BRUSH_SIZE++;
            if(keys.IsKeyDown(Keys.P))
                if (Consts.TopScope.BRUSH_SIZE > Consts.TopScope.BRUSH_MIN_SIZE)
                    Consts.TopScope.BRUSH_SIZE--;
             //   Game1.graphics.PreferredBackBufferHeight = 540;
              //  Game1.graphics.PreferredBackBufferWidth = 1046;
               // Consts.TopScope.WORLD_SIZE_X = Game1.graphics.PreferredBackBufferWidth;
                //Consts.TopScope.WORLD_SIZE_Y = Game1.graphics.PreferredBackBufferHeight;
               // Consts.TopScope.GAME_SCALE = new Structs.S_XY(Game1.graphics.PreferredBackBufferWidth / Consts.TopScope.WORLD_SIZE_X, Game1.graphics.PreferredBackBufferHeight / Consts.TopScope.WORLD_SIZE_Y);

               
            
        }

        








    }

}
