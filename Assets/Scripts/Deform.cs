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
        Debug.Log("Hit with " + collision.gameObject.layer);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Hits"))
        {
            Debug.Log("We made it");
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
    }
 
    
    

    // void UpdateMeshVertices()
    // {
    //     filter.mesh.vertices = meshVertices;
    //     coll.sharedMesh = filter.mesh;
        
    //     uvs = new Vector2[meshVertices.Length];

    //     Vector3 minBounds = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    //     Vector3 maxBounds = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

    //     foreach (Vector3 vertex in meshVertices) {
    //         minBounds = Vector3.Min(minBounds, vertex);
    //         maxBounds = Vector3.Max(maxBounds, vertex);
    //     }

    //     Vector3 size = maxBounds - minBounds; // Compute size of the bounding box

    //     for (int i = 0; i < meshVertices.Length; i++) {
    //         Vector3 normalizedPosition = new Vector3(
    //             (meshVertices[i].x) / size.x,
    //             (meshVertices[i].y) / size.y,
    //             (meshVertices[i].z) / size.z
    //         );

    //         if (Mathf.Approximately(meshVertices[i].x, minBounds.x) || Mathf.Approximately(meshVertices[i].x, maxBounds.x)){
    //             uvs[i] = new Vector2(normalizedPosition.y, normalizedPosition.z);
    //         } else if (Mathf.Approximately(meshVertices[i].y, minBounds.y) || Mathf.Approximately(meshVertices[i].y, maxBounds.y)){
    //             uvs[i] = new Vector2(normalizedPosition.x, normalizedPosition.z);
    //         } else if (Mathf.Approximately(meshVertices[i].z, minBounds.z) || Mathf.Approximately(meshVertices[i].z, maxBounds.z)){
    //             uvs[i] = new Vector2(normalizedPosition.x, normalizedPosition.y);
    //         }
    //     }

    //     for (int i = 0; i < meshVertices.Length; i++)
    //     {
            
    //         // if (Mathf.Approximately(0, meshVertices[i].x)|| Mathf.Approximately(xSize, meshVertices[i].x)){
    //         //     float u = (meshVertices[i].y);
    //         //     float v = (meshVertices[i].z);
    //         //     uvs[i] = new Vector2(u/20, v/20);
    //         // }
    //         // if (Mathf.Approximately(0, meshVertices[i].y)|| Mathf.Approximately(ySize, meshVertices[i].y)){
    //         //     float u = (meshVertices[i].x);
    //         //     float v = (meshVertices[i].z);
    //         //     uvs[i] = new Vector2(u/20, v/20);
    //         // }
    //         // if (Mathf.Approximately(0, meshVertices[i].z)|| Mathf.Approximately(zSize, meshVertices[i].z)){
    //         //     float u = (meshVertices[i].x);
    //         //     float v = (meshVertices[i].y);
    //         //     uvs[i] = new Vector2(u/20, v/20);
    //         // }
           
    //     }
    //     filter.mesh.uv = uvs;
    // }

    void UpdateMeshVertices()
{
    filter.mesh.vertices = meshVertices;
    coll.sharedMesh = filter.mesh;
    
    uvs = new Vector2[meshVertices.Length];

    Vector3 minBounds = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    Vector3 maxBounds = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

    //find bounds of cube
    foreach (Vector3 vertex in meshVertices) {
        minBounds = Vector3.Min(minBounds, vertex);
        maxBounds = Vector3.Max(maxBounds, vertex);
    }

    Vector3 size = maxBounds - minBounds;

    float lastY = minBounds.y;
    float lengthY = 0;


    for (int i = 0; i < meshVertices.Length; i++) {
        //move along segments of the cube to apply uvs
        float segmentLength = meshVertices[i].y - lastY;
        lengthY += segmentLength;
        lastY = meshVertices[i].y;

        float normalizedLengthY = lengthY / size.y;

        //set UVs based on normalized segment position
        if (Mathf.Approximately(meshVertices[i].x, minBounds.x) || Mathf.Approximately(meshVertices[i].x, maxBounds.x)){
            uvs[i] = new Vector2(normalizedLengthY, meshVertices[i].z / size.z);
        } else if (Mathf.Approximately(meshVertices[i].y, minBounds.y) || Mathf.Approximately(meshVertices[i].y, maxBounds.y)){
            uvs[i] = new Vector2(meshVertices[i].x / size.x, meshVertices[i].z / size.z);
        } else if (Mathf.Approximately(meshVertices[i].z, minBounds.z) || Mathf.Approximately(meshVertices[i].z, maxBounds.z)){
            uvs[i] = new Vector2(meshVertices[i].x / size.x, normalizedLengthY);
        }
    }

    filter.mesh.uv = uvs; 
    filter.mesh.RecalculateNormals();
}
}