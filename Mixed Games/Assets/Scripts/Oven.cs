using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectControll;



public class Oven : MonoBehaviour
{
    public Player player;
    private bool _isOrderReceived;
    private int _objectCount;
    private int _rowCount;
    private int _columnCount;
    private int _currentColumn = -1;
    private int _currentRow = 0;
    private int _objectLevel = 0;

    private int _indexCounter;
    private Color _color;

    private IEnumerator _cookingCoroutine;

    public List<Transform> madePizzasPool = new List<Transform>();

    Vector3 initialSlotPosition;

    private void Start()
    {
        (initialSlotPosition, _rowCount, _columnCount) = ObjectSizeLibrary.ObjectSizeCalculation(madePizzasPool[0], transform);
        _cookingCoroutine = startCookingRoutine(0.5f);
        setPackageColor(Color.yellow);
        _isOrderReceived = true;
    }

    private void Update()
    {
        if (_isOrderReceived)
        {
            _isOrderReceived = false;
            StartCoroutine(_cookingCoroutine);
        }
    }
    public void setOrderStatus(bool isReceived)
    {
        _isOrderReceived = isReceived;
    }

    public void setPackageColor(Color c) 
    { 
        _color = c; 
    }

    public int getPizzaCount()
    {
        return madePizzasPool.Count;
    }

    public int PlayerInteraction()
    {
        StopCoroutine(_cookingCoroutine);

        int pizaCounter = 0;

        for (int i = 0; i < madePizzasPool.Count; i++)
        {
            if (madePizzasPool[i].gameObject.activeInHierarchy)
            {
                madePizzasPool[i].gameObject.SetActive(false);

                pizaCounter++;
            }
        }

        return pizaCounter;
    }

    public void AllReset()
    {
        resetObjectCount();
        resetObjectPosition();
    }

    public void resetObjectPosition()
    {
        _currentColumn = -1;
        _currentRow = 0;
        _objectLevel = 0;
    }

    public void resetObjectCount()
    {
        _objectCount = 0;
        _indexCounter = 0;
    }

    public int GetIndexCounter()
    {
        return _indexCounter;
    }

    public void SetIndexCounter()
    {
        _indexCounter = 0;
    }

    private IEnumerator startCookingRoutine(float t)
    {
        _objectCount = 0;
        _indexCounter = 0;

        while (_objectCount < madePizzasPool.Count)
        {

            _currentColumn++;

            if (_currentColumn >= _columnCount)
            {
                _currentRow++;
                _currentColumn = 0;
            }

            if (_currentRow >= _rowCount)
            {
                _objectLevel++;
                _currentRow = 0;
                _currentColumn = 0;
            }

            for (int i = 0; i < madePizzasPool.Count; i++)
            {
                if (!madePizzasPool[i].gameObject.activeInHierarchy)
                {
                    madePizzasPool[i].gameObject.SetActive(true);
                    madePizzasPool[i].position = initialSlotPosition + new Vector3(((madePizzasPool[i].lossyScale.x) * _currentColumn),
                                                                                    (_objectLevel * madePizzasPool[i].lossyScale.y),
                                                                                    ((-madePizzasPool[i].lossyScale.z) * _currentRow));

                    madePizzasPool[i].GetComponent<MeshRenderer>().material.color = _color;

                    _objectCount++;
                    _indexCounter++;

                    if (_objectCount == madePizzasPool.Count)
                    {
                        for (int j = 0; j < madePizzasPool.Count; j++)
                        {
                            if (madePizzasPool[j].gameObject.activeInHierarchy)
                            {
                                yield return new WaitForSeconds(.1f);
                                madePizzasPool[j].GetComponent<MeshRenderer>().material.color = Color.black;
                                yield return new WaitForSeconds(.1f);
                                madePizzasPool[j].gameObject.SetActive(false);
                                _objectCount--;

                                if (_objectCount == 0)
                                {
                                    _currentRow = 0;
                                    _currentColumn = -1;
                                    _objectLevel = 0;
                                }
                            }
                        }
                    }
                    yield return new WaitForSeconds(0.5f);
                    break;
                }
            }
        }
    }


    







        // INSTANTIATE ÝLE OBJELERÝ OLUÞTURMA

        //private IEnumerator CreateBookOnTheTable(Transform book)
        //{
        //    while (true)
        //    {
        //        currentColumn++;

        //        if (currentColumn >= columnCount)
        //        {
        //            currentRow++;
        //            currentColumn = 0;
        //        }

        //        if (currentRow >= rowCount)
        //        {
        //            bookLevel++;
        //            currentRow = 0;
        //            currentColumn = 0;
        //        }

        //        Transform myBook = Instantiate(book, (initialSlotPosition + new Vector3(((book.lossyScale.x) * currentColumn), (bookLevel * book.lossyScale.y), ((-book.lossyScale.z) * currentRow))), Quaternion.identity);
        //        myBook.parent = bookParent;
        //        myBook.rotation = transform.rotation;
        //        myBook.GetComponent<MeshRenderer>().material.color = Color.blue;
        //        _player.collectableBlueBooks.Add(myBook);

        //        yield return new WaitForSeconds(0.5f);
        //    }
        //}
    
}

