using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
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
        [SerializeField]
        private bool shouldUseModifiers = false;

        public event Action onLevelUp;
        private LazyValue<int> currentLevel;
        private Experience experience;

        private void Awake() {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start() {
            currentLevel.ForceInit();
        }

        private void OnEnable() {
            if (experience != null) {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable() {
            if (experience != null) {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        public void UpdateLevel() {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value) {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect() {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat) {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat) {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() {
            return currentLevel.value;
        }

        private float GetAdditiveModifier(Stat stat) {
            if (!shouldUseModifiers) {
                return 0;
            }

            float total = 0;

            foreach (var provider in GetComponents<IModifierProvider>()) {
                foreach (var modifier in provider.GetAdditiveModifiers(stat)) {
                    total += modifier;
                }
            }

            return total;
        }

        private float GetPercentageModifier(Stat stat) {
            if (!shouldUseModifiers) {
                return 0;
            }

            float total = 0;

            foreach (var provider in GetComponents<IModifierProvider>()) {
                foreach (var modifier in provider.GetPercentageModifiers(stat)) {
                    total += modifier;
                }
            }

            return total;
        }

        public int CalculateLevel() {
            var experience = GetComponent<Experience>();

            if (experience == null) {
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

