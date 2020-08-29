using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	public float timeToExplode = 2.0f;

	public float radius = 5.0F;
	public float power = 10.0F;

	public ParticleSystem explodeParticles;

	// Use this for initialization
	void Start () {
		Invoke ("Explode", timeToExplode);
	}
		
	private void Explode(){
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders)
		{
			Rigidbody rb = hit.GetComponent<Rigidbody>();

			if (rb != null) {
				rb.AddExplosionForce (power, explosionPos, radius, 3.0F,ForceMode.Impulse);
			}
		}

		GetComponent<MeshRenderer> ().enabled = false;
		GetComponent<SphereCollider> ().enabled = false;
		explodeParticles.Play ();
		Destroy (this.gameObject,5);
	}
}
