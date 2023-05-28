using UnityEngine;

[CreateAssetMenu(fileName = "GunParameters", menuName = "GunParameters")]
public class GunParameters : ScriptableObject
{
    public float Distance = 100f;

    public int BulletsCount;

    public float Damage;

    public float Speed;

    public ParticleSystem Shoot;

    public Bullet Bullet;

    public void MakeShoot(Transform transform)
    {
        Bullet bullet = Instantiate(Bullet, transform.position, transform.rotation);
    }
}

/*RaycastHit2D hit;

        Vector3 outPosition = transform.position;

        hit = Physics2D.Raycast(outPosition, transform.up, Distance, 6);
        Debug.Log(hit);
        if (hit)
        {
            WallMap map = hit.transform.gameObject.GetComponent<WallMap>();
            map.GetPainted(hit);   
            Debug.Log($"Painted {map}");
        }*/