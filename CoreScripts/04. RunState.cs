using Data;
using UnityEngine;

public class RunState : IPlayerState
{
    // Player의 RunState 행동과 전이 로직을 정의하는 클래스

    public void Enter(NetworkPlayerController player)
    {
        // 상태 진입 시 네트워크 동기화 변수인 State를 Run으로 설정
        player.State = Define.State.Run;
    }

    public void Exit(NetworkPlayerController player) { }

    public void HandleInput(NetworkPlayerController player, NetworkInputData data)
    {
        // 네트워크 입력 데이터를 분석하여 run 상태에서의 행동 및 상태 전이를 제어
        // 마우스 이동과 소모품 사용 처리
        player.OnMouseMove(data);
        player.EatCookie(data);

        // 우선순위에 따른 상태 전이 제어
        if (player.ToJump(data)) return;
        if (player.ToWalk(data)) return;
        
        // 플레이어의 현 상태를 적용해 이동
        var stat = player._PlayerStat;
        if (player.Move(data, stat.RunSpeed / stat.WalkSpeed)) return;

    }

    public void Update(NetworkPlayerController player, NetworkInputData data)
    {
        // 지면의 상태를 확인
        player.CheckFloor();

        if (player.playetBuff.IsOnStickyFloor) // 지면에 따라 상태 전이
            player.ChangeState(Define.State.Walk);

        // 스테미너 소모 관리
        player.Stamina -= player.NeedRunStamina();

        //조건에 따른 상태 전이
        if (player.Stamina <= 0)
        {
            player.ChangeState(Define.State.Tired);
            return;
        }

        //지면에 따라 소음 조절
        if ((player.playetBuff.IsOnIronFloor))
        {
            float noiseRange = player._PlayerStat.RunNoiseRange;
            noiseRange = player._PlayerStat.RunNoiseRange * 2f; // 철판은 두 배 소음
        }
    }

}
