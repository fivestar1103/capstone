using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectScript : MonoBehaviour, IHittable
{
    [SerializeField]
    private float curHP;
    [SerializeField]
    private float attack;
    [SerializeField]
    private float curSpeed;


    public float CurHP
    {
        get { return curHP; }                        // 현재 HP
        protected set { curHP = value; }
    }
    public float Attack
    {
        get { return attack; }                        // 현재 공격력
        protected set { attack = value; }
    }
    public virtual float CurSpeed
    {
        get { return curSpeed; }          // 현재 속도
        protected set { curSpeed = value; }
    }


    public bool IsDead { get; protected set; }                          // 죽음 상태
    public virtual bool IsUnstoppable { get; } = true;                  // 히트 상태 가능 여부

    public virtual bool IsPlayer { get { return false; } }      
    public virtual bool IsMonster { get { return false; } }     

    public virtual void GetHit(Collider _hit)
    {

    }
}
