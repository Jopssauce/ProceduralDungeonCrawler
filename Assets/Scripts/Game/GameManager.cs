using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DungeonManager dungeonManager;
    public PartyManager partyManager;
    public GameObject enemyPrefab;

    private void Awake()
    {
        dungeonManager.dungeonTiler.onComplete += PlaceParty;
    }

    private void Update()
    {
        //Debug Tool
        if (Input.GetKeyDown(KeyCode.F2))
        {
            PlaceEnemies();
        }

        if (partyManager.partyLeader != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 leaderPos = partyManager.partyLeader.transform.position;
            Vector3 newPos = Vector3.MoveTowards(cameraPos, new Vector3(leaderPos.x, leaderPos.y, leaderPos.z - 10), 50 * Time.deltaTime);
            Camera.main.transform.position = new Vector3(newPos.x, Camera.main.transform.position.y, newPos.z);
        }
    }


    void PlaceParty()
    {
        DungeonGenerator.Room room = dungeonManager.dungeonGenerator.Rooms[Random.Range(0, dungeonManager.dungeonGenerator.Rooms.Count)];

        partyManager.InitializeParty(dungeonManager.grid.GetCellCenterWorld(room.centerGridTile), dungeonManager.grid);
    }

    void PlaceEnemies()
    {
        DungeonGenerator dungeonGenerator = dungeonManager.dungeonGenerator;
        for (int i = 0; i < dungeonManager.dungeonGenerator.Rooms.Count; i++)
        {
            DungeonGenerator.Room room = dungeonGenerator.Rooms[i];
            GameObject enemy = Instantiate(enemyPrefab, dungeonManager.grid.GetCellCenterWorld(room.centerGridTile) - Vector3.one, enemyPrefab.transform.rotation);
            enemy.GetComponent<Character>().dungeonManager = dungeonManager;
        }
    }

}
