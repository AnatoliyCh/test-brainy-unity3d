using Domain.Interfaces;
using UnityEngine;

[DisallowMultipleComponent]
public class GameController : MonoBehaviour
{
    private IGenerator levelGenerator;
    private IPathFinder pathFinder;

    //player
    //player

    private void Awake()
    {
        levelGenerator = gameObject.GetComponent<Domain.Model.LevelGeneration.LevelGenerator>();
        pathFinder = gameObject.GetComponent<Domain.Model.PathFinding.PathFinder>();
        if (levelGenerator == null) levelGenerator = gameObject.AddComponent<Domain.Model.LevelGeneration.LevelGenerator>();
        if (pathFinder == null) pathFinder = gameObject.AddComponent<Domain.Model.PathFinding.PathFinder>();
    }

    private void Start()
    {
        levelGenerator?.Generation();
        (pathFinder as IGenerator)?.Generation(levelGenerator?.GetCreatedObjects());
        levelGenerator?.DestroyGenerator();
        levelGenerator = null;
    }

    public void GameReset()
    {

    }
}