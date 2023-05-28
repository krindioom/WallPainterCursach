

[System.Serializable]
public struct LevelPrefs
{
    public PlayerPosition PlayerPosition;
    public int EnemyCount;
    public TilePosition[] PaintedWalls;
}

[System.Serializable]
public struct PlayerPosition
{
    public float x;
    public float y;
}

[System.Serializable]
public struct TilePosition
{
    public int x;
    public int y;
    public string color;
}