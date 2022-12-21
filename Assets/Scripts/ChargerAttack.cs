using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAttack : MonoBehaviour
{
    [SerializeField] int damping; // Larger the number, faster it catches up to player
    [SerializeField] float chargeBuildUp;
    [SerializeField] float chargeTime;
    [SerializeField] float chargeSpeed;
    [SerializeField] float chargeCoolDown;

    [SerializeField] Transform origin;
    [SerializeField] GameObject blast;
    [SerializeField] GameObject chargeWall;
    [SerializeField] GameObject clusterBomb;

    private bool targeting = true;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        StartCoroutine(AttackPattern());
    }

    // Update is called once per frame
    void Update()
    {
        if(targeting) {
            Vector3 targetPos = target.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(targetPos);
            transform.rotation = rotation;
        }
    }

    // Launch the charger in facing direction
    IEnumerator Charge(Vector3 goal)
    {
        chargeWall.SetActive(true);

        float elapsed = 0f;

        while(elapsed < chargeTime) {
            transform.position = Vector3.MoveTowards(transform.position, goal, elapsed / chargeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = goal;

        chargeWall.SetActive(false);
    }

    IEnumerator AttackPattern()
    {
        float time = 0f;

        while(true) {
            time += Time.deltaTime;
            if(targeting && time > chargeBuildUp) {
                targeting = false;
                StartCoroutine(Charge(target.position));
                time = 0f;
            }
            else if(!targeting && time > chargeCoolDown) {
                targeting = true;
                time = 0f;
            }
            else if(targeting && Mathf.Floor(time) % 5 == 0) {
                for(int i = 0; i < 4; i++) {
                    float interval = i * 8 + (-4);

                    Vector3 blastVector = (origin.position - target.position).normalized;
                    Quaternion blastQuaternion = Quaternion.LookRotation(blastVector);
                    Quaternion rot = Quaternion.Euler(new Vector3(0, interval, 0));

                    GameObject shotLaser = Instantiate(blast, origin.position, rot * blastQuaternion);

                    shotLaser.GetComponent<Rigidbody>().velocity = rot * blastVector * -8;
                }
            }
            yield return null;
        }
    }
}
