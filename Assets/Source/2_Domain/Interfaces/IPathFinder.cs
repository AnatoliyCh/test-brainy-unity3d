using Domain.Model.PathFinding;
using System.Collections.Generic;

using UnityEngine;

namespace Domain.Interfaces
{
    public interface IPathFinder
    {
        void DebugGrid(bool debug);
        List<PathCell> FindPath(Vector2 start, Vector2 end);
    }
}