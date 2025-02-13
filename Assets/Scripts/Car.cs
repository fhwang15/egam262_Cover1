using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    //public Color of the car that will affect the linerenderer
    public Color lineColor = Color.red;
    public bool isClicked = false;
    public Collider2D Carcollider;

    public GameObject goalObject;

    public float moveSpeed = 5f; 
    public float rotationSpeed = 10f;
    public bool isMoving;

    private int currentWaypointIndex = 0;
    public List<Vector3> waypoints;

    public Rigidbody2D carRigidBody2D;
    public float driftFactor = 0.95f;


    void Start()
    {
        isMoving = false;
        currentWaypointIndex = 0;
        waypoints = new List<Vector3>();
        
    }

    void Update()
    {
        if (isMoving && waypoints.Count > 0)
        {
            CarMovement();
            KillOrthogonalVelocity();
        }
    }

    public void SetWaypoints(List<Vector3> SavedWaypoints)
    {
        waypoints = SavedWaypoints;
        currentWaypointIndex = 0;
    }

    public void startMoving()
    {
        isMoving = true;
    }

    public void CarMovement()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
          
            Vector3 targetWaypoint = waypoints[currentWaypointIndex];

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, moveSpeed * Time.deltaTime);


            Vector3 direction = (targetWaypoint - transform.position).normalized;

            if(direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                transform.rotation = Quaternion.Euler(0, 0, targetRotation.eulerAngles.z);
            }

           
            if (Vector3.Distance(transform.position, targetWaypoint) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            isMoving = false; // ³¡³µÀ¸¸é ¸ØÃß±â
        }
    }
    void KillOrthogonalVelocity()
    {

        Vector2 forwardVelocity = transform.forward * Vector2.Dot(carRigidBody2D.linearVelocity, transform.forward);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidBody2D.linearVelocity, transform.right);

        carRigidBody2D.linearVelocity = forwardVelocity + rightVelocity * driftFactor;

    }
}
