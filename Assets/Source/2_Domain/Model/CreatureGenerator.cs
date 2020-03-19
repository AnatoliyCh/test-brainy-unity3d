using Domain.Interfaces;
using Domain.Model.Creature;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain.Model.CreatureGeneration
{
    public class CreatureGenerator : MonoBehaviour, IGenerator
    {
        private const int MAX_CREATURE = 2;

        [SerializeField] private GameObject bullet;
        [SerializeField] private GameObject creaturePrefab;

        private List<GameObject> creatures = new List<GameObject>();

        private GameObject GetGameZone(List<GameObject> gameObjects)
        {
            foreach (var item in gameObjects)
                if (item.transform.tag == "GameZone") return item;
            return null;
        }

        private void SetUser()
        {            
            creatures[0].tag = "Player";
            creatures[0].name = "Player";
            var playerBehavior = creatures[0].AddComponent<PlayerBehavior>();
            playerBehavior.mainCamera = Camera.main;         
        }

        public void Generation() => Generation(null);

        public void Generation(List<GameObject> gameObjects)
        {
            if (creaturePrefab != null && Camera.main != null && gameObjects.Count > 0)
            {
                var zone = GetGameZone(gameObjects).transform;
                if (zone != null)
                {
                    var xMin = new Vector3(zone.lossyScale.x, zone.lossyScale.y) / -2.1f;
                    var xMax = new Vector3(-zone.lossyScale.x, zone.lossyScale.y) / -2.1f;
                    var yMin = new Vector3(zone.lossyScale.x, zone.lossyScale.y) / -2.1f;
                    var yMax = new Vector3(zone.lossyScale.x, -zone.lossyScale.y) / -2.1f;
                    // спавн сущностей
                    for (int i = 0; i < MAX_CREATURE; i++)
                    {
                        creatures.Add(Instantiate(creaturePrefab, new Vector3(Random.Range(xMin.x, xMax.x), Random.Range(yMin.y, yMax.y), -1), creaturePrefab.transform.rotation));
                        var creatureController = creatures[creatures.Count - 1].AddComponent<CreatureController>();
                        if (bullet != null) creatureController.SetBullet(bullet);
                        creatures[creatures.Count - 1].name = "Creature_" + i;
                    }
                    SetUser(); // установка пользователя
                }
            }
        }

        public void DestroyGenerator() => Destroy(this);

        public List<GameObject> GetCreatedObjects()
        {
            return creatures;
        }
    }
}