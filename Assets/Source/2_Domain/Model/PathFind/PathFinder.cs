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

        public void Generation() => Generation(null);

        public void Generation(List<GameObject> gameObjects)
        {
            gridGenerator = gameObject.GetComponent<GridGenerator>();
            if (gridGenerator == null) gridGenerator = gameObject.AddComponent<GridGenerator>();
            gridGenerator?.Generation(gameObjects);
            DestroyGenerator();
        }

        public void DestroyGenerator() => gridGenerator?.DestroyGenerator();

        public List<GameObject> GetCreatedObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}
