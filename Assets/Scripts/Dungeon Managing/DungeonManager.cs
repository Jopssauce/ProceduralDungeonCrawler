using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public Grid grid;
    public DungeonGenerator dungeonGenerator;
    public DungeonTiler dungeonTiler;

    private void Start()
    {
        dungeonGenerator.Generate();
    }

    private void Update()
    {
        //Debug Tool
        if (Input.GetKeyDown(KeyCode.F1))
        {
            dungeonGenerator.Clear();
            dungeonTiler.dungeonBase.ClearAllTiles();
            dungeonGenerator.Generate();
        }
    }

    

}
