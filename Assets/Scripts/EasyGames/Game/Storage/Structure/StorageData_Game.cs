using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGames
{
    [System.Serializable]
    public class StorageData_Game
    {
        public List<GridCell> GridCells;
        public int Score;

        public StorageData_Game(List<GridCell> gridCells, int score)
        {
            GridCells = gridCells;
            Score = score;
        }
    }
}
