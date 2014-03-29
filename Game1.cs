#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using BodyParts;
using EntStructEnum;
using Structs;
using EntSys;
#endregion
using FactSys;
namespace ColiSys
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public enum GameState { MainGame, Factory }
        public static GameState gameState;
        public static GraphicsDeviceManager graphics;
        OneFactory Factory;
        TestContent tc;
        DebugCheatCodes cheats;
        public static SpriteBatch spriteBatch;
        GRAPHICTestWorld world;
       // DNABuilder dnaBuilder;
        //HumanPlayer human;
        ShapeGenerator shapeGen;
        Global.Bus bus = Global.Bus.Instance;
        NodeManipulator nami;
        


        public Game1()
            : base()
        {
            gameState = GameState.MainGame;
            Shape shapo;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            Consts.TopScope.WORLD_SIZE_X = graphics.PreferredBackBufferWidth;
            Consts.TopScope.WORLD_SIZE_Y = graphics.PreferredBackBufferHeight;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "ColiEngine/Content";
            BodyPartFactory bpFact = BodyPartFactory.Instance;
            TestDriver td = new TestDriver();

            Consts.TopScope.GRAPHICS_BUFFER_HEIGHT = graphics.PreferredBackBufferHeight;
            Consts.TopScope.GRAPHICS_BUFFER_WIDTH = graphics.PreferredBackBufferWidth;
            Consts.TopScope.GAME_SCALE = new S_XY(graphics.PreferredBackBufferWidth / Consts.TopScope.WORLD_SIZE_X, graphics.PreferredBackBufferHeight / Consts.TopScope.WORLD_SIZE_Y);

            //Consts.TopScope.GAME_SCALE = new S_XY(1, 1);
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to OverlapType.Before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Factory = OneFactory.Instance;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            tc = TestContent.Instance;
            tc.LoadContent(Content);


            cheats = DebugCheatCodes.Instance;
            
            nami = NodeManipulator.Instance;
            shapeGen = ShapeGenerator.Instance;
            
            //loading content
            
            world = new GRAPHICTestWorld();
            /////////////

           //singletons
           


           
            
            //MattDriver d = new MattDriver();
            //dnaBuilder = new DNABuilder();
            //human = new HumanPlayer(dnaBuilder.buildEntDNA(new S_XY(300, 360), new S_XY(50, 50)));
           // RockList = new Rock[5];
           // for (int i = 0; i < 5; i++)
             //   RockList[i] = new Rock(null, null, null, null, null);
            
            //world.LinkColiLists(human); //link human to world! Very important!
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
            
            world.LoadWorldTexture(tc.dirt);
            //human.DebugLoadHuman();
           // int c = 0;
           // foreach (Rock rock in RockList)
           // {-
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

            switch (gameState)
            {
                case GameState.Factory:
                    Factory.Update(rt);
                    break;
                case GameState.MainGame:
                    //human.Update(rt);
                    world.Update(rt);
                    break;
            }
    
            base.Update(gameTime);
           
        }

        protected void Input(float rt)
        {
            KeyboardState keys = Keyboard.GetState();
            MouseState mouses = Mouse.GetState();

            switch (gameState)
            {
                case GameState.Factory:
                    Factory.Input(keys, mouses);
                    cheats.Input();
                    break;
                case GameState.MainGame:
                    world.Input(keys,mouses);
                    cheats.Input();
                    //human.Input();
                    break;
            }
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);
            spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Factory:
                    Factory.Draw();
                    break;
                case GameState.MainGame:
                     world.Draw();
                     //human.Draw();
                    break;

            }
            spriteBatch.End();
           
            base.Draw(gameTime);
            
        }
    }
}
