using UnityEngine;
using System.Collections;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {
        [SerializeField]
        private ProgressionCharacterClass[] characterClass = null;

        [System.Serializable]
        class ProgressionCharacterClass {
            [SerializeField]
            private CharacterClass characterClass;
            [SerializeField]
            private float[] health;
        }
    }
}

