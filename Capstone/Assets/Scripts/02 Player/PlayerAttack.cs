using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 1.5f;

    public ESkill skill { get; set; }
    public ECCType ccType { get; set; }

    public void SetSkill(ESkill _skill, EEmotion _emotion)
    {
        SetSkillType(_skill);
        SetEmotion(_emotion);
    }

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
