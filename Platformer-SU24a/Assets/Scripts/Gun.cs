using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float gunFireCD = 0.2f;

    Vector2 mousePos;
    float lastFireTime = 0f;

    PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        Shoot();
        RotateGun();
    }

    void Shoot()
    {
        if (Input.GetButton("Fire1") && Time.time >= lastFireTime)
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        lastFireTime = Time.time + gunFireCD;
        Bullet newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(bulletSpawnPoint.position, mousePos);
    }

    void RotateGun()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = playerController.transform.InverseTransformPoint(mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}