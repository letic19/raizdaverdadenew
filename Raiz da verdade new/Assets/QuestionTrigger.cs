using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class QuestionTrigger : MonoBehaviour
{
    public GameObject perguntaPanel; 
    public Tilemap bridgeTilemap; 
    public TileBase bridgeTile; 

    public Vector3Int tilePosition; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            perguntaPanel.SetActive(true);
            Time.timeScale = 0f; 
        }
    }

    
    public void RespostaCerta()
    {
        bridgeTilemap.SetTile(tilePosition, bridgeTile);
        perguntaPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    
    public void RespostaErrada()
    {
        perguntaPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}