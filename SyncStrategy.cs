//By 2011aad (Jian Zhang) in Aug.2017
//This script defines the specific synchronize strategy, currently it contains 
//4 kinds of strategy for performance comparision. It defines "ALWAYS SYNC" for
//periodically synchronization, "NON_SYNC" for not synchronize, "SYNC_ON_COLLIDE"
//for simply send sync message on collision enter and exit, "SYNC_ON_COLLIDE_OPTIMIZED"
//for an heurestic algorithm to culling continuous colliding.

//This script should be a component of the spawnable gameObject.

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncStrategy : NetworkBehaviour {

	public const int CONTINUOUS_COLLIDING = 0;
	public const int GRAVATIONAL = 1;
	public const int TRANSITION_G2C = 2;
	public const int TRANSITION_C2G = 3;

	private StrategyController sc;
	public int strategy;
	private int last_strategy;

	private int collide_counter = 0;

	private float last_time = 0f;

	private const int STORE_N_STATE = 5;

	[SyncVar]
	private int status = GRAVATIONAL;

	public bool[] stored_state = new bool[STORE_N_STATE];
	private int head = 0;
	private bool collide_occur = false;

	private NetworkTransform trans;

	// Use this for initialization
	void Start () {

		sc = GameObject.Find ("Synchronize Manager").GetComponent<StrategyController>();
		strategy = sc.strategy;

		trans = this.gameObject.GetComponent<NetworkTransform>();

		ResetStrategy ();
	}

	public void ResetStrategy(){
		if (strategy == StrategyController.ALWAYS_SYNC) {
			trans.sendInterval = sc.STATE_INTERVAL;
			trans.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
		} else if (strategy == StrategyController.NON_SYNC) {
			trans.sendInterval = 0;
			trans.transformSyncMode = NetworkTransform.TransformSyncMode.SyncNone;
		} else if (strategy == StrategyController.SYNC_ON_COLLIDE) {
			trans.sendInterval = 0;
			trans.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
			trans.SetDirtyBit (1);
		} else {
			trans.sendInterval = 0;
			trans.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
			trans.SetDirtyBit (1);
			status = GRAVATIONAL;
			collide_counter = 0;
			head = 0;
			for (int i = 0; i < STORE_N_STATE; ++i) {
				stored_state [i] = false;
			}
		}
	}

	
	// Update is called once per frame
	void Update () {
		strategy = sc.strategy;
		if (!isServer)
			return;

		if (strategy != last_strategy) {
			ResetStrategy ();
			last_strategy = strategy;
		}
			
		if (strategy == StrategyController.SYNC_ON_COLLIDE_OPTIMIZED) {
			if (Time.time - last_time > sc.STATE_INTERVAL) {
				if (collide_occur == false && NoneDirty() && status != GRAVATIONAL) {
					trans.sendInterval = 0;
					trans.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
					trans.SetDirtyBit (1);
					status = GRAVATIONAL;
				}
				stored_state [head] = collide_occur;
				++head;
				if (head == STORE_N_STATE)
					head = 0;

				last_time = Time.time;
			}
				
		}

		collide_occur = false;
	}

	public void onCollisionEnter(Collision collision){

		if (!isServer) {
			return;
		}

		collide_occur = true;

		if(strategy==StrategyController.SYNC_ON_COLLIDE)
			trans.SetDirtyBit (1);

		if (strategy == StrategyController.SYNC_ON_COLLIDE_OPTIMIZED) {
			if (status == GRAVATIONAL) {
				trans.sendInterval = 0;
				trans.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
				trans.SetDirtyBit (1);
				status = TRANSITION_G2C;
			} else if (status == TRANSITION_G2C) {
				if (AllDirty ()) {
					status = CONTINUOUS_COLLIDING;
					collide_counter = 0;
				}
			} else if (status == CONTINUOUS_COLLIDING) {
				++collide_counter;
				if (collide_counter == sc.SYNC_EVERY_N_COLLIDE) {
					trans.sendInterval = 0;
					trans.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
					trans.SetDirtyBit (1);
					collide_counter = 0;
				}
			}
		}
	}
		
	public void onCollisionStay(Collision collision){
		if (!isServer) {
			return;
		}

		collide_occur = true;

		++collide_counter;
	}

	public void onCollisionExit(Collision collision){
		if (!isServer)
			return;

		if (strategy == StrategyController.SYNC_ON_COLLIDE) {
			trans.SetDirtyBit (1);
		}
	}

	bool AllDirty(){
		for (int i = 0; i < STORE_N_STATE; ++i) {
			if (stored_state [i] == false)
				return false;
		}
		return true;
	}

	bool NoneDirty(){
		for (int i = 0; i < STORE_N_STATE; ++i) {
			if (stored_state [i] == true)
				return false;
		}
		return true;
	}
}
