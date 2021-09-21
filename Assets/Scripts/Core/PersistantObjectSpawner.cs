using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {
    public class PersistantObjectSpawner : MonoBehaviour {
        [SerializeField]
        private GameObject persistantObjectPrefab;
        private static bool hasSpawned = false;
        
        private void Awake() {
            if (hasSpawned) {
                return;
            }

            SpawnPersistentObjects();

            hasSpawned = true;
        }

        private void SpawnPersistentObjects() {
            GameObject persistantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}

