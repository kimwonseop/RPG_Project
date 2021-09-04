using UnityEngine;
using System.Collections;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {
        [SerializeField]
        private ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level) {
            foreach (var progressionClass in characterClasses) {
                if(progressionClass.characterClass == characterClass) {
                    //return progressionClass.health[level - 1];
                }
            }
            return 30;
        }

        [System.Serializable]
        private class ProgressionCharacterClass {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        private class ProgressionStat {
            public Stat stat;
            public float[] levels;
        }
    }
}

