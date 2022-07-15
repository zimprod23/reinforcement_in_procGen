using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MeshGenerator {
     
	 public static List<Vector3> SpawmPosition = new List<Vector3>();
	public static MeshData GenerateTerrainMesh(float[,] heightMap,float heightMultiplyer,AnimationCurve heightCuve,Gradient gradient) {
		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);
		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;
		float minTerrainHeight = 0.0f;
		float maxiTerrainHeight = 0.0f;
		MeshData meshData = new MeshData (width, height);
		MeshData forest = new MeshData(width/5,height/5);
		int vertexIndex = 0;
        var lastNoiseHeight = 0.0f;
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				float CurvedHeight = heightCuve.Evaluate(heightMap [x, y])*heightMultiplyer;
				float RegularHeight = heightMap [x, y]*heightMultiplyer;
				if(RegularHeight > maxiTerrainHeight)maxiTerrainHeight=RegularHeight;
				if(RegularHeight < minTerrainHeight)minTerrainHeight=RegularHeight;
				meshData.vertices [vertexIndex] = new Vector3 (topLeftX + x, RegularHeight, topLeftZ - y);
				// if(System.Math.Abs(lastNoiseHeight-RegularHeight) <  25){
				 	if(RegularHeight > 80){
                         if(Random.Range(0,7) == 1){
							SpawmPosition.Add(meshData.vertices[vertexIndex]);
						 }
					 }
					// }
				lastNoiseHeight = meshData.vertices [vertexIndex].y;
				meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);
				if (x < width - 1 && y < height - 1) {
					meshData.AddTriangle (vertexIndex, vertexIndex + width + 1, vertexIndex + width);
					meshData.AddTriangle (vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
					float LearptHeight = Mathf.InverseLerp(minTerrainHeight,maxiTerrainHeight,meshData.vertices[vertexIndex].y);
                    meshData.colors[vertexIndex] = gradient.Evaluate(LearptHeight);
				//	Debug.Log(meshData.colors[vertexIndex]);
				}

				vertexIndex++;
			}
		}

		return meshData;

	}

	
}

public class GenerateTrees{
	public  List<Vector3>positions;

	Mesh mesh;
	public  GameObject[] objectToSpawn;
	 Renderer r ;
	public GenerateTrees(List<Vector3>positionsxx, Renderer rend,GameObject[] obj){
		positions = positionsxx;
		//mesh = meshF.sharedMesh;
		objectToSpawn = obj;
		r = rend;
	}
	public Vector3 CreateTrees(){
		 // assumes the terrain is in a mesh renderer on the same GameObject
		float randomX = Random.Range(r.bounds.min.x, r.bounds.max.x);
		float randomZ = Random.Range(r.bounds.min.z, r.bounds.max.z);
		
		RaycastHit hit;
		if (Physics.Raycast(new Vector3(randomX, r.bounds.max.y + 5f, randomZ), -Vector3.up, out hit)) {
			return hit.point;//(objectToSpawn[Random.Range(0,objectToSpawn.Length - 1)],hit.point,Quaternion.identity);
		} 
		return Vector3.zero;
	}
}

public class MeshData {
	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uvs;
    public Color[] colors;
    
	int triangleIndex;

	public MeshData(int meshWidth, int meshHeight) {
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
        colors = new Color[meshWidth * meshHeight];
	}

    public Color[] Colors { get => colors; set => colors = value; }

    public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		//mesh.uv = uvs;
        mesh.colors = colors;
		mesh.RecalculateNormals ();
		return mesh;
	}

}






// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(MeshFilter))]
// public class MeshGenerator : MonoBehaviour
// {

//     Vector3[] vertices;
//     int[] triangles;
//     Mesh mesh;

//     public int xSize = 20;
//     public int zSize = 20;
//     // Start is called before the first frame update
//     void Start()
//     {
//         mesh = new Mesh();
//         GetComponent<MeshFilter>().mesh = mesh;
//         CreateShape();
//         UpdateMesh();
//     }
    
 
//     void UpdateMesh(){
//         mesh.Clear();
//         mesh.vertices = vertices;
//         mesh.triangles = triangles;
//         mesh.RecalculateNormals();
//     }

//      void CreateShape(){
//         vertices = new Vector3[((xSize + 1)*(zSize + 1))];
    
//         for (int i = 0, z = 0; z < zSize; z++)
//         {
//             for (int x = 0; x < xSize; x++)
//             {
//                 float perlin = Mathf.PerlinNoise(x * .3f,z * .3f)*2f;
//                 vertices[i] = new Vector3(x,perlin,z);
//                 i++;
//             }
//         }
//         triangles = new int[zSize * xSize  * 6];
//         int vert = 0;
//         int tris = 0;
//        for (int z = 0; z < zSize; z++)
//         {
//             for (int x = 0; x < xSize; x++)
//             {
//                 triangles[tris + 0] = vert + 0;
//                 triangles[tris + 1] = vert + xSize + 1;
//                 triangles[tris + 2] = vert + 1;
//                 triangles[tris + 3] = vert + 1;
//                 triangles[tris + 4] = vert + xSize + 1;
//                 triangles[tris + 5] = vert + xSize + 2;

//                 vert++;
//                 tris += 6;
               
//             }
//             vert++;
//         }
//      }

    
//     // Update is called once per frame
   
// }
