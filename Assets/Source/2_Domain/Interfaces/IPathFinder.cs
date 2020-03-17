using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Domain.Interfaces
{
    interface IPathFinder
    {
        /// <summary> установка препятствий </summary> <param name="gameObjects"></param>
        void SetObstacles(List<GameObject> gameObjects);
    }
}