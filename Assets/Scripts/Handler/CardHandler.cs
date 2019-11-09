using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onCardClick);
    }

    void onCardClick()
    {
        Destroy(gameObject);
    }
}
