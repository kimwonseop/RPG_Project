
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.Combat {
    public class EnemyHealthDisplay : MonoBehaviour {
        private Fighter fighter;

        private void Awake() {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update() {
            if(fighter.GetTarget() == null) {
                GetComponent<Text>().text = "N/A";
                return;
            }

            var health = fighter.GetTarget();
            GetComponent<Text>().text = string.Format("{0:0}%", health.GetPercentage());
        }
    }
}

