using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrackDrawer : MonoBehaviour
{

    public LineRenderer lineRendererPrefab;
    public float minWaypointDistance; //Distance between the waypoints
    
    public List<Car> cars; //List of cars on the screen
    public List<GameObject> goals;

    private Car selectedCar;
    private bool isDrawing;

    private Dictionary<Car, List<Vector3>> carsWaypoints = new Dictionary<Car, List<Vector3>>();
    private Dictionary<Car, LineRenderer> trackDrawers = new Dictionary<Car, LineRenderer>();
    private Dictionary<Car, bool> isAtGoal = new Dictionary<Car, bool>();

    private bool allGoal;


    public GameObject StartButton; //Start Button


    //public GameObject goalPoint;



    
    private void Start()
    {
        StartButton.SetActive(false);

        foreach(Car car in cars)
        {
            isAtGoal[car] = false;
        }

    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            SelectCarAndDraw();          
        }

        if (isDrawing && Input.GetMouseButton(0)) 
        {
            DrawLine();
        }

        if (Input.GetMouseButtonUp(0) && !isDrawing)
        {
            EndDrawing();
        } 

        if (isDrawing && selectedCar != null)
        {
            List<Vector3> waypoints = carsWaypoints[selectedCar];

            if (waypoints.Count > 0&& Vector3.Distance((waypoints[waypoints.Count-1]), selectedCar.goalObject.transform.position) <= 1f){
                EndDrawing();
            }
        }


    }


    void SelectCarAndDraw()
    {
        Vector3 mousePos = GetMousePosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if(hit.collider != null)
        {
            Car car = hit.collider.GetComponent<Car>();

            if (car != null)
            {
                selectedCar = car;

                if (!carsWaypoints.ContainsKey(selectedCar))
                {
                    carsWaypoints[selectedCar] = new List<Vector3>();
                }

                if (!trackDrawers.ContainsKey(selectedCar))
                {
                    LineRenderer lineRenderer = Instantiate(lineRendererPrefab, transform);
                    lineRenderer.startColor = selectedCar.lineColor;
                    lineRenderer.endColor = selectedCar.lineColor;
                    lineRenderer.useWorldSpace = true;

                    trackDrawers[selectedCar] = lineRenderer;


                }


                List<Vector3> waypoints = carsWaypoints[selectedCar]; //select the list that you want the waypoints to be added
                waypoints.Clear();
                waypoints.Add(selectedCar.transform.position);

                LineRenderer currentLine = trackDrawers[selectedCar]; //select the current colored line you are usin
                currentLine.positionCount = 0;

                selectedCar.SetWaypoints(waypoints); //send it to the car
                StartDrawing();
            }

        }


    }

    void StartDrawing()
    {
        if (selectedCar == null) return;

        LineRenderer currentDrawer = trackDrawers[selectedCar];
        currentDrawer.positionCount = 1;
        currentDrawer.SetPosition(0, selectedCar.transform.position); //start drawin

        isDrawing = true;

    }

    void EndDrawing()
    {

        if (selectedCar == null) return;

        List<Vector3> waypoints = carsWaypoints[selectedCar];

        if (waypoints.Count > 0)
        {
            Vector3 lastpoint = waypoints[waypoints.Count - 1];
            bool AnyGoal = false;

            foreach (GameObject goal in goals)
            {
                if (Vector3.Distance(lastpoint, goal.transform.position) <= 1f)
                {
                    AnyGoal = true;
                    break;
                }
            }

            isAtGoal[selectedCar] = AnyGoal;
            CheckAllCarsAtGoal();
        }
        isDrawing = false;

    }

        


    void DrawLine()
    {

        Vector3 mousePos = GetMousePosition();
        mousePos.z = 0;
        mousePos.x += 0.1f;

        List<Vector3> waypoints = carsWaypoints[selectedCar];
        

        float distance = Vector3.Distance(mousePos, waypoints[waypoints.Count - 1]);

        if (distance > minWaypointDistance)
        {
            waypoints.Add(mousePos);
            selectedCar.SetWaypoints(new List<Vector3>(waypoints));

            LineRenderer currentDrawer = trackDrawers[selectedCar];
            currentDrawer.positionCount = waypoints.Count;
            currentDrawer.SetPosition(waypoints.Count - 1, mousePos);

        }
    }

    Vector3 GetMousePosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 0;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        return worldPos;
    }

    public void Onclick()
    {
        foreach(Car car in cars)
        {
            car.startMoving();
        }
    }

    void CheckAllCarsAtGoal()
    {

        bool isAtCorrectGoal = true;

        foreach(Car car in cars)
        {
            if (!isAtGoal[car])
            {
                isAtCorrectGoal = false;
                return;
            }
        }


        if (isAtCorrectGoal)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }

    }



}
