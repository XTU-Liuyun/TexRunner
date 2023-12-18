using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TexRunner.Entities;
using TexRunner.Extensions;
using TexRunner.Graphics;
using TexRunner.System;

namespace TexRunner
{
    public class TexGunnerGame : Game
    {
        public enum DisplayMode
        {
            Default,
            Zoomed
        }
        private const string GAME_TITLE = "T-Rex Runner";
        private const string ASSET_NAME_SPRITESHEET = "TrexSpritesheet";
        private const string ASSET_NAME_SFX_HIT = "hit"; 
        private const string ASSET_NAME_SFX_SCORE_REACHED = "score-reached";
        private const string ASSET_NAME_SFX_BUTTON_PRESS = "button-press";
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SoundEffect _sfxHit;
        private SoundEffect _sfxButtonPress;
        private SoundEffect _sfxScoreReached;
        private Texture2D _spriteSheetTexture;
        private Texture2D _fadeInTexture;
        private Texture2D _invertSpriteSheet;
        
        private float _fadeInTexturePosX;
        private Trex _trex;
        private InputController _inputController;
        private ScoreBoard _scoreBoard; 
        public const int WINDOW_HEIGHT = 150;
        public const int WINDOW_WIDTH = 600;
        public const int TREX_START_POS_X = 1;
        public const int TREX_START_POS_Y= WINDOW_HEIGHT-16;
        private const int SCORE_BOARD_POS_X = WINDOW_WIDTH-130;
        private const int SCORE_BOARD_POS_Y = 10;
        private const string SAVE_FILE_NAME = "Save.dat";
        public const int DISPLAY_ZOOM_FACTOR = 2;
        private GroundManager _groundManager;
        private EntityManager _entityManager;
        private SkyManager _skyManager;
        private ObstacleManager _obstacleManager;
        private GameOverScreen _gameOverScreen;
        private KeyboardState _previousKeyBoardState;
        private DateTime _highscoreDate;
        private Matrix _transformMatrix=Matrix.Identity;
        public GameState State { get; private set; }
        public DisplayMode WindeoDisplayMode { get; private set; } = DisplayMode.Default;
        public float ZoomFactor => WindeoDisplayMode == DisplayMode.Default ? 1 : DISPLAY_ZOOM_FACTOR;
        public TexGunnerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _entityManager = new EntityManager();
            State = GameState.Initial;
            _fadeInTexturePosX = Trex.TREX_DEFAULT_SPRITE_WIDTH;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            Window.Title = GAME_TITLE;
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            _graphics.SynchronizeWithVerticalRetrace = true;    
            _graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _sfxButtonPress = Content.Load<SoundEffect>(ASSET_NAME_SFX_BUTTON_PRESS);
            _sfxScoreReached = Content.Load<SoundEffect>(ASSET_NAME_SFX_SCORE_REACHED);
            _sfxHit = Content.Load<SoundEffect>(ASSET_NAME_SFX_HIT);
            
            _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);
            _invertSpriteSheet = _spriteSheetTexture.InvertColors(Color.Transparent);
            
            _fadeInTexture = new Texture2D(GraphicsDevice, 1, 1);
            _fadeInTexture.SetData(new Color[] { Color.White}) ;
            _trex = new Trex(_spriteSheetTexture, new Vector2(TREX_START_POS_X, TREX_START_POS_Y-Trex.TREX_DEFAULT_SPRITE_HEIGHT),_sfxButtonPress);
            _trex.DrawOrder = 10;
            _trex.JumpComplete += trex_JumpComplete;
            _trex.Died += trex_Died;
            _scoreBoard = new ScoreBoard(_spriteSheetTexture,new Vector2(SCORE_BOARD_POS_X, SCORE_BOARD_POS_Y),_trex,_sfxScoreReached);
            //_scoreBoard.Score = 498;
            //_scoreBoard.HighScore = 12345;
            _inputController = new InputController(_trex);
            _groundManager = new GroundManager(_spriteSheetTexture, _entityManager,_trex);
            _obstacleManager=new ObstacleManager(_entityManager,_trex,_scoreBoard,_spriteSheetTexture);
            _skyManager = new SkyManager(_trex, _spriteSheetTexture,_invertSpriteSheet, _entityManager, _scoreBoard);
            _gameOverScreen = new GameOverScreen(_spriteSheetTexture,this);
            _gameOverScreen.Position = new Vector2(WINDOW_WIDTH / 2 - GameOverScreen.GAME_OVER_SPRITE_WIDTH / 2, WINDOW_HEIGHT / 2 - 30);
            
            _entityManager.AddEntity(_trex);
            _entityManager.AddEntity(_groundManager);
            _entityManager.AddEntity(_scoreBoard);
            _entityManager.AddEntity( _obstacleManager);
            _entityManager.AddEntity(_gameOverScreen);
            _entityManager.AddEntity(_skyManager);
            _groundManager.Initialize();

            LoadSaveState();
        }

        private void trex_Died(object sender, EventArgs e)
        {
            
            State = GameState.GameOver;
            _sfxHit.Play(); 
            _obstacleManager.IsEnabled= false;
            _gameOverScreen.IsEnabled = true;
            if (_scoreBoard.DisplayScore>_scoreBoard.HighScore)
            {
                Console.WriteLine("New highscore set:" + _scoreBoard.DisplayScore);
                _scoreBoard.HighScore = _scoreBoard.DisplayScore;   
                _highscoreDate=DateTime.Now;
                SaveGame();
            }
        }

        private void trex_JumpComplete(object sender, EventArgs e)
        {
            if(State==GameState.Transition)
            {
                State = GameState.Playing;
                _trex.Initialize();
                _obstacleManager.IsEnabled = true;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // TODO: Add your update logic here

            base.Update(gameTime);
            KeyboardState keyboardState = Keyboard.GetState();
            _entityManager.Update(gameTime);
            if(keyboardState.IsKeyDown(Keys.F8)&&!_previousKeyBoardState.IsKeyDown(Keys.F8))
            {
                ResetSaveState();   
            }
            if (keyboardState.IsKeyDown(Keys.F12) && !_previousKeyBoardState.IsKeyDown(Keys.F12))
            {
                ToggleDisplayMode();    
            }
            if (State == GameState.Playing)
            {
                _inputController.ProcessControls(gameTime);
            }
            else if(State==GameState.Transition)
            {
                _fadeInTexturePosX += (float)gameTime.ElapsedGameTime.TotalSeconds * 500;
            }
            else if(State==GameState.Initial)
            {
                bool isStartKeyPressed=keyboardState.IsKeyDown(Keys.Up)||keyboardState.IsKeyDown(Keys.Space);
                bool wasStartKeyPressed=_previousKeyBoardState.IsKeyDown(Keys.Up)|| _previousKeyBoardState.IsKeyDown(Keys.Space);
                if(isStartKeyPressed&&!wasStartKeyPressed) 
                {
                    StartGame();
                }
            }
            _previousKeyBoardState = keyboardState;
        }

        protected override void Draw(GameTime gameTime)
        {
            if(_skyManager == null)
            {
                GraphicsDevice.Clear(Color.White);
            }
            else
            {
                GraphicsDevice.Clear(_skyManager.ClearColor);
            }
            //Console.WriteLine(_skyManager.ClearColor);
            _spriteBatch.Begin(samplerState:SamplerState.PointClamp,transformMatrix:_transformMatrix);
            
            _entityManager.Draw(gameTime, _spriteBatch);
            if(State==GameState.Initial||State==GameState.Transition)
            {
                _spriteBatch.Draw(_fadeInTexture, new Rectangle((int)Math.Round(_fadeInTexturePosX), 0, WINDOW_WIDTH, WINDOW_HEIGHT),Color.White);
            }
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            
        }
        private bool StartGame()
        {
            if(State!=GameState.Initial)
            {
                return false;
            }
            _scoreBoard.Score = 0;
            State = GameState.Transition;
            _trex.BeginJump();
            return true;
        }
        public bool Replay()
        {
            
            if(State != GameState.GameOver) {return false;}
            State = GameState.Playing;
            _trex.Initialize();
            _obstacleManager.Reset();
            _obstacleManager.IsEnabled = true;
            _gameOverScreen.IsEnabled = false;
            _scoreBoard.Score = 0;
            _groundManager.Initialize();
            _inputController.BlockInputTemporarily();
            return true;
        }
        public void SaveGame()
        {
            SaveState saveState = new SaveState()
            {
                Highscore = _scoreBoard.HighScore,
                HighscoreDate = _highscoreDate
            };
            try
            {
                using (FileStream fileStream = new FileStream(SAVE_FILE_NAME, FileMode.Create))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();    
                    binaryFormatter.Serialize(fileStream, saveState);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occurred while saving the game:" + ex.Message);
            }
        }
        public void LoadSaveState()
        {
            try
            {
                using (FileStream fileStream = new FileStream(SAVE_FILE_NAME, FileMode.OpenOrCreate))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    SaveState saveState=binaryFormatter.Deserialize(fileStream) as SaveState;
                    if (saveState != null)
                    {
                        if (_scoreBoard != null)
                        {
                            _scoreBoard.HighScore = saveState.Highscore;
                        }
                        _highscoreDate=saveState.HighscoreDate;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading the game:" + ex.Message);
            }
        }
        private void ResetSaveState()
        {
            _scoreBoard.HighScore = 0;
            _highscoreDate=default(DateTime);
            SaveGame();
        }
        private void ToggleDisplayMode()
        {
            if (WindeoDisplayMode==DisplayMode.Default)
            {
                WindeoDisplayMode= DisplayMode.Zoomed;
                _graphics.PreferredBackBufferWidth = WINDOW_WIDTH*DISPLAY_ZOOM_FACTOR;
                _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT*DISPLAY_ZOOM_FACTOR;
                _transformMatrix = Matrix.Identity * Matrix.CreateScale(DISPLAY_ZOOM_FACTOR, DISPLAY_ZOOM_FACTOR, 0);
            }
            else
            {
                WindeoDisplayMode = DisplayMode.Default;
                _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
                _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
                _transformMatrix = Matrix.Identity;
            }
            _graphics.ApplyChanges();
        }
    }
}
