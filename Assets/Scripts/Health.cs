using UnityEngine;

public class Health : MonoBehaviour{
    [SerializeField] private bool isPlayer;
    [SerializeField] private int health = 50;
    [SerializeField] private int score = 50;
    [SerializeField] private ParticleSystem hitEffect;

    [SerializeField] private bool applyCameraShake;
    private CameraShake _cameraShake;

    private AudioPlayer _audioPlayer;
    private ScoreKeeper _scoreKeeper;
    private LevelManager _levelManager;

    private void Awake(){
        _cameraShake = Camera.main.GetComponent<CameraShake>();
        _audioPlayer = FindObjectOfType<AudioPlayer>();
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        var damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer == null) return;
        TakeDamage(damageDealer.GetDamage());
        PlayHitEffect();
        _audioPlayer.PlayDamageClip();
        ShakeCamera();
        damageDealer.Hit();
    }

    public int GetHealth(){
        return health;
    }

    private void TakeDamage(int damage){
        health -= damage;
        if (health <= 0){
            Die();
        }
    }

    private void Die(){
        if (!isPlayer){
            _scoreKeeper.ModifyScore(score);
        }
        else{
            _levelManager.LoadGameOver();
        }

        Destroy(gameObject);
    }

    private void PlayHitEffect(){
        if (hitEffect == null) return;
        var instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }

    private void ShakeCamera(){
        if (_cameraShake != null && applyCameraShake){
            _cameraShake.Play();
        }
    }
}