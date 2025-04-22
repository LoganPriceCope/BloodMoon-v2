using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Player
{


    public class PlayerScript : MonoBehaviour
    {

        public PlayerMovement playerMovement;

        public bool runMode = false;


        // EEE
        public Transform[] points;
        public NavMeshAgent nav;
        public int destPoint;

        public int maxDistanceOk = 100;

        public RaycastHit hit;

        public GameObject deathFrame;
        public GameObject player;
        public GameObject enemy;
        //EEE

        public LayerMask playerLayer;

        public float sightRange, killRange, awareRange;
        public bool playerInSightRange, playerInKillRange, playerInAwareRange;

        // variables holding the different player states
        public AttackState attackState;
        public PartolState partolState;
        public AwareState awareState;

        public StateMachine sm;



        // Start is called before the first frame update
        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

            sm = gameObject.AddComponent<StateMachine>();

            // add new states here
            attackState = new AttackState(this, sm);
            partolState = new PartolState(this, sm);
            awareState = new AwareState(this, sm);

            // initialise the statemachine with the default state
            sm.Init(partolState);

        }

        // Update is called once per frame
        public void Update()
        {
            awareRange = playerMovement.currentLoudness;
            //if (playerMovement.carryingWinningPart)
            //{
             //   sightRange = 1000f;

            //}
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            playerInKillRange = Physics.CheckSphere(transform.position, killRange, playerLayer);
            playerInAwareRange = Physics.CheckSphere(transform.position, awareRange, playerLayer);
            sm.CurrentState.LogicUpdate();

            //output debug info to the canvas
            string s;
            s = string.Format("last state={0}\ncurrent state={1}", sm.LastState, sm.CurrentState);

            //UIscript.ui.DrawText(s);

            //UIscript.ui.DrawText("Press I for idle / R for run");



        }



        void FixedUpdate()
        {
            sm.CurrentState.PhysicsUpdate();
            Debug.Log(sm.CurrentState);
            Aware();
            Kill();
        }



        public void CheckForPartol()
        {
            if (!playerInSightRange && !playerInAwareRange)
            {
                sm.ChangeState(partolState);
            }
            else if (!playerInSightRange)
            {
                sm.ChangeState(awareState);
            }
        }

        public void CheckForAttack(Vector3 center, float radius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.SendMessage("E");
            }
        }

        public void Aware()
        {
            if (playerInAwareRange == true && !playerInSightRange)
            {
                sm.ChangeState(awareState);
            }
        }

        public void Attack()
        {
            if (playerInSightRange == true)
            {
                sm.ChangeState(attackState);
            }
        }

        public void Kill()
        {
            if (playerInKillRange)
            {
                Debug.Log("Enters");
                deathFrame.SetActive(true);
                Invoke("Restart", 5f);
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene("SampleScene");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, sightRange);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, killRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, awareRange);
        }
    }
}