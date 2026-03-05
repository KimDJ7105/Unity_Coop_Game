using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.AI;
using static Unity.Collections.Unicode;
using Fusion;

public class JumpState : IPlayerState
{
    // Player의 JumpState 행동과 전이 로직을 정의하는 클래스

    NetworkPlayerController _player;
    const float _JumpDelay = 2f;

    public void Enter(NetworkPlayerController player)
    {
        // 상태 진입 시 네트워크 동기화 변수인 State를 Jump로 설정
        player.State = Define.State.Jump;

        // 점프에 필요한 스테미너 소모
        _player = player;
        player.Stamina -= player.NeedJumpStamina();
        // 상태 전이를 위한 타이머 설정
        CoroutineRunner.Instance.Run(JumpTimer());
    }

    public void Exit(NetworkPlayerController player) { }

    public void HandleInput(NetworkPlayerController player, NetworkInputData data)
    {
        // 네트워크 입력 데이터를 분석하여 jump 상태에서의 행동 및 상태 전이를 제어
        // 마우스 이동과 소모품 사용 처리
        player.OnMouseMove(data);
        player.EatCookie(data);

        // 네트워크 입력에 따른 플레이어 이동
        player.Move(data, 1.0f);
    }

    public void Update(NetworkPlayerController player, NetworkInputData data)
    {
        // 지면의 상태를 확인
        player.CheckFloor();
    }

    IEnumerator JumpTimer()
    {
        // 타이머 조건에 따라 상태 전이
        yield return new WaitForSeconds(_JumpDelay);

        // 스테미너 조건에 따라 상태 전이
        if (_player.Stamina < 0)
        {
            _player.ChangeState(Define.State.Tired);
        }
        else
        {
            _player.ChangeState(Define.State.Idle);

        }
    }

}
