using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Deform : MonoBehaviour
{
    [Range(0, 10)]
    public float deformRadius = 0.2f;
    [Range(0, 10)]
    public float maxDeform = 0.1f;
    [Range(0, 1)]
    public float damageFalloff = 1;
    [Range(0, 10)]
    public float damageMultiplier = 1;
    [Range(0, 100000)]
    public float minDamage = 1;
 
    public AudioClip[] collisionSounds;
 
    private MeshFilter filter;
    private Rigidbody physics;
    private MeshCollider coll;
    private Vector3[] startingVertices;
    private Vector3[] meshVertices;
    private Vector2[] uvs;
 
    void Start()
    {
        filter = GetComponent<MeshFilter>();
        physics = GetComponent<Rigidbody>();
 
        if (GetComponent<MeshCollider>())
            coll = GetComponent<MeshCollider>();
 
        startingVertices = filter.mesh.vertices;
        meshVertices = filter.mesh.vertices;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        float collisionPower = collision.impulse.magnitude;
 
        if (collisionPower > minDamage)
        {
            if (collisionSounds.Length > 0)
                AudioSource.PlayClipAtPoint(collisionSounds[Random.Range(0, collisionSounds.Length)], transform.position, 0.5f);
 
            foreach (ContactPoint point in collision.contacts)
            {
                for (int i = 0; i < meshVertices.Length; i++)
                {
                    Vector3 vertexPosition = meshVertices[i];
                    Vector3 pointPosition = transform.InverseTransformPoint(point.point);
                    float distanceFromCollision = Vector3.Distance(vertexPosition, pointPosition);
                    float distanceFromOriginal = Vector3.Distance(startingVertices[i], vertexPosition);
 
                    if (distanceFromCollision < deformRadius && distanceFromOriginal < maxDeform) // If within collision radius and within max deform
                    {
                        float falloff = 1 - (distanceFromCollision / deformRadius) * damageFalloff;
 
                        float xDeform = pointPosition.x * falloff;
                        float yDeform = pointPosition.y * falloff;
                        float zDeform = pointPosition.z * falloff;
 
                        xDeform = Mathf.Clamp(xDeform, 0, maxDeform);
                        yDeform = Mathf.Clamp(yDeform, 0, maxDeform);
                        zDeform = Mathf.Clamp(zDeform, 0, maxDeform);
 
                        Vector3 deform = new Vector3(xDeform, yDeform, zDeform);
                        meshVertices[i] -= deform * damageMultiplier;
                    }
                }
            }
 
            UpdateMeshVertices();
        }
    }
 
    void UpdateMeshVertices()
    {
        filter.mesh.vertices = meshVertices;
        coll.sharedMesh = filter.mesh;
        
        uvs = new Vector2[meshVertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(meshVertices[i].x, meshVertices[i].z);
        }
        filter.mesh.uv = uvs;
    }
}