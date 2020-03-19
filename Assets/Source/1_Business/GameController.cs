using Domain.Interfaces;
using UnityEngine;

[DisallowMultipleComponent]
public class GameController : MonoBehaviour
{
    private IGenerator levelGenerator;
    private IGenerator creatureGenerator;
    private IPathFinder pathFinder;

    //player
    //player

    private void Awake()
    {
        levelGenerator = gameObject.GetComponent<Domain.Model.LevelGeneration.LevelGenerator>();
        creatureGenerator = gameObject.GetComponent<Domain.Model.CreatureGeneration.CreatureGenerator>();
        pathFinder = gameObject.GetComponent<Domain.Model.PathFinding.PathFinder>();
        if (levelGenerator == null) levelGenerator = gameObject.AddComponent<Domain.Model.LevelGeneration.LevelGenerator>();
        if (creatureGenerator == null) creatureGenerator = gameObject.AddComponent<Domain.Model.CreatureGeneration.CreatureGenerator>();
        if (pathFinder == null) pathFinder = gameObject.AddComponent<Domain.Model.PathFinding.PathFinder>();
    }

    private void Start()
    {
        levelGenerator?.Generation();
        (pathFinder as IGenerator)?.Generation(levelGenerator?.GetCreatedObjects());
        creatureGenerator?.Generation(levelGenerator?.GetCreatedObjects());
        levelGenerator?.DestroyGenerator();
        creatureGenerator?.DestroyGenerator();
        levelGenerator = null;



        var test = pathFinder.FindPath(new Vector2(-20, -20), new Vector2(10, 10));
        if (test != null)
            for (int i = 0; i < test.Count - 1; i++)
                Debug.DrawLine(new Vector3(test[i].Position.x, test[i].Position.y), new Vector3(test[i + 1].Position.x, test[i + 1].Position.y), Color.red, 2.5f);
        //pathFinder.DebugGrid(true);
    }

    public void GameReset()
    {

    }
}