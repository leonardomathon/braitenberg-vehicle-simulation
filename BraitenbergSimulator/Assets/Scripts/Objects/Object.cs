using UnityEngine;

namespace Objects
{
    public class Object : Selectable
    {
        // Random object id
        private int objectId;

        // Name of the object
        public string objectName;

        // Boolean that stores if object is currently movable
        public bool isMovable;

        protected new void Start()
        {
            base.Start();
            objectId = Random.Range(1, int.MaxValue);
        }

        // Getter for object id
        public string GetObjectId()
        {
            return objectId.ToString();
        }


        // Getter for object name
        public string GetObjectName()
        {
            return objectName;
        }

        // Getter and setters for isMovable
        public bool IsMovable()
        {
            return isMovable;
        }

        public void Move()
        {
            isMovable = true;

            // Temporarily disable gravity for the object
            foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>())
            {
                r.useGravity = false;
            }

            // Disable collisions
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }
        }

        public void Place()
        {
            isMovable = false;

            // Enable gravity for the object
            foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>())
            {
                r.useGravity = false;
            }

            // Enable collisions
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                c.enabled = true;
            }
        }
    }
}