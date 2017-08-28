//By 2011aad (Jian Zhang) in Aug.2017
//API for users to add synchronizer for fragments. 
//For a gameObject (go) to be synchronized, (go) don't need to be a prefab, but the server and
//client need to have the same object and using the same name.

//EXAMPLE: Calling AddSync(go) on the server.

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SynchronizerManager : NetworkBehaviour {

	public GameObject SynchronizerPrefab;
	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {

	}

	public GameObject AddSync(GameObject go){

		go.AddComponent<FindSynchronizer> ();

		if (!isServer)
			return null;

		var sync = (GameObject)Instantiate (
			SynchronizerPrefab,
			go.GetComponent<Transform>().position,
			go.GetComponent<Transform>().rotation);

		sync.GetComponent<LocalSync> ().parent = go.name;

		go.GetComponent<FindSynchronizer> ().setSynchronizer (sync);

		NetworkServer.Spawn (sync, (NetworkHash128)SynchronizerPrefab.GetComponent<NetworkIdentity> ().assetId);

		return sync;
	}
}
