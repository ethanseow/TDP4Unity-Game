using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    private GameObject basicAbilityGameObject;
    private BoxCollider2D playerCollider;
    [SerializeField] GameObject basicAbilityPrefab = null;
    private void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Start_Ability(basicAbilityPrefab);
        }
    }
    void Start_Ability(GameObject abilityPrefab)
    {
        basicAbilityGameObject = Instantiate(abilityPrefab, transform.position, new Quaternion(0, 0, 0, 0));
    }
}
