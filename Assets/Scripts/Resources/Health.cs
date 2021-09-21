using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources {
    public class Health : MonoBehaviour, ISaveable {
        [SerializeField]
        private float regenerationPercentage = 70;

        [SerializeField]
        private float healthPoints = -1f;
        private bool isDead = false;

        private void Start() {
            if (healthPoints < 0) {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage) {
            print(gameObject.name + " took damage : " + damage);

            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0) {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints() {
            return healthPoints;
        }

        public float GetMaxHealthPoints() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        } 

        private void AwardExperience(GameObject instigator) {
            var experience = instigator.GetComponent<Experience>();

            if (experience == null) {
                return;
            }

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth() {
            var regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        public float GetPercentage() {
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Die() {
            if (isDead) {
                return;
            }

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState() {
            return healthPoints;
        }

        public void RestoreState(object state) {
            healthPoints = (float)state;

            if (healthPoints == 0) {
                Die();
            }
        }
    }
}

