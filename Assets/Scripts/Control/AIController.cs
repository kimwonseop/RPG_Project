using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;

namespace RPG.Control {
    public class AIController : MonoBehaviour {
        [SerializeField]
        private float chaseDistance = 5f;
        [SerializeField]
        private float suspicionTime = 3f;
        [SerializeField]
        private PatrolPath patrolPath;
        [SerializeField]
        private float waypointTolerance = 1f;
        [SerializeField]
        private float wayPointDwellTime = 3f;
        [Range(0,1)]
        [SerializeField]
        private float patrolSpeedFraction = 0.2f;

        private Fighter fighter;
        private Health health;
        private Mover mover;
        private GameObject player;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int currentWayPointIndex = 0;

        private void Awake() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
        }

        private void Start() {
            guardPosition = transform.position;
        }

        private void Update() {
            if (health.IsDead()) {
                return;
            }

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) {
                timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime) {
                SuspicionBehaviour();
            }
            else {
                PatrolBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void AttackBehaviour() {
            fighter.Attack(player);
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null) {
                if (AtWayPoint()) {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWayPoint();
                }

                nextPosition = GetCurrentWayPoint();
            }

            if (timeSinceArrivedAtWaypoint > wayPointDwellTime) {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWayPoint() {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWayPoint < waypointTolerance;
        }

        private void CycleWayPoint() {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private Vector3 GetCurrentWayPoint() {
            return patrolPath.GetWayPoint(currentWayPointIndex);
        }

        private void SuspicionBehaviour() {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private bool InAttackRangeOfPlayer() {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

