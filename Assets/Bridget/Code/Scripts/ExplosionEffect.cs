using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField]
    private float timer = 0.0f;

    private ParticleSystem explosion;

    void Start()
    {
        explosion = GetComponent<ParticleSystem>();

        if (!explosion.isPlaying)
            explosion.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
