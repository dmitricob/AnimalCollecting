using UnityEngine;

namespace Character.Base
{
    // Describe abstract class Character with general logic of character
    public abstract class Character : MonoBehaviour
    {
        protected abstract void Move();

        protected virtual void FixedUpdate()
        {
            Move();
        }
    }
    

}
