using System;
using System.Collections.Generic;
using Domain.Interfaces;
using Domain.Model.Creature;
using UnityEngine;

[DisallowMultipleComponent]
public class GameController : MonoBehaviour
{
    [SerializeField] private bool debug = false;

    private IGenerator levelGenerator;
    private IGenerator creatureGenerator;    
    private List<ICreatureController> creatures;

    public IPathFinder pathFinder;

    // отрисовка сетки и путей ботов
    private void DebugDraw()
    {
        pathFinder.DebugPath(true);
        foreach (var item in creatures)
            item.Debug = true;

        //pathFinder.FindPath(new Vector2(-20, -20), new Vector2(10, 10));
    }
    // получение всех существ (боты и игроки)
    private void SetCreatures(List<GameObject> gameObjects)
    {
        if (gameObjects != null && gameObjects.Count > 0)
        {
            creatures = new List<ICreatureController>();
            foreach (var item in gameObjects)
            {
                var creatureController = item.GetComponent<ICreatureController>();
                if (creatureController != null) creatures.Add(creatureController);
            }
            if (creatures.Count > 0) CreatureController.gameController = this;
        }
    }

    private void Awake()
    {
        levelGenerator = gameObject.GetComponent<Domain.Model.LevelGeneration.LevelGenerator>();
        creatureGenerator = gameObject.GetComponent<CreatureGenerator>();
        pathFinder = gameObject.GetComponent<Domain.Model.PathFinding.PathFinder>();
        if (levelGenerator == null) levelGenerator = gameObject.AddComponent<Domain.Model.LevelGeneration.LevelGenerator>();
        if (creatureGenerator == null) creatureGenerator = gameObject.AddComponent<CreatureGenerator>();
        if (pathFinder == null) pathFinder = gameObject.AddComponent<Domain.Model.PathFinding.PathFinder>();
    }

    private void Start()
    {
        levelGenerator?.Generation();
        (pathFinder as IGenerator)?.Generation(levelGenerator?.GetCreatedObjects());
        creatureGenerator?.Generation(levelGenerator?.GetCreatedObjects());
        SetCreatures(creatureGenerator?.GetCreatedObjects());
        levelGenerator?.DestroyGenerator();
        creatureGenerator?.DestroyGenerator();
        levelGenerator = null;

        if (debug) DebugDraw();
    }

    // рестарт раунда с тегом победителя для таблицы очков
    public void GameReset(string winnerTag)
    {
        foreach (var item in creatures)
        {
            item.ResetPosition();
        }

    }
}