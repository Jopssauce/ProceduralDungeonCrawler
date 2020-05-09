using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Character
{
    public Character parentPartyMember;
    public Character childPartyMember;

    protected override void Start()
    {
        base.Start();
        //parentPartyMember.onSetNextTile += MoveToParentTile;
    }

    void MoveToParentTile()
    {
        //NextTile = parentPartyMember.currentTile;
    }
}
