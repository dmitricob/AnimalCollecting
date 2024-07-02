using UnityEngine;

namespace GeneralInput
{
    
    // Determine some logic of different input source,
    // e.g. from Unity input or from some plugins as Rewired
    public interface IInputSystem 
    {
        // ToDo: implement if needed 
        // public float GetAxis(string axisName);
        // public bool GetButtonDown(string buttonName);
        // public bool GetButtonUp(string buttonName);
        bool GetMouseButton(int id);
        bool GetMouseButtonDown(int id);
        public Vector3 GetMousePosition();
    }
}
