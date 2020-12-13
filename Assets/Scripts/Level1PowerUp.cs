using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1PowerUp : Triggerable
{
    public override void OnTriggered()
    {
        base.OnTriggered();
        var character = GetComponent<CharacterController>();
        character.allowDash = true;
    }
}
