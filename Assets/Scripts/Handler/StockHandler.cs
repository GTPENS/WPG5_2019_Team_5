using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockHandler : MonoBehaviour
{
    [SerializeField] GameObject[] stocks;

    public void updateStocksPrice(List<Stock> stocksData)
    {
        for (int i = 0; i < stocks.Length; i++)
        {
            Text text = stocks[i].GetComponentInChildren<Text>();
            text.text = $"${stocksData[i].price}";
        }
    }
}
