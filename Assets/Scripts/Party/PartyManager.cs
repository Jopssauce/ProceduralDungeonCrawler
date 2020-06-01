using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private List<PartyMember> partyMembers = null;

    public PartyMember partyLeader;
    public List<PartyMember> party;

    public DungeonManager dungeonManager;

    public void InitializeParty(Vector3 position, Grid grid)
    {
        party = new List<PartyMember>();

        for (int i = 0; i < partyMembers.Count; i++)
        {
            PartyMember partyMember = Instantiate(partyMembers[i], new Vector3(position.x, 1.5f, position.z), partyMembers[i].transform.rotation);
            partyMember.partyIndex = i;
            partyMember.dungeonManager = dungeonManager;
            party.Add(partyMember);
        }

        for (int i = 1; i < party.Count; i++)
        {
            int index = i - 1;
            party[i].transform.position = new Vector3(party[index].transform.position.x + 1, 1.5f, party[index].transform.position.z);
        }

        this.partyLeader = party[0];
    }

    

}
