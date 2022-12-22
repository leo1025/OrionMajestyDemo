using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBomb : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] float bombSpeed;
    [SerializeField] Transform origin;
    [SerializeField] float pulseTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ClusterExplosion());
    }

    IEnumerator ClusterExplosion()
    {
        yield return new WaitForSeconds(.5f);

        float time = 0f;

        GameObject pulseBomb = Instantiate(bomb, origin.position, origin.rotation);
        pulseBomb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        while(time < pulseTime) {
            if(time < pulseTime/2) {
                pulseBomb.transform.localScale += new Vector3(15f*Time.deltaTime, 15f*Time.deltaTime, 15f*Time.deltaTime);
            }
            else {
                pulseBomb.transform.localScale -= new Vector3(15f*Time.deltaTime, 15f*Time.deltaTime, 15f*Time.deltaTime);
            }
            time += Time.deltaTime;

            yield return null;
        }

        Destroy(pulseBomb);

        for(int i = 0; i < 10; i++) {
            float deg = 120f;

            for(int j = 0; j < 3; j++) {
                Quaternion bombVector = Quaternion.Euler(new Vector3(0, j * deg, 0));

                GameObject shotBomb = Instantiate(bomb, origin.position, origin.rotation);

                shotBomb.GetComponent<Rigidbody>().velocity = origin.rotation * bombVector * new Vector3(0, 0, 1) * -bombSpeed;
            }

            origin.Rotate(new Vector3(0, 20, 0));

            yield return new WaitForSeconds(.15f);
        }

        yield return new WaitForSeconds(.25f);

        Destroy(gameObject);
    }
}
