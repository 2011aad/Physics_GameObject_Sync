//By 2011aad (Jian Zhang) in Aug.2017
//This script should be a component of synchronizer prefab.
//Every frame, update the synchronizer state(position, velocity, etc.) using fragment state on server,
//update the fragment state(position, velocity, etc.) using synchronizer state on client.

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LocalSync : NetworkBehaviour {

	[SyncVar]
	public string parent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var frag = GameObject.Find (parent);

		if (frag == null) {
			if (isServer) {
				Destroy (this.gameObject);
			}
			
			return;
		}
		
		if (isServer) {
			this.gameObject.GetComponent<Rigidbody> ().velocity = frag.GetComponent<Rigidbody> ().velocity;
			this.gameObject.GetComponent<Rigidbody> ().angularVelocity = frag.GetComponent<Rigidbody> ().angularVelocity;
			this.gameObject.GetComponent<Rigidbody> ().position = frag.GetComponent<Rigidbody> ().position;
			this.gameObject.GetComponent<Rigidbody> ().rotation = frag.GetComponent<Rigidbody> ().rotation;
		}
		else if (isClient) {
			if ((this.gameObject.GetComponent<SyncStrategy> ().strategy == StrategyController.ALWAYS_SYNC) || (Time.time - this.gameObject.GetComponent<NetworkTransform>().lastSyncTime < 0.02)) {
				frag.GetComponent<Rigidbody> ().velocity = this.gameObject.GetComponent<Rigidbody> ().velocity;
				frag.GetComponent<Rigidbody> ().angularVelocity = this.gameObject.GetComponent<Rigidbody> ().angularVelocity;
				frag.GetComponent<Rigidbody> ().position = this.gameObject.GetComponent<Rigidbody> ().position;
				frag.GetComponent<Rigidbody> ().rotation = this.gameObject.GetComponent<Rigidbody> ().rotation;
			}
		}
	
	}
}
