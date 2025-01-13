using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DropBox : UI_Base
{
    public void PlayerDropSound()
    {
        Managers.SoundManager.PlaySFX("UISounds/BoxDrop");
    }
    public void PlayerVibeSound()
    {
        Managers.SoundManager.PlaySFX("UISounds/BoxVibe");
    }
    public void PlayerOpenSound()
    {
        Managers.SoundManager.PlaySFX("UISounds/BoxOpen");
    }
}
