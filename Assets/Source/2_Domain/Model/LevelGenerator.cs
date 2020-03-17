﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Domain.Interfaces;
using Business.ServiceMethods;

namespace Domain.Model.LevelGeneration
{
    [DisallowMultipleComponent]
    public class LevelGenerator : MonoBehaviour, IGenerator
    {
        [SerializeField] private GameObject squarePrefab; // базовый блок для генерации
        [SerializeField] private Vector2Int sizeZone; // размер игрового поля
        [SerializeField] private Color colorZone;

        [SerializeField] private GameObject triggerPrefab; // триггер для генерации препятствий
        [SerializeField] private int maxNumberObstacles; // макс. кол-во препятствий
        [SerializeField] private Color[] colorsObstacles; // цвета препятствий

        private List<GameObject> createdObjects;

        // генерация препятствий
        private void GenerationObstacles(Transform parent)
        {
            var trigger = Instantiate(triggerPrefab);
            var collision = false; // обнаружение столкновений
            for (int i = 0; i < maxNumberObstacles; i++)
            {                
                // относительно размера игрового поля, +1 для проходов
                trigger.transform.localScale = new Vector2(Random.Range(1.0f, ServiceMethods.PercentNumber(10, sizeZone.x)) + 1, Random.Range(1.0f, ServiceMethods.PercentNumber(20, sizeZone.y)) + 1);
                trigger.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 90));
                // размер относительно игрового поля с отступами т.е: (+-ширинаХ / 2) +- (ширинаБлокаХ / 2 (т.к pivot в центре)) +- ширинаБлокаХ / 4 (чтобы при вращении не выходить за края)
                trigger.transform.localPosition = new Vector3(
                    Random.Range(-sizeZone.x / 2 + trigger.transform.localScale.x / 2 + trigger.transform.localScale.x / 4, sizeZone.x / 2 - trigger.transform.localScale.x / 2 - trigger.transform.localScale.x / 4),
                    Random.Range(-sizeZone.y / 2 + trigger.transform.localScale.y / 2 + trigger.transform.localScale.y / 4, sizeZone.y / 2 - trigger.transform.localScale.y / 2 - trigger.transform.localScale.y / 4), -1);
                for (int j = 0; j < createdObjects.Count; j++)
                {
                    if (Vector3.Distance(createdObjects[j].transform.localPosition, trigger.transform.localPosition) <
                        ServiceMethods.GetObjectRadius(createdObjects[j].transform.localScale / 2) + ServiceMethods.GetObjectRadius(trigger.transform.localScale / 2))
                    {
                        collision = true;
                        break;
                    }
                }
                if (!collision) // создание объекта
                {
                    var newObstacles = Instantiate(squarePrefab, trigger.transform.position, trigger.transform.rotation);
                    newObstacles.transform.localScale = new Vector2(trigger.transform.localScale.x - 1, trigger.transform.localScale.y - 1);
                    if (colorsObstacles.Length > 0) newObstacles.GetComponent<SpriteRenderer>().color = colorsObstacles[Random.Range(0, colorsObstacles.Length)];
                    createdObjects.Add(newObstacles);
                }
                collision = false;
            }
            // если есть препятствия
            if (createdObjects.Count > 0)
                foreach (var item in createdObjects)
                    item.transform.SetParent(parent);
        }

        public void Generation()
        {
            var zone = Instantiate(squarePrefab);
            zone.transform.position = Vector2.zero;
            zone.transform.localScale = new Vector3(sizeZone.x, sizeZone.y);
            zone.name = "Zone";
            var spriteRenderer = zone.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) spriteRenderer.color = colorZone;

            if (sizeZone.x > 0 && sizeZone.y > 0) GenerationObstacles(zone.transform);

            createdObjects.Add(zone);
        }
        public void DestroyGenerator()
        {
            Destroy(this);
        }

        public List<GameObject> GetCreatedObjects()
        {
            return createdObjects;
        }
    }
}