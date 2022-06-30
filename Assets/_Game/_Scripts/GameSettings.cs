using UnityEngine;
using Woska.Bakalarka;
using Woska.Core;

namespace Woska
{
    public class GameSettings : Singleton<GameSettings>
    {
        public string GameVersion = "1.0.0";
        public int MaximumPlayers = 4;
        public int MinimumPlayers = 1;

        public Color[] PlayerColors;
        public Material[] PlayerMaterials;

        public int RoundsToPlay = 2;
        
        public int MaximumScore = 10;
        public int ScoreForWin = 2;


        public SceneIndexes[] AvalibleMinigames;
        public int NumberOfMiniGames => AvalibleMinigames.Length;


        public SceneIndexes RandomMiniGame => AvalibleMinigames[Random.Range(0,NumberOfMiniGames)];

        public float TimePerMiniGame = 15f;


        public float NormalPressurePlateCooldown = 5f;
    }
}
