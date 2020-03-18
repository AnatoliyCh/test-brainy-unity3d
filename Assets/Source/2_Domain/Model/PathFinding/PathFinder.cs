using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Domain.Interfaces;

namespace Domain.Model.PathFinding
{
    [DisallowMultipleComponent]
    public class PathFinder : MonoBehaviour, IPathFinder, IGenerator
    {
        private IGenerator gridGenerator;
        public PathCell[,] Grid { get; protected set; }

        public void DebugGrid()
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
            throw new System.NotImplementedException();
        }
    }
}
