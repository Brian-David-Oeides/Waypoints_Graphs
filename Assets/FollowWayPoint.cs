using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWayPoint : MonoBehaviour
{
    // array of game objects
    public GameObject[] waypoints; 
    int currentWayPoint = 0; //keep track of the waypoint they are on
    public float speed = 10.0f;// gameobject movement
    public float rotationSpeed = 10f; //rotation speed of gameobject

    GameObject tracker; 
    public float lookAhead = 10f;

    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // to see where tracker is 
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false; //makes tracker invisible
        tracker.transform.position = this.transform.position; //'this' is the tank and == the tracker position
        tracker.transform.rotation = this.transform.rotation; //'this' is the tank and == the tracker rotation
    }
    // move the tracker using the original method used for the tank 

    void ProgressTracker() // method
    {
        //test the distance of the tracker to the waypoint - copied from void Update()
        //if(Vector3.Distance(this.transform.position, waypoints[currentWayPoint].transform.position) < 6) //if less than 3 // commented out when tracker was added
        if(Vector3.Distance(tracker.transform.position, waypoints[currentWayPoint].transform.position) < 3) //if less than 3
        {
            currentWayPoint ++; //add/go to the next waypoint
        }

        if(currentWayPoint >= waypoints.Length) //reset the waypoint cycle
        {
            currentWayPoint = 0;
        } 
        tracker.transform.LookAt(waypoints[currentWayPoint].transform);
        tracker.transform.Translate(0,0, (speed + 10) * Time.deltaTime); // modify speed of tracker to stay ahead of tank

        if(Vector3.Distance(tracker.transform.position, this.transform.position) > lookAhead)return;
    }
    // Update is called once per frame
    void Update() // loop through every waypoint test if gameobject is close then go to the next waypoint.
    {   
        ProgressTracker();
        //commented out when tracker was added
        /* if(Vector3.Distance(this.transform.position, waypoints[currentWayPoint].transform.position) < 6) //if less than 3
        {
            currentWayPoint ++; //add/go to the next waypoint
        }

        if(currentWayPoint >= waypoints.Length) //reset the waypoint cycle
        {
            currentWayPoint = 0;
        } */
        // commented out - LookAt unnatural turn subtitute with Quaternion
        // this.transform.LookAt(waypoints[currentWayPoint].transform); //move gameobject forward to next waypoint
        Quaternion turnNextWayPoint = Quaternion.LookRotation(tracker.transform.position - this.transform.position);
        //Quaternion turnNextWayPoint = Quaternion.LookRotation(waypoints[currentWayPoint].transform.position - this.transform.position); //get goal location - the position of game object
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, turnNextWayPoint, rotationSpeed * Time.deltaTime); 
        this.transform.Translate(0,0,speed * Time.deltaTime);
           
    }
}
