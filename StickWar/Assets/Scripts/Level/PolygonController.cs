using UnityEngine;
using System.Collections;

public class PolygonController : MonoBehaviour {
	
	public GameObject polygon_prefab;

	private int map_size_x = 512;
	private int map_size_y = 64;

	private PolygonGenerator[] polygonGenerators; 

	// Use this for initialization
	void Start () {
		spawnPolygons ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void spawnPolygons() {
		int polyCount = map_size_x / 128;
		polygonGenerators = new PolygonGenerator[polyCount];
		Vector3 pos = transform.position;

		for (int i = 0; i < polyCount; i++) {
			Vector2 posOnMap = new Vector2 (pos.x + 128 * i, pos.y);
			GameObject go = Instantiate (polygon_prefab, new Vector3 (posOnMap.x, posOnMap.y, pos.z), Quaternion.identity, this.transform) as GameObject;
			polygonGenerators [i] = go.GetComponent<PolygonGenerator> ();
			polygonGenerators [i].createPolygons (128, 128, posOnMap);
		}
	}

	public void setBlockValue (int x, int y, byte value) {
		int block = 0;
		while (x > 128) {
			x -= 128;
			block++;
		}
		polygonGenerators [block].setBlock (x, y, value);
	}

	public Vector2 getSize() {
		return new Vector2 (this.map_size_x, this.map_size_y);
	}
}
