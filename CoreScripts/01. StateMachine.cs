/*
플레이어의 상태 전이를 관리하고 현재 활성화된 상태의 생명주기를 제어하는 Finite State Machine 클래스
컨트롤러와 구체적인 상태 로직을 분리하여 시스템의 결합도를 낮추고, 
새로운 상태 추가 시 기존 코드를 수정할 필요가 없는 개방 폐쇄 원칙을 준수
 */

public class StateMachine
{
    public IPlayerState CurrentState { get; private set; } // 현재 플레이어가 처해 있는 활성화된 상태 객체

    public void Initialize(IPlayerState startingState, NetworkPlayerController player)
    {
        // 초기 상태를 설정하고 해당 상태의 진입 로직을 실행
        CurrentState = startingState;
        CurrentState.Enter(player);
    }

    public void ChangeState(IPlayerState newState, NetworkPlayerController player)
    {
        // 상태 전환을 처리하는 핵심 메서드, 상태 간의 연결 관계를 관리
        // 동일한 상태로의 중복 전이 요청을 방지하는 Guard Clause를 통해 불필요한 초기화 연산을 제거
        if (CurrentState == newState) return; 

        CurrentState?.Exit(player);
        CurrentState = newState;
        CurrentState.Enter(player);
    }

    public void Update(NetworkPlayerController player, NetworkInputData data)
    {
        // 매 틱마다 현재 상태의 물리 연산 및 논리 업데이트를 수행
        CurrentState?.Update(player, data);
    }

    public void HandleInput(NetworkPlayerController player, NetworkInputData data)
    {
        // 네트워크 입력 데이터를 현재 상태에 전달하여 입력에 따른 행위 결정 및 전이 여부를 판단
        // 입력 처리 로직을 상태별로 캡슐화
        CurrentState?.HandleInput(player, data);
    }
}