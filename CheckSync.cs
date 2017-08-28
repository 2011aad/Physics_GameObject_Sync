//By 2011aad (Jian Zhang) in Aug.2017
//This script is used to check whether the gameObject is in synchronized state.
//Add this script as a component of the gameObject prefab.

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CheckSync : NetworkBehaviour {


	public const float THRESHOLD = 0.5f;

	[SyncVar]
	private float transOnServer_px;

	[SyncVar]
	private float transOnServer_py;

	[SyncVar]
	private float transOnServer_pz;

	[SyncVar]
	private float transOnServer_rx;

	[SyncVar]
	private float transOnServer_ry;

	[SyncVar]
	private float transOnServer_rz;

	// Use this for initialization
	void Start () {


	}

	// Update is called once per frame
	void Update () {
		if (isServer) {
			Transform trans = this.gameObject.GetComponent<Transform> ();
			transOnServer_px = trans.position.x;
			transOnServer_py = trans.position.y;
			transOnServer_pz = trans.position.z;
			transOnServer_rx = trans.forward.x;
			transOnServer_ry = trans.forward.y;
			transOnServer_rz = trans.forward.z;
		}

		if (!IsSynchronized ()) {
			this.gameObject.GetComponent<MeshRenderer> ().material.color = Color.red;
		} else {
			this.gameObject.GetComponent<MeshRenderer> ().material.color = Color.white;
		}
	}

	bool IsSynchronized(){
		if (!isClient)
			return true;

		Transform trans = this.GetComponent<Transform> ();

		return Mathf.Abs (2 * (trans.position.x - transOnServer_px) / (trans.position.x + transOnServer_px)) < THRESHOLD &&
			Mathf.Abs (2 * (trans.position.y - transOnServer_py) / (trans.position.y + transOnServer_py)) < THRESHOLD &&
			Mathf.Abs (2 * (trans.position.z - transOnServer_pz) / (trans.position.z + transOnServer_pz)) < THRESHOLD &&             //check position synchronization
			(trans.forward.normalized - new Vector3 (transOnServer_rx, transOnServer_ry, transOnServer_rz).normalized).magnitude < THRESHOLD;		//check rotation synchronization
	}
		
}
