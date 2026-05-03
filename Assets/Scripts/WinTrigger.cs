using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null) ui.WinGame();
        }
    }
}
