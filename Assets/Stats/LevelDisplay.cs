using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.Stats {
    public class LevelDisplay : MonoBehaviour {
        private BaseStats baseStats;
        private void Awake() {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update() {
            GetComponent<Text>().text = string.Format("{0:0}", baseStats.CalculateLevel());
        }
    }
}

