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
            if (keys.IsKeyDown(Keys.I))
            {
                

            }
        }

        








    }

}
