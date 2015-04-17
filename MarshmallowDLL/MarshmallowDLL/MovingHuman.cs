using UnityEngine;
using System.Collections;

namespace MarshmallowDLL
{
	public class MovingHuman : MonoBehaviour
	{
		GameObject fire;
		GameObject tree;
		GameObject wood;
		bool goToTree;
		bool hasWood;
		float proximity = 1.8f;
		// Use this for initialization
		void Start ()
		{
			fire = GameObject.FindGameObjectWithTag ("Fire"); //[AJOUT]A remplacer par "DEMANDER AU SERVEUR LA POSTION D'UN FEU"
			tree = GameObject.FindGameObjectWithTag ("Tree");
			wood = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			wood.transform.SetParent (transform);
			wood.transform.position = new Vector3 (0f, 0.644f, 0.207f);
			wood.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			wood.transform.Rotate (new Vector3 (90f, 0f, 90f));
			wood.transform.GetComponent<MeshRenderer> ().enabled = false;
			goToTree = false;
			hasWood = false;
		}

		public void UpdateDistance (float delta)
		{
			Debug.Log ("Marc updating distance with delta : " + delta);

			fire = GameObject.FindGameObjectWithTag ("Fire");
			tree = GameObject.FindGameObjectWithTag ("Tree");


			if (delta == 0) {
				return;
			}

			if ((Vector3.Distance (fire.transform.position, transform.position) <= proximity) && (fire.GetComponent<Fire> ().intensity < 1)) {
				//MODE MOVE TO TREE
				Debug.Log (fire.GetComponent<Fire> ().intensity);
				goToTree = true;
			}

			Debug.Log ("Tree located at : " + tree.transform.position);

			if (goToTree) {
				Debug.Log ("Moving to tree");
				transform.position = Vector3.MoveTowards (transform.position, tree.transform.position, 0.1f);
				transform.LookAt (tree.transform.position);
				if (Vector3.Distance (tree.transform.position, transform.position) <= 0.9f) {
					Debug.Log ("Wood got");
					hasWood = true;
					goToTree = false;
					wood.transform.GetComponent<MeshRenderer> ().enabled = true;
				}
				return;
			}

			if (hasWood) {
				Debug.Log ("Have wood going to fire");
				transform.position = Vector3.MoveTowards (transform.position, fire.transform.position, 0.1f);
				transform.LookAt (fire.transform.position);
				if (Vector3.Distance (fire.transform.position, transform.position) <= proximity) {
					Debug.Log ("More wood added");
					hasWood = false;
					fire.GetComponent<Fire> ().wood += 3;
					wood.transform.GetComponent<MeshRenderer> ().enabled = false;
				}
				return;
			}

			if (!double.IsNaN (delta)) {
				Debug.Log ("Cooking");
				//Debug.Log ("translatttttion of " + delta);
				if (transform.position == fire.transform.position) {
					transform.Translate (transform.forward * Mathf.Min (-delta, 0));
					return;
				}
				transform.position = Vector3.MoveTowards (transform.position, fire.transform.position, -delta);
				transform.LookAt (fire.transform.position);
			}
		}
	}
}
