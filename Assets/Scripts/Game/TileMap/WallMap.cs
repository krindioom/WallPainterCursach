using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallMap : MonoBehaviour
{
    private Tilemap _map;

    public Color Color;

    public HashSet<TilePosition> Positions = new HashSet<TilePosition>();

    private void Start()
    {
        _map = GetComponent<Tilemap>();

        Vector3Int pos;

 

        foreach (var item in Positions)
        {
            pos = new Vector3Int(item.x, item.y);
            if (_map.GetTile(pos) != null)
            {
                _map.SetTileFlags(pos, TileFlags.None);
                _map.SetColor(new Vector3Int(item.x, item.y), Color.red);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bullet bullet))
        {
            Vector3Int overlapPoint = _map.WorldToCell(bullet.transform.position);

            _map.SetTileFlags(overlapPoint, TileFlags.None);

            var tilePosition = new TilePosition() { x = overlapPoint.x, y = overlapPoint.y };

            if (!Positions.Contains(tilePosition))
            {
                _map.SetColor(overlapPoint, bullet.GetComponent<SpriteRenderer>().color);
            }
            if(Color == null)
            {
                Color = bullet.GetComponent<SpriteRenderer>().color;
            }

            Positions.Add(new TilePosition
            {
                x = overlapPoint.x,
                y = overlapPoint.y
            });
        }
    }

    public void SetSavedWalls(int x, int y)
    {
        _map.SetColor(new Vector3Int(x, y), Color);
    }

}
