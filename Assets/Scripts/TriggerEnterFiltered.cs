using System;
using UnityEngine;

public class TriggerEnterFiltered : MonoBehaviour
{
    [SerializeField] private string _tagFilter;
    public event Action<GameObject> Entered;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(_tagFilter))
            Entered?.Invoke(other.gameObject);
    }
}
