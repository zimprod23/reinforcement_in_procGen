using UnityEngine;
using System.Collections.Generic;

public class MapDisplay : MonoBehaviour {

	public Renderer textureRender;
	public MeshFilter meshFilter;
    public MeshCollider meshCollider;
	public MeshRenderer meshRenderer;
	//public  List<Vector3>positions;
	public  GameObject[] objectToSpawn;

	public void DrawTexture(Texture2D texture) {
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
	}

	public void DrawMesh(MeshData meshData, Texture2D texture) {
        Mesh mesh = meshData.CreateMesh ();
		meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
		//meshRenderer.sharedMaterial.mainTexture = texture;
	}

}