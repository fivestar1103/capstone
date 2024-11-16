using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PlayerAttack : MonoBehaviour
{
    private Renderer renderer;

    public string Spell { get; set; }
    public ESkill Skill { get; set; }
    public EStatusEffect StatusEffect { get; set; }

    public void SetSkill(ESkill _skill, EEmotion _emotion)
    {
        SetSkillType(_skill);
        SetEmotion(_emotion);
    }

    private void Start()
    {
        // Destroy(this.gameObject, lifeTime);
    }
}
