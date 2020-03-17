using System.Collections;
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
        [SerializeField] private int numberObstacles;

        // генерация припятствий
        private void GenerationObstacles(Transform parent)
        {
            var trigger = Instantiate(triggerPrefab);
            var obstacles = new List<GameObject>(); // лист препятствий
            var collision = false; // обнаружение столкновений
            for (int i = 0; i < numberObstacles; i++)
            {
                // относительно размера игрового поля, +1 для проходов
                trigger.transform.localScale = new Vector2(Random.Range(1, ServiceMethods.PercentNumber(20, sizeZone.x)) + 1, Random.Range(1, ServiceMethods.PercentNumber(20, sizeZone.y)) + 1);
                trigger.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 90));
                // размер относительно игрового поля с отступами т.е: (+-ширинаХ / 2) +- (ширинаБлокаХ / 2 (т.к pivot в центре)) +- ширинаБлокаХ / 4 (чтобы при вращении не выходить за края)
                trigger.transform.localPosition = new Vector2(
                    Random.Range(-sizeZone.x / 2 + trigger.transform.localScale.x / 2 + trigger.transform.localScale.x / 4, sizeZone.x / 2 - trigger.transform.localScale.x / 2 - trigger.transform.localScale.x / 4),
                    Random.Range(-sizeZone.y / 2 + trigger.transform.localScale.y / 2 + trigger.transform.localScale.y / 4, sizeZone.y / 2 - trigger.transform.localScale.y / 2 - trigger.transform.localScale.y / 4));
                for (int j = 0; j < obstacles.Count; j++)
                {
                    if (Vector2.Distance(obstacles[j].transform.position, trigger.transform.position) < 
                        ServiceMethods.GetObjectRadius(obstacles[j].transform.localScale) + ServiceMethods.GetObjectRadius(trigger.transform.localScale))
                    {
                        collision = true;
                        break;
                    }
                }
                // создание объекта
                if (!collision)
                {
                    obstacles.Add(Instantiate(squarePrefab, trigger.transform.position, trigger.transform.rotation));
                    obstacles[obstacles.Count - 1].transform.localScale = new Vector2(trigger.transform.localScale.x - 1, trigger.transform.localScale.y - 1);
                }
                collision = false;
            }
        }

        public void Generation()
        {
            var soze = Instantiate(squarePrefab);
            soze.transform.position = Vector2.zero;
            soze.transform.localScale = new Vector3(sizeZone.x, sizeZone.y);
            soze.name = "Zone";
            var spriteRenderer = soze.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) spriteRenderer.color = colorZone;

            if (sizeZone.x > 0 && sizeZone.y > 0) GenerationObstacles(soze.transform);
        }
        public void DestroyGenerator()
        {
            Destroy(this);
        }
    }
}