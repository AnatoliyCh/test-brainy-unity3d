using Domain.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain.Model.PathFinding
{
    public class GridGenerator : MonoBehaviour, IGenerator
    {
        [SerializeField] private GameObject squarePrefab; // базовый блок для генерации
        [SerializeField] private GameObject triggerPrefab; // триггер для определения препятствий

        //private List<GameObject> obstacles; // созданные ранее препятствия

        public void Generation() => Generation(null);

        public void Generation(List<GameObject> gameObjects)
        {
            //obstacles = gameObjects;


        }

        public void DestroyGenerator() => Destroy(this);

        public List<GameObject> GetCreatedObjects()
        {
            throw new System.NotImplementedException();
        }        
    }
}