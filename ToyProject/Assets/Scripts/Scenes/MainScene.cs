using UnityEngine;

public class MainScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Define.Scene.Main;
		//Managers.UI.ShowSceneUI<UI_Inven>();
		GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
		Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

		for(int i = 1; i <= 5; ++i)
		{
			
			GameObject Floor = Managers.Resource.Instantiate("Floor");
			Floor.transform.position = new Vector3(0, -5 * i, 0);
			Floor.transform.localScale = new Vector3(6 - i, 1, 6 - i);
			Floor.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
			
			
			GameObject go = new GameObject { name = "SpawningPool" };
			SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
			pool.SetKeepMonsterCount((6 - i) *3);
			pool._spawnPos=Floor.transform.position;
			pool._spawnRadius= ((6 - i) * 3);

		}

		


	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public override void Clear()
	{
		Debug.Log("MainScene Clear");
	}
}
