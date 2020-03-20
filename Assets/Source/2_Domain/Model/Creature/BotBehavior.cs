using Domain.Model.PathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain.Model.Creature
{
    public class BotBehavior : BaseCreature
    {
        private const int VIEWING_RADIUS = 5; // радиус обзора

        public static Transform player;
        private float rollbackShot;

        private List<PathCell> path = new List<PathCell>();
        private int pathIndex = -1; // ругулятор поиска пути
        // видимость игрока
        private bool isPlayerInRange = false; // игрок в радиусе обзора
        private bool isVisiblePlayer = false; // прямая видимость
        private RaycastHit2D raycastHit;

        private void Start() => GetPath();

        private void Update()
        {
            HandleShot();
            HandleMovement();
            HandleRotation();
        }

        private void FixedUpdate()
        {
            if (creatureController.Debug) Debug.DrawRay(transform.position, transform.right * VIEWING_RADIUS);
            if (isPlayerInRange)
            {
                raycastHit = Physics2D.Raycast(transform.position, transform.right, VIEWING_RADIUS, (1 << 8)); // 1 << 8 лучи только на коллайдеры в слое 8
                if (raycastHit.collider != null)
                {
                    if (raycastHit.collider.gameObject.GetComponent<PlayerBehavior>() != null) isVisiblePlayer = true;
                    else isVisiblePlayer = false;
                }
            }
        }

        protected override void HandleMovement()
        {
            if (pathIndex != -1)
            {
                var vectorDirection = (new Vector3(path[pathIndex].Position.x, path[pathIndex].Position.y, transform.position.z) - transform.position).normalized;
                creatureController.Movement(vectorDirection);
                if (Vector2.Distance(transform.position, path[pathIndex].Position) < 0.1f)
                {
                    pathIndex++;
                    if (pathIndex >= path.Count) pathIndex = -1; // достиг цели
                }
            }
            else GetPath();
        }

        protected override void HandleRotation()
        {
            var distance = Vector2.Distance(transform.position, player.position);
            Vector3 vectorDirection = new Vector3();
            if (distance <= VIEWING_RADIUS)
            {
                vectorDirection = (new Vector3(player.position.x, player.position.y, transform.position.z) - transform.position).normalized;
                isPlayerInRange = true;
            }
            else if (pathIndex != -1)
            {
                vectorDirection = (new Vector3(path[pathIndex].Position.x, path[pathIndex].Position.y, transform.position.z) - transform.position).normalized;
                isPlayerInRange = isVisiblePlayer = false;
            }
            if (vectorDirection != Vector3.zero)
            {
                var angle = Mathf.Atan2(vectorDirection.y, vectorDirection.x) * Mathf.Rad2Deg;
                creatureController.Rotation(new Vector3(0, 0, angle));
            }
        }

        protected override void HandleShot()
        {
            if (rollbackShot <= 0 && isVisiblePlayer)
            {
                creatureController.Shot();
                rollbackShot = 2f;
                StartCoroutine(Timer());
            }
        }

        private IEnumerator Timer()
        {
            while (rollbackShot > 0)
            {
                yield return null;
                rollbackShot -= Time.deltaTime;
            }
        }

        // нахождение пути к игроку
        public void GetPath()
        {
            var unitCircle = Random.insideUnitCircle * 2;
            path = GameController.Instance?.pathFinder.FindPath(transform.position, new Vector2(unitCircle.x + player.position.x, unitCircle.y + player.position.y));
            pathIndex = path != null && path.Count > 0 ? 0 : -1;
        }
    }
}