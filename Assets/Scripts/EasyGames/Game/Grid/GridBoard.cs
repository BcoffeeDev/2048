using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace EasyGames
{
    public class GridBoard : MonoBehaviour
    {
        public bool isActive;
        
        [Title("Grid Setting")]
        public int size;
        public GameObject gridBuffer;
        public Transform gridParent;
        public GridLayoutGroup gridBufferGridLayout;
        
        [Title("Cell Setting")]
        public CellBuffer cellBuffer;
        public Transform cellParent;
        public GridLayoutGroup cellBufferGridLayout;
        
        [Title("Tween Setting")]
        public float moveDuration;
        public Ease moveEase;
        
        private List<GridCell> _gridCells = new();
        private List<GameObject> _gridBuffers = new();
        private List<CellBuffer> _cellBuffers = new();

        #region Action

        public static Action OnGridCreated;
        public static Action<int> OnMergeGrid;
        public static Action OnGridIsMove;
        public static Action OnGridCannotMove;

        #endregion
        
        public List<GridCell> GridCells
        {
            get => _gridCells;
            set
            {
                _gridCells = value;
                DrawGrid();
            }
        }

        public void Clear()
        {
            _gridCells.Clear();

            foreach (var buffer in _gridBuffers)
            {
                Destroy(buffer);
            }
            _gridBuffers.Clear();
            
            foreach (var buffer in _cellBuffers)
            {
                Destroy(buffer.gameObject);
            }
            _cellBuffers.Clear();
        }

        public void CreateGrid()
        {
            gridBufferGridLayout.constraintCount = size;
            cellBufferGridLayout.constraintCount = size;
            
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var cell = new GridCell(x, y);
                    _gridCells.Add(cell);
                }
            }

            var checkedIndex = new List<int>();
            for (int i = 0; i < 2; i++)
            {
                int index;
                do
                {
                    index = Random.Range(0, _gridCells.Count);
                } while (checkedIndex.Contains(index));

                _gridCells[index].Value = GetRandomValue();
                checkedIndex.Add(index);
            }
            
            DrawGrid();
        }

        private void GenerateNextValue()
        {
            var hasEmptyGrid = false;
            for (int i = 0; i < _gridCells.Count; i++)
            {
                var cell = _gridCells[i];
                if (cell.Value != 0)
                    continue;
                hasEmptyGrid = true;
                break;
            }

            if (!hasEmptyGrid)
                return;
            
            var isGenerateNextValue = false;
            while (!isGenerateNextValue)
            {
                var cell = _gridCells.GetRandom();
                if (cell.Value != 0)
                    continue;
                cell.Value = GetRandomValue();
                cell.isGenerateCell = true;
                isGenerateNextValue = true;
            }
        }

        private int GetRandomValue()
        {
            return Random.value < .6 ? 2 : 4;
        }

        private void DrawGrid()
        {
            #region Create grid

            if (_gridBuffers.Count <= 0  || _cellBuffers.Count <= 0)
            {
                // Grid Buffer
                for (int i = 0; i < _gridCells.Count; i++)
                {
                    var gBuffer = Instantiate(gridBuffer, gridParent);
                    _gridBuffers.Add(gBuffer);
                }
                
                // Cell Buffer
                for (int i = 0; i < _gridCells.Count; i++)
                {
                    var cBuffer = Instantiate(cellBuffer, cellParent);
                    _cellBuffers.Add(cBuffer);
                }

                OnGridCreated?.Invoke();
            }

            #endregion
            
            // Value
            for (int i = 0; i < _gridCells.Count; i++)
            {
                var grid = _gridCells[i];
                var cBuffer = _cellBuffers[i];
                
                cBuffer.DOKill(true);
                cBuffer.SetNumber(grid.Value);
                
                if (!grid.isGenerateCell) continue;
                
                cBuffer.transform.DOScale(Vector3.zero, 0);
                cBuffer.transform.DOScale(Vector3.one, 0.1f).SetDelay(0.08f);
                grid.isGenerateCell = false;
            }
        }

        private GridCell GetGrid(int x, int y)
        {
            for (int i = 0; i < _gridCells.Count; i++)
            {
                var cell = _gridCells[i];
                if (cell.X != x)
                    continue;
                if (cell.Y != y)
                    continue;
                return cell;
            }

            return null;
        }

        private GameObject GetGridBuffer(int x, int y)
        {
            var index = -1;
            for (int i = 0; i < _gridCells.Count; i++)
            {
                var cell = _gridCells[i];
                if (cell.X != x)
                    continue;
                if (cell.Y != y)
                    continue;
                index = i;
            }

            if (index == -1)
                return null;
            return _gridBuffers[index];
        }

        private bool CheckIsCanMove()
        {
            var isCanMove = false;
            for (int i = 0; i < _gridCells.Count; i++)
            {
                var grid = _gridCells[i];

                var up = GetGrid(grid.X, grid.Y + 1);
                if (up is not null)
                {
                    isCanMove = up.IsEmpty || up.CheckSameValue(grid);
                    if (isCanMove)
                        break;
                }

                var down = GetGrid(grid.X, grid.Y - 1);
                if (down is not null)
                {
                    isCanMove = down.IsEmpty || down.CheckSameValue(grid);
                    if (isCanMove)
                        break;
                }

                var left = GetGrid(grid.X - 1, grid.Y);
                if (left is not null)
                {
                    isCanMove = left.IsEmpty || left.CheckSameValue(grid);
                    if (isCanMove)
                        break;
                }

                var right = GetGrid(grid.X, grid.Y - 1);
                if (right is not null)
                {
                    isCanMove = right.IsEmpty || right.CheckSameValue(grid);
                    if (isCanMove)
                        break;
                }
            }

            return isCanMove;
        }
        
        private void MergeGrid(GridCell grid, GridCell nextGrid)
        {
            var flag = grid.Value == nextGrid.Value;
        
            grid.Value += nextGrid.Value;
            nextGrid.Value = 0;

            nextGrid.moveX = grid.X;
            nextGrid.moveY = grid.Y;
        
            if (flag)
            {
                OnMergeGrid?.Invoke(grid.Value);
            }
        }

        private async UniTask TweenGrid()
        {
            cellBufferGridLayout.enabled = false;
            
            for (int i = 0; i < _gridCells.Count; i++)
            {
                var grid = _gridCells[i];
                var cBuffer = _cellBuffers[i];
                
                if (grid.moveX == -1 && grid.moveY == -1)
                    continue;
                
                var gBuffer = GetGridBuffer(grid.moveX, grid.moveY);
                if (gBuffer is null)
                    continue;

                var cBufferTransform = cBuffer.transform;
                cBufferTransform.DOLocalMove(gBuffer.transform.localPosition, moveDuration).SetEase(moveEase);
                
                grid.moveX = -1;
                grid.moveY = -1;
            }

            var delay = (int)(moveDuration * 1000);
            await UniTask.Delay(delay);

            cellBufferGridLayout.enabled = true;
        }

        #region Movement

        public async void MoveUp()
        {
            if (!isActive)
                return;

            if (!CheckIsCanMove())
            {
                OnGridCannotMove?.Invoke();
                isActive = false;
                return;
            }
            
            var hasMerge = false;
            
            for (int x = 0; x < size; x++)
            {
                for (int y = size - 1; y >= 0; y--)
                {
                    // Get grid
                    var grid = GetGrid(x, y);

                    // Calc through all grids before current grid
                    for (int i = grid.Y - 1; i >= 0; i--)
                    {
                        var next = GetGrid(x, i);
                        
                        if (next == null)
                            break;
                        
                        if (next.Value == 0)
                            continue;
                        
                        if (grid.Value == 0)
                        {
                            MergeGrid(grid, next);
                            hasMerge = true;
                            continue;
                        }

                        if (grid.Value != next.Value)
                            break;

                        MergeGrid(grid, next);
                        hasMerge = true;
                        break;
                    }
                }
            }

            if (!hasMerge)
                return;
            
            await TweenGrid();

            if (CheckIsCanMove())
            {
                GenerateNextValue();
                DrawGrid();
                await UniTask.DelayFrame(1);
                OnGridIsMove?.Invoke();
            }
            else
            {
                OnGridCannotMove?.Invoke();
                isActive = false;
            }
        }

        public async void MoveDown()
        {
            if (!isActive)
                return;

            if (!CheckIsCanMove())
            {
                OnGridCannotMove?.Invoke();
                isActive = false;
                return;
            }

            var hasMerge = false;
            
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    // Get grid
                    var grid = GetGrid(x, y);

                    // Calc through all grids before current grid
                    for (int i = grid.Y + 1; i < size; i++)
                    {
                        var next = GetGrid(x, i);
                        
                        if (next == null)
                            break;
                        
                        if (next.Value == 0)
                            continue;
                        
                        if (grid.Value == 0)
                        {
                            MergeGrid(grid, next);
                            hasMerge = true;
                            continue;
                        }

                        if (grid.Value != next.Value)
                            break;

                        MergeGrid(grid, next);
                        hasMerge = true;
                        break;
                    }
                }
            }

            if (!hasMerge)
                return;
            
            await TweenGrid();

            if (CheckIsCanMove())
            {
                GenerateNextValue();
                DrawGrid();
                await UniTask.DelayFrame(1);
                OnGridIsMove?.Invoke();
            }
            else
            {
                OnGridCannotMove?.Invoke();
                isActive = false;
            }
        }

        public async void MoveLeft()
        {
            if (!isActive)
                return;

            if (!CheckIsCanMove())
            {
                OnGridCannotMove?.Invoke();
                isActive = false;
                return;
            }

            var hasMerge = false;
            
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // Get grid
                    var grid = GetGrid(x, y);

                    // Calc through all grids before current grid
                    for (int i = grid.X + 1; i < size; i++)
                    {
                        var next = GetGrid(i, y);
                        
                        if (next == null)
                            break;
                        
                        if (next.Value == 0)
                            continue;
                        
                        if (grid.Value == 0)
                        {
                            MergeGrid(grid, next);
                            hasMerge = true;
                            continue;
                        }

                        if (grid.Value != next.Value)
                            break;

                        MergeGrid(grid, next);
                        hasMerge = true;
                        break;
                    }
                }
            }

            if (!hasMerge)
                return;
            
            await TweenGrid();
            
            if (CheckIsCanMove())
            {
                GenerateNextValue();
                DrawGrid();
                await UniTask.DelayFrame(1);
                OnGridIsMove?.Invoke();
            }
            else
            {
                OnGridCannotMove?.Invoke();
                isActive = false;
            }
        }

        public async void MoveRight()
        {
            if (!isActive)
                return;

            if (!CheckIsCanMove())
            {
                OnGridCannotMove?.Invoke();
                isActive = false;
                return;
            }

            var hasMerge = false;
            
            for (int y = 0; y < size; y++)
            {
                for (int x = size - 1; x >= 0; x--)
                {
                    // Get grid
                    var grid = GetGrid(x, y);

                    // Calc through all grids before current grid
                    for (int i = grid.X - 1; i >= 0; i--)
                    {
                        var next = GetGrid(i, y);
                        
                        if (next == null)
                            break;
                        
                        if (next.Value == 0)
                            continue;
                        
                        if (grid.Value == 0)
                        {
                            MergeGrid(grid, next);
                            hasMerge = true;
                            continue;
                        }

                        if (grid.Value != next.Value)
                            break;

                        MergeGrid(grid, next);
                        hasMerge = true;
                        break;
                    }
                }
            }

            if (!hasMerge)
                return;
            
            await TweenGrid();

            if (CheckIsCanMove())
            {
                GenerateNextValue();
                DrawGrid();
                await UniTask.DelayFrame(1);
                OnGridIsMove?.Invoke();
            }
            else
            {
                OnGridCannotMove?.Invoke();
                isActive = false;
            }
        }

        #endregion

    }
}
