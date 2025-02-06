using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TrackDrawer : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public float minWaypointDistance; //Distance between the waypoints

    bool isAtGoal;

    List<Vector3> wayPoints = new List<Vector3>();

    private void Start()
    {
        lineRenderer.positionCount = 0;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing(); //renews the List of wayPoints. New drawing.
        } 
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrawing();
        }

    }


    void StartDrawing()
    {
        wayPoints.Clear();
        lineRenderer.positionCount = 0;

    }

    void EndDrawing()
    {
        wayPoints.Clear();
        lineRenderer.positionCount = 0;
        
        //if the end of line collides/within the goal point, then isAtGoal = true;
        //wayPoints[wayPoints.Count-1] 


    }


    private void OnMouseDrag()
    {
        Vector3 mousePos = GetMousePosition();

        if (wayPoints.Count == 0 || Vector3.Distance(mousePos, wayPoints[wayPoints.Count - 1]) > minWaypointDistance)
        {
            wayPoints.Add(mousePos);

            Vector3 point = new Vector3(mousePos.x, mousePos.y, 0f);
            lineRenderer.positionCount = wayPoints.Count;
            lineRenderer.SetPosition(wayPoints.Count - 1, point);

        }
    }

    Vector3 GetMousePosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -10f;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        return worldPos;
    }

}
