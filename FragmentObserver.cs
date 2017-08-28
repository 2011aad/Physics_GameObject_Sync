//By 2011aad (Jian Zhang) in Aug.2017
//Check and add synchronizer for fragments in the fragment poll.


using UnityEngine;
using System.Collections;

public class FragmentObserver : MonoBehaviour {

	public GameObject syncm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		GameObject FragmentParent = GameObject.Find ("PreCutFragments");

		if (FragmentParent != null) {
			foreach (Transform child in FragmentParent.transform) {
				if (child.gameObject.activeSelf && (child.gameObject.GetComponent<FindSynchronizer> () == null)) {
					syncm.GetComponent<SynchronizerManager> ().AddSync (child.gameObject);
				}
			}
		}

		GameObject FragmentRoot = GameObject.Find ("FragmentRoot");

		if (FragmentRoot != null) {
			foreach (Transform child in FragmentRoot.transform) {
				if (child.gameObject.activeSelf && (child.gameObject.GetComponent<FindSynchronizer> () == null)) {
					syncm.GetComponent<SynchronizerManager> ().AddSync (child.gameObject);
				}
			}
		}
	}
}
