using UnityEngine;

namespace Carriage
{
    public class CarriageAiMover: AbstractCarriageMover
    {
        public GameObject ball;

        protected override Vector3 Move()
        {
            var position = transform.position;
            var ballPosition = ball.transform.position;
            var zDirection = (position - ballPosition).normalized.z;
            var actualSpeed = -zDirection * speed;
            return new Vector3(position.x, position.y, 
                Mathf.Clamp(position.z + actualSpeed * Time.deltaTime, -limit, limit));
        }
    }
}