using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log("Enter PlayerJoined Func");

        if (player == Runner.LocalPlayer)
        {
            Debug.Log("Enter Spawn");
            Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
}