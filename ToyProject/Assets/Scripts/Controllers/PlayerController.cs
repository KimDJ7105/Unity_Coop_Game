using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
	[SerializeField]
	float _speed = 10f;
	Vector3 _size = new Vector3(3, 3, 3);

	public override void Init()
    {
		WorldObjectType = Define.WorldObject.Player;

		Managers.Input.KeyAction -= OnKeyboardEvent;
		Managers.Input.KeyAction += OnKeyboardEvent;
	}



	void OnKeyboardEvent()
	{
		if(Input.GetKey(KeyCode.W)) 
		{
			transform.position += transform.forward * Time.deltaTime * _speed;
		}

		if (Input.GetKey(KeyCode.S))
		{
			transform.position -= transform.forward * Time.deltaTime * _speed;
		}

		if (Input.GetKey(KeyCode.A))
		{
			transform.position -= transform.right * Time.deltaTime * _speed;
		}

		if (Input.GetKey(KeyCode.D))
		{
			transform.position += transform.right * Time.deltaTime * _speed;
		}
	}

	public void EatCrystal()
	{
		_size *= 0.98f;
		transform.localScale=_size;
	}

}
