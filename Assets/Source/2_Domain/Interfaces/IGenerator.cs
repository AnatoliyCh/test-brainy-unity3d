using System.Collections.Generic;
using UnityEngine;

namespace Domain.Interfaces
{
    public interface IGenerator
    {
        void Generation();
        void Generation(List<GameObject> gameObjects);
        void DestroyGenerator();
        List<GameObject> GetCreatedObjects();
    }
}