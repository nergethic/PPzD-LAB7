using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PlayerAudioController : PlayerSubsystem {
    [SerializeField, EventRef] string jumpRef;
    [SerializeField, EventRef] string landingRef;
    [SerializeField, EventRef] string deathRef;
    [SerializeField, EventRef] string attackRef;
    [SerializeField, EventRef] string footstepRef;
    [SerializeField, EventRef] string blockHitRef;
    [SerializeField, EventRef] string gotHitRef;
    [SerializeField, EventRef] string heartbeatRef;
    
    PlayerAudioData playerAudioData;
    Bus heartbeatGroup;
    EventInstance heartbeat;
    
    bool isHeartbeatPlaying;
    const float healthHeartbeatThreshold = 0.3f;
    float minHeartbeatPitch;
    float maxHeartbeatPitch;
    float minHeartbeatVolumeDb;
    float maxHeartbeatVolumeDb;

    private void Start() {
        heartbeat = RuntimeManager.CreateInstance(heartbeatRef);
        heartbeat.start();
        
        heartbeat.getPitch(out minHeartbeatPitch);
        maxHeartbeatPitch = minHeartbeatPitch + 0.3f;

        minHeartbeatVolumeDb = GetVolumeFromDecibels(-30f);
        maxHeartbeatVolumeDb = GetVolumeFromDecibels(0f);

        heartbeatGroup = RuntimeManager.GetBus("bus:/Master/SFX/Player/HeartbeatGrp");
        HandlePlayerHealthChanged(1f);

        RuntimeManager.StudioSystem.setParameterByName("MasterPitchParam", 0f);
    }

    public override void HandleEvent(PlayerEventType eventType) {
        switch (eventType) {
            case PlayerEventType.Jump:
                RuntimeManager.PlayOneShot(jumpRef);
                break;
            case PlayerEventType.Landing:
                RuntimeManager.PlayOneShot(landingRef);
                break;
            case PlayerEventType.Death:
                RuntimeManager.PlayOneShot(deathRef);
                break;
            case PlayerEventType.Attack:
                RuntimeManager.PlayOneShot(attackRef);
                break;
            case PlayerEventType.BlockHit:
                RuntimeManager.PlayOneShot(blockHitRef);
                break;
            case PlayerEventType.GotHit:
                RuntimeManager.PlayOneShot(gotHitRef);
                HandlePlayerHealthChanged(playerAudioData.healthPercentage);
                break;
            case PlayerEventType.Footstep:
                RuntimeManager.PlayOneShot(footstepRef);
                break;
        }
    }

    public void UpdatePlayerAudioData(float newHealthPercentage) {
        playerAudioData.healthPercentage = newHealthPercentage;
    }
    
    void HandlePlayerHealthChanged(float healthNormalized) {
        if (healthNormalized > 0.3f || Mathf.Approximately(healthNormalized, 0f)) {
            StopHeartbeat();

            heartbeatGroup.setVolume(GetVolumeFromDecibels(-80f));
        } else {
            StartHeartbeat();
            
            float t = 1f - Mathf.Clamp01(healthNormalized * (1f / healthHeartbeatThreshold));

            float newVolume = Mathf.Lerp(minHeartbeatVolumeDb, maxHeartbeatVolumeDb, t);
            heartbeatGroup.setVolume(newVolume);
            
            float newPitch = Mathf.Lerp(minHeartbeatPitch, maxHeartbeatPitch, t);
            heartbeat.setPitch(newPitch);
            
            RuntimeManager.StudioSystem.setParameterByName("MasterPitchParam", t);
        }
    }

    void StopHeartbeat() {
        if (isHeartbeatPlaying) {
            heartbeat.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isHeartbeatPlaying = false;
        }
    }

    void StartHeartbeat() {
        if (isHeartbeatPlaying == false) {
            heartbeat.start();
            isHeartbeatPlaying = true;
        }
    }

    float GetVolumeFromDecibels(float db) {
        return Mathf.Pow(10.0f, db / 20f);
    }
}

public struct PlayerAudioData {
    public float healthPercentage;
}