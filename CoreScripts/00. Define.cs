using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff 
{
    /*플레이어의 동적 능력치와 환경 상태 데이터를 관리하는 데이터 컨테이너
    연산 로직과 데이터를 분리, 버프나 환경 변화에 따른 스탯 수정 시 발생할 수 있는
    참조 복잡도를 낮추고 유지보수성을 높이기 위해 설계*/

    // 이동속도 관련
    public float WalkSpeed = 0;
    public float RunSpeed = 0;
    //스테미너 관리 및 소모 비용 데이터
    public float MaxStamina = 0;
    public float RegenStaminaPerSecond = 0;
    public float AdditionalStaminaPerCookie = 0;
    public float NeedJumpStamina = 0;
    public float NeedRunStaminaPerSec = 0;
    //환경 상태 플래그
    public bool IsOnStickyFloor = false;
    public bool IsOnIronFloor = false;
}

public interface IPlayerState
{
    // 상태 패턴(State Pattern) 구현을 위한 인터페이스 정의
    // 모든 상태는 본 인터페이스를 상속받아 로직 계층과 표현 계층을 분리하여 구현

    void Enter(NetworkPlayerController player);

    // FixedUpdateNetwork 틱에 맞춰 실행되는 결정적 로직 시뮬레이션 레이어
    void Update(NetworkPlayerController player, NetworkInputData data);

    // 네트워크 입력 데이터에 따른 상태 전이 및 행위 결정 로직
    void HandleInput(NetworkPlayerController player, NetworkInputData data);

    void Exit(NetworkPlayerController player);
}

public class Define // 프로젝트 전반에서 공통으로 사용되는 전역 상수 및 열거형 데이터를 관리하는 정적 정의 클래스
{
    public const float Epsilon = 0.01f;

	public enum WorldObject
    {
        // 월드 내 객체 식별을 위한 타입 정의
        // 객체 간 상호작용 로직이나 물리 판정 시 필터링 기준으로 활용
        Unknown,
        Player,
        Monster,
    }

	public enum State
	{
        // 플레이어 상태 머신(FSM)에서 사용되는 핵심 상태 식별자
        // IPlayerState 인터페이스를 기반으로 하는 논리 계층의 전이 제어권 확보를 위해 정의
        Walk,
        Run,
		Idle,
        Jump,
        Tired
	}

    public enum Layer
    {
        // 유니티 엔진의 물리 및 렌더링 시스템과 연동되는 레이어 번호 정의
        // 레이캐스트 판정 및 충돌 필터링의 기술적 일관성 유지
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    public enum Scene
    {
        // 게임 전체 흐름을 제어하는 씬 식별자
        // 씬 전환 및 리소스 로딩 시스템의 안정적인 흐름 제어를 위해 사용
        Unknown,
        Login,
        Lobby,
        Game,
        LoadingScene
    }
}
