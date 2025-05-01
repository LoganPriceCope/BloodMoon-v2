using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using FSR;
using Unity.VisualScripting;

namespace Player
{


    public class PlayerScript : MonoBehaviour
    {
        public PlayerMovement playerMovement;

        public bool runMode = false;


        public Transform[] points;
        public NavMeshAgent nav;
        public int destPoint;

        public int maxDistanceOk = 100;

        public RaycastHit hit;

        public GameObject deathFrame;
        public GameObject player;
        public GameObject enemy;

        public LayerMask playerLayer;

        public float sightRange, killRange, awareRange;
        public bool playerInSightRange, playerInKillRange, playerInAwareRange;

        public AttackState attackState;
        public PartolState partolState;
        public AwareState awareState;

        public StateMachine sm;
        public AudioManager am;

        public float currentLoudness;
        bool alert = false;
        bool alertCooldown = false;

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
                currentLoudness = 35f;
                Debug.Log("Running");
            }
            else if (!playerMovement.isRunning && !playerMovement.isWalking && !playerMovement.isCrouching)
            {
                currentLoudness = 16f;
                Debug.Log("Idle");
            }
                /*else
                {
                    if (!am.SFXSource.isPlaying && !playerMovement.isCrouching && !playerMovement.isRunning)
                    {
                        currentLoudness = 12f;
                        Debug.Log("Idle");
                    }
                    else if (playerMovement.isRunning)
                    {
                        if (am.SFXSource.isPlaying && am.SFXSource != null)
                        {
                            AudioClip currentAudioClip = am.SFXSource.clip;
                            if (currentAudioClip == am.Footsteps1 || currentAudioClip == am.Footsteps2 || currentAudioClip == am.Footsteps3)
                            {
                                currentLoudness = 24f;
                                Debug.Log("Running");
                            }
                        }
                    else if (!playerMovement.isRunning)
                    {
                            if (am.SFXSource.isPlaying && am.SFXSource != null)
                            {
                                AudioClip currentAudioClip = am.SFXSource.clip;
                                if (currentAudioClip == am.Footsteps1 || currentAudioClip == am.Footsteps2 || currentAudioClip == am.Footsteps3)
                                {
                                    currentLoudness = 18f;
                                    Debug.Log("Walking");
                                }
                            }
                        }
                    }  
                }*/


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
                alert = true;
            }
            else
            {
                if (playerInSightRange)
                {
                    sm.ChangeState(attackState);
                }
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
                deathFrame.SetActive(true);
                Invoke("Restart", 5f);
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene("SampleScene");
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