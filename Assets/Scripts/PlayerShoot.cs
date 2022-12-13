using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] GameObject laser;
    [SerializeField] Transform shootOrigin;
    [SerializeField] float laserSpeed;
    [SerializeField] float fireRate;
    [SerializeField] int laserDamage;

    private float shootTime;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1") && (Time.time >= shootTime))
        {
            shootTime = Time.time + fireRate;

            GameObject shotLaser = Instantiate(laser, shootOrigin.position, shootOrigin.rotation);
            shotLaser.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, laserSpeed);
        }
    }
}