using UnityEngine;

public class OrderSubmitTrigger : MonoBehaviour {
    private void OnMouseDown() {
        PlayerStack player = FindObjectOfType<PlayerStack>();
        if (player != null) {
            player.TrySubmitOrder();
        }
    }
}
