using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public PlayerController player;
    private Vector3Int cellNow;
    private Vector3Int cellBefore;
    public Tilemap tileMapColision;

    public TileBase blockedTile;

    public GridLayout gridLayout;

    void Start()
    {
        cellNow = detectarPosisionPlayer(player);
        cellBefore = detectarPosisionPlayer(player);
    }

    void FixedUpdate()
    {
        cellNow = detectarPosisionPlayer(player);
        if (cellBefore != cellNow)
        {
            //Marcamos la celda anterior
            tileMapColision.SetTile(cellBefore, blockedTile);
            cellBefore = cellNow;
        }
    }

    private Vector3Int detectarPosisionPlayer(PlayerController player)
    {
        Vector3 position = player.transform.position;
        position.y += 0.5f;
        return gridLayout.WorldToCell(position);
    }
}
