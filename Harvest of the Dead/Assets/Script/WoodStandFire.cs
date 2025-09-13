using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class TorchFireSetup : MonoBehaviour
{
    void Awake()
    {
        SetupTorchFire();
    }

    void SetupTorchFire()
    {
        var ps = GetComponent<ParticleSystem>();

        // --- Main Module ---
        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Local; // follow the torch
        main.startSpeed = 0f;                                       // no forward velocity
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.3f, 0.7f); // short flame life
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);     // small flame
        main.gravityModifier = 0f;                                 // ignore gravity

        // --- Shape ---
        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.radius = 0.02f;   // tiny source
        shape.angle = 7f;       // slight spread

        // --- Disable unwanted motion ---
        var inheritVelocity = ps.inheritVelocity;
        inheritVelocity.enabled = false;

        var velocityOverLifetime = ps.velocityOverLifetime;
        velocityOverLifetime.enabled = false;

        var forceOverLifetime = ps.forceOverLifetime;
        forceOverLifetime.enabled = false;

        var limitVelocityOverLifetime = ps.limitVelocityOverLifetime;
        limitVelocityOverLifetime.enabled = false;

        var externalForces = ps.externalForces;
        externalForces.enabled = false;

        // --- Renderer (to avoid stretched trails) ---
        var rnd = ps.GetComponent<ParticleSystemRenderer>();
        if (rnd != null)
        {
            rnd.renderMode = ParticleSystemRenderMode.Billboard;
            rnd.velocityScale = 0f;
            rnd.lengthScale = 1f;
        }
    }
}
