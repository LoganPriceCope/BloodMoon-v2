
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
namespace Player
{
    public class AwareState : State
    {
        // constructor
        public AwareState(PlayerScript player, StateMachine sm) : base(player, sm)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            player.CheckForPartol();
            player.Attack();
            MoveNearPlayer();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public void MoveNearPlayer()
        {
            if (player.points.Length == 0)
            {
                return;
            }
            Vector3 directionAway = (player.player.transform.position - player.enemy.transform.position).normalized;
            float distance = -5.7f;
            player.nav.destination = player.player.transform.position + directionAway * distance;
        }
    }
}