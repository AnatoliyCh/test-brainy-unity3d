using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Interfaces;

namespace Domain.Model.PathFinding
{
    [DisallowMultipleComponent]
    public class PathFinder : MonoBehaviour, IPathFinder, IGenerator
    {
        private const int COST_MOVE_STRAIGHT = 10;
        private const int COST_MOVE_DIAGONAL = 14;

        private IGenerator gridGenerator;
        private PathCell[,] Grid { get; set; }

        private void DrawingGrid(float alphaColor)
        {
            for (int i = 0; i < Grid.GetLength(0); i++)
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    var spriteRenderer = Grid[j, i].GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null) spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alphaColor);
                }
        }
        // ближайшая точка
        private PathCell GetGridCell(Vector2 target)
        {
            var pathCell = Grid[0, 0];
            for (int y = 0; y < Grid.GetLength(0); y++)
                for (int x = 0; x < Grid.GetLength(1); x++)
                    if (!Grid[x, y].IsBlocked && Vector2.Distance(Grid[x, y].Position, target) <= Vector2.Distance(pathCell.Position, target))
                        pathCell = Grid[x, y];
            return pathCell;
        }
        // стоимость расстояния
        private float CalculateDistanceCost(Vector2 a, Vector2 b)
        {
            var xDistance = Mathf.Abs(a.x - b.x);
            var yDistance = Mathf.Abs(a.y - b.y);
            var remaining = Mathf.Abs(xDistance - yDistance);
            return COST_MOVE_DIAGONAL * Mathf.Min(xDistance, yDistance) + COST_MOVE_STRAIGHT + remaining;
        }
        // самая низкая fCost
        private PathCell GetLowestFCost(List<PathCell> pathCells)
        {
            var lowestFCostCell = pathCells[0];
            foreach (var item in pathCells)
                if (item.FCost < lowestFCostCell.FCost)
                    lowestFCostCell = item;
            return lowestFCostCell;
        }
        // итоговый путь
        private List<PathCell> CalculatePath(PathCell endCell)
        {
            List<PathCell> path = new List<PathCell> { endCell };
            var currentCell = endCell;
            while (currentCell.CameFromCell != null)
            {
                path.Add(currentCell.CameFromCell);
                currentCell = currentCell.CameFromCell;
            }
            path.Reverse();
            return path;
        }

        private List<PathCell> GetNeighborList(PathCell currentCell)
        {
            List<PathCell> neighborList = new List<PathCell>();

            if (currentCell.PositionGrid.x - 1 >= 0)
            {
                neighborList.Add(Grid[currentCell.PositionGrid.x - 1, currentCell.PositionGrid.y]); // левый сосед
                if (currentCell.PositionGrid.y - 1 >= 0) neighborList.Add(Grid[currentCell.PositionGrid.x - 1, currentCell.PositionGrid.y - 1]); // левый нижний сосед
                if (currentCell.PositionGrid.y + 1 < Grid.GetLength(0)) neighborList.Add(Grid[currentCell.PositionGrid.x - 1, currentCell.PositionGrid.y + 1]); // левый верхний сосед
            }
            if (currentCell.PositionGrid.x + 1 < Grid.GetLength(1))
            {
                neighborList.Add(Grid[currentCell.PositionGrid.x + 1, currentCell.PositionGrid.y]); // правый сосед
                if (currentCell.PositionGrid.y - 1 >= 0) neighborList.Add(Grid[currentCell.PositionGrid.x + 1, currentCell.PositionGrid.y - 1]); // правый нижний сосед
                if (currentCell.PositionGrid.y + 1 < Grid.GetLength(0)) neighborList.Add(Grid[currentCell.PositionGrid.x + 1, currentCell.PositionGrid.y + 1]); // правый верхний сосед
            }
            if (currentCell.PositionGrid.y - 1 >= 0) neighborList.Add(Grid[currentCell.PositionGrid.x, currentCell.PositionGrid.y - 1]); // нижний сосед
            if (currentCell.PositionGrid.y + 1 < Grid.GetLength(1)) neighborList.Add(Grid[currentCell.PositionGrid.x, currentCell.PositionGrid.y + 1]); // верхний сосед
            return neighborList;
        }

        public void DebugGrid(bool debug)
        {
            if (debug)
            {
                var rows = new List<string>();
                var summStr = "";
                for (int i = 0; i < Grid.GetLength(0); i++)
                {
                    rows.Add("");
                    for (int j = 0; j < Grid.GetLength(1); j++)
                        rows[rows.Count - 1] += Grid[j, i].IsBlocked ? "X " : "O ";
                }
                rows.Reverse();
                foreach (var str in rows)
                    summStr += str + "\n";
                Debug.Log(summStr);
                DrawingGrid(.6f);
            }
            else
            {
                Debug.ClearDeveloperConsole();
                DrawingGrid(.0f);
            }
        }

        public List<PathCell> FindPath(Vector2 start, Vector2 end)
        {
            var startPathCell = GetGridCell(start);
            var endPathCell = GetGridCell(end);
            var openList = new List<PathCell> { startPathCell };
            var closeList = new List<PathCell>();

            for (int y = 0; y < Grid.GetLength(0); y++)
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    var pathCell = GetGridCell(Grid[x, y].Position);
                    pathCell.GCost = int.MaxValue;
                    pathCell.CalculateFCost();
                    pathCell.CameFromCell = null;
                }
            startPathCell.GCost = 0;
            startPathCell.HCost = CalculateDistanceCost(start, end);
            startPathCell.CalculateFCost();

            while (openList.Count > 0)
            {
                var currentCell = GetLowestFCost(openList);
                if (currentCell == endPathCell) return CalculatePath(endPathCell);

                openList.Remove(currentCell);
                closeList.Add(currentCell);

                foreach (var neighborCell in GetNeighborList(currentCell))
                {
                    if (closeList.Contains(neighborCell)) continue;

                    var tentativeGCost = currentCell.GCost + CalculateDistanceCost(currentCell.Position, neighborCell.Position);
                    if (tentativeGCost < neighborCell.GCost)
                    {
                        neighborCell.CameFromCell = currentCell;
                        neighborCell.GCost = tentativeGCost;
                        neighborCell.HCost = CalculateDistanceCost(neighborCell.Position, endPathCell.Position);
                        neighborCell.CalculateFCost();

                        if (!openList.Contains(neighborCell))
                            openList.Add(neighborCell);
                    }
                }
            }
            // если вышли за рамки
            return null;
        }

        public void Generation() => Generation(null);

        public void Generation(List<GameObject> gameObjects)
        {
            gridGenerator = gameObject.GetComponent<GridGenerator>();
            if (gridGenerator == null) gridGenerator = gameObject.AddComponent<GridGenerator>();
            gridGenerator?.Generation(gameObjects);
            Grid = (gridGenerator as GridGenerator)?.Grid;
            DestroyGenerator();
        }

        public void DestroyGenerator()
        {
            gridGenerator?.DestroyGenerator();
            gridGenerator = null;
        }

        public List<GameObject> GetCreatedObjects()
        {
            return null;
        }
    }
}