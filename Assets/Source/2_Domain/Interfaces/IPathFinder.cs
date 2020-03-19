using Domain.Model.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Domain.Interfaces
{
    public interface IPathFinder
    {
        void DebugGrid(bool debug);
        List<PathCell> FindPath(Vector2 start, Vector2 end);
    }
}