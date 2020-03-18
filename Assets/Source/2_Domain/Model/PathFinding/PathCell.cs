using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Domain.Model.PathFinding
{
    public class PathCell : MonoBehaviour
    {
        public Vector2 Position { get; set; }
        public bool IsBlocked { get; set; } // если заблокировать то в поиске пути не участвует
        public List<PathCell> Neighbors { get; set; } // соседи
    }
}
