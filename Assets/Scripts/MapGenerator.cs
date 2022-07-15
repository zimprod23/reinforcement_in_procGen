using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh,FallOff};
	public DrawMode drawMode;

	public GameObject[] Trees;
    public Renderer meshFilter;
    public Gradient gradient;
    public AnimationCurve animationHeightCurve;
    public float heightMultiplyer;
	public int mapWidth;
	public int mapHeight;
	public float noiseScale;
    public bool useFallOffMap;
	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;
    public float[,] falloffMap;
	public TerrainType[] regions;
    void Awake(){
        falloffMap = FalloffGenerator.GenerateFallOffMap(mapWidth);
    }
	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

		Color[] colourMap = new Color[mapWidth * mapHeight];
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
                if(useFallOffMap){
                    noiseMap[x,y] = Mathf.Clamp01(noiseMap[x,y]-falloffMap[x,y]);
                }
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						colourMap [y * mapWidth + x] = regions [i].colour;//gradient.Evaluate(noiseMap[x,y]);
						break;
					}
				}
			}
		}
  
		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, mapWidth, mapHeight));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap,heightMultiplyer*2.2f,animationHeightCurve,gradient), TextureGenerator.TextureFromColourMap (colourMap, mapWidth, mapHeight));
		}else if (drawMode == DrawMode.FallOff) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFallOffMap(mapWidth)));
		}
	}
    void Start(){
		Physics.gravity *= 2;
        GenerateMap();
		//GetSpawmPosForTrees();
    }
	  public void GetSpawmPosForTrees(){
		List<Vector3> spawmpos = MeshGenerator.SpawmPosition;
		GameObject gg = GameObject.FindGameObjectWithTag("Ground");
		GameObject grass = GameObject.FindGameObjectWithTag("Grass");

        GenerateTrees ft = new GenerateTrees(MeshGenerator.SpawmPosition,meshFilter,Trees);
		for (int i = 0; i < spawmpos.Count * 7; i++)
		{
		  Instantiate(Trees[Random.Range(0,Trees.Length - 1)],ft.CreateTrees(),Quaternion.identity);	
		}
		
		// if(spawmpos != null)
		// {
        //     //  for (int i = 0; i < spawmpos.Count; i++)
		// 	// {
		// 	// 	Debug.Log(spawmpos[i]);
		// 	// }
		// 	for (int i = 0; i < spawmpos.Count; i++)
		// 	{
		// 		int rg = Random.Range(0,Trees.Length - 1);
		// 		Debug.Log(spawmpos[i]);
		// 		// if(transform.TransformPoint(spawmpos[i]).y > 50)
		// 		// Instantiate(Trees[rg],transform.TransformPoint(spawmpos[i]),Quaternion.identity);
		// 	}
		// }
	  }
void Update(){
   // GenerateMap();
}
	void OnValidate() {
		if (mapWidth < 1) {
			mapWidth = 1;
		}
		if (mapHeight < 1) {
			mapHeight = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
        falloffMap = FalloffGenerator.GenerateFallOffMap(mapWidth);
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}