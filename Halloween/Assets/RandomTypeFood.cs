using UnityEngine;
using System.Collections;

public class RandomTypeFood : MonoBehaviour {

	public Texture[] textures;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Renderer>().material.mainTexture = textures[Mathf.FloorToInt(Random.value * textures.Length)];
	}
}
