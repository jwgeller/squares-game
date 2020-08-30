using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareController : MonoBehaviour
{
    private int _row;
    private int _col;

    public void SetBoardPosition(int row, int col)
    {
        _row = row;
        _col = col;
    }

    public void ToggleColor(bool hint)
    {
        var sr = GetComponent<SpriteRenderer>();

        if (hint)
        {
            sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, 0.15f);
        }
        else
        {
            sr.color = sr.color == Color.white ? Color.black : Color.white;
        }
    }

    public void ResetAlpha()
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, 1.0f);
    }

    void OnMouseUp()
    {
        if (Input.touchCount > 1)
        {
            GameObject.Find("Board").GetComponent<BoardController>().SetSquareToggleEnabled(false);
        }
        GameObject.Find("Board").GetComponent<BoardController>().ToggleSquares(_row, _col);
    }    

    void OnMouseOver()
    {
        GameObject.Find("Board").GetComponent<BoardController>().ToggleSquares(_row, _col, true);
    }
}
