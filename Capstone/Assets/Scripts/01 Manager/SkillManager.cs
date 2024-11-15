using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    private string[] spells = { ValueDefinition.SPELL1, ValueDefinition.SPELL2, ValueDefinition.SPELL3 };

    public PlayerAttack[] Skills = new PlayerAttack[(int)ESkill.LAST];

    private void SetSkillSpell()
    {
        for(int i = 0; i < Skills.Length; i++)
        {
            Skills[i].Spell = spells[i];
            // Debug.Log(skills[i].Spell);
        }
    }

    public void SetManager()
    {
        SetSkillSpell();
    }
}
