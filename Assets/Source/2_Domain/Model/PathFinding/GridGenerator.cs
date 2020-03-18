using Business.ServiceMethods;
using Domain.Interfaces;
using System;
using System.Collections;
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
        public PathCell[,] Grid { get; protected set; }       

        public void Generation() => Generation(null);

        public void Generation(List<GameObject> gameObjects)
        {
            if (squarePrefab != null && triggerPrefab != null)
            {
                var trigger = Instantiate(triggerPrefab).transform;
                var zone = gameObjects[gameObjects.Count - 1].transform;
                gameObjects.RemoveAt(gameObjects.Count - 1);
                trigger.localScale = new Vector2(squarePrefab.transform.localScale.x, squarePrefab.transform.localScale.y) / 2;
                Grid = new PathCell[(int)Math.Ceiling(zone.localScale.x / trigger.localScale.x), (int)Math.Ceiling(zone.localScale.y / trigger.localScale.y)];
                // parent для сетки
                var parentGrid = new GameObject();
                parentGrid.transform.SetParent(zone);
                parentGrid.name = "Grid";
                for (int i = 0; i < Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < Grid.GetLength(1); j++)
                    {                        
                        trigger.localPosition = new Vector3(-zone.localScale.x + trigger.localScale.x + j, -zone.localScale.y + trigger.localScale.y + i, -4) / 2;
                        Grid[j, i] = Instantiate(squarePrefab, trigger.localPosition, trigger.rotation, parentGrid.transform).AddComponent<PathCell>();
                        Grid[j, i].transform.localScale = trigger.localScale;
                        Grid[j, i].Position = trigger.localPosition;
                        Grid[j, i].gameObject.name = i + "_" + j;
                        var spriteRenderer = Grid[j, i].gameObject.GetComponent<SpriteRenderer>();
                        spriteRenderer.color = cellNotBlocked;                       
                    }
                }
                // проверка на столкновение
                foreach (var item in gameObjects)
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
                // удаляем коллайдеры
                foreach (var item in Grid)
                    Destroy(item.GetComponent<Collider2D>());
            }
        }

        public void DestroyGenerator() => Destroy(this);

        public List<GameObject> GetCreatedObjects()
        {
            return null;
        }       
    }
}