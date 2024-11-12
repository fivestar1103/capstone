using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerAttack
{ 
    public void SetEmotion(EEmotion _emotion)
    {
        // 감정과 ccType을 일치
        ccType = (ECCType)_emotion;

        // 시각효과 적용을 위해 있는데 뭐 더 넣을 듯
        switch(_emotion)
        {
            case EEmotion.EHappy:

                break;
            case EEmotion.EAngry:

                break;
            case EEmotion.EDisgust:

                break;
            case EEmotion.EFear:

                break;
            case EEmotion.ENeutral:

                break;
            case EEmotion.ESad:

                break;
            case EEmotion.ESurprise:

                break;
            default: 
                return;
        }
    }

    public void SetSkillType(ESkill _skill)
    {
        skill = _skill;
    }
}
