using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;
using System.Linq;
using mania4key_v2.Legacy;

namespace mania4key_v2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        OsuMap Map;
        public static int i;
        Replay replay;
        KeyboardState previousState;
        private RenderTarget2D _backBuffer;
        private RenderTarget2D _backBuffer2;
        private RenderTarget2D _backBuffer3;
        BasicEffect effect;
        VertexPositionTexture[] floorVerts;
        float z1 = 0.5f;
        float z2 = 0.5f;
        float z3 = 0.5f;
        float z4 = 0.5f;

        bool z = false;
        bool x = false;
        bool c = false;
        bool v = false;
        int GameState = 0;

        bool landscape = false;

        _3DRenderer.Quads ground;
        _2DRenderer.OsuMania mania2d;
        _2DRenderer.OsuManiaInGame mania2dingame;
        _2DRenderer.IntroOutro introOutro;

        TouchCollection touchcoll;

        //legacy lyb!mania 4key launcher stuff
        public static string osu_path = "";
        public static bool legacy = true;
        public static int exit = 0;
        
        double curtime;
        int msecdelay = 0;
        int msecfinished = 0;
        bool beatenhighscore = false;
        bool letsaplay = false;
        SpriteEffects s = SpriteEffects.FlipHorizontally;

        private PerfProfiling.FrameCounter _frameCounter = new PerfProfiling.FrameCounter();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            ManagedBass.Bass.Init();

            //test new maplist parser
            //OsuMaps maps = new OsuMaps("D:\\Users\\lybxlpsv\\AppData\\Local\\osu!\\Songs");
            //maps.SaveToJson();
            //System.Environment.Exit(1);

            if (osu_path == "")
            {
                Exit();
            }
            Map = new OsuMap(osu_path);
            Map.path = Path.GetDirectoryName(osu_path);
            Map.filename = Path.GetFileName(osu_path);
            Map.LoadNotes();
            Map.LoadNotesSample();
            Map.SaveToJson();
            replay = new Replay(Map.bmid);
            i = ManagedBass.Bass.CreateStream(Map.path + IO.Path.sp + Map.audiofilename);

            OsuSoundManager.DefaultHitSounds.SetOsuDefault(IO.Path.datapath + IO.Path.audiopath);
            OsuSoundManager.DefaultHitSounds.LoadSounds();
            OsuSoundManager.CustomHitSounds.SetOsuDefault(Map.path + IO.Path.sp);
            OsuSoundManager.CustomHitSounds.LoadSounds();

            ManagedBass.Bass.ChannelSetAttribute(i, ManagedBass.ChannelAttribute.Volume, 0.8);
            //ManagedBass.Bass.ChannelPlay(i);

            introOutro = new _2DRenderer.IntroOutro(Map, GraphicsDevice, Content);

            int calcleadin = (6000 - Map.audioleadin);
            if (calcleadin <= 0)
            msecdelay = Map.audioleadin;
             else msecdelay = Map.audioleadin + (6000 - Map.audioleadin);

            floorVerts = new VertexPositionTexture[6];

            floorVerts[0].Position = new Vector3(-25, -360, 0);
            floorVerts[1].Position = new Vector3(-25, 25, 0);
            floorVerts[2].Position = new Vector3(25, -360, 0);

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(25, 25, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            // texcoord
            floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            floorVerts[1].TextureCoordinate = new Vector2(0, 1);
            floorVerts[2].TextureCoordinate = new Vector2(1, 0);

            floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            floorVerts[4].TextureCoordinate = new Vector2(1, 1);
            floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

            effect = new BasicEffect(graphics.GraphicsDevice);

            ground = new _3DRenderer.Quads(floorVerts, _backBuffer);
            mania2d = new _2DRenderer.OsuMania(GraphicsDevice);
            mania2dingame = new _2DRenderer.OsuManiaInGame(GraphicsDevice, Content);

            base.Initialize();
        }
        float testy = 54; //35f
        float testz = 27; //21f
        Vector3 cameraPosition = new Vector3(0, 54, 27);
        Vector3 cameraLookAtVector = new Vector3(0, 0, 8.4f);
        Vector3 cameraUpVector = new Vector3(0, 0, 1);

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (GraphicsDevice.PresentationParameters.Bounds.Width > GraphicsDevice.PresentationParameters.Bounds.Height)
            {
                    _backBuffer = new RenderTarget2D(GraphicsDevice, 300, 3600);
                    _backBuffer2 = new RenderTarget2D(GraphicsDevice, 1920, 1080);
                    landscape = true;
            }
            else
            {
                    _backBuffer = new RenderTarget2D(GraphicsDevice, 300, 800);
                    _backBuffer2 = new RenderTarget2D(GraphicsDevice, 1080, 1920);
                    landscape = false;
            }

            _backBuffer3 = new RenderTarget2D(GraphicsDevice, 1080, 1920);

            _2DRenderer.Fonts.LoadFonts(Content);
            
            // TODO: use this.Content to load your game content here
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            ManagedBass.Bass.Free();
            exit = 5;
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                exit++;

            if (GraphicsDevice.PresentationParameters.Bounds.Width > GraphicsDevice.PresentationParameters.Bounds.Height)
            {
               if (!landscape)
                {
                    _backBuffer = new RenderTarget2D(GraphicsDevice, 300, 3600);
                    _backBuffer2 = new RenderTarget2D(GraphicsDevice, 1920, 1080);
                    landscape = true;
                }
            } else
            {
                if (landscape)
                {
                    _backBuffer = new RenderTarget2D(GraphicsDevice, 300, 800);
                    _backBuffer2 = new RenderTarget2D(GraphicsDevice, 1080, 1920);
                    landscape = false;
                }
            }
            
            curtime = (ManagedBass.Bass.ChannelBytes2Seconds(i, ManagedBass.Bass.ChannelGetPosition(i, ManagedBass.PositionFlags.Bytes)) * 1000);

            KeyboardState state = Keyboard.GetState();
            bool zz = false;
            bool xx = false;
            bool cc = false;
            bool vv = false;

            if (msecdelay > 0)
            {

                msecdelay = msecdelay - gameTime.ElapsedGameTime.Milliseconds;
                curtime = -msecdelay;
            }
            else
            {
                if (letsaplay == false)
                {
                    ManagedBass.Bass.ChannelPlay(i);
                    letsaplay = true;
                }
            }
            /**
            if (!(previousState.IsKeyDown(Keys.D)) && state.IsKeyDown(Keys.D))
            { replay.inputs.Add(new ReplayInputs(1, (int)curtime, 1));  }
            if (!(previousState.IsKeyDown(Keys.F)) && state.IsKeyDown(Keys.F))
            { replay.inputs.Add(new ReplayInputs(3, (int)curtime, 1)); }
            if (!(previousState.IsKeyDown(Keys.J)) && state.IsKeyDown(Keys.J))
            { replay.inputs.Add(new ReplayInputs(5, (int)curtime, 1)); }
            if (!(previousState.IsKeyDown(Keys.K)) && state.IsKeyDown(Keys.K))
            { replay.inputs.Add(new ReplayInputs(7, (int)curtime, 1));  }
            if (!(previousState.IsKeyUp(Keys.D)) && state.IsKeyUp(Keys.D))
            { replay.inputs.Add(new ReplayInputs(1, (int)curtime, 2));  }
            if (!(previousState.IsKeyUp(Keys.F)) && state.IsKeyUp(Keys.F))
            { replay.inputs.Add(new ReplayInputs(3, (int)curtime, 2));  }
            if (!(previousState.IsKeyUp(Keys.J)) && state.IsKeyUp(Keys.J))
            { replay.inputs.Add(new ReplayInputs(5, (int)curtime, 2));  }
            if (!(previousState.IsKeyUp(Keys.K)) && state.IsKeyUp(Keys.K))
            { replay.inputs.Add(new ReplayInputs(7, (int)curtime, 2));  }
            **/

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
            //ButtonState.Pressed)
            //    exit++;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                ManagedBass.Bass.Free();
                exit = 5;
                Exit();
                
            }

            if (state.IsKeyDown(Keys.D))
                zz = true;
            else zz = false;

            if (state.IsKeyDown(Keys.F))
                xx = true;
            else xx = false;

            if (state.IsKeyDown(Keys.J))
                cc = true;
            else cc = false;

            if (state.IsKeyDown(Keys.K))
                vv = true;
            else vv = false;

            TouchCollection itouch = TouchPanel.GetState();
            if (landscape)
            {
                foreach (TouchLocation tl in itouch)
                {
                    if (tl.State == TouchLocationState.Moved)
                    {
                        int calcs = this.GraphicsDevice.Viewport.Width / 6;
                        if (tl.Position.X <= calcs * 2) { zz = true; }
                        else
                        if (tl.Position.X <= calcs * 3) { xx = true; }
                        else
                        if (tl.Position.X <= calcs * 4) { cc = true; }
                        else
                        if (tl.Position.X <= calcs * 6) { vv = true; }
                    }
                }
            }
            else
            {
                foreach (TouchLocation tl in itouch)
                {
                    if (tl.State == TouchLocationState.Moved)
                    {
                        int calcs = this.GraphicsDevice.Viewport.Width / 4;
                        if (tl.Position.X <= calcs * 1) { zz = true; }
                        else
                        if (tl.Position.X <= calcs * 2) { xx = true; }
                        else
                        if (tl.Position.X <= calcs * 3) { cc = true; }
                        else
                        if (tl.Position.X <= calcs * 4) { vv = true; }
                    }
                }
            }

            if (!(z) && (zz))
            {
                replay.inputs.Add(new ReplayInputs(1, (int)curtime, 1));
            }

            if (!(x) && (xx))
            {
                replay.inputs.Add(new ReplayInputs(3, (int)curtime, 1));
            }

            if (!(c) && (cc))
            {
                replay.inputs.Add(new ReplayInputs(5, (int)curtime, 1));
            }

            if (!(v) && (vv))
            {
                replay.inputs.Add(new ReplayInputs(7, (int)curtime, 1));
            }

            if ((z) && !(zz))
            {
                replay.inputs.Add(new ReplayInputs(1, (int)curtime, 2));
            }

            if ((x) && !(xx))
            {
                replay.inputs.Add(new ReplayInputs(3, (int)curtime, 2));
            }

            if ((c) && !(cc))
            {
                replay.inputs.Add(new ReplayInputs(5, (int)curtime, 2));
            }

            if ((v) && !(vv))
            {
                replay.inputs.Add(new ReplayInputs(7, (int)curtime, 2));
            }

            if (zz)
            { z4 = 1.0f; }
            if (xx)
            { z3 = 1.0f; }
            if (cc)
            { z2 = 1.0f; }
            if (vv)
            { z1 = 1.0f; }

            if (state.IsKeyDown(Keys.U))
            { testz = testz + 0.05f; cameraPosition.Z = testz; }

            if (state.IsKeyDown(Keys.I))
            { testz = testz - 0.05f; cameraPosition.Z = testz; }

            if (state.IsKeyDown(Keys.O))
            { testy = testy + 0.05f; cameraPosition.Y = testy; }

            if (state.IsKeyDown(Keys.P))
            { testy = testy - 0.05f; cameraPosition.Y = testy; }

            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 16;
            
            if (z1 >= 0.5f)
                z1 = z1 - (0.025f * delta);
            if (z2 >= 0.5f)
                z2 = z2 - (0.025f * delta);
            if (z3 >= 0.5f)
                z3 = z3 - (0.025f * delta);
            if (z4 >= 0.5f)
                z4 = z4 - (0.025f * delta);
            previousState = state;
            
            if (ManagedBass.Bass.ChannelGetPosition(i, ManagedBass.PositionFlags.Bytes) == ManagedBass.Bass.ChannelGetLength(i, ManagedBass.PositionFlags.Bytes))
            {
                msecfinished = msecfinished + gameTime.ElapsedGameTime.Milliseconds;
                if (replay.saved == false)
                {
                    replay.saved = true;
                    if (legacy == false)
                        replay.SaveToJson();
                    else
                    {
                        replay.SaveToJson();
                        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                        string filename = "scores";

                        beatenhighscore = false;

                        string dbPath = Path.Combine(path, filename);
                        Ini ini = new Ini(dbPath);
                        string bmid = Map.bmid.ToString();
                        float score = replay.score;
                        string maxmm = replay.maxcombo.ToString();
                           //replay.CalculateMaxCombo(Map, ManagedBass.Bass.ChannelGetLength(i)).ToString();
                        string acc = replay.CalculateAccuracyAndTotals(Map, ManagedBass.Bass.ChannelGetLength(i));

                        if (int.Parse(ini.GetValue(bmid, "score", "0")) < score)
                        {
                            ini.WriteValue(bmid, "score", score.ToString());
                            ini.WriteValue(bmid, "maxmm", maxmm.ToString());
                            ini.WriteValue(bmid, "acc", acc.ToString());
                            ini.WriteValue(bmid, "totalhit", replay.totalhit.ToString());
                            ini.WriteValue(bmid, "totalmiss", replay.totalmiss.ToString());
                            beatenhighscore = true;
                        }

                        ini.Save();
                    }
                } else
                {
                    if (zz || xx || cc || vv)
                    {
                        exit = 5;
                    }
                }
            }

            replay.CalculateScore(Map, curtime);

            z = zz;
            x = xx;
            c = cc;
            v = vv;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            var fps = string.Format("{0}", System.Math.Round(_frameCounter.AverageFramesPerSecond));

            GraphicsDevice.SetRenderTarget(_backBuffer);
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            mania2d.Render(Map, spriteBatch, _backBuffer, curtime, z1, z2, z3, z4, landscape);
            spriteBatch.End();

            //this.Window.Title = replay.score.ToString() + "|" + replay.combo.ToString() + "|" + testy.ToString() + "|" + testz.ToString();

            GraphicsDevice.SetRenderTarget(_backBuffer2);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            mania2dingame.Render(spriteBatch, _backBuffer2, curtime, replay, landscape);
            if (!landscape) introOutro.Render(msecdelay, msecfinished, beatenhighscore, spriteBatch, GraphicsDevice, replay);
            spriteBatch.End();

            if (landscape)
            {
                GraphicsDevice.SetRenderTarget(_backBuffer3);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin();
                introOutro.Render(msecdelay, msecfinished, beatenhighscore, spriteBatch, GraphicsDevice, replay);
                spriteBatch.End();
            }

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();
            if (GraphicsDevice.PresentationParameters.Bounds.Width > GraphicsDevice.PresentationParameters.Bounds.Height)
            {
                ground.Render(GraphicsDevice, graphics, _backBuffer, effect, cameraPosition, cameraLookAtVector, cameraUpVector);
            }
            else spriteBatch.Draw(texture: _backBuffer, destinationRectangle: new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), color: Color.White, effects: s);

            spriteBatch.Draw(_backBuffer2, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            if (landscape)
            {
                int width = (int)(GraphicsDevice.Viewport.Height / 16 * 9);
                int widthx = (GraphicsDevice.Viewport.Width / 2) - (width /2);
                spriteBatch.Draw(_backBuffer3, new Rectangle(widthx, 0, width, GraphicsDevice.Viewport.Height), Color.White);
            }
            spriteBatch.DrawString(Content.Load<SpriteFont>("File"), fps, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
