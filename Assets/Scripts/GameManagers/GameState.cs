using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public Transform waypointParent;
    public Transform waypointA;
    public Transform waypointB;
    public List<Transform> wraithWaypoints;



    public static GameState Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern (ensures one persistent GameState)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene load event

        wraithWaypoints = new List<Transform>();
        InitializeWaypoints();
    }

    void Start()
    {
        
    }

    public void InitializeWaypoints()
    {
        wraithWaypoints.Clear();

        if (waypointParent == null)
        {
            Debug.LogWarning("GameState: No waypoint parent assigned.");
            return;
        }

        // Add each child in order of appearance in Hierarchy
        for (int i = 0; i < waypointParent.transform.childCount; i++)
        {
            Transform child = waypointParent.transform.GetChild(i);
            wraithWaypoints.Add(child);
        }

        Debug.Log($"GameState: Loaded {wraithWaypoints.Count} waypoints from {waypointParent.name}");
    }


    public Transform GetNextWaypoint(int currentWaypoint, int path = 0)
    {
        if (wraithWaypoints == null || wraithWaypoints.Count == 0) return null;



        int idx = currentWaypoint;

        if (idx == -2)
        {
            if (path == 0)
                return waypointA;

            else return waypointB;
        }

        if (idx == -1)
        {
            return wraithWaypoints[0];
        }

        // If last, return same (signals arrival/stop)
        if (idx >= wraithWaypoints.Count - 1) return wraithWaypoints[idx];

        // Otherwise, next in list
        return wraithWaypoints[idx + 1];
    }

    
}
