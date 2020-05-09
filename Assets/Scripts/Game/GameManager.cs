using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DungeonManager dungeonManager;
    public PartyManager partyManager;
    public GameObject playerPrefab;
    public GameObject player;

    private void Awake()
    {
        dungeonManager.dungeonTiler.onComplete += PlacePlayer;
    }

    private void Update()
    {
        //Debug Tool
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Destroy(player);
        }

        if (player != null)
        {
            Vector3 newPos = Vector3.MoveTowards(Camera.main.transform.position, player.transform.position, 50 * Time.deltaTime);
            Camera.main.transform.position = new Vector3(newPos.x, newPos.y, Camera.main.transform.position.z);
        }
    }


    void PlacePlayer()
    {
        DungeonGenerator.Room room = dungeonManager.dungeonGenerator.Rooms[Random.Range(0, dungeonManager.dungeonGenerator.Rooms.Count)];
        player = Instantiate(playerPrefab, dungeonManager.grid.GetCellCenterWorld(room.centerGridTile), playerPrefab.transform.rotation);
        player.GetComponent<Player>().grid = dungeonManager.grid;

        partyManager.InitializeParty(player.GetComponent<Character>(), dungeonManager.grid);
    }

}
