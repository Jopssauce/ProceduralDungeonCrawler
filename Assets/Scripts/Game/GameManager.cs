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
        if (Input.GetKey(KeyCode.Alpha1))
        {
            partyManager.partyLeader = partyManager.party[0];
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            partyManager.partyLeader = partyManager.party[1];
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            partyManager.partyLeader = partyManager.party[2];
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            partyManager.partyLeader = partyManager.party[3];
        }

        if (partyManager.partyLeader != null)
        {
            Vector3 newPos = Vector3.MoveTowards(Camera.main.transform.position, partyManager.partyLeader.transform.position, 50 * Time.deltaTime);
            Camera.main.transform.position = new Vector3(newPos.x, newPos.y, Camera.main.transform.position.z);
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
            enemy.GetComponent<Character>().grid = dungeonManager.grid;
        }
    }

}
