using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Transform waypointParent;
    public Transform waypointA;
    public Transform waypointB;
    public List<Transform> wraithWaypoints;

    private int playerMoney = 0;

    public static event System.Action<int> OnPlayerMoneyChange;

    public enum GameState
    {
        Preparation,     // setup time before the wave starts
        WaveInProgress,  // enemies are spawning/active
        AllEnemiesSpawned,
        Victory          // player won the round
    }

    [SerializeField] TextMeshProUGUI bottomText;

    public static GameManager Instance { get; private set; }

    // Current state (readable everywhere)
    public static GameState State { get; private set; } = GameState.Preparation;
    public static event System.Action<GameState, GameState> OnGameStateChanged;
    // (prevState, newState)

    [SerializeField] private WaveManager waveManager;

    [SerializeField] private int numEnemies = 0;


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
        SetState(GameState.Preparation);

        SetMoney(100);
    }

    public void SetState(GameState newState)
    {
        if (State == newState) return;

        var prev = State;
        State = newState;

        // Debug helper
        Debug.Log($"[GameManager] State changed: {prev} -> {newState}");

        // Notify listeners
        OnGameStateChanged?.Invoke(prev, newState);

        // Optional: do built-in side effects per state
        switch (newState)
        {
            case GameState.Preparation:
                bottomText.text = "Press ENTER near the Tower's Crystal to start!";
                break;

            case GameState.WaveInProgress:
                waveManager.StartWaves();
                bottomText.text = "Defend the Crystal!";
                break;
            case GameState.AllEnemiesSpawned:
                break;

            case GameState.Victory:
                bottomText.text = "You win!!!";
                break;
        }
    }

    void OnEnable()
    {
        CrystalController.OnCrystalDamage += handleCrystal;
    }

    void OnDisable()
    {
        CrystalController.OnCrystalDamage -= handleCrystal;
    }

    void handleCrystal(int hp, int maxHP)
    {
        if (hp <= 0)
        {
            SceneManager.LoadScene("Game_Over");
        }
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

    public int GetMoney()
    {
        return playerMoney;
    }

    public void SetMoney(int money)
    {
        playerMoney += money;
        OnPlayerMoneyChange?.Invoke(playerMoney);

    }


    public void AddEnemy()
    {
        numEnemies++;
    }
    
    public void SubtractEnemy()
    {
        numEnemies--;
        if (numEnemies <= 0)
        {
            if (State == GameState.AllEnemiesSpawned)
            {
                SetState(GameState.Victory);
            }
        }
    }

}
