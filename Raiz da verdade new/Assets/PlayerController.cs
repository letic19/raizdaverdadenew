using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Configurações de movimento")]
    public float velocidade = 5f;
    public float forcaPulo = 10f;

    [Header("Referências de UI")]
    public GameObject painelDePerguntas; 

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool noChao = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        float move = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) move = -velocidade;
        if (Input.GetKey(KeyCode.RightArrow)) move = velocidade;

        // Eu não aguento mais
        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);

        // Flip do sprite
        if (move != 0) sr.flipX = move < 0;

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && noChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }

    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
            noChao = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
            noChao = false;
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("painel"))
        {
            if (painelDePerguntas != null)
            {
                painelDePerguntas.SetActive(true); 
            }
            else
            {
                Debug.LogWarning("Painel de perguntas não atribuído no Inspector!");
            }
        }
    }
}