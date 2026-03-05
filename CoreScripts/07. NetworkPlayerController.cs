public class NetworkPlayerController : NetworkBehaviour
{
    // 플레이어의 네트워크 동기화 및 상태 머신 구동을 담당하는 핵심 컨트롤러 클래스

    // 플레이어 제어를 위한 상태 머신 및 능력치 관리 변수
    public StateMachine _stateMachine { get; private set; }
    public Data.PlayerStat _PlayerStat;
    public PlayerBuff playetBuff = new PlayerBuff();
    public Dictionary<Define.State, IPlayerState> _states = new Dictionary<Define.State, IPlayerState>();

    // 네트워크 동기화가 필요한 상태 변수
    [Networked] public float Stamina { get; set; }
    [Networked] public Define.State State { get; set; }

    // 캐릭터 컨트롤러 컴포넌트 및 이전 프레임 상태 기록용 변수
    public NetworkCharacterController _cc;
    public Define.State _prevState;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _stateMachine = new StateMachine();
    }

    public override void Spawned()
    {
        // Define.State 열거형을 순회하며 리플렉션을 통해 모든 상태 클래스를 자동 등록
        // 새로운 상태가 추가되어도 본 컨트롤러 클래스의 수정 없이 유연한 확장이 가능함
        foreach (Define.State s in Enum.GetValues(typeof(Define.State)))
        {
            string className = $"{s}State";
            Type type = Type.GetType(className);

            if (type != null && typeof(IPlayerState).IsAssignableFrom(type))
            {
                IPlayerState stateInstance = (IPlayerState)Activator.CreateInstance(type);
                _states[s] = stateInstance;
            }
        }

        // 원본 데이터 보호를 위한 스탯 딥카피 및 유휴 상태로 초기 초기화 수행
        _PlayerStat = Managers.Data.PlayerStat.DeepCopy();
        _stateMachine.Initialize(_states[Define.State.Idle], this);

        State = Define.State.Idle;
    }

    public override void FixedUpdateNetwork()
    {
        // 상태 머신을 통해 현재 상태의 입력 처리와 시뮬레이션 로직을 실행

        if (GetInput(out NetworkInputData data))
        {
            _stateMachine.HandleInput(this, data);
            _stateMachine.Update(this, data);
        }
    }

    public override void Render()
    {
        // 시뮬레이션 로직과 표현 계층을 분리하여 네트워크 지연 상황에서도 부드러운 연출 보장
        if (_prevState != State)
        {
            UpdateAnimation(State);
            _prevState = State;
        }
    }

    public void ChangeState(Define.State state)
    {
        // 외부 로직에서 특정 상태로 강제 전이를 요청할 때 사용
        _stateMachine.ChangeState(_states[state], this);
    }

    public void UpdateAnimation(Define.State newState)
    {
        // 현재 상태 식별자에 대응하는 애니메이션을 재생

        Animator anim = GetComponent<Animator>();
        switch (newState)
        {
            case Define.State.Idle: anim.CrossFade("IDLE", 0.1f); break;
            case Define.State.Walk: anim.CrossFade("WALK", 0.1f); break;
            case Define.State.Run: anim.CrossFade("RUN", 0.1f); break;
            case Define.State.Jump: anim.CrossFade("JUMP", 0.1f); break;
            case Define.State.Tired: anim.CrossFade("TIRED", 0.1f); break;
        }
    }

    public bool ToJump(NetworkInputData data)
    {
        // 점프 가능 여부를 판단하고 조건 충족 시 점프 상태로 전이함

        if (_stateMachine.CurrentState is JumpState) return false;
        if (playetBuff.IsOnStickyFloor) return false;
        if (!data.IsSpaceDown) return false;

        _stateMachine.ChangeState(_states[Define.State.Jump], this);
        return true;
    }

    public bool ToWalk(NetworkInputData data)
    {
        // 걷기 상태 전이 조건을 확인하며, 이동 입력이 없을 경우 Idle 상태로 전환함

        if (data.IsShiftDown) return false;

        Vector3 dir = Vector3.zero;
        if (data.IsWDown) dir += _cc.transform.forward;
        if (data.IsSDown) dir -= _cc.transform.forward;
        if (data.IsADown) dir -= _cc.transform.right;
        if (data.IsDDown) dir += _cc.transform.right;

        if (dir.magnitude < 0.01f)
        {
            _stateMachine.ChangeState(_states[Define.State.Idle], this);
            return true;
        }

        _stateMachine.ChangeState(_states[Define.State.Walk], this);
        return true;
    }

    public bool ToRun(NetworkInputData data)
    {
        // 달리기 상태 진입 조건을 확인하고 상태를 전이함

        if (!data.IsShiftDown) return false;

        Vector3 dir = Vector3.zero;
        if (data.IsWDown) dir += _cc.transform.forward;
        if (data.IsSDown) dir -= _cc.transform.forward;
        if (data.IsADown) dir -= _cc.transform.right;
        if (data.IsDDown) dir += _cc.transform.right;

        if (dir.magnitude < 0.01f) return false;

        _stateMachine.ChangeState(_states[Define.State.Run], this);
        return true;
    }

    public bool Move(NetworkInputData data, float mul)
    {
        // 각 이동 상태에서 호출되는 물리 이동 함수
        // 상태별 속도 가중치를 적용, 캐릭터 컨트롤러의 이동을 제어

        Vector3 dir = Vector3.zero;
        if (data.IsWDown) dir += _cc.transform.forward;
        if (data.IsSDown) dir -= _cc.transform.forward;
        if (data.IsADown) dir -= _cc.transform.right;
        if (data.IsDDown) dir += _cc.transform.right;

        if (dir.magnitude < 0.01f) return false;

        _cc.maxSpeed = _PlayerStat.WalkSpeed * mul;
        _cc.Move(dir.normalized);

        return true;
    }
}