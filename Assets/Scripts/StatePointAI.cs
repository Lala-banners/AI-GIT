using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [AddComponentMenu("AI/State Point AI")]

    public class StatePointAI : MonoBehaviour
    {
        #region VARIABLES
        [Header("General Stats")]
        public float attackStrength;
        public float health = 100f;
        public float speed = 5f;
        public float maxHealth = 100;

        [Header("State Stats")]
        public float chasePlayerDistance = 5f;
        public float attackPlayerDistance = 5f;
        public float fleePlayerDistance = 5f;

        [Header("Other References")]
        public int index = 0;
        public GameObject player;
        public Player.PlayerController playerController; // == null
        public GameObject[] Waypoint;
        public float minDistance = 0.5f;
        public enum AIBehaviour//Behaviours for AI
        {
            patrol,
            chase,
            attack,
            flee,
        }
        public AIBehaviour state;//States for AI
        #endregion

        private void Start()
        {
            playerController = player.GetComponent<Player.PlayerController>();
            NextState();
        }

        private IEnumerator patrolState()
        {
            
            while (state == AIBehaviour.patrol)
            {
                HealthCheck();
                Patrol();
                yield return null;
                if (player != null)
                {
                    //If the distance between the player and AI is less than 5(chase player range) then AI chase player
                    if (Vector2.Distance(player.transform.position, transform.position) < chasePlayerDistance)
                    {
                        state = AIBehaviour.chase;

                    }
                }
                
               
            }
            
            NextState();
        }
        private IEnumerator chaseState()
        {   
            while (state == AIBehaviour.chase)
            {
                HealthCheck();
                MoveAI(player.transform.position);
                yield return null;
                //If the distance between the player and AI is greater than 5(chase player range) then AI patrols
                if (Vector2.Distance(player.transform.position, transform.position) > chasePlayerDistance)
                {
                    //Change state to patrol state
                    state = AIBehaviour.patrol;
                }
                //If the distance between the player and AI is less than 5(attack player range)  
                else if (Vector2.Distance(player.transform.position, transform.position) < attackPlayerDistance)
                {
                    //Change state to attack state and damage player
                    state = AIBehaviour.attack;
                }

            }
            
            NextState();
        }

        private IEnumerator attackState()
        {
           
            while (state == AIBehaviour.attack)
            {
                HealthCheck();
                playerController.health -= attackStrength; //damages player
                health -= playerController.attackStrength; //damages enemy
                MoveAI(player.transform.position);
                yield return new WaitForSeconds(1f);
                //If there is no player, AI patrols
                if (player == null)
                {
                    state = AIBehaviour.patrol;
                    Debug.Log("Did it work?");
                }
                //If there is no player, then none of these get checked
                else
                {
                    //If the distance between the player and AI is less than 5(attack player range) then AI chases player
                    if (Vector2.Distance(player.transform.position, transform.position) < attackPlayerDistance)
                    {
                        //Change state to chase state 
                        state = AIBehaviour.chase;
                    }
                    //Else if distance between AI and player is greater than attack player range then AI patrols
                    else if (Vector2.Distance(player.transform.position, transform.position) > attackPlayerDistance)
                    {
                        //Change attack state to chase
                        state = AIBehaviour.chase;
                    }
                }
                
                //Used for testing - Debug.Log("Within attacking distance");
            }
           
            //Call next state
            NextState();
        }

        private IEnumerator fleeState()
        {
            while (state == AIBehaviour.flee)
            {
                DeathCheck();
                Vector3 difference = playerController.transform.position - transform.position;
                MoveAI(transform.position - difference);
                yield return null;
                //If the player position and AI position are less than the attackPlayerDistance, then damage each other
                if (Vector2.Distance(player.transform.position, transform.position) < attackPlayerDistance)
                {
                    playerController.health -= attackStrength; //damages player
                    health -= playerController.attackStrength; //damages enemy
                }
            }
            NextState();
        }

        void Patrol()
        {
            //when AI reaches waypoint 1,
            //go to next waypoint 2 etc
            float distance = Vector2.Distance(transform.position, Waypoint[index].transform.position);
            if (distance < minDistance)
            {
                index++;
            }
            if (index >= Waypoint.Length)
            {
                index = 0; ;
            }

            MoveAI(Waypoint[index].transform.position);
        }

        //Function to move the AI
        void MoveAI(Vector2 targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        //Function for calling the next state
        private void NextState()
        {
            //Work out the name of the method to run
            string methodName = state.ToString() + "State"; //If current state is "walk" then this returns "walkState"
                                                            //gives a variable so run a method using its name
            System.Reflection.MethodInfo info =
                GetType().GetMethod(methodName,
                                    System.Reflection.BindingFlags.NonPublic |
                                    System.Reflection.BindingFlags.Instance);
            //Run our method
            StartCoroutine((IEnumerator)info.Invoke(this, null));
            //Using StartCoroutine() means we can leave and come back to the method that is running
            //All Coroutines must return IEnumerator
        }

        //Function for checking whether the AI has lost all health and is dead
        public void DeathCheck()
        {
            //Check if AI is defeated
            if (health <= 0)
            {
                Destroy(gameObject);
                return;
            }

        }

        public void HealthCheck()
        {
            //make sure death check is called first
            DeathCheck();
            //Flee from player if AI health is lower than player health and also lower than 25%
            if (health <= 25 && playerController.health > health)
            {
                //flee state
                state = AIBehaviour.flee;
            }
        }
    }
}







