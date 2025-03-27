using System;
using UnityEngine;

public class Cell
{
    //Поля
    private Vector2Int _position;
    private int _value;

    //События
    public event Action OnValueChanged;
    public event Action OnPositionChanged;

    //Свойства
    public Vector2Int Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                OnPositionChanged?.Invoke();
            }
        }
    }

    public int Value
    {
        get => _value;
        set
        {
            if (this._value != value)
            {
                this._value = value;
                OnValueChanged?.Invoke();
            }
        }
    }

    // Конструктор
    public Cell(Vector2Int startPos, int startValue)
    {
        _position = startPos;
        _value = startValue;
    }
}