using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Global
{
    class Button
    {
        public ColiSys.Hashtable ht = new ColiSys.Hashtable();
        ColiSys.NodeManipulator nami = ColiSys.NodeManipulator.Instance;
        int buttonID;
        Func<int,bool> eventCalled; 

        public Button(ColiSys.Hashtable ht, Func<int,bool> eventcalled, int buttonID)
        {
            this.ht = ht;
            eventCalled = eventcalled;
            this.buttonID = buttonID;

        }

        public bool Click(MouseState mouse)
        {
            if (ht.Coli(nami.CreateNodeFromClick(mouse)))
            {
                return eventCalled(buttonID);
            }

            return false;
        }

    }
}
