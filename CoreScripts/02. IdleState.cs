using Data;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using static Unity.Collections.Unicode;

public class IdleState : IPlayerState 
{
    // Player의 IdleState 행동과 전이 로직을 정의하는 클래스
    // 정지 상태에서의 자원 회복을 관리하며, 입력 데이터에 따라 다른 행동 상태로의 전이 여부를 판단

    public void Enter(NetworkPlayerController player)
    {
        // 상태 진입 시 네트워크 동기화 변수인 State를 idle로 설정
        player.State = Define.State.Idle;
    }

    public void Exit(NetworkPlayerController player) { }

    public void HandleInput(NetworkPlayerController player, NetworkInputData data)
    {
        // 네트워크 입력 데이터를 분석하여 idle 상태에서의 행동 및 상태 전이를 제어
        player.OnMouseMove(data);
        player.EatCookie(data);

        // 시점 회전과 소모품 사용 로직을 처리한 뒤, 우선순위에 따라 타 상태로의 전이 조건을 검사
        if (player.ToJump(data)) return;
        if (player.ToRun(data)) return;
        if (player.ToWalk(data)) return;
    }

    public void Update(NetworkPlayerController player, NetworkInputData data)
    {
        // 지면 상태를 확인하고 스태미나 회복 시뮬레이션을 수행
        player.CheckFloor();

        player.StamainaRegen();
    }
}