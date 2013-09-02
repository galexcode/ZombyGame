using UnityEngine;
using System.Collections;

public class ZombieScript : MonoBehaviour {
	
	GameObject player;
	bool aggro;
	
	public float damage;
	public int hitpoints;
	public AudioClip death;

	void Start()
	{
		player = GameObject.Find("Player");
		
		transform.position = new Vector3(transform.position.x, 1, transform.position.z);
	}
	
	void Update()
	{
		Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
		
		/*if (Vector3.Distance(transform.position, player.transform.position) < 20)
		{
			aggro = true;
		} else if (Vector3.Distance(transform.position, player.transform.position) > 50) {
			aggro = false;
		}*/

		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction * 10);

		if (Physics.Raycast(ray, out hit, 10))
		{
			if (hit.collider.gameObject.name == "Player")
			{
				aggro = true;
			} else {
				aggro = false;
			}
		}
	}
	
	void FixedUpdate()
	{
		rigidbody.velocity = Vector3.zero;
		transform.LookAt(player.transform.position);
		
		if (aggro)
		{
			rigidbody.AddRelativeForce(0, 0, Random.value * 150 + 50);
		}
	}

	public void TakeDamage(int dmg, string deathSound)
	{
		hitpoints -= dmg;

		if (hitpoints <= 0)
		{
			if (deathSound == "swordSlash")
			{
				AudioSource.PlayClipAtPoint(death, transform.position, 1);
			}

			Instantiate(Resources.Load("Blood"), transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
	
	void OnCollisionStay(Collision col)
	{
		if (col.collider.gameObject.name == "Player")
		{
			col.collider.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
		}
	}
}
