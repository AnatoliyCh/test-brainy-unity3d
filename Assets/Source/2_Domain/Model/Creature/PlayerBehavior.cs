using Business.ServiceMethods;
using Domain.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain.Model.Creature
{
    public class PlayerBehavior : MonoBehaviour
    {
        private ICreatureController creatureController;

        public Camera mainCamera;

        private void HandleMovement()
        {
            float moveX = 0f;
            float moveY = 0f;
            if (Input.GetKey(KeyCode.W)) moveY = +1f;
            if (Input.GetKey(KeyCode.S)) moveY = -1f;
            if (Input.GetKey(KeyCode.A)) moveX = -1f;
            if (Input.GetKey(KeyCode.D)) moveX = +1f;
            creatureController.Movement(new Vector3(moveX, moveY).normalized);
        }

        private void HandleRotation()
        {
            var rotateDirection = (ServiceMethods.GetMouseWorldPosition() - transform.position).normalized;
            var angle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
            creatureController.Rotation(new Vector3(0, 0, angle));
        }

        private void Start() => creatureController = gameObject.GetComponent<CreatureController>();

        private void Update()
        {
            HandleMovement();
            HandleRotation();

            if (Input.GetMouseButtonDown(0)) creatureController.Shot();

            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }
}
