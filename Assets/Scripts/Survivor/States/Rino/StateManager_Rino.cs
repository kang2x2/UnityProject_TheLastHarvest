using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_Rino
{
    public enum RinoState
    {
        Idle,
        Run,
        Break,
        End
    }

    RinoState_Base[] _states = new RinoState_Base[(int)RinoState.End];
    public RinoState_Base _curState;

    public void Init(Boss_Rino rino)
    {
        _states[(int)RinoState.Idle] = new RinoState_Idle();
        _states[(int)RinoState.Run] = new RinoState_Run();
        _states[(int)RinoState.Break] = new RinoState_Break();

        foreach(RinoState_Base state in _states)
        {
            state.Init(rino);
        }

        _curState = _states[(int)RinoState.Idle];
    }

    public void FixedUpdate()
    {
        _curState.FixedUpdate();
    }
    public void Update()
    {
        _curState.Update();
    }
    public void LateUpdate()
    {
        _curState.LateUpdate();
    }

    public void ChangeState(RinoState state)
    {
        if(_curState != null)
        {
            _curState.Exit();
        }
        _curState = _states[(int)state];
        _curState.Enter();
    }
}
