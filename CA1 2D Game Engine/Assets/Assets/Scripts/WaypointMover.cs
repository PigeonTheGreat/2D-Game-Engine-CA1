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
    private bool isWaiting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = new Transform[waypointParent.childCount];

        for(int i = 0; i < waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i); //Getting all the waypoints from the waypointParent gameObject
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }

    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        //If looping is enabled: increment currentWaypointndex and wrap around if needed.
        //If not looping: increment currentWaypointIndex but don't exceed last waypoint;
        currentWaypointIndex = loopWaypoints ? (currentWaypointIndex + 1) % waypoints.Length : Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);

        isWaiting = false;
    }

}
