using UnityEngine;

public class TigerAttackShooter : MonoBehaviour
{
    public Camera cam;
    public GameObject projectile;
    public Transform firePoint;
    public float fireRate = 4;

    private Vector3 destination;
    private float timeToFire;
    private TigerAttack tigerAttackScript;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (cam != null)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            destination = ray.GetPoint(1000);
            InstantiateProjectile();
        }
        else
        {
            Debug.Log("B");
            InstantiateProjectileAtFirePoint();
        }
    }
    
    void InstantiateProjectile()
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;

        tigerAttackScript = projectileObj.GetComponent<TigerAttack>();
        RotateToDestination(projectileObj, destination, true);
        projectileObj.GetComponent<Rigidbody>().linearVelocity = transform.forward * tigerAttackScript.speed;
    }

    void InstantiateProjectileAtFirePoint()
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;

        tigerAttackScript = projectileObj.GetComponent<TigerAttack>();
        RotateToDestination(projectileObj, firePoint.transform.forward * 1000, true);
        projectileObj.GetComponent<Rigidbody>().linearVelocity = firePoint.transform.forward * tigerAttackScript.speed;
    }
    
    void RotateToDestination(GameObject obj, Vector3 destination, bool onlyY)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation(direction);

        if (onlyY)
        {
            rotation.x = 0;
            rotation.z = 0;
        }

        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
}
