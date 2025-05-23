using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using FSR;
using Unity.VisualScripting;
using UnityEngine.Rendering.VirtualTexturing;

namespace Player
{


    public class PlayerScript : MonoBehaviour
    {
        public PlayerMovement playerMovement;
        public DiarySystem diarySystem;

        public bool runMode = false;


        public Transform[] points;
        public NavMeshAgent nav;
        public int destPoint;

        public int maxDistanceOk = 100;

        public RaycastHit hit;

        public GameObject deathFrame;
        public GameObject player;
        public GameObject enemy;

        public Animator enemyAnimator;

        public LayerMask playerLayer;

        public float sightRange, killRange, awareRange;
        public bool playerInSightRange, playerInKillRange, playerInAwareRange;

        public AttackState attackState;
        public PartolState partolState;
        public AwareState awareState;

        public StateMachine sm;
        public AudioManager am;

        public AudioSource attackSource;

        public float currentLoudness;
        bool alert = false;
        bool alertCooldown = false;
        bool canWalk = true;
        bool canAttackSound = true;
        bool canAttack = true;
        public bool isInAttackZone = false;

        // Start is called before the first frame update
        void Start()
        {
            am = AudioManager.instance;

            currentLoudness = 4f;

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
            if (canWalk)
            {
                nav.speed = 4.1f * (1f + (diarySystem.pages / 10f)); // was 4.1
            }
            else
            {
                nav.speed = 1f;
            }
                awareRange = currentLoudness;

            if (alert == true)
            {
                if (alertCooldown == false)
                {
                    alertCooldown = true;
                    sightRange = sightRange * 2.5f;
                    StartCoroutine(AlertCooldown());
                }
            }
            if (playerMovement.isCrouching && playerMovement.isWalking)
            {
                currentLoudness = 8f;
                Debug.Log("Crouching Walking");
            }
            else if (playerMovement.isCrouching && !playerMovement.isWalking)
            {
                currentLoudness = 6f;
                Debug.Log("Crouching");
            }
            else if (playerMovement.isWalking && !playerMovement.isRunning && !playerMovement.isCrouching)
            {
                currentLoudness = 20f;
                Debug.Log("Walking");
            }
            else if (playerMovement.isWalking && playerMovement.isRunning)
            {
                currentLoudness = 28f;
                Debug.Log("Running");
            }
            else if (!playerMovement.isRunning && !playerMovement.isWalking && !playerMovement.isCrouching)
            {
                currentLoudness = 16f;
                Debug.Log("Idle");
            }
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
            Aware();
            Kill();
            Attack();
            print(isInAttackZone);
        }

        public void CheckForPartol()
        {
            if (!playerInSightRange && !playerInAwareRange)
            {
                sm.ChangeState(partolState);
                print("Partol State");
            }
            else if (!playerInSightRange)
            {
                sm.ChangeState(awareState);
                print("Aware State");
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
            if (playerInAwareRange && !playerInSightRange)
            {
                sm.ChangeState(awareState);
                print("Aware State");
                alert = true;
            }
        }

        public void Attack()
        {
            if (playerInSightRange)
            {
                sm.ChangeState(attackState);
                print("Attack State");
            }
        }

        public void Kill()
        {
            print("Calling kill");
            if (playerInKillRange && canAttack)
            {
                print("Starting attack");
                canAttack = false;
                canWalk = false;
                if (canAttackSound)
                {
                    attackSource.Play();
                    //canAttackSound = false;
                }
                enemyAnimator.SetBool("Attacking", true);
                //Invoke("ResetAnimation", 1.5f);
                Invoke("KillPlayer", 0.4f);
            }
        }

        public void KillPlayer()
        {
            print("Starting kill player");
            if (playerInKillRange)
            {
                print("The player has been killed");
                if(playerMovement.health >= 0)
                {
                    playerMovement.health--;
                    am.SFXSource.PlayOneShot(am.bloodEffect);
                }
            }
            Invoke("ResetAnimation", 1.75f);
        }
        public void ResetAnimation()
        {
                print("Starting reset animation");
                canWalk = true;
                enemyAnimator.SetBool("Attacking", false);
                //canAttackSound = true;
                canAttack = true;
        }
        public void Restart()
        {
            SceneManager.LoadScene("MainGame");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, sightRange);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, killRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, awareRange);
        }

        public IEnumerator AlertCooldown()
        {
            am.SFXSource.PlayOneShot(am.heartBeat);
            yield return new WaitForSeconds(4);
            alert = false;
            alertCooldown = false;
            sightRange = sightRange / 2.5f;
        }
    }
}