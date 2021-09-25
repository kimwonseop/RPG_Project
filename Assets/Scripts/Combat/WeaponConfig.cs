using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject {
        [SerializeField]
        private AnimatorOverrideController animatorOverride = null;
        [SerializeField]
        private Weapon equippedPrefab = null;
        [SerializeField]
        private float weaponDamage = 5f;
        [SerializeField]
        private float percentageBonus = 0;
        [SerializeField]
        private float weaponRange = 2f;
        [SerializeField]
        private bool isRightHanded = true;
        [SerializeField]
        private Projectile projectile = null;

        private const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator) {
            DestroyOldWeapon(rightHand, leftHand);

            if (equippedPrefab != null) {
                Transform handTransform = GetTransform(rightHand, leftHand);
                Weapon weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null) {

                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null) {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand) {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null) {
                oldWeapon = leftHand.Find(weaponName);
            }

            if (oldWeapon == null) {
                return;
            }

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand) {
            Transform handTransform;

            if (isRightHanded) {
                handTransform = rightHand;
            }
            else {
                handTransform = leftHand;
            }

            return handTransform;
        }

        public bool HasProjectile() {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage) {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetDamage() {
            return weaponDamage;
        }

        public float GetPercentageBonus() {
            return percentageBonus;
        }

        public float GetRange() {
            return weaponRange;
        }
    }
}

