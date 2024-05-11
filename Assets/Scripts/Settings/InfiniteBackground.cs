using System;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [SerializeField] Transform playerTranform;
    Vector2Int currentTilePosition = new Vector2Int(0,0);
    [SerializeField] Vector2Int playerTilePosition;
    Vector2Int onTileGridPlayerPosition;
    [SerializeField] float tileSize = 32f;
    GameObject[,] terrainTiles;

    [SerializeField] int terrainTilesHorizontalCount;
    [SerializeField] int terrainTilesVerticalCount;

    [SerializeField] int fieldOfViewHeight = 3;
    [SerializeField] int fieldOfViewWidth = 3;

    internal void Add(GameObject tileGameObject, Vector2Int tilePosition)
    {
        terrainTiles[tilePosition.x, tilePosition.y] = tileGameObject;
    }

    void Awake()
    {
        terrainTiles = new GameObject[terrainTilesHorizontalCount, terrainTilesVerticalCount];
    }

    private void Start()
    {
        UpdateTilesOnScreen();
    }

    void Update()
    {
        if(playerTranform != null)
        {
            playerTilePosition.x = (int)(playerTranform.position.x / tileSize);
            playerTilePosition.y = (int)(playerTranform.position.y / tileSize);

            playerTilePosition.x -= playerTranform.position.x < 0 ? 1 : 0;
            playerTilePosition.y -= playerTranform.position.y < 0 ? 1 : 0;

            if (currentTilePosition != playerTilePosition)
            {
                currentTilePosition = playerTilePosition;

                onTileGridPlayerPosition.x = CalculatePositionOnAxis(onTileGridPlayerPosition.x, true);
                onTileGridPlayerPosition.y = CalculatePositionOnAxis(onTileGridPlayerPosition.y, false);

                UpdateTilesOnScreen();
            }
        }

    }

    private void UpdateTilesOnScreen()
    {
        for (int pov_x = -(fieldOfViewWidth / 2); pov_x <= fieldOfViewWidth / 2; pov_x++)
        {
            for(int pov_y = -(fieldOfViewHeight / 2); pov_y <= fieldOfViewHeight / 2; pov_y++)
            {
                int tileToUpdate_x = CalculatePositionOnAxis(playerTilePosition.x + pov_x, true);
                int tileToUpdate_y = CalculatePositionOnAxis(playerTilePosition.y + pov_y, false);

                GameObject tile = terrainTiles[tileToUpdate_x,tileToUpdate_y];
                tile.transform.position = CalculateTilePosition(
                    playerTilePosition.x + pov_x,
                    playerTilePosition.y + pov_y);
            }
        }
    }

    private Vector3 CalculateTilePosition(int x, int y)
    {
        return new Vector3(x * tileSize, y * tileSize, 0f);
    }

    private int CalculatePositionOnAxis(float currentValue, bool horizontal)
    {
        if (horizontal)
        {
            if(currentValue >= 0)
            {
                currentValue = currentValue % terrainTilesHorizontalCount;
            }else
            {
                currentValue += 1;
                currentValue = terrainTilesHorizontalCount - 1 + currentValue % terrainTilesHorizontalCount;
            }
        }
        else
        {
            if(currentValue >= 0)
            {
                currentValue = currentValue % terrainTilesVerticalCount;
            } else
            {
                currentValue += 1;
                currentValue = terrainTilesVerticalCount - 1 + currentValue % terrainTilesVerticalCount;
            }
        }
        return (int)currentValue;
    }
}
