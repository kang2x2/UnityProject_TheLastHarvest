using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum SceneType
    {
        TitleScene,
        GameScene,
        End
    }

    public enum MapType
    {
        Field,
        Cave,
        End
    }

    public enum CharacterType
    {
        Character1,
        Character2,
        Character3,
        Character4,
        End
    }

    public enum UIEvent
    {
        Click,
        Drag,
        End
    }

    public enum ItemType
    {
        Gun, // �⺻ ����
        Shovel,
        Scythe,
        Trident,
        Shotgun,
        Thompson,

        Shoose,
        Margent,
        PowerCore,
        BoxUpgrade,
        ExpBoost,
        HealthPack,

        End
    }
    public enum AbilityType
    {
        Speed,
        CoolTime,
        Fen,
        Amount,
        Attack,
        Size,
        Init, // ������ ���� ȹ��
        FloatUtil, // �нú� �� ���������� ���� �ɷ�ġ
        IntUtil, // �нú� �� ���������� ���� �ɷ�ġ
        End
    }

    public enum MonsterType
    {
        // Field
        Goombella,
        Slime,
        Snake,
        Dino,
        Goblin,

        // Cave
        WeakZombie,
        Zombie,
        CaveSlime,
        Skeleton,
        DevilStone,

        End
    }
}
