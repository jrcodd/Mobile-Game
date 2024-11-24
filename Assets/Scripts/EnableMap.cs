using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMap : MonoBehaviour
{
    [SerializeField] private GameObject map;

    private void OnEnable()
    {
        map.SetActive(true);
    }
}
