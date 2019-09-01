using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Persistance
{
    public static class SessionRepository
    {
        private const string ScoreKey = "Score";

        public static List<int> PrizeList { get; set; }

        public static void SaveScore(int score)
        {
            PlayerPrefs.SetInt(ScoreKey, score);
        }

        public static int LoadScore()
        {
            return PlayerPrefs.GetInt(ScoreKey);
        }
    }
}
