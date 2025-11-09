using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformsController : MonoBehaviour
{
    public GameObject fplateform;
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = Instantiate(fplateform, transform.position, Quaternion.identity);
        g.transform.parent = transform;
    }

    // Update is called once per frame
    /*void Update()
    {
        if (transform.childCount == 0)
        {
            GameObject g = Instantiate(fplateform, transform.position, Quaternion.identity);
            g.transform.parent = transform;
        }
    }*/
    public void createplateform()
    {
        GameObject g = Instantiate(fplateform, transform.position, Quaternion.identity);
        g.transform.parent = transform;
    }
}
