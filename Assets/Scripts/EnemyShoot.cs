using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] GameObject blast;
    [SerializeField] GameObject bomb;
    [SerializeField] float blastSpeed;
    [SerializeField] float bombSpeed;
    [SerializeField] Transform origin;
    [SerializeField] float bombTimer;

    private Transform playerTransform;
    private bool bombingState = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttackPattern(4f));
    }

    // Update is called once per frame
    void Update()
    {
        if(bombingState) {
            transform.Rotate(Vector3.up.normalized * 30 * Time.deltaTime);
        }
    }

    void BlasterVolley(int size)
    {
        StartCoroutine(BlasterVolleyShoot(0.1f, size));
    }

    void BombingBloom(int rings, int intervals)
    {
        StartCoroutine(BombingBloomShoot(0.5f, rings, intervals));
    }

    IEnumerator AttackPattern(float delay)
    {
        float t = 0f;
        while(t < bombTimer)
        {
            yield return new WaitForSeconds(delay);
            playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            UnityEngine.Debug.Log(playerTransform.position);
            BlasterVolley(15);
            t += delay;
        }

        bombingState = true;

        while(true) {
            yield return new WaitForSeconds(delay);
            BombingBloom(3, 20);
        }
    }

    IEnumerator BlasterVolleyShoot(float rate, int loops)
    {
        for(int i = 0; i < loops; i++) {
            yield return new WaitForSeconds(rate);

            Vector3 blastVector = (origin.position - playerTransform.position).normalized;
            Quaternion blastQuaternion = Quaternion.LookRotation(blastVector);
            Quaternion rand = Quaternion.Euler(new Vector3(0, Random.value * 30 + (-15), 0));

            GameObject shotLaser = Instantiate(blast, origin.position, rand * blastQuaternion);

            shotLaser.GetComponent<Rigidbody>().velocity = rand * blastVector * -blastSpeed;
        }
        yield break;
    }

    IEnumerator BombingBloomShoot(float rate, int rings, int intervals)
    {
        for(int i = 0; i < rings; i++) {
            yield return new WaitForSeconds(rate);

            float deg = 360f/intervals;
            for(int j = 0; j < intervals; j++) {
                Quaternion bombVector = Quaternion.Euler(new Vector3(0, j * deg, 0));

                GameObject shotBomb = Instantiate(bomb, origin.position, origin.rotation);

                shotBomb.GetComponent<Rigidbody>().velocity = bombVector * new Vector3(0, 0, 1) * -bombSpeed;
            }
        }
    }
}
