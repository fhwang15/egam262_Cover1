using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrackDrawer : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public float minWaypointDistance; //Distance between the waypoints

    public Car selectedCar;
    bool isDrawing;


    List<Vector3> wayPoints = new List<Vector3>();

    private void Start()
    {
        selectedCar = GetComponentInParent<Car>();

        lineRenderer.positionCount = 0;
        lineRenderer.startColor = selectedCar.lineColor;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && carisSelected())
        {
            StartDrawing(); //renews the List of wayPoints. New drawing.
            
        } 
        else if (Input.GetMouseButtonUp(0))
        {
            //
            EndDrawing();
        } 
        else 
        {
            //If the trackdrawer does not reach the goal, it will yeah.
            //can be goal OnTrigger
            wayPoints.Clear();
            lineRenderer.positionCount = 0;
        }

    }


    void StartDrawing()
    {
        Debug.Log("starting?");
        wayPoints.Clear();
        lineRenderer.positionCount = 0;
        isDrawing = true;

    }

    void EndDrawing()
    {
        wayPoints.Clear();
        lineRenderer.positionCount = 0;
        isDrawing = false;
        //if the end of line collides/within the goal point, then isAtGoal = true;
        //wayPoints[wayPoints.Count-1] 


    }


    private void OnMouseDrag()
    {
        if (isDrawing)
        {   
            Debug.Log("drawin");
            Vector3 mousePos = GetMousePosition();

            if (wayPoints.Count == 0 || Vector3.Distance(mousePos, wayPoints[wayPoints.Count - 1]) > minWaypointDistance)
            {
                wayPoints.Add(mousePos);

                Vector3 point = new Vector3(mousePos.x, mousePos.y, 0f);
                lineRenderer.positionCount = wayPoints.Count;
                lineRenderer.SetPosition(wayPoints.Count - 1, point);

            }
        }
        

    }

    bool carisSelected()
    {

        //if the input.mouseposition => GetMousePosition hits the collider of selected car, then it will return true;
        Vector3 mp = GetMousePosition();

        if (selectedCar.Carcollider.OverlapPoint(mp))
        {
            
            return true;
        }
        else
        {
            return false;
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
