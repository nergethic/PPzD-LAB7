using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {
    [SerializeField] List<GameArea> areas;
    [SerializeField] AmbienceAudioController ambienceAudioController;
    
    void Start() {
        foreach (var area in areas) {
            area.Initialize(this);
        }
        
        ambienceAudioController.OnAreaChanged(AreaType.MEADOW);
    }

    public void NotifyAboutNewAreaEntered(AreaType type) {
        ambienceAudioController.OnAreaChanged(type);
    }    
}