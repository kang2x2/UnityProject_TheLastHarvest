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

    public enum GameOverType
    {
        Clear,
        Dead,
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
        Weapon,
        Passive,
        Consumption,
        End
    }

    public enum ItemName
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
        MaxHp,
        Recovery,
        CriticalUp,

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
        Consumption, // �Һ� ������

        End
    }

    public enum UserStatType
    {
        Attack,
        Exp,
        MoveSpeed,
        MaxHP,
        Recovery,
        Critical,
        SelectCount,
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

    public enum FlyMonsterType
    {
        Bird,
        Ghost,
        End
    }

    public enum DamageType
    {
        Normal,
        Critical,
        End
    }

    public enum RankingType
    {
        Kill,
        Clear,
        End
    }
}
