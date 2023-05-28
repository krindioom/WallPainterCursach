using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelParameters", menuName = "LevelParameters")]
public class LevelParameters : ScriptableObject
{
    public int Id;

    public string Name;

    public int Difficulty;

    public string Description;
}
