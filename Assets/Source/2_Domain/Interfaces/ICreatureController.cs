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
        void Movement(Vector3 moveDirection);
        void Rotation(Vector3 rotateDirection);
        void Shot();
    }
}