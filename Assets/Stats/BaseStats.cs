using System;
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
        [SerializeField]
        private GameObject levelUpParticleEffect = null;

        public event Action onLevelUp; 

        private int currentLevel = 0;

        private void Start() {
            currentLevel = CalculateLevel();
            var experience = GetComponent<Experience>();
            if(experience != null) {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        public void UpdateLevel() {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel) {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect() {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat) {
            return progression.GetStat(stat, characterClass, startingLevel);
        }

        public int GetLevel() {
            if(currentLevel < 1) {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }

        public int CalculateLevel() {
            var experience = GetComponent<Experience>();

            if(experience == null) {
                return startingLevel;
            }

            var currentXP = experience.GetPoints();
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

