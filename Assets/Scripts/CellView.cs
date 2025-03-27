using UnityEngine;
using UnityEngine.UI;
using TMPro; // Если вы используете TextMeshPro

public class CellView : MonoBehaviour
{
    // Ссылка на текстовый элемент, где будем отображать значение.
    // Можно использовать обычный Text или TextMeshProUGUI
    [SerializeField] public TextMeshProUGUI valueText;

    private Cell cell;
    private GameField gameField; // Чтобы через GameField вычислять позицию

    /// <summary>
    /// Инициализирует CellView ссылкой на Cell и подписывается на события.
    /// </summary>
    public void Init(Cell newCell, GameField field)
    {
        cell = newCell;
        gameField = field;

        // Подписка на события
        cell.OnValueChanged += UpdateValue;
        cell.OnPositionChanged += UpdatePosition;

        // Первоначальное обновление
        UpdateValue();
        UpdatePosition();
    }

    /// <summary>
    /// Обновляет визуальное значение клетки.
    /// </summary>
    private void UpdateValue()
    {
        if (valueText != null && cell != null)
        {
            double displayedNumber = System.Math.Pow(2, cell.Value);
            valueText.text = displayedNumber.ToString();
        }
    }

    private void UpdatePosition()
    {
        if (cell != null && gameField != null)
        {
            // Преобразуем координаты клетки из массива в позицию на поле
            transform.localPosition = gameField.CalculateCellPosition(cell.Position);
        }
    }
    
    /// <summary>
    /// Обновляет позицию на экране согласно позиции в Cell.
    /// </summary>
    private void OnDestroy()
    {
        if (cell != null)
        {
            cell.OnValueChanged -= UpdateValue;
            cell.OnPositionChanged -= UpdatePosition;
        }
    }
}