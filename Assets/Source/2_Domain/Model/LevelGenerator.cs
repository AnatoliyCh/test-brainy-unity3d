using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Domain.Interfaces;

namespace Domain.Model.LevelGeneration
{
    [DisallowMultipleComponent]
    public class LevelGenerator : MonoBehaviour, IGenerator
    {        
        [SerializeField] private GameObject squarePrefab; // базовый блок для генерации
        [SerializeField] private Vector2Int sizeZone; // размер игрового поля
        public void Generation()
        {
            
        }
        public void DestroyGenerator()
        {
            Destroy(this);
        }
    }
}