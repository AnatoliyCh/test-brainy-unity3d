using UnityEngine;

namespace Domain.Interfaces
{
    public interface ICreatureController
    {
        bool Debug { get; set; }
        Vector3 StartPosition { get; set; } // позиция при старте
        void ResetPosition(); // возвращение к стартовой позиции
        void Movement(Vector3 moveDirection);
        void Rotation(Vector3 rotateDirection);
        void Shot();
    }
}