using UnityEngine;

public class WalkState : IPlayerState
{
    // Player의 WalkState 행동과 전이 로직을 정의하는 클래스

    public void Enter(NetworkPlayerController player)
    {
        // 상태 진입 시 네트워크 동기화 변수인 State를 Walk로 설정
        player.State = Define.State.Walk;
    }

    public void Exit(NetworkPlayerController player) { }

    public void HandleInput(NetworkPlayerController player, NetworkInputData data)
    {
        // 네트워크 입력 데이터를 분석하여 walk 상태에서의 행동 및 상태 전이를 제어
        // 마우스 이동과 소모품 사용 처리
        player.OnMouseMove(data);
        player.EatCookie(data);

        //우선순위에 따른 상태 전이 제어
        if (player.ToJump(data)) return;
        if (player.ToRun(data)) return;
       
        // 네트워크 입력에 따른 플레이어 이동
        player.Move(data, 1.0f);

        //조건에 따라 idle 상태로 전이
        if (data.IsWDown || data.IsSDown || data.IsADown || data.IsDDown) return;
        player.ChangeState(Define.State.Idle);
    }

    public void Update(NetworkPlayerController player, NetworkInputData data)
    {
        // 지면 상태를 확인하고 스태미나 회복 시뮬레이션을 수행
        player.CheckFloor();

        player.StamainaRegen();
    }

}
