using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoState_Idle : RinoState_Base
{
    public override void Init(Boss_Rino rino)
    {
        base.Init(rino);
    }

    public override void Enter()
    {
        _rinoAnim.Play("Rino_Idle");
    }

    public override void FixedUpdate()
    {

    }
    public override void Update()
    {

    }
    public override void LateUpdate()
    {

    }
    public override void Exit()
    {

    }

}
