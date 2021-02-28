using UnityEngine;

public class GameArea : MonoBehaviour {
    [SerializeField] AreaType type;
    
    AreaManager areaManager;
    int playerLayer;

    private void Awake() {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void Initialize(AreaManager areaManager) {
        this.areaManager = areaManager;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == playerLayer) {
            areaManager.NotifyAboutNewAreaEntered(type);
        }
    }
}

public enum AreaType {
    FOREST = 0,
    MEADOW
}