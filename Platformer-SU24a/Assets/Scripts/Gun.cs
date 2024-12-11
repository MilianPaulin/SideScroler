using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float gunFireCD = 0.2f;
    [SerializeField] float spreadAngle = 10f; // Angle between the bullets in degrees

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

        // Shoot the central bullet
        SpawnBullet(0);

        // Shoot the side bullets with spread
        SpawnBullet(-spreadAngle);
        SpawnBullet(spreadAngle);
       
    }

    void SpawnBullet(float angleOffset)
    {
        // Calculate the direction of the bullet
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2)mousePos - (Vector2)bulletSpawnPoint.position).normalized;

        // Rotate the direction by the specified angle offset
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle = baseAngle + angleOffset;
        Vector2 rotatedDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        // Instantiate and initialize the bullet
        Bullet newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(bulletSpawnPoint.position, (Vector2)bulletSpawnPoint.position + rotatedDirection); // Corrected direction
    }

    void RotateGun()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}