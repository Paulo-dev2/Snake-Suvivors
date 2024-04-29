using UnityEngine;
using UnityEngine.Events;

public class Apple : MonoBehaviour
{
    public UnityEvent AppleDestroyedEvent;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Snake")
        {
            Destroy(gameObject);
            if (AppleDestroyedEvent != null) AppleDestroyedEvent.Invoke();
        }
    }
}
