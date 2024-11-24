using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Map : MonoBehaviour
{
    public int mapSize = 10; // The size of the map so 5 means that things can spawn with (-5,-5) to (5,5)
    public GameObject[] regionPrefabs;
    public int numSpawns = 10; // The number of things to spawn in the map
    public Transform startObject;
    public float dashLength = 0.2f;
    public float gapLength = 0.1f;
    public float lineWidth = 0.1f;
    public Material lineMaterial;
    public Canvas canvas;
    public Button buttonPrefab;
    public GameObject caveCanvas;
    public GameObject bossCanvas;
    public GameObject baseCanvas;
    public GameObject resourceCanvas;
    public GameObject current;
    private List<GameObject> regions;



    private LineRenderer lineRenderer;
    private Button button;

private static Map _singleton;
    public static Map Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                print($"{nameof(Map)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    private void OnEnable()
    
    {

        // Create LineRenderer component
        if (gameObject.GetComponent<LineRenderer>() == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }
        lineRenderer.enabled = false;

        // Create button
        if (button == null)
        {
            button = Instantiate(buttonPrefab, canvas.transform);
        }
        button.gameObject.SetActive(false);

        if (regions == null)
        {
            regions = new();

        }
        if (regions.Count > 0)
        {
            foreach (GameObject obj in regions)
            {
                obj.SetActive(true);
            }
        }
        GenerateMap();
    }
    private void OnDisable()
    {
        if(regions != null) {
            if (regions.Count > 0)
            {
                foreach (GameObject obj in regions)
                {
                    if (obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }
    }
    private void GenerateMap()
    {
        if (regions.Count == 0)
        {
            for (int i = 0; i < numSpawns; i++)
            {
                GameObject region = regionPrefabs[Random.Range(0, regionPrefabs.Length)];
                Vector3 spawnPos = new Vector3(Random.Range(-mapSize, mapSize), 0, Random.Range(-mapSize, mapSize));
                GameObject regionObject = Instantiate(region, spawnPos, region.transform.rotation);
                regions.Add(regionObject);
            }
        }
    }

    public void ShowDashedLineAndButton(Transform endObject)
    {
        if (startObject == null || endObject == null)
        {
            Debug.LogError("Start or end object is not assigned!");
            return;
        }

        Vector3 startPosition = startObject.position;
        Vector3 endPosition = endObject.position;

        // Calculate dash count
        float lineLength = Vector3.Distance(startPosition, endPosition);
        int dashCount = Mathf.FloorToInt(lineLength / (dashLength + gapLength));

        // Set up line renderer
        lineRenderer.positionCount = dashCount * 2;
        lineRenderer.enabled = true;

        // Create dashed line
        for (int i = 0; i < dashCount; i++)
        {
            float startT = i * (dashLength + gapLength) / lineLength;
            float endT = startT + dashLength / lineLength;

            lineRenderer.SetPosition(i * 2, Vector3.Lerp(startPosition, endPosition, startT));
            lineRenderer.SetPosition(i * 2 + 1, Vector3.Lerp(startPosition, endPosition, endT));
        }

        // Position button in the middle of the line
        Vector3 buttonPosition = Vector3.Lerp(startPosition, endPosition, 0.5f);
        button.transform.position = Camera.main.WorldToScreenPoint(buttonPosition);
        Region selectedRegion = endObject.gameObject.GetComponent<Region>();
        if(selectedRegion.regionType == RegionType.cave)
        {
            button.gameObject.GetComponent<TravelButton>().UpdateButton(caveCanvas, current, (int) lineLength*100);
        }
        else if(selectedRegion.regionType == RegionType.boss)
        {
            button.gameObject.GetComponent<TravelButton>().UpdateButton(bossCanvas, current, (int) lineLength*100);
        }
        else if(selectedRegion.regionType == RegionType.home)
        {
            button.gameObject.GetComponent<TravelButton>().UpdateButton(baseCanvas, current, (int) lineLength*100);
        }
        else if(selectedRegion.regionType == RegionType.resource)
        {
            button.gameObject.GetComponent<TravelButton>().UpdateButton(resourceCanvas, current, (int) lineLength*100);
        }
        button.gameObject.SetActive(true);
    }

    public void HideDashedLineAndButton()
    {
        if (lineRenderer != null)
        {
            Invoke(nameof(DisableLineAndButton), 0.1f);
        }
    }
    void DisableLineAndButton()
    {
        lineRenderer.enabled = false;
        if (button.gameObject.activeSelf)
        {

            button.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        Singleton = this;
        gameObject.SetActive(false);
    }
}
