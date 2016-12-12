using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Vector3 spawnPos;
	public GameObject playerPrefab;
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = Instantiate (playerPrefab, spawnPos, Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
