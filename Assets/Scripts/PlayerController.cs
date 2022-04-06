using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float _speed;
    Rigidbody2D _playerRigidBody;
    float _inputX;
    public LayerMask _wallLayer;
    public float _rayLength;
    bool _canJump;
    public float _jumpHeight;

    bool _hurt;
    public float _maxHealth;
    [SerializeField ]float _health;
    public float _timeBetweenHurt;
    float _iframe;
    Animator _anim;
    SpriteRenderer _rend;
    [SerializeField]int _coins;

    public Image _healthImage;
    public Text _coinsText;

    public GameObject _gameOverUI;
    bool _gameover;
    protected AudioSource _src;

    void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _canJump = false;
        _health = _maxHealth;
        _hurt = false;
        _iframe = _timeBetweenHurt;
        _anim = GetComponent<Animator>();
        _rend = GetComponent<SpriteRenderer>();
        _coins = 0;
        _gameover = false;
        _src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        if (_inputX != 0)
            _playerRigidBody.AddForce(Vector2.right * _inputX * _speed * Time.deltaTime);
        
        _rend.flipX = (_inputX < 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _wallLayer);

        if (hit.collider != null)
        {
            _canJump = true;
        }
        
        if (_canJump && Input.GetKeyDown(KeyCode.Space))
        {
            _playerRigidBody.AddForce(Vector2.up * _jumpHeight);
            _canJump = false;
        }

        Debug.DrawRay(transform.position, Vector2.down * _rayLength);

        if (_iframe > 0) 
            _iframe -= Time.deltaTime;
        
        if (!_hurt && Input.GetKeyDown(KeyCode.LeftControl))
            Damage(2);
        
        _healthImage.fillAmount = Mathf.Lerp(_healthImage.fillAmount, _health/_maxHealth, Time.deltaTime * 10f);
        _coinsText.text = "X " + _coins.ToString();

        _anim.SetBool("moving", _inputX != 0);
        _anim.SetBool("canJump", _canJump);
        _anim.SetBool("hurt", _hurt);

        if(_gameover && Input.anyKeyDown)
        {
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1f;
        }
    }

    public void Damage(float amt)
    {
        if (_iframe < 0)
        {
            _health -= amt;
            _hurt = true;
            Invoke("ResetHurt", 0.2f);
            if (_health <= 0)
            {
                _healthImage.fillAmount = 0f;
                GameOver();
            }
            _iframe = _timeBetweenHurt;
        }
    }

    private void GameOver()
    {
        _gameover = true;
        _gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }
    void ResetHurt()
    {
        _hurt = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            _coins++;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _playerRigidBody.velocity.y < 0)
        {
            float boundsY = collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y/2;
            if(transform.position.y > collision.gameObject.transform.position.y + boundsY)
            {
                _playerRigidBody.AddForceAtPosition(-_playerRigidBody.velocity.normalized * _jumpHeight/2f, _playerRigidBody.position);
                _src.Play();
                collision.gameObject.GetComponent<EnemyController>().Damage(5f);
            }
        }
    }
}
