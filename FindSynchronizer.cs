//By 2011aad (Jian Zhang) in Aug.2017
//Synchronized fragment should have this component to find its synchronizer
//and synchronize engine use this component to check whether or not this fragment
//has a synchronizer.

//When the fragment detects a collision, it will direct the information to its synchronizer.


using UnityEngine;
using System.Collections;

public class FindSynchronizer : MonoBehaviour {

	private GameObject synchronizer;
	private SyncStrategy sc;

	// Use this for initialization
	void Start () {
		if (synchronizer)
			sc =  synchronizer.GetComponent<SyncStrategy> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	void OnCollisionStay(Collision collision){
//		if(sc!=null)
//			sc.onCollisionStay (collision);
//	}

	void OnCollisionExit(Collision collision){
		if(sc!=null)
			sc.onCollisionExit (collision);
	}

	void OnCollisionEnter(Collision collision){
		if(sc!=null)
			sc.onCollisionEnter (collision);
	}

	public void setSynchronizer(GameObject go){
		synchronizer = go;
	}

	public GameObject GetSynchronizer(){
		return synchronizer;
	}

}
