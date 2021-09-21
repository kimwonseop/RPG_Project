using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement {
    public class SavingWrapper : MonoBehaviour {
        private const string defaultSaveFile = "save";
        [SerializeField]
        private float fadeInTime = 0.2f;

        public void Awake() {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene() {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            var fader = FindObjectOfType<Fader>();
            fader.FadeOutImediate();
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.Backspace)) {
                Delete();
            }
        }

        public void Save() {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load() {
            //call to saving system
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Delete() {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}


