using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardSetting
{
    public int xSize, ySize;
    public Tile tileGo;
    public List<Sprite> tileSprite;

  
}


public class GameManager : MonoBehaviour
{
    [Header("Параметры игровой доски")]
    public BoardSetting boardSetting;

    void Start()
    {
        BoardController.instance.SetValue(Board.instance.SetValue(boardSetting.xSize, boardSetting.ySize, boardSetting.tileGo, boardSetting.tileSprite),
            boardSetting.xSize, boardSetting.ySize, boardSetting.tileSprite);
      

    }

    void Update()
    {

    }
}
