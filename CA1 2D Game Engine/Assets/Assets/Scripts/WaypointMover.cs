using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{

    public Transform waypointParent;
    public float moveSpeed = 3f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private Animator animator;
    private int directionX;
    private int directionY;
    private bool isIdle;
    private int idleTime = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = new Transform[waypointParent.childCount];
        animator = GetComponent<Animator>();

        for(int i = 0; i < waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i); //Getting all the waypoints from the waypointParent gameObject
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle && idleTime < 0)
        {
            directionX = directionX * -1;
            animator.SetInteger("DirectionX", directionX);
            directionY = directionY * -1;
            animator.SetInteger("DirectionY", directionY);
            animator.SetFloat("MoveX", 1);
            animator.SetFloat("MoveY", 1);
            isIdle = false;
        }
        else if (!isIdle)
        {
            idleTime = 2;
            isIdle = true;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
        }

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex]; //Moving to each waypoint
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, target.position) < 0.1f) //if the npc is within the Waypoint
        {
            StartCoroutine(WaitAtWaypoint());
        }

    }

    IEnumerator WaitAtWaypoint()
    {
        
        yield return new WaitForSeconds(waitTime);

        //If looping is enabled: increment currentWaypointndex and wrap around if needed.
        //If not looping: increment currentWaypointIndex but don't exceed last waypoint;
        currentWaypointIndex = loopWaypoints ? (currentWaypointIndex + 1) % waypoints.Length : Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);

        
    }

}
