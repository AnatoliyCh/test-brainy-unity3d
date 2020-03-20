using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Domain.Interfaces
{
    public interface ICreatureController
    {
        bool Debug { get; set; }
        Vector3 StartPosition { get; set; } // позиция при старте
        void ResetPosition(); // возвращение к стартовой позиции
        GameController GetGameController { get; }
        void Movement(Vector3 moveDirection);
        void Rotation(Vector3 rotateDirection);
        void Shot();
    }
}