using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Temp", menuName = "Temp")]
public class Temp : ScriptableObject
{
    public int A;

    public int B;

    public int Count()
    {
        return A + B;
    }
}
