using UnityEngine;

    public abstract class AbstractCarriageMover: MonoBehaviour
    {
        public float limit;
        public float speed;
        
        // Update is called once per frame
        private void Update()
        {
            transform.position = Move();
        }

        protected abstract Vector3 Move();
    }