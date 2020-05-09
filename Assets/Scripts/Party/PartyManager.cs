using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public List<Character> partyMembers;
    List<Character> party;
    public Character partyLeader;
    public DungeonManager dungeonManager;

    bool isInitialized;

    private void Update()
    {
        if (partyLeader.currentTile != partyLeader.NextTile)
        {
            MoveParty();
        }
    }

    public void InitializeParty(Character partyLeader, Grid grid)
    {
        party = new List<Character>();
        this.partyLeader = partyLeader;
        party.Add(partyLeader);

        for (int i = 0; i < partyMembers.Count; i++)
        {
            int index = i - 1;
            Character partyMember = Instantiate(partyMembers[i]);
            partyMember.grid = grid;
            party.Add(partyMember);
        }

        for (int i = 1; i < party.Count; i++)
        {
            int index = i - 1;
            party[i].transform.position = new Vector3(party[index].transform.position.x + 1, party[index].transform.position.y, 0);
        }

        isInitialized = true;
    }

    void MoveParty()
    {
        for (int i = 1; i < party.Count; i++)
        {
            int index = i - 1;
            party[i].SetNextTile(party[index].currentTile);
        }
    }

}
