using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurretPlacer : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private Camera cam;                  // If null, defaults to Camera.main
    [SerializeField] private Grid grid;                   // Your Grid (usually parent of tilemaps)
    [SerializeField] private Tilemap groundTilemap;       // Terrain tilemap
    [SerializeField] private Tilemap turretTilemap;       // Turret tilemap

    [Header("Turret Assets")]
    [SerializeField] private TileBase turretBaseTile;     // Lower turret tile (on top of ground)
    [SerializeField] private TileBase turretTopTile;      // Upper turret tile
    [SerializeField] private GameObject turretPrefab;     // Actual turret GameObject

    [Header("Input")]
    [SerializeField] private KeyCode placeKey = KeyCode.Mouse1; // Left click by default

    private void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(placeKey))
        {
            Vector3 world = cam.ScreenToWorldPoint(Input.mousePosition);
            TryPlaceTurretAtWorld(world);
        }
    }

    public bool TryPlaceTurretAtWorld(Vector3 worldPosition)
    {
        if (GameManager.Instance.GetMoney() < 50) return false;

        if (grid == null || groundTilemap == null || turretTilemap == null || turretPrefab == null || turretBaseTile == null || turretTopTile == null)
        {
            Debug.LogWarning("[TurretPlacer] Missing references.");
            return false;
        }

        // Convert to grid column we clicked in
        Vector3Int clickedCell = grid.WorldToCell(worldPosition);

        // Find supporting ground: either directly below the LOWER turret cell or one cell further.
        // We’ll compute placement relative to the *column* clicked, not the exact row.
        // 1) Check ground directly below clickedCell
        // 2) Or one more below that
        // Then set the LOWER turret cell to be directly above the supporting ground.

        // Case A: ground at clickedCell - 1 => lower = clickedCell
        // Case B: ground at clickedCell - 2 => lower = clickedCell - 1
        // Else: invalid (no support)

        Vector3Int supportBelow = clickedCell + Vector3Int.down;
        Vector3Int supportTwoBelow = clickedCell + Vector3Int.down * 2;

        bool hasGroundBelow = groundTilemap.HasTile(supportBelow);
        bool hasGroundTwoBelow = groundTilemap.HasTile(supportTwoBelow);

        Vector3Int lowerCell;
        if (hasGroundBelow)
        {
            lowerCell = clickedCell; // directly above ground
        }
        else if (hasGroundTwoBelow)
        {
            lowerCell = clickedCell + Vector3Int.down; // bring turret down to sit on ground
        }
        else
        {
            Debug.Log("Invalid: no supporting ground at y-1 or y-2.");
            return false;
        }

        Vector3Int upperCell = lowerCell + Vector3Int.up;

        // Rule: target column must NOT already have a turret on these two cells
        if (turretTilemap.HasTile(lowerCell) || turretTilemap.HasTile(upperCell))
        {
            Debug.Log("Invalid: turret already occupies this column (lower or upper).");
            return false;
        }

        // Rule: placement cells must NOT have ground tiles right now
        if (groundTilemap.HasTile(lowerCell) || groundTilemap.HasTile(upperCell))
        {
            Debug.Log("Invalid: ground tiles present where turret should go.");
            return false;
        }

        // All checks passed—place tiles and prefab.
        turretTilemap.SetTile(lowerCell, turretBaseTile);
        turretTilemap.SetTile(upperCell, turretTopTile);

        // Spawn the turret GameObject at the UPPER cell (you asked to match the "turret tile spawned just above")
        Vector3 upperWorld = grid.GetCellCenterWorld(upperCell);
        Instantiate(turretPrefab, upperWorld, Quaternion.identity);

        GameManager.Instance.SetMoney(-50);
        return true;
    }
}
