using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField]
        private int startingLevel = 1;
        [SerializeField]
        private CharacterClass characterClass;
        [SerializeField]
        private Progression progression;

        public void Update() {
            if (gameObject.tag == "Player") {
                print(GetLevel());
            }
        }

        public float GetStat(Stat stat) {
            return progression.GetStat(stat, characterClass, startingLevel);
        }

        public int GetLevel() {
            var experience = GetComponent<Experience>();
            var currentXP = experience.GetPoints();

            if(experience == null) {
                return startingLevel;
            }

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++) {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);

                if (XPToLevelUp > currentXP) {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}

