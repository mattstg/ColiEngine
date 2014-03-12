using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BodyParts;
using EntSys;
using ColiSys;
using Microsoft.Xna.Framework.Input;

namespace FactSys
{
    public enum FactoryState
    {
        BodyPartDesigner,CharacterDesigner,MaterialDesigner

    }


    class OneFactory
    {

        BodyPartDesigner bpDesigner;
        CharacterDesigner charDesigner;
        MaterialDesigner matDesigner;
        MaterialFactory matFactory;

        public FactoryState state;





        private static OneFactory instance;
        public static OneFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OneFactory();
                }
                return instance;
            }
        }
        private OneFactory()
        {
            bpDesigner = BodyPartDesigner.Instance;
            charDesigner = CharacterDesigner.Instance;
            matDesigner = MaterialDesigner.Instance;
            matFactory = MaterialFactory.Instance;



            state = FactoryState.BodyPartDesigner;
        }






        public void Draw()
        {
            switch (state)
            {
               case FactoryState.BodyPartDesigner:
                    bpDesigner.Draw();
                break;

                case FactoryState.CharacterDesigner:
                    charDesigner.Draw();
                break;
            
                case FactoryState.MaterialDesigner:
                break;




            }

        }

        
        public void Update(float rt)
        {
            switch (state)
            {
                case FactoryState.BodyPartDesigner:
                    bpDesigner.Update(rt);
                    break;

                case FactoryState.CharacterDesigner:
                    charDesigner.Update(rt);
                    break;

                case FactoryState.MaterialDesigner:
                    break;




            }

        }

        public void Input(KeyboardState keys, MouseState mouses)
        {
            switch (state)
            {
                case FactoryState.BodyPartDesigner:
                    bpDesigner.Input(keys, mouses);
                    break;

                case FactoryState.CharacterDesigner:
                    charDesigner.Input(keys,mouses);
                    break;

                case FactoryState.MaterialDesigner:
                    break;




            }

        }



    }
}
