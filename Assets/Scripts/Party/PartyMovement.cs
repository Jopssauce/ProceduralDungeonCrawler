using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PartyManager))]
public class PartyMovement : MonoBehaviour
{
    PartyManager partyManager;

    public float rayDistance = 1f;
    Vector3 collisionRayDirection;
    RaycastHit2D hit;
    Ray2D ray2D;

    private void Awake()
    {
        partyManager = GetComponent<PartyManager>();
    }

    private void Update()
    {
        PartyMember partyLeader = partyManager.partyLeader;

        //For gizmos
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            collisionRayDirection = Vector3.right * Input.GetAxisRaw("Horizontal");
        }
        else
        {
            collisionRayDirection = Vector3.up * Input.GetAxisRaw("Vertical");
        }

        ray2D = new Ray2D(partyLeader.transform.position, collisionRayDirection);
        hit = Physics2D.Raycast(ray2D.origin, ray2D.direction, rayDistance);
    }

    private void FixedUpdate()
    {
        PartyMember partyLeader = partyManager.partyLeader;

        if (partyLeader.currentTile == partyLeader.NextTile)
        {
            MovePartyLeader(partyLeader, partyManager.dungeonManager.grid);
        }

        //If Party Leader is moving move the whole party
        if (partyLeader.currentTile != partyLeader.NextTile)
        {
            MoveParty();
        }
    }

    void MovePartyLeader(PartyMember partyLeader, Grid grid)
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") != 0 && !partyLeader.WillCollide(Vector2.right * Input.GetAxisRaw("Horizontal"), rayDistance))
        {
            direction = Vector3.right * Input.GetAxisRaw("Horizontal");
            partyLeader.SetNextTile(partyLeader.currentTile + grid.WorldToCell(direction));
        }
        else if (Input.GetAxisRaw("Vertical") != 0 && !partyLeader.WillCollide(Vector2.up * Input.GetAxisRaw("Vertical"), rayDistance))
        {
            direction = Vector3.up * Input.GetAxisRaw("Vertical");
            partyLeader.SetNextTile(partyLeader.currentTile + grid.WorldToCell(direction));
        }
    }

    void MoveParty()
    {
        for (int i = 1; i < partyManager.party.Count; i++)
        {
            int index = i - 1;
            partyManager.party[i].SetNextTile(partyManager.party[index].currentTile);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray2D.origin, ray2D.direction * rayDistance);
    }

}
