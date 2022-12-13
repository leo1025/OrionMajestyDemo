using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    [SerializeField] float killTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(killOverTime());
    }

    IEnumerator killOverTime()
    {
        yield return new WaitForSeconds(killTime);
        Destroy(gameObject);
    }
}
