using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public GameObject _referenceSquare;
    public int _rowCount = 5;
    public int _colCount = 8;
    public float _tileSize = 1.05f;
    public float _messageDuration = 0.5f;
    public GameObject _messagePanel;
    public GameObject _messageText;

    private List<List<GameObject>> _squares;
    private int _battleMode = 0;
    private int _moveCount = 0;
    private bool _showMessage = false;
    private float _showMessageDuration = 0.0f;

    void Start()
    {
        ConfigureBoard();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            _battleMode = _battleMode == 1 ? 0 : 1;
        }

        // TODO: break message handling out into separate controller
        if (_showMessage)
        {
            _showMessageDuration += Time.deltaTime;
            if (_showMessageDuration >= _messageDuration)
            {
                HideMessage();
                _showMessage = false;
            }
        }
    }

    void ConfigureBoard()
    {
        _squares = new List<List<GameObject>>();
        for (var row = 0; row < _rowCount; row++)
        {
            var squaresRow = new List<GameObject>();
            for (var col = 0; col < _colCount; col++)
            {
                GameObject newSquare = Instantiate(_referenceSquare, transform);
                newSquare.transform.position = new Vector2(col * _tileSize, row * -_tileSize);
                newSquare.GetComponent<SquareController>().SetBoardPosition(row, col);
                if (Random.Range(0,2) > 0)
                {
                    newSquare.GetComponent<SquareController>().ToggleColor(false);
                }
                squaresRow.Add(newSquare);
            }
            _squares.Add(squaresRow);
        }

        float gridW = _colCount * _tileSize;
        float gridH = _rowCount * _tileSize;
        transform.position = new Vector2(-gridW / 2 + _tileSize / 2, gridH / 2 - _tileSize / 2);
    }

    public void ToggleSquares(int row, int col, bool hint = false)
    {
        foreach (var squaresRow in _squares)
        {
            foreach (var square in squaresRow)
            {
                square.GetComponent<SquareController>().ResetAlpha();
            }
        }

        switch (_battleMode)
        {
            case 0:            
                if (row > 0)
                {
                    _squares[row - 1][col].GetComponent<SquareController>().ToggleColor(hint);            
                }
                if (col > 0)
                {
                    _squares[row][col - 1].GetComponent<SquareController>().ToggleColor(hint);            
                }
                if (row < _rowCount - 1)
                {
                    _squares[row + 1][col].GetComponent<SquareController>().ToggleColor(hint);            
                }
                if (col < _colCount - 1)
                {
                    _squares[row][col + 1].GetComponent<SquareController>().ToggleColor(hint);            
                }
                break;
            case 1:
                if (row > 0 && col > 0)
                {
                    _squares[row - 1][col - 1].GetComponent<SquareController>().ToggleColor(hint);            
                }
                if (row < _rowCount - 1 && col < _colCount - 1)
                {
                    _squares[row + 1][col + 1].GetComponent<SquareController>().ToggleColor(hint);            
                }
                if (row > 0 && col < _colCount - 1)
                {
                    _squares[row - 1][col + 1].GetComponent<SquareController>().ToggleColor(hint);            
                }
                if (row < _rowCount - 1 && col > 0)
                {
                    _squares[row + 1][col - 1].GetComponent<SquareController>().ToggleColor(hint);            
                }
                break;        
        }
        _squares[row][col].GetComponent<SquareController>().ToggleColor(hint);   

        if (!hint)
        {
            _moveCount += 1;
            CheckForComplete();
            ShowMessage();         
        }
    }

    void HideMessage()
    {
        var image = _messagePanel.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.b, image.color.g, 0.0f);
        _messageText.GetComponent<Text>().text = "";
    }

    void ShowMessage()
    {
        _showMessage = true;
        var image = _messagePanel.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.b, image.color.g, 0.4f);

        _messageText.GetComponent<Text>().text = _moveCount.ToString();
        _showMessageDuration = 0.0f;
    }

    void CheckForComplete()
    {
        int completeCount = 0;
        foreach (var squaresRow in _squares)
        {
            foreach (var square in squaresRow)
            {
                if (square.GetComponent<SpriteRenderer>().color == Color.black)
                {
                    completeCount++;
                }
            }            
        }
        if (completeCount == _colCount * _rowCount)
        {
            ResetBoard();
        }
    }

    void ResetBoard()
    {
        foreach (var squaresRow in _squares)
        {
            foreach (var square in squaresRow)
            {
                if (Random.Range(0,2) > 0)
                {
                    square.GetComponent<SquareController>().ToggleColor(false);
                }
            }            
        }
        _moveCount = 0;
    }
}
