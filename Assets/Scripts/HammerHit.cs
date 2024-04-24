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

    // detect any collision with hammer
    void OnCollisionEnter(Collision collision)
    {

        // if necessary velocity is reached
        if (collision.relativeVelocity.magnitude >= 0.1f)
        {
            // make sure the collision was with a chisel
            if (collision.gameObject == largeChisel)
            {
                InstantiateSphere(largeSpawn, largeHit);
            }
            else if(collision.gameObject == mediumChisel)
            {
                // Debug.Log("Medium Hit");
                InstantiateSphere(mediumSpawn, mediumHit);
            }
            else if (collision.gameObject == smallChisel)
            {
                InstantiateSphere(smallSpawn, smallHit);
            }
        }

        StartCoroutine(DelayCoroutine());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
    }

    // create temp shpere to represent hit velocity
    void InstantiateSphere(GameObject spawn, GameObject spherePrefab)
    {
        GameObject sphere = Instantiate(spherePrefab, spawn.transform.position, Quaternion.identity); // generate sphere
        Rigidbody sphereRigidbody = sphere.GetComponent<Rigidbody>(); // get rigidbody component

        if (sphereRigidbody != null)
        {
            sphereRigidbody.velocity = spawn.transform.forward * sphereSpeed; // set velocity of sphere
        }

        Destroy(sphere, sphereLifespan); // destroy sphere soon after creation
    }
}
