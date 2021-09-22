using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText {
    public class DamageTextSpawner : MonoBehaviour {
        [SerializeField]
        private DamageText damageTextPrefab = null;

        private void Start() {
            Spawn(11);
        }

        public void Spawn(float damageAmount) {
            var instance = Instantiate<DamageText>(damageTextPrefab, transform);

        }
    }

}
