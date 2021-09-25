using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable {
        [SerializeField]
        private WeaponConfig weapon = null;
        [SerializeField]
        private float respawnTime = 5;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player") {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter) {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds) {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow) {
            GetComponent<Collider>().enabled = shouldShow;
            transform.GetChild(0).gameObject.SetActive(shouldShow);
            foreach (Transform child in transform) {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController) {
            if (Input.GetMouseButtonDown(0)) {
                Pickup(callingController.GetComponent<Fighter>());
            }

            return true;
        }

        public CursorType GetCursorType() {
            return CursorType.Pickup;
        }
    }
}
