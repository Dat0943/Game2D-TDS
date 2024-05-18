using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class ActorVisual : MonoBehaviour
{
    FlashVfx flashVfx;
    protected Actor actor;

    protected virtual void Awake()
    {
        flashVfx = GetComponent<FlashVfx>();
        actor = GetComponent<Actor>();
    }

    public virtual void OnTakeDamage()
    {
        if (flashVfx == null || actor == null || actor.IsDead) return;

        flashVfx.Flash(actor.statData.knockbackTime);
    }
}
