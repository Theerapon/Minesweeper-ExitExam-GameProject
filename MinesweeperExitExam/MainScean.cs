using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MinesweeperExitExam.GameObjects;
using System;
using System.Collections.Generic;

namespace MinesweeperExitExam
{
    
    public class MainScean : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D imagesObject;
        Texture2D imagesPlayingBackgroud;
        Texture2D imagesMainBackgroud;
        Random rnd;

        Song _bgm;
        SoundEffect _sfx;

        Texture2D pixel;
        List<GameObject> _gameObjects;
        int _numObject;

        SpriteFont _font;
        SpriteFont _result;

        int timeBetweenShots = 5000; // Thats 1000 milliseconds
        int shotTimer = 0;

        int timeBetweenShotsCountTime = 1000; // Thats 1000 milliseconds
        int shotCountTime = 0;


        public MainScean()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = Singleton.WIDTH;
            graphics.PreferredBackBufferHeight = Singleton.HEIGHT;
            graphics.ApplyChanges();

            
            _gameObjects = new List<GameObject>();
            rnd = new Random();

            
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
            imagesPlayingBackgroud = this.Content.Load<Texture2D>("Images/playingBG");
            imagesMainBackgroud = this.Content.Load<Texture2D>("Images/mainBG");
            _font = Content.Load<SpriteFont>("File");
            _result = Content.Load<SpriteFont>("Result");
            _bgm = Content.Load<Song>("bensound_creepy");
            _sfx = Content.Load<SoundEffect>("bounce");
            
            //test
            Singleton.Instance._currentGameState = Singleton.GameState.GameMain;
            Singleton.Instance.cardDictionary = new Dictionary<int, Card>();
            Singleton.Instance.MasterSFXVolume = 0.1f;
            Singleton.Instance.MasterSFXDEADVolume = 1f;

            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.AliceBlue });


        }

        protected override void UnloadContent()
        {

        }

        
        protected override void Update(GameTime gameTime)
        {
            Singleton.Instance.CurrentKey = Keyboard.GetState();

            _numObject = _gameObjects.Count;

            switch (Singleton.Instance._currentGameState){
                case Singleton.GameState.GameMain:
                    Singleton.Instance.Time = 0;
                    Singleton.Instance.Marks = 99;
                    if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) && Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space))
                    {
                        
                        _sfx.Play(volume: 0.1f, pitch: 0.0f, pan: 0.0f);
                        //Space keys pressed to start
                        Singleton.Instance._currentGameState = Singleton.GameState.GameStart;
                    }
                    break;
                case Singleton.GameState.GameStart:
                    bool check = Reset();
                    if (check)
                    {
                        foreach (KeyValuePair<int, Card> entry in Singleton.Instance.cardDictionary)
                        {
                            
                            Singleton.Instance.cardDictionary[entry.Key].checkNeighbors();
                        }
                            Singleton.Instance._currentGameState = Singleton.GameState.GamePlaying;
                    } else
                    {
                        Singleton.Instance._currentGameState = Singleton.GameState.GameStart;
                    }
                    break;
                case Singleton.GameState.GamePlaying:


                    shotCountTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (shotCountTime > timeBetweenShotsCountTime)
                    {
                        shotCountTime = 0;
                        Singleton.Instance.Time++;
                    }


                    for (int i = 0; i < _numObject; i++)
                    {

                        if (_gameObjects[i].IsActive) _gameObjects[i].Update(gameTime, _gameObjects);


                    }

                    for (int i = 0; i < _numObject; i++)
                    {
                        if (!_gameObjects[i].IsActive)
                        {
                            _gameObjects.RemoveAt(i);
                            i--;
                            _numObject--;
                        }
                    }

                    

                    if(Singleton.Instance.blankCorrect == Singleton.TOTAL - Singleton.MAXBIRD)
                    {
                        Singleton.Instance._currentGameState = Singleton.GameState.GameEnded;
                        Singleton.Instance._currentGameResult = Singleton.GameResult.Win;
                    }

                    
                    break;

                case Singleton.GameState.GameEnded:
                    
                    foreach (KeyValuePair<int, Card> entry in Singleton.Instance.cardDictionary)
                    {
                        shotTimer += gameTime.ElapsedGameTime.Milliseconds;
                        if (shotTimer > timeBetweenShots)
                        {
                            shotTimer = 0;
                            if (Singleton.Instance.cardDictionary[entry.Key].isShow)
                            {
                                Singleton.Instance.cardDictionary[entry.Key].showBird();
                            }
                            
                        }
                    }
                    
                    if(Singleton.Instance.showBird >= 99)
                    {
                        
                        _gameObjects.Clear();
                        if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) && Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space))
                        {
                            Singleton.Instance.showBird = 0;
                            _sfx.Play(volume: 0.1f, pitch: 0.0f, pan: 0.0f);

                            //Space keys pressed to start
                            Singleton.Instance._currentGameState = Singleton.GameState.GameMain;
                        }
                    }

                    
                    break;
            }
            
            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

            base.Update(gameTime);
        }

        Vector2 fontSize;
        
        protected override void Draw(GameTime gameTime)
        {
            float xCenter;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(imagesPlayingBackgroud, new Vector2(0, 0), Color.White);

            for (int i = 0; i < _gameObjects.Count; i++)
            {

                _gameObjects[i].Draw(spriteBatch);

                //handle border for eactobject hovered
                if (_gameObjects[i].isHovered)
                {
                    DrawBorder(_gameObjects[i].Rectangle, 5, new Color(18, 27, 43));
                }

            }

            //TIME
            String time = convertTo3String(Singleton.Instance.Time);
            fontSize = _font.MeasureString(time);
            spriteBatch.DrawString(_font, time, new Vector2(320, 60), Color.White);

            //MARKS
            fontSize = _font.MeasureString(Singleton.Instance.Marks.ToString());
            spriteBatch.DrawString(_font, Singleton.Instance.Marks.ToString(), new Vector2(1180, 60), Color.White);


            //each state
            switch (Singleton.Instance._currentGameState)
            {
                case Singleton.GameState.GameMain:
      
                    fontSize = _font.MeasureString("Press spacebar to start");
                    xCenter = (Singleton.WIDTH / 2) - (fontSize.Length() / 2);
                    spriteBatch.DrawString(_font, "Press spacebar to start", new Vector2(xCenter, Singleton.HEIGHT / 2), Color.White);

                    break;
                case Singleton.GameState.GameStart:
                    break;
                case Singleton.GameState.GamePlaying:
                    break;
                case Singleton.GameState.GameEnded:
                    if (Singleton.Instance.showBird >= 99)
                    {
                        MediaPlayer.Stop();
                        fontSize = _result.MeasureString(Singleton.Instance._currentGameResult.ToString());
                        xCenter = (Singleton.WIDTH / 2) - (fontSize.Length() / 2);
                        spriteBatch.DrawString(_result, Singleton.Instance._currentGameResult.ToString(), new Vector2(xCenter, 320), new Color(215, 189, 226));

                        fontSize = _font.MeasureString("Press spacebar to continue");
                        xCenter = (Singleton.WIDTH / 2) - (fontSize.Length() / 2);
                        spriteBatch.DrawString(_font, "Press spacebar to continue", new Vector2(xCenter, Singleton.HEIGHT / 2), Color.White);
                    }
                    break;
            }

            
            spriteBatch.End();
            graphics.BeginDraw();
            base.Draw(gameTime);
        }

        protected bool Reset()
        {
            Singleton.Instance.blankCorrect = 0;
            Singleton.Instance.showBird = 0;
            Singleton.Instance.markCorrect = 0;
            int countCrow = 0;
            double random;
            Singleton.Instance.cardDictionary.Clear();


            Singleton.Instance.MasterBGMVolume = 0.4f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Singleton.Instance.MasterBGMVolume;
            MediaPlayer.Play(_bgm);


            imagesObject = this.Content.Load<Texture2D>("Images/gameObject");
            _gameObjects.Clear();

            //crate form row x to row y
            //row 1 to 4
            for(int row = 1; row <= Singleton.ROW; row++){
                for(int column = 1; column <= Singleton.COLUMN; column++){
                    bool typecrow = false;

                    if (countCrow < Singleton.MAXBIRD)
                    {
                        random = rnd.NextDouble();
                        if (random <= 0.23625) {
                            typecrow = true;
                            countCrow++;
                        } 
                    }                           
                    addCard(row, column, typecrow);   
                }
            }


            //check count crow
            if (countCrow != Singleton.MAXBIRD) {
                Console.WriteLine("Fail " + (countCrow));
                Singleton.Instance._currentGameState = Singleton.GameState.GameStart;
                return false;
            } else
            {
                Console.WriteLine("Pass " + (countCrow));

                foreach (GameObject s in _gameObjects)
                {
                    s.Reset();
                }
                return true;
            }

            
        }

        private void addCard(int row, int column, bool typecrow) 
        {
            int x = Singleton.HITBOX * column;
            int y = (Singleton.HITBOX * (row + 2));
            int id = (row * 100) + column;

            Card card = new Card(imagesObject, typecrow, id)
            {
                Position = new Vector2(x, y),
                SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"Click", Content.Load<SoundEffect>("bounce").CreateInstance() },
                                {"Dead", Content.Load<SoundEffect>("laugh").CreateInstance() }

                            }
            };
            _gameObjects.Add(card);
            Singleton.Instance.cardDictionary.Add(id, card);
        }

        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                    rectangleToDraw.Y,
                                    thicknessOfBorder,
                                    rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                    rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                    rectangleToDraw.Width,
                                    thicknessOfBorder), borderColor);
        }

        private String convertTo3String(double number)
        {
            string str = "";
            double checkNumber = number / 100;
            if(checkNumber >= 1)
            {
                str = number.ToString();
            } else if(checkNumber >= 0.1)
            {
                str = "0" + number.ToString();
            } else
            {
                str = "00" + number.ToString();
            }

            return str;
        }

       
    }
}
