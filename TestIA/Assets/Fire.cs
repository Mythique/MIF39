using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float intensity;
	public float factorEpuisement;
	public Light lightSrc;

	public float wood;

	private float initialInt;
	private Vector3 initScale;
	// Use this for initialization
	void Start () {
		initialInt = intensity;
		initScale = transform.localScale;
		factorEpuisement = 0.01f;
		wood = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (wood > 0) {
			intensity += factorEpuisement*3;
			wood -= factorEpuisement*3;
		}

		intensity -= factorEpuisement;
		if (intensity < 0) {
			transform.localScale = Vector3.zero;
			intensity = 0f;
			lightSrc.intensity = intensity;
			return;
		}
		lightSrc.intensity = intensity;
		transform.localScale = initScale * ((intensity / initialInt)*2);
	}
	
}
