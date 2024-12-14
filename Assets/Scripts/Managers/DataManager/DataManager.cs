using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public CharacterData Character { get; private set; } = new CharacterData();
    public SoundData Sound { get; private set; } = new SoundData();
    public StoreData Store { get; private set; } = new StoreData();
    public UserData User { get; private set; } = new UserData();

    public void Init()
    {
        Character.Init();
        Sound.Init();
        Store.Init();
        User.Init();
    }

    public void DataReset()
    {
        Character.DataReset();
        Store.DataReset();
        User.DataReset();
    }
}
