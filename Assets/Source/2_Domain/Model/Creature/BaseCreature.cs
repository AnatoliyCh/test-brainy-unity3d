using Domain.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain.Model.Creature
{
    /// <summary> Базовый класс для существ </summary>
    public abstract class BaseCreature : MonoBehaviour
    {
        protected ICreatureController creatureController;       

        protected abstract void HandleMovement();
        
        protected abstract void HandleRotation();

        protected abstract void HandleShot();

        protected void Awake() => creatureController = gameObject.GetComponent<CreatureController>();
    }
}