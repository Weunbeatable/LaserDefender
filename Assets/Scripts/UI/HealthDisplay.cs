using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LD.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        TextMeshProUGUI _HealthText;
        Player _playerInfo;
        // Start is called before the first frame update
        void Start()
        {
            _HealthText = GetComponent<TextMeshProUGUI>();
            _playerInfo = FindObjectOfType<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            _HealthText.text = _playerInfo.GetHealth().ToString();
           
        }
    }
}