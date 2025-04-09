using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.UIElements;

public class CrystalController : BaseController
{
    public int _spawnerId;
    float _speed = 30.0f;

	public override void Init()
	{
		WorldObjectType = Define.WorldObject.Crystal;
	}

	void Update()
    {
		transform.Rotate(Vector3.up * Time.deltaTime * _speed, Space.World);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == Managers.Game.GetPlayer())
		{
			Managers.Game.GetPlayer().GetComponent<PlayerController>().EatCrystal();
			Managers.Game.Despawn(gameObject);
		}
	}
}
