using UnityEngine;

public class WhisperPresenter : MonoBehaviour
{
    [SerializeField] private WhisperModel whisperModel; // Reference to WhisperModel component
    private string[] spells = new string[] { ValueDefinition.SPELL1, ValueDefinition.SPELL2, ValueDefinition.SPELL3 };
    private float accuracy;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!whisperModel) whisperModel = GetComponent<WhisperModel>(); // Get WhisperModel component if not present
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            whisperModel.StartRecording(); // Start recording when the left control key is pressed
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            ProcessVoiceInput(); // Process voice input when the left control key is released
        }
    }

    private void CheckSpell(string _spell)
    {
        int matchingCount = 0;

        foreach(string spell in spells)
        {
            if (_spell.Length != spell.Length)
                return;

            for(int i = 0; i < spell.Length; i++)
            {
                if (_spell[i] == spell[i])
                    matchingCount++;
                
                accuracy = matchingCount / spell.Length;
            }
            if (accuracy > 0.7f)
            {
                _spell = spell;
                break;
            }
        }
        PlayManager.PrepareSkill(_spell, EEmotion.EAngry);
    } 

    private async void ProcessVoiceInput()
    {
        var result = await whisperModel.StopRecording(); // Stop recording and start transcription
        Debug.Log($"{result}"); // Output the result
        CheckSpell(result);
    }
}