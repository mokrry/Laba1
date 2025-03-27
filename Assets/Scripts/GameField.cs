using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [Header("Настройки поля")]
    [SerializeField] public int fieldSize = 4;          // Квадратное поле 4x4
    [SerializeField] public GameObject cellPrefab;      // Префаб клетки с CellView
    [SerializeField] private GameObject gameFieldInsideCanvas; // Ссылка на Canvas, чтобы туда ставить клетки
    [SerializeField] private GameObject emptyCellPrefab; // Ссылка на префаб с пустой клеткой

    private List<Cell> cells = new List<Cell>();

    private void Start()
    {
        CreateField();
        CreateCell();
        CreateCell();
    }

    /// <summary>
    /// Возвращает координаты случайной пустой клетки на поле.
    /// </summary>
    private Vector2Int GetEmptyPosition()
    {
        List<Vector2Int> allPositions = new List<Vector2Int>();
        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                allPositions.Add(new Vector2Int(x, y));
            }
        }

        // Убираем уже занятые позиции
        foreach (var cell in cells)
        {
            if (allPositions.Contains(cell.Position))
            {
                allPositions.Remove(cell.Position);
            }
        }

        if (allPositions.Count == 0)
        {
            Debug.LogWarning("На поле нет свободных позиций!");
            return new Vector2Int(-1, -1);
        }

        int randomIndex = Random.Range(0, allPositions.Count);
        return allPositions[randomIndex];
    }

    /// <summary>
    /// Создаёт пустые ячейки на поле
    /// </summary>
    public void CreateField()
    {
        // Проверяем, назначен ли префаб пустой клетки
        if (emptyCellPrefab == null)
        {
            Debug.LogError("emptyCellPrefab не назначен в инспекторе!");
            return;
        }
        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                // Instantiate пустую клетку
                GameObject emptyCell = Instantiate(emptyCellPrefab, gameFieldInsideCanvas.transform);

                // Позиционируем её в соответствии с координатами (x, y)
                Vector2Int cellPos = new Vector2Int(x, y);
                emptyCell.transform.localPosition = CalculateCellPosition(cellPos);

                // Переименовываем для удобства, чтобы видеть в иерархии
                emptyCell.name = $"EmptyCell_{x}_{y}";
            }
        }
    }

    
    /// <summary>
    /// Создаёт новую клетку в случайной пустой позиции (90% - 1, 10% - 2).
    /// </summary>
    public void CreateCell()
    {
        Vector2Int emptyPos = GetEmptyPosition();
        if (emptyPos.x < 0 || emptyPos.y < 0)
        {
            Debug.LogWarning("Нет свободных позиций на поле!");
            return;
        }

        int newValue = Random.value < 0.9f ? 1 : 2;
        
        Cell newCell = new Cell(emptyPos, newValue);
        cells.Add(newCell);

        if (cellPrefab != null)
        {
            GameObject cellObj = Instantiate(cellPrefab, gameFieldInsideCanvas.transform);

            CellView view = cellObj.GetComponent<CellView>();
            view.Init(newCell, this);
        }
        else
        {
            Debug.LogError("cellPrefab не назначен в инспекторе!");
        }
    }

    /// <summary>
    /// Преобразует координаты клетки в локальные координаты для UI.
    /// </summary>
    public Vector3 CalculateCellPosition(Vector2Int cellPos)
    {
        float offset = 10f;
        float cellSize = 130f;
        float startX = -(560f / 2f) + (cellSize / 2f);
        float startY = -(560f / 2f) + (cellSize / 2f);

        float xPos = startX + cellPos.x * (offset + cellSize);
        float yPos = startY + cellPos.y * (offset + cellSize);

        return new Vector3(xPos, yPos, 0f);
    }
}
