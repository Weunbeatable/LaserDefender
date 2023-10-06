using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    TextMeshProUGUI HealthText;
    Player playerInfo;
    // Start is called before the first frame update
    void Start()
    {
        HealthText= GetComponent<TextMeshProUGUI>();
       playerInfo = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthText.text = playerInfo.GetHealth().ToString();
        Debug.Log(HealthText);
    }
}
