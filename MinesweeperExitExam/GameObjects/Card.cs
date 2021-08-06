using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MinesweeperExitExam.GameObjects
{
    class Card : GameObject
    {
        public bool isShow;
        private bool opened;
        private int id;
        private int column;
        private int row;
        public MouseState PreviousMouse, CurrentMouse;

        public enum MarksStates
        {
            MARKED,
            UNMARKED
        }
        public MarksStates _currentmarksState;

        public enum TypeBird
        {
            EMPTY,
            BIRD
        }
        public TypeBird _typeBird;

        public enum NumberOnCard
        {
            ZERO,
            ONE,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX,
            SEVEN,
            EIGHT,
            NONNUMBER
        }
        public NumberOnCard _numberOnCard;


        public Card(Texture2D texture, bool typebird, int id) : base(texture) {
            setOpened(false);
            this.id = id;
            this.row = id / 100;
            this.column = id % 100;

           
            if(typebird){
                isShow = true;
                _typeBird = TypeBird.BIRD;
                _numberOnCard = NumberOnCard.NONNUMBER;
               // Viewport = new Rectangle(480, 0, Singleton.HITBOX, Singleton.HITBOX);
            } else {
                isShow = false;
                _typeBird = TypeBird.EMPTY;
                //Viewport = new Rectangle(0, 0, Singleton.HITBOX, Singleton.HITBOX);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,
                            Position,
                            Viewport,
                            Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            _currentmarksState = MarksStates.UNMARKED;


            Viewport = new Rectangle(528, 0, Singleton.HITBOX, Singleton.HITBOX);
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            CurrentMouse = Mouse.GetState();
            var mousePosition = new Point(CurrentMouse.X, CurrentMouse.Y);
            var rectangle = new Rectangle(mousePosition.X, mousePosition.Y, this._texture.Width, this._texture.Height);

            if (rectangle.Contains(mousePosition) && inArea())
            {
                isHovered = true;
            } else
            {
                isHovered = false;
            }

            

            if (!opened)
            {
                
                if (PreviousMouse.RightButton == ButtonState.Released && CurrentMouse.RightButton == ButtonState.Pressed && inArea())
                {
                    SoundEffects["Click"].Volume = Singleton.Instance.MasterSFXVolume;
                    SoundEffects["Click"].Play();
                    switch (_currentmarksState)
                    {
                        case MarksStates.MARKED:
                            Reset();
                            Singleton.Instance.Marks++;
                            if (_typeBird.Equals(TypeBird.BIRD))
                            {
                                Singleton.Instance.markCorrect--;
                            }
                            break;
                        case MarksStates.UNMARKED:
                            
                            Viewport = new Rectangle(432, 0, Singleton.HITBOX, Singleton.HITBOX);
                            _currentmarksState = MarksStates.MARKED;
                            Singleton.Instance.Marks--;
                            if (_typeBird.Equals(TypeBird.BIRD))
                            {
                                Singleton.Instance.markCorrect++;
                            }
                            break;
                    }

                    if(Singleton.Instance.markCorrect == 99 && Singleton.Instance.Marks == 0)
                    {
                        Singleton.Instance._currentGameResult = Singleton.GameResult.Win;
                        Singleton.Instance._currentGameState = Singleton.GameState.GameEnded;
                    }
                }

                if (PreviousMouse.LeftButton == ButtonState.Released && CurrentMouse.LeftButton == ButtonState.Pressed && inArea() 
                    && _currentmarksState.Equals(MarksStates.UNMARKED))
                {

                    SoundEffects["Click"].Volume = Singleton.Instance.MasterSFXVolume;
                    SoundEffects["Click"].Play();
                    // React to the click

                    if (_typeBird.Equals(TypeBird.BIRD))
                    {
                        SoundEffects["Dead"].Volume = Singleton.Instance.MasterSFXDEADVolume;
                        SoundEffects["Dead"].Play();
                        showBird();
                        Singleton.Instance._currentGameResult = Singleton.GameResult.Lose;
                        Singleton.Instance._currentGameState = Singleton.GameState.GameEnded;
                    } else
                    {
                        if ( !_numberOnCard.Equals(NumberOnCard.ZERO))
                        {
                            switchPicture();
                        } else
                        {
                            checkNumber();
                            switchPicture();
                            //checkNumber();
                        }


                    }
                }
                
            }
   

            PreviousMouse = CurrentMouse;

            base.Update(gameTime, gameObjects);
        }

        private void switchPicture()
        {

            if (!opened)
            {
                Singleton.Instance.blankCorrect++;
                switch (_numberOnCard)
                {
                    case NumberOnCard.ZERO:
                        Viewport = new Rectangle(0, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                    case NumberOnCard.ONE:
                        Viewport = new Rectangle(48, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                    case NumberOnCard.TWO:
                        Viewport = new Rectangle(96, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                    case NumberOnCard.THREE:
                        Viewport = new Rectangle(144, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                    case NumberOnCard.FOUR:
                        Viewport = new Rectangle(192, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                    case NumberOnCard.FIVE:
                        Viewport = new Rectangle(240, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                    case NumberOnCard.SIX:
                        Viewport = new Rectangle(288, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                    case NumberOnCard.SEVEN:
                        Viewport = new Rectangle(336, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                    case NumberOnCard.EIGHT:
                        Viewport = new Rectangle(384, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                    case NumberOnCard.NONNUMBER:
                        Viewport = new Rectangle(480, 0, Singleton.HITBOX, Singleton.HITBOX);
                        opened = !(opened);
                        break;
                }
            }
               
        }

        public void setOpened(bool value)
        {
            opened = value;
        }

        public bool getOpened()
        {
            return opened;
        }

        private bool inArea()
        {
            CurrentMouse = Mouse.GetState();
            var mousePosition = new Point(CurrentMouse.X, CurrentMouse.Y);
            if ((mousePosition.X > this.Position.X && mousePosition.X < this.Position.X + Singleton.HITBOX)
                && (mousePosition.Y > this.Position.Y && mousePosition.Y < + this.Position.Y + Singleton.HITBOX))
            {
                return true;
            }
            return false;
        }

        public void checkNeighbors()
        {
            if (this._typeBird.Equals(TypeBird.BIRD))
            {
                _numberOnCard = NumberOnCard.NONNUMBER;
                return;
            }
            int count = 0;

            int neighbor1 = ((this.row - 1) * 100) + (column - 1);
            int neighbor2 = ((this.row - 1) * 100) + column;
            int neighbor3 = ((this.row - 1) * 100) + (column + 1);
            int neighbor4 = (this.row * 100) + (column - 1);
            int neighbor5 = (this.row * 100) + (column + 1);
            int neighbor6 = ((this.row + 1) * 100) + (column - 1);
            int neighbor7 = ((this.row + 1) * 100) + column;
            int neighbor8 = ((this.row + 1) * 100) + (column + 1);

            
            count += calCount(neighbor1);
            count += calCount(neighbor2);
            count += calCount(neighbor3);
            count += calCount(neighbor4);
            count += calCount(neighbor5);
            count += calCount(neighbor6);
            count += calCount(neighbor7);
            count += calCount(neighbor8);


            //assign numberOnCard
            switch (count)
            {
                case 0:
                    _numberOnCard = NumberOnCard.ZERO;
                    break;
                case 1:
                    _numberOnCard = NumberOnCard.ONE;
                    break;
                case 2:
                    _numberOnCard = NumberOnCard.TWO;
                    break;
                case 3:
                    _numberOnCard = NumberOnCard.THREE;
                    break;
                case 4:
                    _numberOnCard = NumberOnCard.FOUR;
                    break;
                case 5:
                    _numberOnCard = NumberOnCard.FIVE;
                    break;
                case 6:
                    _numberOnCard = NumberOnCard.SIX;
                    break;
                case 7:
                    _numberOnCard = NumberOnCard.SEVEN;
                    break;
                case 8:
                    _numberOnCard = NumberOnCard.EIGHT;
                    break;
            }

        }

        private int calCount(int id)
        {
            if (Singleton.Instance.cardDictionary.ContainsKey(id)
                && Singleton.Instance.cardDictionary[id]._typeBird.Equals(TypeBird.BIRD))
            {
                return 1;
            }
            return 0;
        }

        private void checkNumber()
        {

            int neighbor1 = ((this.row - 1) * 100) + (column - 1);
            int neighbor2 = ((this.row - 1) * 100) + column;
            int neighbor3 = ((this.row - 1) * 100) + (column + 1);
            int neighbor4 = (this.row * 100) + (column - 1);
            int neighbor5 = (this.row * 100) + (column + 1);
            int neighbor6 = ((this.row + 1) * 100) + (column - 1);
            int neighbor7 = ((this.row + 1) * 100) + column;
            int neighbor8 = ((this.row + 1) * 100) + (column + 1);


            openCard(neighbor1);
            openCard(neighbor2);
            openCard(neighbor3);
            openCard(neighbor4);
            openCard(neighbor5);
            openCard(neighbor6);
            openCard(neighbor7);
            openCard(neighbor8);


        }

        private void openCard(int id)
        {
            if(Singleton.Instance.cardDictionary.ContainsKey(id) 
                && Singleton.Instance.cardDictionary[id]._typeBird.Equals(TypeBird.EMPTY)
                && !Singleton.Instance.cardDictionary[id].opened)
            {
                Singleton.Instance.cardDictionary[id].switchPicture();
                if (Singleton.Instance.cardDictionary[id]._numberOnCard.Equals(NumberOnCard.ZERO))
                {
                    Singleton.Instance.cardDictionary[id].checkNumber();
                }
            }
        }

        public void showBird()
        {        
            if (_typeBird.Equals(TypeBird.BIRD))
            {
                SoundEffects["Click"].Volume = Singleton.Instance.MasterSFXVolume;
                SoundEffects["Click"].Play();
                Viewport = new Rectangle(480, 0, Singleton.HITBOX, Singleton.HITBOX);
                Singleton.Instance.showBird++;
                isShow = false;
            }

        }
    }
}
