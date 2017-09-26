using UnityEngine;

namespace ProjetSynthese
{
    public class AIController : ActorController
    {
        public Vector3 MapDestination { get; set; }
        public Vector3 OpponentTargetDestination { get; set; }
        public Vector3 ItemTargetDestination { get; set; }

        public bool MapDestinationIsKnown { get; set; }
        public bool OpponentTargetDestinationIsKnown { get; set; }
        public bool ItemTargetDestinationIsKnown { get; set; }

        public enum MoveTarget { Item,Map,Opponent };
        public MoveTarget AIMoveTarget { get; set; }

        public enum SpeedLevel { Walking, Jogging, Running,Swimming };
        public SpeedLevel AISpeed { get; set; }

        [SerializeField]
        private const float WalkingSpeed = 0.75f;
        [SerializeField]
        private const float JoggingSpeed = 2.0f;
        [SerializeField]
        private const float RunningSpeed = 5.0f;
        [SerializeField]
        private const float SwimmingSpeed = 0.5f;

        private float currentSpeedLevel;

        private const float errorPositionTolerance = 0.001f;

        private void Start()
        {
            MapDestinationIsKnown = false;
            OpponentTargetDestinationIsKnown = false;
            ItemTargetDestinationIsKnown = false;
            AISpeed = SpeedLevel.Walking;
        }

         public bool HasReachedMapDestination()
        {

            float distance = Vector3.Distance(MapDestination, this.transform.position);
            if (distance < errorPositionTolerance)
            {
                return true;
            }
            return false;
        }

        public bool HasReachedOpponentTargetDestination()
        {

            float distance = Vector3.Distance(OpponentTargetDestination, this.transform.position);
            if (distance < errorPositionTolerance)
            {
                return true;
            }
            return false;
        }

        public bool HasReachedItemTargetDestination()
        {

            float distance = Vector3.Distance(ItemTargetDestination, this.transform.position);
            if (distance < errorPositionTolerance)
            {
                return true;
            }
            return false;
        }

        public override void Move()
        {
            //move toward choix type
            
            //float pas = this.currentSpeedLevel * Time.deltaTime;

            ////La méthode MoveTowards de vector2 fait pas mal la job de déplacement pour nous.  Pour le moment elle va chercher la nouvelle position, mais le déplacement n'est pas encore fait.
            //Vector2 nouvellePosition = Vector2.MoveTowards(transform.position, GetDestination(), pas);

            ////Différence entre la nouvelle et l'ancienne position, pour calculer l'angle de rotation
            //Vector2 mouvement = new Vector2(nouvellePosition.x - transform.position.x, nouvellePosition.y - transform.position.y);

            ////Angle de rotation en degrée (car l'affichage de Unity veut les angles en degrée); Mathf.Rad2Deg nécessaire car Mathf.Atan2 un résultat en radiants
            //float angle = -Mathf.Atan2(mouvement.x, mouvement.y) * Mathf.Rad2Deg;

            ////On applique la nouvelle position
            //transform.position = nouvellePosition;

            ////On applique la nouvelle rotation
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void MoveDestination(MoveTarget moveTarget)
        {
            float pas = this.currentSpeedLevel * Time.deltaTime;

            ////La méthode MoveTowards de vector2 fait pas mal la job de déplacement pour nous.  Pour le moment elle va chercher la nouvelle position, mais le déplacement n'est pas encore fait.
            //Vector2 nouvellePosition = Vector2.MoveTowards(transform.position, GetDestination(), pas);

            ////Différence entre la nouvelle et l'ancienne position, pour calculer l'angle de rotation
            //Vector2 mouvement = new Vector2(nouvellePosition.x - transform.position.x, nouvellePosition.y - transform.position.y);

            ////Angle de rotation en degrée (car l'affichage de Unity veut les angles en degrée); Mathf.Rad2Deg nécessaire car Mathf.Atan2 un résultat en radiants
            //float angle = -Mathf.Atan2(mouvement.x, mouvement.y) * Mathf.Rad2Deg;

            ////On applique la nouvelle position
            //transform.position = nouvellePosition;

            ////On applique la nouvelle rotation
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
}