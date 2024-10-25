using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State { Idle, Key, Rot, Dash, Red, Dead, Size } // 상태 종류
    [SerializeField] State curState = State.Idle;
    private BaseState[] states = new BaseState[(int)State.Size];

    [SerializeField] GameObject[] games;      // 상태 마다 활성화 시킬 게임오브젝트 종류
    // [0] Key, [1] Rot, [2] Dash, [3] Red, [4] Dead

    private void Awake()
    {
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Key] = new KeyState(this);
        states[(int)State.Rot] = new RotState(this);
        states[(int)State.Dash] = new DashState(this);
        states[(int)State.Red] = new RedState(this);
        states[(int)State.Dead] = new DeadState(this);
    }

    private void Start()
    {
        states[(int)curState].Enter();
        games[0].SetActive(false);
        games[1].SetActive(false);
        games[2].SetActive(false);
        games[3].SetActive(false);
        games[4].SetActive(false);

    }

    private void OnDestroy()
    {
        states[(int)curState].Exit();
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    public void ChangeState(State nextState)
    {
        if (curState != nextState)
        {
            states[(int)curState].Exit();
            curState = nextState;
            states[(int)curState].Enter();
        }
    }

    private class PlayerState : BaseState
    {
        public PlayerController player;

        public PlayerState(PlayerController player)
        {
            this.player = player;
        }
    }

    private class IdleState : PlayerState
    {
        public IdleState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
                //bot.ChangeState(State.Attack);
        }
    }

    private class KeyState : PlayerState
    {
        public KeyState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            player.games[0].SetActive(true);
            Debug.Log("활성");
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            player.games[0].SetActive(false);
            Debug.Log("비활성");
        }
    }

    private class RotState : PlayerState
    {
        public RotState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            player.games[1].SetActive(true);
            Debug.Log("롤활성");
        }

        public override void Update()
        {
            
        }
        public override void Exit()
        {
            player.games[1].SetActive(false);
            Debug.Log("롤비활성");
        }
    }

    private class DashState : PlayerState
    {
        public DashState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            
        }
    }

    private class RedState : PlayerState
    {
        public RedState(PlayerController player) : base(player)
        {
        }


    }


    private class DeadState : PlayerState
    {
        public DeadState(PlayerController bot) : base(bot)
        {
        }
        public override void Enter()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {

        }
    }

    public void GetKey()
    {
        ChangeState(State.Key);
    }

    public void GetRed()
    {
        ChangeState(State.Rot);
    }
}
