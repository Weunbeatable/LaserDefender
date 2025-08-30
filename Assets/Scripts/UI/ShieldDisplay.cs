using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDisplay : MonoBehaviour
{
    /// <summary>
    /// This could have been part of health display.... oh well. 
    /// </summary>
    // Start is called before the first frame update
    Player _playerInfo;
    DOTween DOTween;
    [SerializeField] Slider _healthImage;

    // Start is called before the first frame update
    void Start()
    {
        _playerInfo = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float _healthPercentage = _playerInfo._GetPlayer_Shields().GetNormalizedShields();
        _healthImage.value = _healthPercentage;  // turn current health into a % value so it can be used from 0-1 in accordance with fillAmount

    }
}
