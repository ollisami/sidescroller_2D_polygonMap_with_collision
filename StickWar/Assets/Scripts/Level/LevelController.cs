using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public PolygonController polygonController;

	private Vector2 mapSize;
	private int flagSpotSize = 30;
	// Use this for initialization
	void Start () {
		mapSize = polygonController.getSize ();

		setFlag (new Vector2 (mapSize.x - 50, mapSize.y / 2));
		setFlag (new Vector2 (50, mapSize.y / 2));
	}

	// Update is called once per frame
	void Update () {

	}

	private void setSpawnPoints() {

	}

	private void setFlag(Vector2 pos) {	

		for (int y = -(flagSpotSize / 2); y < flagSpotSize / 2; y++) {
			for (int x = -(flagSpotSize / 2); x < flagSpotSize/2; x++) {
				if(Vector2.Distance(pos, new Vector2(x + pos.x, y + pos.y)) < flagSpotSize/2)
					polygonController.setBlockValue (x + (int)pos.x, y + (int)pos.y, 0);
			}

		}
	}
}