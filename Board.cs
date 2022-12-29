using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board instance;

    private int xSize, ySize;
    private Tile tileGO;
    private List<Sprite> tileSprite = new List<Sprite>();

    private void Awake()
    {
        instance = this;
    }

    public Tile[,] SetValue(int xSize, int ySize, Tile tileGO, List<Sprite> tileSprite)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        this.tileGO = tileGO;
        this.tileSprite = tileSprite;

       return CreateBoard();
    }

    private Tile[,] CreateBoard()
    {

        Tile[,] tileArray = new Tile[xSize, ySize];
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 tileSize = tileGO.spriteRenderer.bounds.size;
        Sprite cashSprite = null;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Tile newTile = Instantiate(tileGO, transform.position, Quaternion.identity);
                newTile.transform.position = new Vector3(xPos + (tileSize.x * x), yPos + (tileSize.y * y), 0);

                newTile.transform.parent = transform;

                tileArray[x, y] = newTile;

                List<Sprite>tempSprite = new List<Sprite>();
                tempSprite.AddRange(tileSprite);

                tempSprite.Remove(cashSprite);
                if (x > 0)
                {
                    tempSprite.Remove(tileArray[x - 1, y].spriteRenderer.sprite);
                }
                
                newTile.spriteRenderer.sprite = tempSprite[Random.Range(0, tempSprite.Count)];
                cashSprite = newTile.spriteRenderer.sprite;

            }
        }
        return tileArray;


    }
}
