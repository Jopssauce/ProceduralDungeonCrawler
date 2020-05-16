using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PartyManager))]
public class PartyCombat : MonoBehaviour
{
    PartyManager partyManager;

    private void Awake()
    {
        partyManager = GetComponent<PartyManager>();
    }

    private void Update()
    {
        Character partyLeader = partyManager.partyLeader;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            partyLeader.GetComponent<CharacterCombat>().Attack(partyLeader.skills.defaultSkill, partyManager.dungeonManager.grid);
        }
    }
}
