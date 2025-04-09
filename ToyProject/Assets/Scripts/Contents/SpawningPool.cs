using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    static int id = 0;
    int _id;

    [SerializeField]
    int _monsterCount = 0;
    int _reserveCount = 0;

    [SerializeField]
    int _keepMonsterCount = 0;

    [SerializeField]
	public Vector3 _spawnPos { get; set; }
    [SerializeField]
	public float _spawnRadius { get; set; }
    [SerializeField]
    public float _spawnTime { get; set; } = 10f;

	public void AddMonsterCount(int value,int inputId) { if(inputId==_id)_monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    void Start()
    {
        _id = id;
        id++;
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Crystal, "Crystal",spawnerId:_id);

        Vector3 randPos;    
        Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
		randDir.y = 0;
		randPos = _spawnPos + randDir;

        obj.transform.position = randPos;
        _reserveCount--;
    }
}
