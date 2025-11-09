using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = transform;
            StartCoroutine(fall());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    IEnumerator fall()
    {
        yield return new WaitForSeconds(1);
        rig.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(2);
        transform.parent.transform.GetComponent<FallingPlatformsController>().createplateform();
        Destroy(this.gameObject);
    }
}
