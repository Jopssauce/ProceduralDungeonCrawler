using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        if (partyLeader.currentTile == partyLeader.NextTile)
        {
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
        CharacterDirection2D characterDirection = partyLeader.characterDirection;
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            direction = Vector3.right * Input.GetAxisRaw("Horizontal");
            characterDirection.SetFront(new Vector2Int((int)direction.x, (int)direction.y));

            if (!partyLeader.WillCollide(Vector2.right * Input.GetAxisRaw("Horizontal"), rayDistance))
            {
                partyLeader.SetNextTile(partyLeader.currentTile + grid.WorldToCell(direction));
            }
        }
        else if (Input.GetAxisRaw("Vertical") != 0)
        {
            direction = Vector3.up * Input.GetAxisRaw("Vertical");
            characterDirection.SetFront(new Vector2Int((int)direction.x, (int)direction.y));

            if (!partyLeader.WillCollide(Vector2.up * Input.GetAxisRaw("Vertical"), rayDistance))
            {
                partyLeader.SetNextTile(partyLeader.currentTile + grid.WorldToCell(direction));
            }
        }
    }

    void MoveParty()
    {
        List<PartyMember> followers = new List<PartyMember>();

        int partyLeaderIndex = partyManager.party.IndexOf(partyManager.partyLeader);
        followers = partyManager.party.Where((v, i) => i != partyLeaderIndex).ToList();

        for (int i = 0; i < followers.Count; i++)
        {
            int index = 0;
            if (i == 0)
            {
                index = Mathf.Abs(partyLeaderIndex);
                followers[i].SetNextTile(partyManager.party[index].currentTile);
                continue;
            }
            index = Mathf.Abs(i - 1);
            followers[i].SetNextTile(followers[index].currentTile);

        }
        FixParty(followers);
    }

    void FixParty(List<PartyMember> party)
    {
        //TODO
        //If more than 1 tiles apart
        //Get between the two disconnected party members
        //Check for empty space between
        //Get Direction between the two members
        //Fill empty space with child party member
        //Do with all party members
        for (int i = 0; i < party.Count - 1; i++)
        {
            PartyMember current = party[i];
            PartyMember next = party[i + 1];
            Vector3Int direction = current.NextTile - next.NextTile;
            float distance = direction.magnitude;

            if (distance > 1)
            {
                //Debug.Log(current.partyIndex + " " + next.partyIndex);
                //Debug.Log(distance + " " + direction);
                Vector3Int newPos = Vector3Int.zero;
                if (direction.x > 0)
                {
                    newPos = next.NextTile + direction - new Vector3Int(1, 0, 0);
                }
                if (direction.x < 0)
                {
                    newPos = next.NextTile + direction + new Vector3Int(1, 0, 0);
                }
                if (direction.y > 0)
                {
                    newPos = next.NextTile + direction - new Vector3Int(0, 1, 0);
                }
                if (direction.y < 0)
                {
                    newPos = next.NextTile + direction + new Vector3Int(0, 1, 0);
                }
                //Debug.Log(next.NextTile + " " + newPos);
                if (!IsMemberAtPosition(newPos))
                {
                    Debug.Log("Fixing");
                    next.SetNextTile(newPos);
                }
            }

        }
    }

    bool IsMemberAtPosition(Vector3Int pos)
    {
        for (int i = 0; i < partyManager.party.Count; i++)
        {
            if (partyManager.party[i].NextTile == pos)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray2D.origin, ray2D.direction * rayDistance);

        if (partyManager != null && partyManager.party != null)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 50;
            for (int i = 0; i < partyManager.party.Count; i++)
            {
                UnityEditor.Handles.Label(partyManager.party[i].transform.position, partyManager.party[i].partyIndex.ToString(), style);
            }
        }
        
    }

}
