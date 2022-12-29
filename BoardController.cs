using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;

    private int xSize, ySize;
    private List<Sprite> tileSprite = new List<Sprite>();
    private Tile[,] tileArray;

   

    private Tile oldSelectTile;

    //Вектор для обозначения границ смены соседних тайлов
    private Vector2[] dirRay = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private bool isFindMatch = false;

    //Поле отвечающее за начало и конец смещения тайлов, чтобы продолжить игру после смещения
    private bool isShift = false;

    //Поиск пустых тайлов в апдейте
    private bool isSearcEmptyTile = false;


    public void SetValue(Tile[,] tileArray, int xSize, int ySize, List<Sprite> tileSprite)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        this.tileArray = tileArray;
        this.tileSprite = tileSprite;
    }

    private void Awake()
    {
        instance = this;
    }

  

    // Update is called once per frame
    void Update()
    {
        //Поиск пустых тайлов
        if (isSearcEmptyTile)
        {
            SearchEmptyTile();
        }


        //проверка удара луча по сенсорному дисплею
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition)); 
            if (ray != false)
            {
                CheckSelectTile(ray.collider.gameObject.GetComponent<Tile>());
            }
        }
    }

    #region(выделение тайла, снятие выделения тайла, управление выделением)

    /// <summary>
    /// Метод для выделения тайлов:
    /// </summary>
    /// <param name="tile"></param>
    private void SelectTile(Tile tile)
    {
        tile.isSelected = true;
        tile.spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        oldSelectTile = tile;
    }

    ///<summary>
    /// Метод снятия выделения тайлов:
    /// </summary>
    /// <param name="tile"></param>
    private void DeselectTile(Tile tile)
    {
        tile.isSelected = false;
        tile.spriteRenderer.color = new Color(1, 1, 1);
        oldSelectTile = null;
    }

    ///Метод проверки на выделения тайла: <summary>
    /// Метод проверки на выделения тайла:
    /// </summary>
    /// <param name="tile"></param>
    private void CheckSelectTile(Tile tile)
    {
        if (tile.isEmpty || isShift)
        {
            return;
        }
        if (tile.isSelected)
        {
            DeselectTile(tile);
        }
        else
        {
            //Первое выделение
            if (!tile.isSelected && oldSelectTile == null)
            {
                SelectTile(tile);
            }
            //Попытка выбрать другой тайл
            else
            {
                //если второй выбранный тайл соседствует с предыдущим
                if (AdjacentTiles().Contains(tile))
                {
                    SwapTwoTile(tile);
                    FindAllMatch(tile);
                    DeselectTile(oldSelectTile);
                }
                //Выделение нового тайла, при текущем выделении, старый забываем
                else
                {
                    DeselectTile(oldSelectTile);
                    SelectTile(tile);
                }

            }
        }
    }
    #endregion

    #region (Поиск совпадения, удаление спрайтов,движение тайлов, смена спрайтов у тайлов)

    /// <summary>
    /// Поиск совпадений по горизонтали или вертикали.
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    private List<Tile> FindMatch(Tile tile, Vector2 dir)
    {
        List<Tile> cashFindTiles = new List<Tile>();
        RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, dir);
        while (hit.collider != null && hit.collider.gameObject.GetComponent<Tile>().spriteRenderer.sprite == tile.spriteRenderer.sprite)
        {
            cashFindTiles.Add(hit.collider.gameObject.GetComponent<Tile>());
            hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, dir);
        }
        return cashFindTiles;
    }


    /// Метод удаления тайлов выстроенных в ряд <summary>
    /// Метод удаления тайлов выстроенных в ряд
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="dirArray"></param>
    private void DeleteSprite(Tile tile, Vector2[] dirArray)
    {
        List<Tile> cashFindSprite = new List<Tile>();
        for (int i = 0; i < dirArray.Length; i++)
        {
            cashFindSprite.AddRange(FindMatch(tile, dirArray[i]));
        }
        if (cashFindSprite.Count >= 2)  
        {
            for (int i = 0; i < cashFindSprite.Count; i++)
            {
                cashFindSprite[i].spriteRenderer.sprite = null;
            }
            isFindMatch = true;
        }
    }
    ///Метод по поиску ВСЕХ совпадений <summary>
    /// Метод по поиску ВСЕХ совпадений
    /// </summary>
    /// <param name="tile"></param>
    private void FindAllMatch(Tile tile)
    {
        if (tile.isEmpty)
        {
            return;
        }
        DeleteSprite(tile, new Vector2[2] { Vector2.up, Vector2.down });
        DeleteSprite(tile, new Vector2[2] { Vector2.left, Vector2.right });
        if (isFindMatch)
        {
            isFindMatch = false;
            tile.spriteRenderer.sprite = null;
            isSearcEmptyTile = true;
        }
    }

    #endregion

    #region (Смена 2 тайлов местами, поиск соседних тайлов)

    /// <summary>
    /// Метод обмена тайлов местами
    /// </summary>
    /// <param name="tile"></param>
    private void SwapTwoTile(Tile tile)
    {
        if (oldSelectTile.spriteRenderer.sprite == tile.spriteRenderer.sprite)
        {
            return;
        }
        Sprite cashSprite = oldSelectTile.spriteRenderer.sprite;
        oldSelectTile.spriteRenderer.sprite = tile.spriteRenderer.sprite;
        tile.spriteRenderer.sprite = cashSprite;

        //Каждый ход отнимает ХП у игрока
        UI.instance.Moves(-3);

    }

    /// <summary>
    /// Метод поиска соседних тайлов
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    private List<Tile> AdjacentTiles()
    {
        List<Tile> cashTiles = new List<Tile>();
        for (int i = 0; i < dirRay.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(oldSelectTile.transform.position, dirRay[i]);
            if (hit.collider != null)
            {

                cashTiles.Add(hit.collider.gameObject.GetComponent<Tile>());
            }
        }
        return cashTiles;
    }
    #endregion

    #region(Поиск пустого тайла, сдвиг тайла вниз, замена изображения, поиск нового изображения)

    ///Метод поиска пустых тайлов <summary>
    /// Метод поиска пустых тайлов
    /// </summary>
    private void SearchEmptyTile()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (tileArray[x,y].isEmpty)
                {
                    ShiftTileDown (x,y);
                    break;
                }
                if (x == xSize && y == ySize - 1)  //Косытль
                {
                    isSearcEmptyTile = false;
                }
            }
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                FindAllMatch(tileArray[x, y]);
            }
        }

    }

    ///Метод смещения тайлов в пустые по оcи Y <summary>
    /// Метод смещения тайлов в пустые по оcи Y
    /// </summary>
    private void ShiftTileDown(int xPos, int yPos)
    {
        isShift = true;

        List<SpriteRenderer> cashRenderer = new List<SpriteRenderer> ();
        int count = 0; 
        for (int y = yPos; y < ySize; y++)
        {
            Tile tile  = tileArray[xPos,y];
            if (tile.isEmpty)
            {
                count++;
            }
            cashRenderer.Add(tile.spriteRenderer);
        }
        for (int i = 0; i < count; i++)
        {
            SetNewSprite(xPos, cashRenderer);

            //Каждый цикл отнимает ХП у врага
            UI.instance.Score(-( 3+i ));
        }


        isShift = false;
    }

    ///Метод установки изображений у пустых тайлов <summary>
    /// Метод установки изображений у пустых тайлов
    /// </summary>
    private void SetNewSprite(int xPos, List<SpriteRenderer> renderer)
    {
        for (int y = 0; y < renderer.Count -1; y++)
        {
            renderer[y].sprite = renderer[y + 1].sprite;
            renderer[y + 1].sprite = GetNewSprite(xPos, ySize - 1);
        }
    }
    ///Метод установки нового тайла с учетом окружающих тайлов <summary>
    /// Метод установки нового тайла с учетом окружающих тайлов
    /// </summary>
    private Sprite GetNewSprite(int xPos, int yPos)
    {
        List<Sprite> cashSprite = new List<Sprite>();
        cashSprite.AddRange(tileSprite);
        if (xPos > 0)
        {
            cashSprite.Remove(tileArray[xPos - 1, yPos].spriteRenderer.sprite);

        }
        if (xPos < xSize-1)
        {
            cashSprite.Remove(tileArray[xPos + 1, yPos].spriteRenderer.sprite);
        }
        if (yPos > 0)
        {
            cashSprite.Remove(tileArray[xPos, yPos - 1].spriteRenderer.sprite);   
        }
    
        if (yPos < ySize -1)
        {
            cashSprite.Remove(tileArray[xPos, yPos +1].spriteRenderer.sprite);
        }
        return cashSprite[Random.Range(0, cashSprite.Count)];
    }

    #endregion





}
