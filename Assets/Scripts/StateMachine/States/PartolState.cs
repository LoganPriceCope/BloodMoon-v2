
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
namespace Player
{
    public class PartolState : State
    {



        public PartolState(PlayerScript player, StateMachine sm) : base(player, sm)
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

            player.Aware();
            if (!player.nav.pathPending && player.nav.remainingDistance < 0.5f)
            {
                GoToNextPoint();
            }

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public void GoToNextPoint()
        {
            if (player.points.Length == 0)
            {
                return;
            }
            player.nav.destination = player.points[player.destPoint].position;
            player.destPoint = (player.destPoint + 1) % player.points.Length;
        }

    }
}