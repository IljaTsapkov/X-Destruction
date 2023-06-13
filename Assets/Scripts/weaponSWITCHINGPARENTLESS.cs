using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSWITCHINGPARENTLESS : MonoBehaviour {
    [Header("References")]
    [SerializeField] private List<Transform> weapons;

    [Header("Keys")]
    [SerializeField] private List<KeyCode> keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    private void Start()
    {
        SetWeapons();
        Select(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }

    private void SetWeapons()
    {
        if (keys == null || keys.Count == 0)
        {
            Debug.LogError("No keys assigned for weapon switching!");
            return;
        }

        if (weapons == null || weapons.Count == 0)
        {
            Debug.LogError("No weapons assigned for switching!");
            return;
        }

        if (keys.Count != weapons.Count)
        {
            Debug.LogError("The number of keys does not match the number of weapons!");
            return;
        }
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        for (int i = 0; i < keys.Count; i++)
        {
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
            {
                selectedWeapon = i;
                break;
            }
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            Select(selectedWeapon);
        }

        timeSinceLastSwitch += Time.deltaTime;
    }

    private void Select(int weaponIndex)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].gameObject.SetActive(i == weaponIndex);
            }
        }

        timeSinceLastSwitch = 0f;

        OnWeaponSelected();
    }
        private void OnWeaponSelected() { }
}