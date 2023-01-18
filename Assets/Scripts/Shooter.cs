using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour{
    [Header("General")] [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 5f;
    [SerializeField] private float baseFiringRate = 0.2f;

    [Header("AI")] [SerializeField] private bool useAI;
    [SerializeField] private float firingRateVariance;
    [SerializeField] private float minimumFiringRate = 0.1f;

    [HideInInspector] public bool isFiring;

    private Coroutine _firingCoroutine;
    private AudioPlayer _audioPlayer;

    private void Awake(){
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void Start(){
        if (useAI){
            isFiring = true;
        }
    }

    private void Update(){
        Fire();
    }

    private void Fire(){
        switch (isFiring){
            case true when _firingCoroutine == null:
                _firingCoroutine = StartCoroutine(FireContinuously());
                break;
            case false when _firingCoroutine != null:
                StopCoroutine(_firingCoroutine);
                _firingCoroutine = null;
                break;
        }
    }

    private IEnumerator FireContinuously(){
        while (true){
            var instance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null){
                rb.velocity = transform.up * projectileSpeed;
            }

            Destroy(instance, projectileLifetime);
            var timeToNextProjectile =
                Random.Range(baseFiringRate - firingRateVariance, baseFiringRate + firingRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);
            _audioPlayer.PlayShootingClip();
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}