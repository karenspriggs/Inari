using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public List<Projectile> projectilePool;
    public GameObject originPointObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootProjectile(bool isFacingRight)
    {
        foreach(Projectile p in projectilePool)
        {
            if (!p.gameObject.activeInHierarchy)
            {
                p.gameObject.SetActive(true);
                p.Shoot(originPointObject.transform.position, isFacingRight);
                return;
            }
        }
    }
}
