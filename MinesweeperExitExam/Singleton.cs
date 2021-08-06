using Microsoft.Xna.Framework.Input;
using MinesweeperExitExam.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperExitExam
{
    class Singleton
    {
        public const int WIDTH = 1536;
        public const int HEIGHT = 960;

        public const int HITBOX = 48;

        public const int WIDTH_CARD = 720;
        public const int HEIGHT_CARD = 312;

        public const int MAXBIRD = 99;
        public const int TOTAL = 480;

        public const int ROW = 16;
        public const int COLUMN = 30;
        

        
        public int countToWin;
        public int Time;
        public int Marks;
        public Dictionary<int, Card>  cardDictionary;
        public float MasterBGMVolume;
        public float MasterSFXVolume;
        public float MasterSFXDEADVolume;
        public int markCorrect;
        public int blankCorrect;

        public int showBird;

        public enum GameState
        {
            GameMain,
            GameStart,
            GamePlaying,
            GameEnded,
        }
        public GameState _currentGameState;

        public enum GameResult
        {
            Win,
            Lose
        }
        public GameResult _currentGameResult;

        public KeyboardState PreviousKey, CurrentKey;

        private Singleton() { }
        private static Singleton instance;

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
