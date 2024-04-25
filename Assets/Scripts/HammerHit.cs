using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHit : MonoBehaviour
{

    public GameObject largeHit;
    public GameObject mediumHit;
    public GameObject smallHit;

    public GameObject largeChisel;
    public GameObject mediumChisel;
    public GameObject smallChisel;

    public GameObject largeSpawn;
    public GameObject mediumSpawn;
    public GameObject smallSpawn;

    public float sphereSpeed;
    public float sphereLifespan;

    public float largeScalar;
    public float mediumScalar;
    public float smallScalar;

    // detect any collision with hammer
    void OnCollisionEnter(Collision collision)
    {

        // if necessary velocity is reached
        if (collision.relativeVelocity.magnitude >= 0.2f)
        {
            // make sure the collision was with a chisel
            if (collision.gameObject == largeChisel)
            {
                InstantiateSphere(largeSpawn, largeHit, largeScalar);
            }
            else if(collision.gameObject == mediumChisel)
            {
                InstantiateSphere(mediumSpawn, mediumHit, mediumScalar);
            }
            else if (collision.gameObject == smallChisel)
            {
                InstantiateSphere(smallSpawn, smallHit, smallScalar);
            }
        }

        StartCoroutine(DelayCoroutine());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
    }

    // create temp shpere to represent hit velocity
    void InstantiateSphere(GameObject spawn, GameObject spherePrefab, float scalar)
    {
        GameObject sphere = Instantiate(spherePrefab, spawn.transform.position, Quaternion.identity); // generate sphere
        //sphere.transform.localScale = new Vector3(scalar, scalar, scalar);
        sphere.transform.localScale = new Vector3(scalar, scalar, scalar);
        //Debug.Log(spherePrefab + " scale: " + sphere.transform.localScale);
        Rigidbody sphereRigidbody = sphere.GetComponent<Rigidbody>(); // get rigidbody component

        if (sphereRigidbody != null)
        {
            sphereRigidbody.velocity = spawn.transform.forward * sphereSpeed; // set velocity of sphere
        }

        Destroy(sphere, sphereLifespan); // destroy sphere soon after creation
    }
}
