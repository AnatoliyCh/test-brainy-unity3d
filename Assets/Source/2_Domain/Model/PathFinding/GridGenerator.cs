using Domain.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Domain.Model.PathFinding
{
    public class GridGenerator : MonoBehaviour, IGenerator
    {
        [SerializeField] private GameObject squarePrefab; // базовый блок для генерации
        [SerializeField] private GameObject triggerPrefab; // триггер для определения препятствий        
        private Color cellNotBlocked = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
        private Color cellBlocked = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);
        private PathCell[,] grid;

        public PathCell[,] Grid
        {
            get
            {
                return grid == null || grid.GetLength(0) == 0 || grid.GetLength(1) == 0 ? null : grid;
            }
        }

        private GameObject GetGameZone(List<GameObject> gameObjects)
        {
            foreach (var item in gameObjects)
                if (item.transform.tag == "GameZone") return item;
            return null;
        }

        public void Generation() => Generation(null);

        public void Generation(List<GameObject> gameObjects)
        {
            if (squarePrefab != null && triggerPrefab != null && gameObjects.Count > 0)
            {
                var trigger = Instantiate(triggerPrefab).transform;
                var zone = GetGameZone(gameObjects).transform;
                trigger.localScale = new Vector2(squarePrefab.transform.localScale.x, squarePrefab.transform.localScale.y) / 2;
                grid = new PathCell[(int)Math.Ceiling(zone.localScale.x / trigger.localScale.x), (int)Math.Ceiling(zone.localScale.y / trigger.localScale.y)];
                // parent для сетки
                var parentGrid = new GameObject();
                parentGrid.transform.SetParent(zone);
                parentGrid.name = "Grid";
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        trigger.localPosition = new Vector3(-zone.localScale.x + trigger.localScale.x + j, -zone.localScale.y + trigger.localScale.y + i, -4) / 2;
                        grid[j, i] = Instantiate(squarePrefab, trigger.localPosition, trigger.rotation, parentGrid.transform).AddComponent<PathCell>();
                        grid[j, i].transform.localScale = trigger.localScale;
                        grid[j, i].Position = trigger.localPosition;
                        grid[j, i].PositionGrid = new Vector2Int(j, i);
                        grid[j, i].gameObject.name = i + "_" + j;
                        var spriteRenderer = grid[j, i].gameObject.GetComponent<SpriteRenderer>();
                        spriteRenderer.color = cellNotBlocked;
                    }
                }
                // проверка на столкновение
                foreach (var item in gameObjects)
                {
                    if (item.transform.tag != "GameZone")
                    {
                        var blockedCells = Physics2D.OverlapBoxAll(item.transform.position, item.transform.lossyScale - new Vector3(.5f, .5f, 0), item.transform.eulerAngles.z);
                        foreach (var cell in blockedCells)
                        {
                            var spriteRenderer = cell.gameObject.GetComponent<SpriteRenderer>();
                            var pathCell = cell.gameObject.GetComponent<PathCell>();
                            if (cell.gameObject.tag != "Obstacle" && spriteRenderer != null && pathCell != null)
                            {
                                spriteRenderer.color = cellBlocked;
                                pathCell.IsBlocked = true;
                            }
                        }
                    }
                }
                // удаляем коллайдеры
                foreach (var item in grid)
                    Destroy(item.GetComponent<Collider2D>());
                Destroy(trigger.gameObject); // удаляем лишний триггер
            }
        }

        public void DestroyGenerator() => Destroy(this);

        public List<GameObject> GetCreatedObjects()
        {
            return null;
        }
    }
}