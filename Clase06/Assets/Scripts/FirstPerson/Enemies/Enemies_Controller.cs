using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Controller : MonoBehaviour
{

    GameObject player;

    [SerializeField]
    GameObject Bullet;

    [SerializeField]
    GameObject shootPoint;

    [SerializeField]
    float bulletForce;

    public float timeBetweenShoots;

    public float startTimeBetweenShoots;

    public bool canShoot;

    private void Awake()
    {
        canShoot = false;
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(player.transform);

        if(canShoot == true && UI_Manager.instance.isUIOpen == false)
        {
            ShootingBullets();
        }

    }

   public void ShootingBullets()
   {

        if (timeBetweenShoots <= 0)
        {
           GameObject bullet = Instantiate(Bullet, shootPoint.transform.position, Quaternion.identity);

           bullet.GetComponent<Rigidbody>().AddForce(shootPoint.transform.forward * bulletForce, ForceMode.Impulse);

            Destroy(bullet, 5);
            timeBetweenShoots = startTimeBetweenShoots;
        }
        else
        {
            timeBetweenShoots -= Time.deltaTime;
        }

   }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            canShoot = false;
            gameObject.SetActive(false);
        }
    }

}
