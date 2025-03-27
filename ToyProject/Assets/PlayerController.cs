using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : NetworkBehaviour
{
	
	public Canvas networkUI;
	public Rigidbody rb;
	float speed = 10.0f;
	void Start()
	{
		
	}
	
	private void Awake()
	{
		rb=GetComponent<Rigidbody>();
		rb.freezeRotation = true;
	}
	void FixedUpdate()
	{

		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
		rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
	}

	void Update()
    {
		if (!IsOwner)
			return;
	
		networkUI = GameObject.Find("Canvas").GetComponent<Canvas>();
		if (Input.GetKeyDown(KeyCode.Q)) {

			if (networkUI.enabled)
				networkUI.enabled = false;
			else
				networkUI.enabled=true;
		}
		
	}
}




	
	