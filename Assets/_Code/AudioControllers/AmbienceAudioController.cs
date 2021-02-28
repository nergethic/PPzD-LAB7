using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AmbienceAudioController : MonoBehaviour {
    [Header("Override Time Day Snapshots")]
    [SerializeField, EventRef] string daySnapshotRef;
    [SerializeField, EventRef] string nightSnapshotRef;
    [SerializeField, EventRef] string sunsetSnapshotRef;
    
    [Header("Blending Player Location Snapshots")]
    [SerializeField, EventRef] string forestSnapshotRef;
    [SerializeField, EventRef] string meadowSnapshotRef;
    
    [Header("Sounds")]
    [SerializeField, EventRef] string birdsSoundRef;
    [SerializeField, EventRef] string cricketsSoundRef;
    [SerializeField, EventRef] string windSoundRef;
    
    EventInstance daySnapshot;
    EventInstance nightSnapshot;
    EventInstance sunsetSnapshot;
    EventInstance forestBlendingSnapshot;
    EventInstance meadowBlendingSnapshot;
    
    EventInstance crickets;
    EventInstance birds;
    EventInstance wind;

    void Start() {
        DayNightController.TimeOfDayChangedEvent += OnTimeOfDayChanged;

        daySnapshot    = RuntimeManager.CreateInstance(daySnapshotRef);
        nightSnapshot  = RuntimeManager.CreateInstance(nightSnapshotRef);
        sunsetSnapshot = RuntimeManager.CreateInstance(sunsetSnapshotRef);
        
        forestBlendingSnapshot = RuntimeManager.CreateInstance(forestSnapshotRef);
        meadowBlendingSnapshot = RuntimeManager.CreateInstance(meadowSnapshotRef);
        
        crickets = RuntimeManager.CreateInstance(cricketsSoundRef);
        crickets.start();
        
        birds = RuntimeManager.CreateInstance(birdsSoundRef);
        birds.start();
        
        wind = RuntimeManager.CreateInstance(windSoundRef);
        wind.start();

        OnTimeOfDayChanged(TimeOfDay.Day);
    }
    
    void OnTimeOfDayChanged(TimeOfDay time) {
        StopSnapshots();
        
        switch (time) {
            case TimeOfDay.Day: {
                daySnapshot.start();
            } break;
            
            case TimeOfDay.Night: {
                nightSnapshot.start();
            } break;
            
            case TimeOfDay.Sunset: {
                sunsetSnapshot.start();
            } break;
        }
    }

    public void OnAreaChanged(AreaType type) {
        StopSnapshot(forestBlendingSnapshot);
        StopSnapshot(meadowBlendingSnapshot);

        switch (type) {
            case AreaType.FOREST: {
                forestBlendingSnapshot.start();
            } break;
            
            case AreaType.MEADOW: {
                meadowBlendingSnapshot.start();
            } break;
        }
    }

    void StopSnapshots() {
        StopSnapshot(daySnapshot);
        StopSnapshot(nightSnapshot);
        StopSnapshot(sunsetSnapshot);
    }

    void StopSnapshot(EventInstance e) {
        e.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnDestroy() {
        DayNightController.TimeOfDayChangedEvent -= OnTimeOfDayChanged;
    }

    void ReleaseSnapshots() {
        daySnapshot.release();
        nightSnapshot.release();
        sunsetSnapshot.release();
        
        forestBlendingSnapshot.release();
        meadowBlendingSnapshot.release();
    }
}