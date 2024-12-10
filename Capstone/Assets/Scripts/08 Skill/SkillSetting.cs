using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerAttack
{
    [SerializeField]
    protected Material[] emotionMaterials;
    public Material emotionColor;

    public bool IsDrained { get; private set; }

    public void SetEmotion(EEmotion _emotion)
    {
        // 시각효과 적용을 위해 있는데 뭐 더 넣을 듯
        switch (_emotion)
        {
            case EEmotion.EHappy:
                emotionColor = emotionMaterials[(int)EEmotion.EHappy];
                break;
            case EEmotion.EAngry:
                emotionColor = emotionMaterials[(int)EEmotion.EAngry];
                break;
            case EEmotion.ENeutral:
                emotionColor = emotionMaterials[(int)EEmotion.ENeutral];
                break;
            case EEmotion.EDisgust:
                emotionColor = emotionMaterials[(int)EEmotion.EDisgust];
                break;
            case EEmotion.EFear:
                emotionColor = emotionMaterials[(int)EEmotion.EFear];
                break;
            case EEmotion.ESad:
                emotionColor = emotionMaterials[(int)EEmotion.ESad];
                break;
            case EEmotion.ESurprise:
                emotionColor = emotionMaterials[(int)EEmotion.ESurprise];
                break;
            default: 
                return;
        }
        StatusEffect = (EStatusEffect)_emotion;
        PlayManager.SetEmotionColor(emotionColor);
    }

    public void SetSkillType(ESkill _skill)
    {
        Skill = _skill;
    }
}

