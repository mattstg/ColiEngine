#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Enums.Node;
using EntStructEnum;
using Structs;
using EntSys;
#endregion

namespace ColiSys
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        
        GraphicsDeviceManager graphics;
        TestContent tc;
        DebugCheatCodes cheats;
        public static SpriteBatch spriteBatch;
        GRAPHICTestWorld world;
        DNABuilder dnaBuilder;
        HumanPlayer human;
        ShapeGenerator shapeGen;
        Rock[] RockList;
        Global.Bus bus = Global.Bus.Instance;
        




        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;

            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "ColiEngine/Content";


            Consts.TopScope.GRAPHICS_BUFFER_HEIGHT = graphics.PreferredBackBufferHeight;
            Consts.TopScope.GRAPHICS_BUFFER_WIDTH = graphics.PreferredBackBufferWidth;
            Consts.TopScope.GAME_SCALE = new S_XY(Consts.TopScope.GRAPHICS_BUFFER_WIDTH / Consts.TopScope.WORLD_SIZE_X,Consts.TopScope.GRAPHICS_BUFFER_HEIGHT / Consts.TopScope.WORLD_SIZE_Y);
            
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to OverlapType.Before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            tc = TestContent.Instance;
            cheats = DebugCheatCodes.Instance;
            world = new GRAPHICTestWorld();
            
            MattDriver d = new MattDriver();
            dnaBuilder = new DNABuilder();
            shapeGen = ShapeGenerator.Instance;
            human = new HumanPlayer(dnaBuilder.buildEntDNA(new S_XY(300, 360), new S_XY(50, 50)), null, null, null, null);
           // RockList = new Rock[5];
           // for (int i = 0; i < 5; i++)
             //   RockList[i] = new Rock(null, null, null, null, null);
            
            world.LinkColiLists(human); //link human to world! Very important!
           // foreach (Rock rock in RockList)
           //     world.LinkColiLists(rock);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tc.LoadContent(Content);
            world.LoadWorldTexture(tc.dirt);
            
            human.DebugLoadSprite(tc.dirt, shapeGen.GenShape(Shape.Human,new S_XY(), new S_XY(5,15)),new S_XY(0,0),Color.Blue);
           // int c = 0;
           // foreach (Rock rock in RockList)
           // {
           //     rock.DebugLoadSprite(tc.dirt, shapeGen.GenShape(Shape.Square, new S_XY(), new S_XY(10, 10)), new S_XY(40*c, 40*c), Color.LightSlateGray);
           //     c++;
           // }
            //human.DebugLoadSprite(tc.dirt, shapeGen.GenShape(Shape.Square, new S_XY(10, 10), new S_XY(3, 6)), Color.Blue);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float rt = gameTime.ElapsedGameTime.Milliseconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Input(rt);
                        
            human.Update(rt);
           // foreach (Rock rock in RockList)
           //     rock.Update(rt);

            world.Update(rt);
                       
            base.Update(gameTime);
           
        }

        protected void Input(float rt)
        {
            world.Input();
            cheats.Input();
            human.Input();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            world.Draw(spriteBatch);
            human.Draw(spriteBatch);
          //  foreach (Rock rock in RockList)
          //      rock.Draw(spriteBatch);
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
            
        }
    }
}
