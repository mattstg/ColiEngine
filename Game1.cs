#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
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
        SpriteBatch spriteBatch;
        GRAPHICTestWorld world;



        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;

            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "ColiEngine/Content";


            Constants.GRAPHICS_BUFFER_HEIGHT = graphics.PreferredBackBufferHeight;
            Constants.GRAPHICS_BUFFER_WIDTH = graphics.PreferredBackBufferWidth;
            Constants.GAME_SCALE = new S_XY(Constants.GRAPHICS_BUFFER_WIDTH / Constants.WORLD_SIZE_X,Constants.GRAPHICS_BUFFER_HEIGHT / Constants.WORLD_SIZE_Y);
            
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




            world.Update(rt);
                       
            base.Update(gameTime);
           
        }

        protected void Input(float rt)
        {
            world.Input();
            cheats.Input();
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
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
            
        }
    }
}
