using System.Collections;
using UnityEngine;

public class TiredState : IPlayerState
{
    // Player의 TiredState 행동과 전이 로직을 정의하는 클래스

    NetworkPlayerController _player;
    const float _TiredTimer = 3f;

    public void Enter(NetworkPlayerController player)
    {
        // 상태 진입 시 네트워크 동기화 변수인 State를 Tired로 설정
        player.State = Define.State.Tired;

        _player = player;
        // 상태 전이를 위한 타이머 설정
        CoroutineRunner.Instance.Run(TiredCouroutine());
    }

    public void Exit(NetworkPlayerController player) { }
    public void HandleInput(NetworkPlayerController player, NetworkInputData data) { }
    public void Update(NetworkPlayerController player, NetworkInputData data) { }

    IEnumerator TiredCouroutine()
    {
        // 클라이언트가 InputAuthority가 있다면 애니메이션 실행
        if (_player.HasInputAuthority) Managers.Scene.CurrentScene.GetComponent<GameScene>().animator.SetTrigger("Tired");

        // 지정된 시간동안 스테미너 고정
        yield return new WaitForSeconds(_TiredTimer);
        _player.Stamina = 0.1f;
        // 타이머 조건에 따른 상태 전이
        _player.ChangeState(Define.State.Idle);

    }
}
