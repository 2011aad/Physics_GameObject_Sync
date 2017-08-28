//By 2011aad (Jian Zhang) in Aug.2017
//This script should be a component of network manager.
//Regist RegisterSpawnHandler() of the synchronizer prefab

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncSpawnHandlerRegister : MonoBehaviour {

	public delegate GameObject SpawnDelegate(Vector3 position, NetworkHash128 assetId);
	public delegate void UnSpawnDelegate(GameObject spawned);	

	public GameObject SynchronizerPrefab;

	// Use this for initialization
	void Start () {
		//Debug.Log ((NetworkHash128)SynchronizerPrefab.GetComponent<NetworkIdentity> ().assetId);
		ClientScene.RegisterSpawnHandler ((NetworkHash128)SynchronizerPrefab.GetComponent<NetworkIdentity> ().assetId, SpawnSychronizer, UnSpawnSychronizer);
	}

	// Update is called once per frame
	void Update () {

	}

	// Handles requests to spawn objects on the client
	public GameObject SpawnSychronizer(Vector3 position, NetworkHash128 assetId){
		var sync = (GameObject)Instantiate (
			SynchronizerPrefab,
			position,
			Quaternion.identity);

		return sync;
	}

	// Handles requests to unspawn objects on the client
	public void UnSpawnSychronizer(GameObject spawned){
		Destroy (spawned);
	}
}
