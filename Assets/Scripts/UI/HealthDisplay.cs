using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace LD.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        TextMeshProUGUI _HealthText;
        Player _playerInfo;
        DOTween DOTween;
        [SerializeField] Image _healthImage;
        
        // Start is called before the first frame update
        void Start()
        {
            _HealthText = GetComponent<TextMeshProUGUI>();
            _playerInfo = FindObjectOfType<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            _HealthText.text = _playerInfo._Get_Player_Health().GetNormalizedHealth().ToString();
            float _healthPercentage = _playerInfo._Get_Player_Health().GetNormalizedHealth();
            _healthImage.fillAmount = _healthPercentage;  // turn current health into a % value so it can be used from 0-1 in accordance with fillAmount
            
        }
    }
}