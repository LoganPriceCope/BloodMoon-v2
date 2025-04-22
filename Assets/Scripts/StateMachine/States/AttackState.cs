
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
namespace Player
{
    public class AttackState : State
    {
        // constructor
        public AttackState(PlayerScript player, StateMachine sm) : base(player, sm)
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
            MoveTowardsPlayer();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public void MoveTowardsPlayer()
        {
            if(player.points.Length == 0)
            {
                return;
            }
            player.nav.destination = player.player.transform.position;
        }
    }
}