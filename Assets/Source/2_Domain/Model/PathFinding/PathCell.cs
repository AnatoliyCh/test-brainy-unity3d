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
        public Vector2Int PositionGrid { get; set; }
        public bool IsBlocked { get; set; } // если заблокировать то в поиске пути не участвует
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get; set; }
        public PathCell CameFromCell { get; set; }

        public void CalculateFCost() => FCost = GCost + HCost;
        public override string ToString()
        {
            return Position.x + ", " + Position.y;
        }
    }
}
