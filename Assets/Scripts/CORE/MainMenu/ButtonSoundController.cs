using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundController : MonoBehaviour
{
    [SerializeField] private List<ButtonCustomBase> _sounds;

    [Header("Sound")]
    private string _soundID = "UIClick";
    private SoundType _soundType = SoundType.UI;


    private void OnEnable()
    {
        foreach (var sound in _sounds)
        {
            sound.OnButtonClick += PlaySound;
        }
    }

    private void OnDisable()
    {
        foreach (var sound in _sounds)
        {
            sound.OnButtonClick -= PlaySound;
        }
    }
    private void PlaySound()
    {
        SystemLinkHolder.Instance.AudioHandler.PlaySound(_soundType, _soundID);
    }
}
