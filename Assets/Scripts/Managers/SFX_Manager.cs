using LD.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    [SerializeField] AudioSource health_Source;
    [SerializeField] AudioSource shield_Source;
    [SerializeField] AudioSource point_Source;
    private void OnEnable()
    {
        InteractableLogic.OnPlayHealthSound += InteractableLogic_OnPlayHealthSound;
        InteractableLogic.OnPlayShieldSound += InteractableLogic_OnPlayShieldSound;
        InteractableLogic.OnPlayPointsSound += InteractableLogic_OnPlayPointsSound;

    }



    private void OnDisable()
    {
        InteractableLogic.OnPlayHealthSound -= InteractableLogic_OnPlayHealthSound;
        InteractableLogic.OnPlayShieldSound -= InteractableLogic_OnPlayShieldSound;
        InteractableLogic.OnPlayPointsSound -= InteractableLogic_OnPlayPointsSound;
    }

    private void InteractableLogic_OnPlayPointsSound()
    {
       health_Source.Play();
    }

    private void InteractableLogic_OnPlayShieldSound()
    {
        shield_Source.Play();
    }

    private void InteractableLogic_OnPlayHealthSound()
    {
        point_Source.Play();
    }
}
