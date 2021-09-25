using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {
        private enum DestinationIdentifier { A, B, C, D, E }

        [SerializeField]
        private int sceneToLoad = -1;
        [SerializeField]
        private Transform spawnPoint;
        [SerializeField]
        private DestinationIdentifier destination;
        [SerializeField]
        private float fadeOutTime = 1f;
        [SerializeField]
        private float fadeInTime = 2f;
        [SerializeField]
        private float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {
            if(sceneToLoad < 0) {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            var wrapper = FindObjectOfType<SavingWrapper>();
            var playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);

            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            var newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal) {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal() {
            foreach (var portal in FindObjectsOfType<Portal>()) {
                if (portal == this) {
                    continue;
                }

                if(portal.destination != destination) {
                    continue;
                }

                return portal;
            }

            return null;
        }
    }
}
