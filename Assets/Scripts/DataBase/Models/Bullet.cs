using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float Distance = 1f;

    public float Speed = 20f;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Fly();
    }

    public void Fly()
    {
        _rigidbody.velocity = transform.up * Distance * Speed * Time.deltaTime;
    }
}
