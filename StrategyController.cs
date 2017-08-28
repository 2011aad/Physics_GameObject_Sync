//By 2011aad (Jian Zhang) in Aug.2017
//This script is used to set global sync strategy.

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StrategyController : NetworkBehaviour {

	public const int ALWAYS_SYNC = 0;
	public const int NON_SYNC = 1;
	public const int SYNC_ON_COLLIDE = 2;
	public const int SYNC_ON_COLLIDE_OPTIMIZED = 3;

	[SyncVar]
	public int SYNC_EVERY_N_COLLIDE = 10;

	[SyncVar]
	public float STATE_INTERVAL = 0.05f;

	//public Text syncStrategy;

	[SyncVar]
	public int strategy = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isServer) {
			if (Input.GetKeyDown (KeyCode.C)) {
				++strategy;
				if (strategy > SYNC_ON_COLLIDE_OPTIMIZED)
					strategy = 0;
			}
		}

//		if (strategy == ALWAYS_SYNC) {
//			syncStrategy.text = "Always_Synchronize.";
//		} else if (strategy == NON_SYNC) {
//			syncStrategy.text = "Never_Synchronize.";
//		} else if (strategy == SYNC_ON_COLLIDE) {
//			syncStrategy.text = "Synchronize_on_colliding.";
//		} else if (strategy == SYNC_ON_COLLIDE_OPTIMIZED) {
//			syncStrategy.text = "Optimized_Synchronize_on_colliding.";
//		}
	}
}
