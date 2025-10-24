using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuestionTrigger : MonoBehaviour
{
    [Header("Referências")]
    public GameObject perguntaPanel;
    public Tilemap bridgeTilemap;
    public TileBase bridgeTile;

    [Header("Posição do tile (mundo)")]
    public Vector3 tilePosition; // posição em world space que você define no Inspector
    public bool useLocalOffset = false; // se true, usa transform.position + tilePosition

    [Header("Debug/Visual")]
    public bool showGizmo = true;

    private Vector3Int? previousTilePosition = null;
    private bool jaAtivado = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{name} OnTriggerEnter2D com {other.name} (tag: {other.tag})");

        if (!other.CompareTag("Player"))
            return;

        if (jaAtivado)
        {
            Debug.Log($"{name}: já ativado antes, ignorando.");
            return;
        }

        if (perguntaPanel == null)
        {
            Debug.LogError($"{name}: perguntaPanel NÃO atribuído!");
            return;
        }

        perguntaPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log($"{name}: abriu painel e pausou o tempo.");
    }

    // Chamado pelo botão do painel quando acerta
    public void RespostaCerta()
    {
        Vector3 worldPos = useLocalOffset ? (transform.position + tilePosition) : tilePosition;
        Vector3Int cellPosition = bridgeTilemap.WorldToCell(worldPos);

        Debug.Log($"{name} RespostaCerta() → worldPos: {worldPos} → cell: {cellPosition}");
        if (previousTilePosition.HasValue)
        {
            Debug.Log($"{name}: removendo tile anterior em {previousTilePosition.Value}");
            bridgeTilemap.SetTile(previousTilePosition.Value, null);
        }

        bridgeTilemap.SetTile(cellPosition, bridgeTile);
        previousTilePosition = cellPosition;
        jaAtivado = true;

        perguntaPanel.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log($"{name}: tile colocado e painel fechado.");
    }

    public void RespostaErrada()
    {
        perguntaPanel.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log($"{name}: resposta errada, painel fechado.");
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmo || bridgeTilemap == null) return;

        Vector3 worldPos = useLocalOffset ? (transform.position + tilePosition) : tilePosition;
        Vector3Int cell = bridgeTilemap.WorldToCell(worldPos);
        Vector3 center = bridgeTilemap.GetCellCenterWorld(cell);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, new Vector3(bridgeTilemap.cellSize.x, bridgeTilemap.cellSize.y, 0.1f));

        // label in editor (only visible in Scene view)
#if UNITY_EDITOR
        Handles.color = Color.white;
        Handles.Label(center + Vector3.up * (bridgeTilemap.cellSize.y * 0.6f), $"{name}\nCell: {cell}");
#endif
    }
}