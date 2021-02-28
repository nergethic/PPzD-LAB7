using FMODUnity;
using UnityEngine;

public class EnemyAudioController : PlayerSubsystem {
    [SerializeField, EventRef] string jumpRef;
    [SerializeField, EventRef] string landingRef;
    [SerializeField, EventRef] string deathRef;
    [SerializeField, EventRef] string attackRef;
    [SerializeField, EventRef] string footstepRef;
    [SerializeField, EventRef] string blockHitRef;
    [SerializeField, EventRef] string gotHitRef;
    
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
                break;
            case PlayerEventType.Footstep:
                RuntimeManager.PlayOneShot(footstepRef);
                break;
        }
    }
}
