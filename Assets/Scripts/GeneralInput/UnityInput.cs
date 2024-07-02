using UnityEngine;

namespace GeneralInput
{
    public class UnityInput : IInputSystem
    {
        public Vector3 GetMousePosition()
        {
            return Input.mousePosition;
        }

        public bool GetMouseButton(int id)
        {
            return Input.GetMouseButton(id);
        }

        public bool GetMouseButtonDown(int id)
        {
            return Input.GetMouseButtonDown(id);
        }
    }
}