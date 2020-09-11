using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaypointAI : MonoBehaviour
{
    public float speed = 5f;
    public GameObject[] Waypoint;
    public float minDistance = 0.5f;
    public float chasePlayerDistance = 5f;
    public int index = 0;
    public GameObject player;
    public PlayerController playerController; // == null
    
    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > chasePlayerDistance)
        {
            //patrol state
            Patrol();
        }
        else
        {
            //chase player state
            playerController.health -= 1 * Time.deltaTime;
            //otherwise Move towards the player
            MoveAI(player.transform.position);
        }
    }
    void Patrol()
    {
        float distance = Vector2.Distance(transform.position, Waypoint[index].transform.position);
        if (distance < minDistance)
        {
            index++;
        }
        if (index >= Waypoint.Length)
        {
            index = 0; ;
        }
        //when we reach waypoint
        //go to next waypoint
        MoveAI(Waypoint[index].transform.position);
    }
    void MoveAI(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    
}
   