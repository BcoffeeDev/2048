using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGames
{
    [System.Serializable]
    public class GridCell
    {
        public int X;
        public int Y;
        public int Value;

        public int moveX;
        public int moveY;

        public bool isGenerateCell;
        
        public GridCell(int x, int y)
        {
            X = x;
            Y = y;
            Value = 0;

            moveX = -1;
            moveY = -1;
        }
        
        public bool IsEmpty => Value == 0;

        public bool CheckSameValue(GridCell gridCell)
        {
            return gridCell.Value == Value;
        }
    }
}
