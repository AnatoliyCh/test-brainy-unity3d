using Business.ServiceMethods;
using Domain.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain.Model.Creature
{
    public class PlayerBehavior : BaseCreature
    {        
        public Camera mainCamera;

        protected override void HandleMovement()
        {
            float moveX = 0f;
            float moveY = 0f;
            if (Input.GetKey(KeyCode.W)) moveY = +1f;
            if (Input.GetKey(KeyCode.S)) moveY = -1f;
            if (Input.GetKey(KeyCode.A)) moveX = -1f;
            if (Input.GetKey(KeyCode.D)) moveX = +1f;
            creatureController.Movement(new Vector3(moveX, moveY).normalized);            
        }

        protected override void HandleRotation()
        {
            var rotateDirection = (ServiceMethods.GetMouseWorldPosition() - transform.position).normalized;
            var angle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
            creatureController.Rotation(new Vector3(0, 0, angle));
        }

        protected override void HandleShot()
        {
            if (Input.GetMouseButtonDown(0)) creatureController.Shot();
        }


        private void Update()
        {            
            HandleMovement();
            HandleRotation();
            HandleShot();

            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }
}