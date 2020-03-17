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

        public void SetObstacles(List<GameObject> gameObjects)
        {
            throw new System.NotImplementedException();
        }

        public void Generation()
        {

            DestroyGenerator();
        }

        public void DestroyGenerator() => gridGenerator.DestroyGenerator();

        public List<GameObject> GetCreatedObjects()
        {
            throw new System.NotImplementedException();
        }        
    }
}
