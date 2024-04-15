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
                InstantiateSphere(largeChisel, largeHit);
            }
            else if(collision.gameObject == mediumChisel)
            {
                // Debug.Log("Medium Hit");
                InstantiateSphere(mediumChisel, mediumHit);
            }
            else if (collision.gameObject == smallChisel)
            {
                InstantiateSphere(smallChisel, smallHit);
            }
        }

        StartCoroutine(DelayCoroutine());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
    }

    // create temp shpere to represent hit velocity
    void InstantiateSphere(GameObject chisel, GameObject spherePrefab)
    {
        Vector3 spherePos = chisel.transform.position;
        if (spherePrefab == largeHit)
        {
            spherePos = new Vector3(chisel.transform.position.x, chisel.transform.position.y, chisel.transform.position.z + 0.01f);
        }

        GameObject sphere = Instantiate(spherePrefab, chisel.transform.position, Quaternion.identity); // generate sphere
        Rigidbody sphereRigidbody = sphere.GetComponent<Rigidbody>(); // get rigidbody component

        if (sphereRigidbody != null)
        {
            sphereRigidbody.velocity = chisel.transform.forward * sphereSpeed; // set velocity of sphere
        }

        Destroy(sphere, sphereLifespan); // destroy sphere soon after creation
    }
}
