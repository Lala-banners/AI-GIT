using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StatePointAI : MonoBehaviour
{
    #region VARIABLES
    public float speed = 5f;
    public GameObject[] Waypoint;
    public float minDistance = 0.5f;
    public float chasePlayerDistance = 5f;
    public int index = 0;
    public GameObject player;
    public PlayerController playerController; // == null
    public float attackPlayerDistance = 5f;
    public float fleePlayerDistance = 5f;
    #endregion
    public enum AIBehaviour//Added states for the AI
    {
        patrol,
        chase,
        attack,
        flee,
    }
    public AIBehaviour state;
    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        NextState();
    }
    
    private IEnumerator patrolState()
    {
        Debug.Log("patrol: Enter");
        while (state == AIBehaviour.patrol)
        {
            Patrol();
            yield return null;
            if (Vector2.Distance(player.transform.position, transform.position) < chasePlayerDistance)
            {
                state = AIBehaviour.chase;
            }
        }
        Debug.Log("patrol: Exit");
        NextState();
    }
    private IEnumerator chaseState()
    {
        Debug.Log("chase: Enter");
        while (state == AIBehaviour.chase)
        {
            MoveAI(player.transform.position);
            yield return null;
            if (Vector2.Distance(player.transform.position, transform.position) > chasePlayerDistance)
            {
                state = AIBehaviour.patrol;
            }
        }
        Debug.Log("chase: Exit");
        NextState();
    }

    private IEnumerator attackState()
    {
        while(state == AIBehaviour.attack)
        {
            MoveAI(player.transform.position);
            yield return new WaitForSeconds(1f);
            if (Vector2.Distance(player.transform.position, transform.position) < attackPlayerDistance)
            {
                state = AIBehaviour.flee;
            }
        }
        NextState();
    }

    private IEnumerator fleeState()
    {
        while (state == AIBehaviour.flee)
        {
            MoveAI(player.transform.position);
            yield return null;
            if (Vector2.Distance(player.transform.position, transform.position) > fleePlayerDistance)
            {
                state = AIBehaviour.chase;
            }
        }
        NextState();
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
    }//From WaypointAI
    void MoveAI(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }//from WaypointAI
    private void NextState()
    {
        //work out the name of the method we want to run
        string methodName = state.ToString() + "State"; //if our current state is "walk" then this returns "walkState"
        //give a variable so we run a method using its name
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);
        //Run our method
        StartCoroutine((IEnumerator)info.Invoke(this, null));
        //Using StartCoroutine() means we can leave and come back to the method that is running
        //All Coroutines must return IEnumerator from StateMachine
    }
}






