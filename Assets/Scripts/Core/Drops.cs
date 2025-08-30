using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Drops : ScriptableObject
{
    //purupose whenever an enemy is destroyed, a ranadom upgrade will be chosen from the dictionary, this is an item the player can interact with. 
    // IF they have maxed out their upgrades, the value will be converted to points instead. Some upgrades can also become temporary boosts and some 

    public Sprite dropSprite;
    public string dropName;
    public int dropChance;
    public GameObject dropObject = null;

    public enum dropType { statModifier = 0, temporaryUpgrades = 1, temporaryDowngrades = 2, one_Time_Use = 3, permanentUpgrades = 4, dimensionChange = 5 }
    public dropType chosenType;
    // One time uses can be a screen wide nuke or a call to change a state of the game temporarily like frozen time, different from 
    // temp upgrades that can do tihngs like slow down enemies inside  a slow field etc. 
    // DIMENSION CHANGE => changes the game to being 3D, for 3D each sprite should have two gameobjects, a 3D and a 2D variant, when you switch types the model will swap out, models will share 
    // same health and shieldpools to reduce issues. 3D should only be in certain events like being attacked in all directions requiring you  to switch the view. 
    // enum for the type

    public Drops(Sprite dropSprite, string dropName, int dropChance) 
    { 
        this.dropSprite = dropSprite;
        this.dropName = dropName;
        this.dropChance = dropChance;
    }
}

