using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float currentSpeed;
    public int projectileDamage;
    public float projectileLifeTime;
    private float _lifeTimeTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
        UpdateLifetimeTimer();
    }

    void Launch(bool isFacingRight)
    {
        if (isFacingRight)
        {
            currentSpeed = speed;
        }
        else
        {
            currentSpeed = -speed;
        }
    }

    private void MoveProjectile()
    {
        this.transform.position = new Vector2(this.transform.position.x + currentSpeed * Time.deltaTime, this.transform.position.y);
    }

    public void Shoot(Vector2 shootingOrigin, bool isFacingRight)
    {
        _lifeTimeTimer = projectileLifeTime;
        this.transform.position = shootingOrigin;
        Launch(isFacingRight);
    }

    void UpdateLifetimeTimer()
    {
        if (_lifeTimeTimer > 0)
        {
            _lifeTimeTimer -= 1 * Time.deltaTime;

            if (_lifeTimeTimer <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
