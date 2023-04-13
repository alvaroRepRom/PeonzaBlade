using UnityEngine;

public class BladeParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem moveParticles;
    [SerializeField] private ParticleSystem shockParticles;

    private bool isActiveMove = false;

    public void SetMoveParticles( bool moveParticlesActive )
    {
        if ( moveParticlesActive && !isActiveMove )
        {
            isActiveMove = true;
            moveParticles.Play();
        }
        else if ( !moveParticlesActive && isActiveMove )
        {
            isActiveMove = false;
            moveParticles.Stop();
        }
    }

    public void SetShockParticles()
    {
        shockParticles.Play();
    }
}
