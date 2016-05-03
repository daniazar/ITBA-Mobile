using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]

public class PathNodeObjects 
{
	public Vector3 position;
	public float width;
}

public struct TerrainPathCell
{
	public Vector3 position;
	public bool isAdded;
};

public class CircularObstacle : MonoBehaviour 
{
	// Array of terrain cells for convenience 
	public TerrainPathCell[] terrainCells;
	
	public bool isRoad;
	public bool isFinalized;
	
	public PathNodeObjects[] nodeObjects;
	public Vector3[] nodeObjectVerts; // keeps vertice positions for handles
	
	public MeshCollider pathCollider;
	
	// central terrian cells
	public ArrayList pathCells;
	public ArrayList totalPathVerts;
	public ArrayList innerPathVerts;
	
	// GUI variables
	public int pathWidth;
	public bool pathFlat;
	public int pathSmooth;
	public int elipticNodes = 28;
	System.Random rand  = new System.Random(randomSeed);
	private bool flatten = true;
	
	public static int randomSeed = (new System.Random()).Next();
	//To check for buildings. 	
	public Vector3[] newVertices;
	
	public void GenerateEasyElipicPath()
	{
		rand = new System.Random(randomSeed);
		ResetPath();
		float theta = 0.0f;
		float x , z;
			int randomFinish =  rand.Next(50);
			int randomStart = rand.Next(randomFinish + 50);
		for(int i=0; i <= elipticNodes - 1 ; i ++){
			theta = (float) 2* ((float)Math.PI) * (i+ randomStart)/ (elipticNodes+ randomFinish);
			
				x =  (float)Math.Cos(theta);
				z =  (float)Math.Sin(theta);
			AddPathNode(new Vector3(x , 0 , z));
			
		}
		
		FinalizePath();
	}
	
	public void rebuild()
	{
			randomSeed = rand.Next();		
			GenerateEasyElipicPath();
	
	}
	
	public void NewPath()
	{
		nodeObjects = new PathNodeObjects[0];
		pathCollider = (MeshCollider)gameObject.AddComponent(typeof(MeshCollider));
	}
	
	
	
	public void ResetPath()
	{
		nodeObjects = new PathNodeObjects[0];
		CreatePath(pathSmooth, false);
		MeshFilter meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
		meshFilter.sharedMesh = new Mesh();		
		pathCollider.sharedMesh = meshFilter.sharedMesh;
	}
	
	public void AddPathNode(Vector3  pathNode)
	{
		TerrainPathCell pathNodeCell = new TerrainPathCell();
		pathNodeCell.position.x = pathNode.x;
		pathNodeCell.position.y = pathNode.y;
		pathNodeCell.position.z = pathNode.z;
		CreatePathNode(pathNodeCell);
	}
	public void CreatePathNode(TerrainPathCell nodeCell)
	{
		Vector3 pathPosition = new Vector3(nodeCell.position.x, nodeCell.position.y , nodeCell.position.z);
		AddNode(pathPosition, pathWidth);
	}
	public void AddNode(Vector3 position, float width)
	{
		PathNodeObjects newPathNodeObject = new PathNodeObjects();
		int nNodes;
		if(nodeObjects == null)
		{
			nodeObjects = new PathNodeObjects[0];
			nNodes = 1;
			newPathNodeObject.position = position;
		}
		else
		{
			nNodes = nodeObjects.Length + 1;
			newPathNodeObject.position = position;
		}
		PathNodeObjects[] newNodeObjects = new PathNodeObjects[nNodes];
		newPathNodeObject.width = width;
		int n = newNodeObjects.Length;
		
		for (int i = 0; i < n; i++) 
		{
			if (i != n - 1) 
			{
				newNodeObjects[i] = nodeObjects[i];
			}
			else 
			{
				newNodeObjects[i] = newPathNodeObject;
			}
		}		
		nodeObjects = newNodeObjects;
	}
	
	public void CreatePath(int smoothingLevel, bool road)
	{
		MeshFilter meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
		
		if(meshFilter == null)
			return;
		
		Mesh newMesh = meshFilter.sharedMesh;		
		pathCells = new ArrayList();
	 
		if (newMesh == null) 
		{
			newMesh = new Mesh();
			newMesh.name = "Generated Path Mesh";
			meshFilter.sharedMesh = newMesh;
		} 
	  
		else 
			newMesh.Clear();

		
		if (nodeObjects == null || nodeObjects.Length < 2) 
		{
			return;
		}
		
		int n = nodeObjects.Length;

		int verticesPerNode = 2 * (smoothingLevel + 1) * 2;
		int trianglesPerNode = 6 * (smoothingLevel + 1);
		Vector2[] uvs = new Vector2[(verticesPerNode * (n - 1))];
		Vector3[] newVertices = new Vector3[(verticesPerNode * (n - 1))];
		int[] newTriangles = new int[(trianglesPerNode * (n - 1))];
		nodeObjectVerts = new Vector3[(verticesPerNode * (n - 1))];
		int nextVertex  = 0;
		int nextTriangle = 0;
		int nextUV = 0;
		
		// variables for splines and perpendicular extruded points
		float[] cubicX = new float[n];
		float[] cubicY = new float[n];
		float[] cubicZ = new float[n];
		Vector3[] g1 = new Vector3[smoothingLevel+1];
		Vector3[] g2 = new Vector3[smoothingLevel+1];
		Vector3[] g3 = new Vector3[smoothingLevel+1];
		Vector3 oldG2 = new Vector3();
		Vector3 extrudedPointL = new Vector3();
		Vector3 extrudedPointR = new Vector3();
		
		for(int i = 0; i < n; i++)
		{
			cubicX[i] = nodeObjects[i].position.x;
			cubicY[i] = nodeObjects[i].position.y;
			cubicZ[i] = nodeObjects[i].position.z;
		}
		
		for (int i = 0; i < n; i++) 
		{
			g1 = new Vector3[smoothingLevel+1];
			g2 = new Vector3[smoothingLevel+1];
			g3 = new Vector3[smoothingLevel+1];
			
			extrudedPointL = new Vector3();
			extrudedPointR = new Vector3();
			
			if (i == 0)
			{
				newVertices[nextVertex] = nodeObjects[0].position;
				nextVertex++;
				uvs[0] = new Vector2(0f, 1f);
				nextUV++;
				newVertices[nextVertex] = nodeObjects[0].position;
				nextVertex++;
				uvs[1] = new Vector2(1f, 1f);
				nextUV++;
				
				continue;
			}
			
			float _widthAtNode = pathWidth;		
			
			// Interpolate points along the path using splines for direction and bezier curves for heights
			for (int j = 0; j < smoothingLevel + 1; j++) 
			{
				// clone the vertex for uvs
				if(i == 1)
				{
					if(j != 0)
					{
						newVertices[nextVertex] = newVertices[nextVertex-2];
						nextVertex++;
						
						newVertices[nextVertex] = newVertices[nextVertex-2];
						nextVertex++;
						
						uvs[nextUV] = new Vector2(0f, 1f);
						nextUV++;
						uvs[nextUV] = new Vector2(1f, 1f);
						nextUV++;
					}
					
					else
						oldG2 = nodeObjects[0].position;
				}
				
				else
				{
					newVertices[nextVertex] = newVertices[nextVertex-2];
					nextVertex++;
					
					newVertices[nextVertex] =newVertices[nextVertex-2];
					nextVertex++;
					
					uvs[nextUV] = new Vector2(0f, 1f);
					nextUV++;
					uvs[nextUV] = new Vector2(1f, 1f);
					nextUV++;
				}
				
				float u = (float)j/(float)(smoothingLevel+1f);
				
				Cubic[] X = calcNaturalCubic(n-1, cubicX);
				Cubic[] Z = calcNaturalCubic(n-1, cubicZ);
				
				Vector3 tweenPoint = new Vector3(X[i-1].eval(u), 0f, Z[i-1].eval(u));
				
				// Add the current tweenpoint as a path cell
				TerrainPathCell tC = new TerrainPathCell();
				tC.position.x = tweenPoint.x;
				tC.position.y = tweenPoint.y;
				tC.position.z = tweenPoint.z;
				pathCells.Add(tC);
				
				// update tweened points
				g2[j] = tweenPoint;
				g1[j] = oldG2;
				g3[j] = g2[j] - g1[j];
				oldG2 = g2[j];
				
				// Create perpendicular points for vertices
				extrudedPointL = new Vector3(0, 0, 0);
				extrudedPointR = new Vector3(g3[j].z, 0, -g3[j].x);
				extrudedPointL.Normalize();
				extrudedPointR.Normalize();
				extrudedPointL *= _widthAtNode;
				extrudedPointR *= _widthAtNode;
				

				// create vertices at the perpendicular points
				newVertices[nextVertex] = tweenPoint + extrudedPointR;
				nodeObjectVerts[nextVertex] = newVertices[nextVertex];
				nextVertex++;
				
				newVertices[nextVertex] =  extrudedPointL;
				nodeObjectVerts[nextVertex] = newVertices[nextVertex];
				nextVertex++;

				
				uvs[nextUV] = new Vector2(0f, 0.001f* i);
				nextUV++;
				uvs[nextUV] = new Vector2(0.001f* i, 0f);
				nextUV++;
				
				// flatten mesh
				if(flatten && !road)
				{
					if(newVertices[nextVertex-1].y < (newVertices[nextVertex-2].y-0.0f))
					{
						extrudedPointL *= 1.5f;
						extrudedPointR *= 1.2f;
						newVertices[nextVertex-1] = tweenPoint + extrudedPointL;
						newVertices[nextVertex-2] = tweenPoint + extrudedPointR;
						
						newVertices[nextVertex-1].y = newVertices[nextVertex-2].y;
					}
				
					else if(newVertices[nextVertex-1].y > (newVertices[nextVertex-2].y-0.0f))
					{
						extrudedPointR *= 1.5f;
						extrudedPointL *= 1.2f;
						newVertices[nextVertex-2] = tweenPoint + extrudedPointR;
						newVertices[nextVertex-1] = tweenPoint + extrudedPointL;
						
						newVertices[nextVertex-2].y = newVertices[nextVertex-1].y;		
					}
				}
				// Create triangles...
				newTriangles[nextTriangle] = (verticesPerNode * (i - 1)) + (4 * j); // 0
				nextTriangle++;
				newTriangles[nextTriangle] = (verticesPerNode * (i - 1)) + (4 * j) + 1; // 1
				nextTriangle++;
				newTriangles[nextTriangle] = (verticesPerNode * (i - 1)) + (4 * j) + 2; // 2
				nextTriangle++;
				newTriangles[nextTriangle] = (verticesPerNode * (i - 1)) + (4 * j) + 1; // 1
				nextTriangle++;
				newTriangles[nextTriangle] = (verticesPerNode * (i - 1)) + (4 * j) + 3; // 3
				nextTriangle++;
				newTriangles[nextTriangle] = (verticesPerNode * (i - 1)) + (4 * j) + 2; // 2
				nextTriangle++;
			}
		}
		
	
		extrudedPointL = new Vector3(0, 0, 0);
//		extrudedPointL = new Vector3(-g3[0].z, 0, g3[0].x);
		extrudedPointR = new Vector3(g3[0].z, 0, -g3[0].x);
		
		extrudedPointL.Normalize();
		extrudedPointR.Normalize();
		extrudedPointL *= nodeObjects[0].width;
		extrudedPointR *= nodeObjects[0].width;
		
		newVertices[0] = nodeObjects[0].position + extrudedPointR;
		newVertices[1] = nodeObjects[0].position + extrudedPointL;
		
		
		newMesh.vertices = newVertices;
		newMesh.triangles = newTriangles;
		
		newMesh.uv =  uvs;
     
    for (var i = 0 ; i < uvs.Length; i++)
        uvs[i] = new Vector2(newVertices[i].x / 10, newVertices[i].z / 10);
    newMesh.uv = uvs;

		Vector3[] myNormals = new Vector3[newMesh.vertexCount];
		for(int p = 0; p < newMesh.vertexCount; p++)
		{
			myNormals[p] = Vector3.up;
		}

		newMesh.normals = myNormals;
		
		TangentSolver(newMesh);

		newMesh.RecalculateNormals();
		pathCollider.sharedMesh = null;
    //GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
		pathCollider.sharedMesh = meshFilter.sharedMesh;
		gameObject.renderer.enabled = true;
		
		transform.localScale = new Vector3(1,1,1);        	
    }
	
	public void FinalizePath()
	{
		CreatePath(pathSmooth, true);
		isFinalized = true;
	}
		
		
	public void TangentSolver(Mesh theMesh)
    {
        int vertexCount = theMesh.vertexCount;
        Vector3[] vertices = theMesh.vertices;
        Vector3[] normals = theMesh.normals;
        Vector2[] texcoords = theMesh.uv;
        int[] triangles = theMesh.triangles;
        int triangleCount = triangles.Length/3;
        Vector4[] tangents = new Vector4[vertexCount];
        Vector3[] tan1 = new Vector3[vertexCount];
        Vector3[] tan2 = new Vector3[vertexCount];
        int tri = 0;
		
		int i1, i2, i3;
		Vector3 v1, v2, v3, w1, w2, w3, sdir, tdir;
		float x1, x2, y1, y2, z1, z2, s1, s2, t1, t2, r;
        for (int i = 0; i < (triangleCount); i++)
        {
            i1 = triangles[tri];
            i2 = triangles[tri+1];
            i3 = triangles[tri+2];

            v1 = vertices[i1];
            v2 = vertices[i2];
            v3 = vertices[i3];

            w1 = texcoords[i1];
            w2 = texcoords[i2];
            w3 = texcoords[i3];

            x1 = v2.x - v1.x;
            x2 = v3.x - v1.x;
            y1 = v2.y - v1.y;
            y2 = v3.y - v1.y;
            z1 = v2.z - v1.z;
            z2 = v3.z - v1.z;

            s1 = w2.x - w1.x;
            s2 = w3.x - w1.x;
            t1 = w2.y - w1.y;
            t2 = w3.y - w1.y;

            r = 1.0f / (s1 * t2 - s2 * t1);
            sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;

            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;

            tri += 3;
        }
		
        for (int i = 0; i < (vertexCount); i++)
        {
            Vector3 n = normals[i];
            Vector3 t = tan1[i];

            // Gram-Schmidt orthogonalize
            Vector3.OrthoNormalize(ref n, ref t);

            tangents[i].x  = t.x;
            tangents[i].y  = t.y;
            tangents[i].z  = t.z;

            // Calculate handedness
            tangents[i].w = ( Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0f ) ? -1.0f : 1.0f;
        }       
		
        theMesh.tangents = tangents;
    }
	
	public Cubic[] calcNaturalCubic(int n, float[] x) 
	{
		float[] gamma = new float[n+1];
		float[] delta = new float[n+1];
		float[] D = new float[n+1];
		int i;
	
		gamma[0] = 1.0f/2.0f;
		
		for ( i = 1; i < n; i++) 
		{
		  gamma[i] = 1/(4-gamma[i-1]);
		}
		
		gamma[n] = 1/(2-gamma[n-1]);
		
		delta[0] = 3*(x[1]-x[0])*gamma[0];
		
		for ( i = 1; i < n; i++) 
		{
		  delta[i] = (3*(x[i+1]-x[i-1])-delta[i-1])*gamma[i];
		}
		
		delta[n] = (3*(x[n]-x[n-1])-delta[n-1])*gamma[n];
		
		D[n] = delta[n];
		
		for ( i = n-1; i >= 0; i--) 
		{
		  D[i] = delta[i] - gamma[i]*D[i+1];
		}
		
		Cubic[] C = new Cubic[n+1];
		for ( i = 0; i < n; i++) {
		  C[i] = new Cubic((float)x[i], D[i], 3*(x[i+1] - x[i]) - 2*D[i] - D[i+1],
				   2*(x[i] - x[i+1]) + D[i] + D[i+1]);
		}
			
		return C;
	}
}

public class Cubic
{
  float a,b,c,d;        

  public Cubic(float a, float b, float c, float d){
    this.a = a;
    this.b = b;
    this.c = c;
    this.d = d;
  }
  
  public float eval(float u) 
  {
    return (((d*u) + c)*u + b)*u + a;
  }
}