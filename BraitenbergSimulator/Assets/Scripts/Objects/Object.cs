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

        protected virtual void Start()
        {
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
            gameObject.GetComponent<Rigidbody>().useGravity = false;

            // Disable collisions
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        public void Place()
        {
            isMovable = false;

            // Enable gravity for the object
            gameObject.GetComponent<Rigidbody>().useGravity = true;

            // Enable collisions
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
}