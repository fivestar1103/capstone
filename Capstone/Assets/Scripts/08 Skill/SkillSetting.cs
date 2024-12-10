using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerAttack
{
    public Material emotionColor;

    public bool IsDrained { get; private set; }

    public void SetEmotion(EEmotion _emotion)
    {
        // 시각효과 적용을 위해 있는데 뭐 더 넣을 듯
        switch (_emotion)
        {
            case EEmotion.EHappy:
                emotionColor = PlayManager.EmotionMaterials[(int)EEmotion.EHappy];
                break;
            case EEmotion.EAngry:
                emotionColor = PlayManager.EmotionMaterials[(int)EEmotion.EAngry];
                break;
            case EEmotion.ENeutral:
                emotionColor = PlayManager.EmotionMaterials[(int)EEmotion.ENeutral];
                break;
            case EEmotion.EDisgust:
                emotionColor = PlayManager.EmotionMaterials[(int)EEmotion.EDisgust];
                break;
            case EEmotion.EFear:
                emotionColor = PlayManager.EmotionMaterials[(int)EEmotion.EFear];
                break;
            case EEmotion.ESad:
                emotionColor = PlayManager.EmotionMaterials[(int)EEmotion.ESad];
                break;
            case EEmotion.ESurprise:
                emotionColor = PlayManager.EmotionMaterials[(int)EEmotion.ESurprise];
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

