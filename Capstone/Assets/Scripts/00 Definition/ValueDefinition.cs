using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueDefinition
{
    // 태그
    public const string MONSTER_TAG = "Monster";
    public const string MONSTER_ATTACK_TAG = "MonsterAttack";

    public const string PLAYER_TAG = "Player";
    public const string PLAYER_ATTACK_TAG = "PlayerAttack";


    // 레이어
    public const int GROUND_LAYER = 1 << 6;   // 땅 레이어
    public const int INTERACT_LAYER = 1 << 7; // 상호작용 레이어

    public readonly static Vector3 NULL_VECTOR = Vector3.up * 100;      // 아무것도 아닌 벡터
}
