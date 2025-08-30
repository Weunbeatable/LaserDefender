using LD.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LD.Core
{
    public class InteractableLogic : MonoBehaviour, IInteractable
    {
        public static event Action OnPlayHealthSound;
        public static event Action OnPlayShieldSound;
        public static event Action OnPlayPointsSound;
        public enum dropType
        {
            health = 0,
            shield = 1,
            points = 2, // affect the total score. 
        }

        [SerializeField] dropType _pickup;

        [SerializeField] int value_to_Add; // this value will be preassigned to the object. 
                                           // it will be added to appropriate component on the player or score value. 
                                           // incrementing it to that appropriate script. 
                                           // on collision should get references to player or points.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            _Check_Interaction_Type(collision);

        }

        private void _Check_Interaction_Type(Collider2D collision)
        {
            switch ((int)_pickup)
            {
                case 0:
                    if (collision.tag == "Player")
                    {
                        if (collision.TryGetComponent<Health>(out Health _healthComponent))
                        {
                            OnPlayHealthSound?.Invoke();
                            _healthComponent.HealHealth(value_to_Add);
                            Destroy(gameObject);
                        }
                    }
                    break;
                case 1:
                    if (collision.tag == "Player")
                    {
                        if (collision.GetComponentInChildren<Shields>(true))
                        {
                            // if it exists on a child component add the appropriate shields. 
                            OnPlayShieldSound?.Invoke();
                            var Buff = collision.GetComponentInChildren<Shields>();
                            Buff.HealShields(value_to_Add);
                            Destroy(gameObject);
                        }
                    }
                    // write logic for others that aren't shields. 
                    break;
                case 2:
                    if (collision.tag == "Player")
                    {
                        OnPlayPointsSound?.Invoke();
                        FindObjectOfType<GameSession>().Add_To_Score(value_to_Add);
                        Destroy(gameObject);
                    }
                    break;
            }
        }

        public void Use()
        {
            // REAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALLY debating if I need this or not
        }
    }
}