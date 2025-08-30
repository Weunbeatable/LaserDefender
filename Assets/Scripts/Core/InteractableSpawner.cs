using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD.Core
{
    public class InteractableSpawner : MonoBehaviour
    {
        // seperating logic of each script is whats important
        // Meteor spawning should only spawn meteors
        // meteor logic should handle how METEORS work
        // meteor logic can trigger an event that can cause other scripts to run but besides that the logic should be  left to other scripts to help keep things separate. 
        // Interactable logic will respond after a meteor has been destroyed spawning a consumable in the spot the object has been destroyed. 
        // Possible side effect is consumables spawning where they shouldn't due to the shredder but testing will have to be done to see if that is the case. 

        [SerializeField] GameObject[] _healthPickups;
        [SerializeField] GameObject[] _shieldPickups;
        [SerializeField] GameObject[] _pointPickups;
        //[SerializeField] GameObject[] _resourcePickups; These will increase player resources e.g. boost health cap or shield cap, add more ammo to a special type etc. 

        int _assignedValue;

        private void OnEnable()
        {
            MeteorLogic.OnMeteorsDestruction += MeteorLogic_OnMeteorsDestruction;
        }

        private void MeteorLogic_OnMeteorsDestruction(bool obj, Vector3 posToSpawn)
        {
            Debug.Log("This is working, test successful!!!");
            if (obj == false)
            {
                _Choose_Health_Item(posToSpawn);
            }
            else if (obj == true) 
            {
                _Choose_Interactable_To_Spawn(posToSpawn);
            }
            
        }

        private void OnDisable()
        {
            MeteorLogic.OnMeteorsDestruction -= MeteorLogic_OnMeteorsDestruction;
        }

        private void _Choose_Health_Item(Vector3 spawnPos)
        {
            // If I ever expand this might need revising. 

            _assignedValue = UnityEngine.Random.Range(1, 3);

            switch (_assignedValue)
            {
                case 1:
                    Instantiate(_healthPickups[0], spawnPos, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(_healthPickups[1], spawnPos, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(_healthPickups[2], spawnPos, Quaternion.identity);
                    break;
            }
        }

        private void _Choose_Interactable_To_Spawn( Vector3 spawnPos)
        {


                int _heads_Or_Tails = UnityEngine.Random.Range(1, 50);
                if (_heads_Or_Tails % 2 == 0)
                {
                    _assignedValue = UnityEngine.Random.Range(1, 3);

                    switch (_assignedValue)
                    {
                        case 1:
                            Instantiate(_shieldPickups[0], spawnPos, Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(_shieldPickups[1], spawnPos, Quaternion.identity);
                            break;
                        case 3:
                            Instantiate(_shieldPickups[2], spawnPos, Quaternion.identity);
                            break;
                    }
                }
                else
                {
                    _assignedValue = UnityEngine.Random.Range(1, 3);

                    switch (_assignedValue)
                    {
                        case 1:
                            Instantiate(_pointPickups[0], spawnPos, Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(_pointPickups[1], spawnPos, Quaternion.identity);
                            break;
                        case 3:
                            Instantiate(_pointPickups[2], spawnPos, Quaternion.identity);
                            break;
                    }

                }
        }
    }
}