using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public float rayDistance = 1f;
    Vector3 collisionRayDirection;
    RaycastHit2D hit;
    Ray2D ray2D;

    protected override void Update()
    {
        base.Update();
        if (currentTile == NextTile)
        {
            ProcessInput();
        }

        //For gizmos
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            collisionRayDirection = Vector3.right * Input.GetAxisRaw("Horizontal");
        }
        else
        {
            collisionRayDirection = Vector3.up * Input.GetAxisRaw("Vertical");
        }
        
        ray2D = new Ray2D(transform.position, collisionRayDirection);
        hit = Physics2D.Raycast(ray2D.origin, ray2D.direction, rayDistance);

    }

    void ProcessInput()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") != 0 && !WillCollide(transform.position, Vector2.right * Input.GetAxisRaw("Horizontal"), rayDistance))
        {
            direction = Vector3.right * Input.GetAxisRaw("Horizontal");
            SetNextTile(currentTile + grid.WorldToCell(direction));
        }
        else if (Input.GetAxisRaw("Vertical") != 0 && !WillCollide(transform.position, Vector2.up * Input.GetAxisRaw("Vertical"), rayDistance))
        {
            direction = Vector3.up * Input.GetAxisRaw("Vertical");
            SetNextTile(currentTile + grid.WorldToCell(direction));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray2D.origin, ray2D.direction * rayDistance);
    }
}
