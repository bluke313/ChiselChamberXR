using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
/**
Generate the Cube and texture
*/
public class Cube : MonoBehaviour {

	private int xSize = 20;
    private int ySize = 20;
    private int zSize = 20;
    private Vector3[] vertices;
	private Vector2[] uvs;
    private Mesh mesh;
	// public Material materialWithTexture;

    private void Awake () {
		Generate();
	}

	private void Generate () {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Cube";
		createVertices();
        createTriangles();
        GetComponent<MeshCollider>().sharedMesh = mesh;

		uvs = new Vector2[vertices.Length];
		//Create UVs so texture is applied
		//if a vertex coordinate is on a face of the cube
		//uv map it with respect to the other 2 directions
		//scale texture by xSize,ySize,zSize
        for (int i = 0; i < vertices.Length; i++)
        {
            if (Mathf.Approximately(0, vertices[i].x)|| Mathf.Approximately(xSize, vertices[i].x)){
                float u = (vertices[i].y);
                float v = (vertices[i].z);
                uvs[i] = new Vector2(u/xSize, v/xSize);
            }
            if (Mathf.Approximately(0, vertices[i].y)|| Mathf.Approximately(ySize, vertices[i].y)){
                float u = (vertices[i].x);
                float v = (vertices[i].z);
                uvs[i] = new Vector2(u/ySize, v/ySize);
            }
            if (Mathf.Approximately(0, vertices[i].z)|| Mathf.Approximately(zSize, vertices[i].z)){
                float u = (vertices[i].x);
                float v = (vertices[i].y);
                uvs[i] = new Vector2(u/zSize, v/zSize);
            }
           
        }


        mesh.uv = uvs;
	}

    private void createTriangles(){
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
		int[] triangles = new int[quads * 6];
        int ring = (xSize + zSize) * 2;
		int t = 0, v = 0;
        for (int y = 0; y < ySize; y++, v++) {
		    for (int q = 0; q < ring - 1; q++, v++) {
			    t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
		    }
		    t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }
        t = CreateTopFace(triangles, t, ring);
        t = CreateBottomFace(triangles, t, ring);
		mesh.triangles = triangles;
    }


    private int CreateTopFace (int[] triangles, int t, int ring) {
		int v = ring * ySize;
        int vMax = v + 21;
        int vMin = ring * (ySize + 1) - 1;
		int vMid = vMin + 1;

		for (int x = 0; x < xSize - 1; x++, v++) {
			t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
		}
		t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);
		
        
        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
		    t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
            for (int x = 1; x < xSize -1; x++, vMid++) {
			    t = SetQuad(
				    triangles, t,
		    		vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
		    }
            //Debug.Log("t:vMid:vMax:vMid + xSize - 1:vMax + 1");
            //Debug.Log(t + ":" + vMid + ":" + vMax + ":" + (vMid+xSize-1) + ":" + (vMax+1));
            if(vMax >= 400 && vMax < 410){vMax += 10;}
            
            t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1); 
        }

        int vTop = vMin - 2;
		t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
		}
        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);
		return t;
	}

    private static int
	SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
		triangles[i] = v00;
		triangles[i + 1] = triangles[i + 4] = v01;
		triangles[i + 2] = triangles[i + 3] = v10;
		triangles[i + 5] = v11;
		return i + 6;
	}
    private void createVertices(){
        int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;
		int faceVertices = (
			(xSize - 1) * (ySize - 1) +
			(xSize - 1) * (zSize - 1) +
			(ySize - 1) * (zSize - 1)) * 2;
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        int v = 0;
		for (int y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++) {
                if(y == 10){
                    //Debug.Log("wrong");
                    //Debug.Log(v);
                }
				vertices[v++] = new Vector3(x, y, 0);
                
			}
			for (int z = 1; z <= zSize; z++) {
                if(y==10){
                    //Debug.Log("right");
                    //Debug.Log(v);
                }
				vertices[v++] = new Vector3(xSize, y, z);
                
                
			}
			for (int x = xSize - 1; x >= 0; x--) {
				vertices[v++] = new Vector3(x, y, zSize);
			}
			for (int z = zSize - 1; z > 0; z--) {
				vertices[v++] = new Vector3(0, y, z);
			}
		}
        for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
                //Debug.Log(v);
				vertices[v++] = new Vector3(x, ySize, z);
			}
		}
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				vertices[v++] = new Vector3(x, 0, z);
			}
		}
        mesh.vertices = vertices;
    }

    private int CreateBottomFace (int[] triangles, int t, int ring) {
		int v = 1;
		int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
		t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= xSize - 2;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
			}
			t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
		}

		int vTop = vMin - 1;
		t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
		
		return t;
	}

    
}