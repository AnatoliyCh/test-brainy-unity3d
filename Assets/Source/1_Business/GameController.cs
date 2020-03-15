using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Domain.Model.LevelGenerator;

[DisallowMultipleComponent]
[RequireComponent(typeof (LevelGenerator))]
public class GameController : MonoBehaviour
{
    private ILevelGenerator levelGenerator;

    //player
    //player
    //path

    private void Awake()
    {
        levelGenerator = gameObject.GetComponent<LevelGenerator>();        
    }

    private void Start()
    {
        LevelGeneration();
        PathGeneration();
    }

    private void LevelGeneration()
    {

    }
    private void PathGeneration()
    {

    }

    public void GameReset()
    {
        
    }
}