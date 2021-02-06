using UnityEngine;
using Ink.Runtime;
using TMPro;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private TextAsset _inkAsset = default;

    //[SerializeField] private TextMeshProUGUI _lineText = default;

    private Story _inkStory = default;

    private bool _waitinForChoice = false;

    void Awake()
    {
        _inkStory = new Story(_inkAsset.text);
    }

    void Update()
    {
        if (_waitinForChoice)
        {
            return;
        }

        while (_inkStory.canContinue)
        {
            //_lineText.text += _inkStory.Continue() + "\n";
            Debug.Log(_inkStory.Continue());
        }
        
        if (_inkStory.currentChoices.Count > 0 ) 
        {
            for (int i = 0; i < _inkStory.currentChoices.Count; ++i)
            {
                Choice choice = _inkStory.currentChoices[i];
                //_lineText.text += $"Choice {i + 1}. {choice.text}\n";
                Debug.Log($"Choice {i + 1}. {choice.text}");
            }
            
            _waitinForChoice = true;
        }
    }
}