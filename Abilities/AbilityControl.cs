using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControl : MonoBehaviour
{
    public List<ChargedAbilities> chargedAbilities = new List<ChargedAbilities>(); // List of all charged abilities
    public List<CooldownAbilities> cdAbilities = new List<CooldownAbilities>();

    private ChargedAbilities charged; //Currently equipped charged ability
    private CooldownAbilities cdAbil; //Currently equipped cooldown ability

    void Start()
    {
        charged = chargedAbilities[0];
        cdAbil = cdAbilities[0];
    }
    void Update()
    {
        detectChange();
        if (Input.GetKeyDown(KeyCode.E))
        {
            charged.Start_Ability(transform.position);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            cdAbil.Start_Ability(this.gameObject);
        }
    }

    private void detectChange() 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeCharged(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeCharged(2);
        }
    }
    
    private void changeCharged(int button) //if press 1, change charged ability to ability at index 0
    {
        Debug.Log("Changed skill to " + chargedAbilities[button-1]);
        charged = chargedAbilities[button - 1];
    }
}
