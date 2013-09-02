using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	public Camera topCam;
	public Camera FPCam;
	
	public float hitPoints;
	public GameObject swordSlash;
	public AudioClip slash;
	public AudioClip gunshot;
	public Light flash;
	
	GUIText hitpointsDisplay;
	GUITexture hpbar;

	void Start()
	{
		topCam.enabled = false;
		FPCam.enabled = true;
		hitpointsDisplay = GameObject.Find("HP Display").GetComponent<GUIText>();
		hpbar = GameObject.Find("HPbar").GetComponent<GUITexture>();
	}

	void FixedUpdate () {
	
		if (topCam.enabled)
		{
			Ray mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition);
			//RaycastHit mousePointHit;
			RaycastHit[] allRaycastHits = Physics.RaycastAll(mousePoint);
			
			foreach (RaycastHit h in allRaycastHits)
			{
				if (h.collider.gameObject.name == "Floor")
				{
					Vector3 pos = new Vector3(h.point.x, 1, h.point.z);
					transform.LookAt(pos);
				}
			}
			
			if (Input.GetKey(KeyCode.W))
			{
				rigidbody.AddForce(0, 0, 120);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				rigidbody.AddForce(0, 0, -120);
			}
			
			if (Input.GetKey(KeyCode.A))
			{
				rigidbody.AddForce(-120, 0, 0);	
			}
			else if (Input.GetKey(KeyCode.D))
			{
				rigidbody.AddForce(120, 0, 0);	
			}
		}
		
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (flash.enabled == false)
			{
				flash.enabled = true;
			} else if (flash.enabled == true) {
				flash.enabled = false;
			}
		}
		
		if (FPCam.enabled)
		{
			Screen.lockCursor = true;
			transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
			
			if (Input.GetKey(KeyCode.W))
			{
				rigidbody.AddRelativeForce(0, 0, 120);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				rigidbody.AddRelativeForce(0, 0, -120);
			}
			
			if (Input.GetKey(KeyCode.A))
			{
				rigidbody.AddRelativeForce(-120, 0, 0);	
			}
			else if (Input.GetKey(KeyCode.D))
			{
				rigidbody.AddRelativeForce(120, 0, 0);	
			}
		}
		
		/*if (Physics.Raycast(mousePoint, out mousePointHit, 1000))
		{
			Vector3 pos = new Vector3(mousePointHit.point.x, 1, mousePointHit.point.z);
			
			//Debug.Log(mousePointHit.collider.gameObject.name);
			//GameObject.Find("Mouse").transform.position = mousePointHit.point;
			transform.LookAt(pos);
		}*/

		hitpointsDisplay.text = hitPoints.ToString("0");

		rigidbody.velocity = Vector3.zero;
		
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (topCam.enabled)
			{
				Screen.lockCursor = true;
				topCam.enabled = false;
				FPCam.enabled = true;
			} else {
				Screen.lockCursor = false;
				FPCam.enabled = false;
				topCam.enabled = true;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.E))
		{
			float number = Random.value;
			string zombieName = "";

			if (number > 0.2f)
			{
				zombieName = "Zombie";
			} else {
				zombieName = "Tank";
			}

			GameObject newZombie = Instantiate(Resources.Load(zombieName), transform.position, Quaternion.identity) as GameObject;	
			newZombie.name = "Zombie";
			newZombie.transform.Rotate(0, Random.value * 360, 0);
			newZombie.transform.Translate(0, 0, 60);
		}
		
		if (Input.GetKeyDown(KeyCode.Q))
		{
			string C4Name = "C4";
			
			GameObject newBomb = Instantiate(Resources.Load(C4Name), transform.position, Quaternion.identity) as GameObject;
			newBomb.name = "C4";
			newBomb.transform.Translate(0, -0.5f, 0);
			
			
		}
		
		if (Input.GetKeyDown(KeyCode.V))
		{
			AudioSource.PlayClipAtPoint(slash, transform.position);
			Collider[] hitObject = Physics.OverlapSphere(swordSlash.transform.position, 1);
			foreach (Collider hitObj in hitObject)
			{
				//Debug.Log(hitObj.gameObject);
				if (hitObj.name != "Zombie")
				{
					continue;
				}
				hitObj.gameObject.GetComponent<ZombieScript>().TakeDamage(10, "swordSlash");
				//Instantiate(Resources.Load("Blood"), transform.position, Quaternion.identity);

				//Destroy(hitObj.gameObject);
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button0))
		{
			Debug.Log("BUTTON 0");
		}
		if (Input.GetKeyDown(KeyCode.Joystick1Button8))
		{
			Debug.Log("BUTTON 8");
		}
		if (Input.GetMouseButton(0))
		{
			//AudioSource.PlayClipAtPoint(gunshot, transform.position);
			Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
			Vector3 random = ray.direction;
			random.x += Random.value * 0.1f - 0.05f;
			random.y += Random.value * 0.1f - 0.05f;
			ray.direction = random;

			//Input.mousePosition

			RaycastHit hit;
			Debug.DrawRay(ray.origin, ray.direction * 10);

			if (Physics.Raycast(ray, out hit, 10))
			{
				if (hit.collider.gameObject.name == "Zombie")
				{
					hit.collider.gameObject.GetComponent<ZombieScript>().TakeDamage(1, "");
				}
			}
		}
		
		GameObject.Find("CameraFollow").transform.position = transform.position;
	}

	public void TakeDamage(float damage)
	{
		hitPoints -= damage;
		
		Debug.Log(hitPoints);
		hpbar.texture.width = Mathf.RoundToInt(hitPoints);
	}
}
