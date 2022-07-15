using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuilding : MonoBehaviour
{
    public GameObject boxPrefab;
    public LayerMask goundMask;

    GameObject spawnedBuilding;
    Grid grid;

    bool isPlacingBuilding;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isPlacingBuilding)
            {
                isPlacingBuilding = true;
                spawnBox();
            }
            else
            {
                BuildFacility();
            }
        }

        if (isPlacingBuilding)
        {
            PlacingFacility();
        }
    }

    void spawnBox()
    {
        spawnedBuilding = Instantiate(boxPrefab);

    }

    void PlacingFacility()
    {
        if (Input.GetButtonDown("Rotation"))
        {
            RotateFacility();
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, goundMask))
        {
            Debug.Log(hit.transform.name);
            Node nodeUnderHit = grid.NodeFromWorldPoint(hit.point);
            float nodePositionX = nodeUnderHit.worldPosition.x;
            float nodePositionY = nodeUnderHit.worldPosition.z;
            float xOffset = (spawnedBuilding.GetComponent<Facility>().sizeX - 1) * grid.nodeRadius;
            float yOffset = (spawnedBuilding.GetComponent<Facility>().sizeY - 1) * grid.nodeRadius;

            nodePositionX = Mathf.Clamp(nodePositionX, -grid.gridWorldSize.x / 2, grid.gridWorldSize.x / 2 - 2 * xOffset);
            nodePositionY = Mathf.Clamp(nodePositionY, -grid.gridWorldSize.y / 2 + 2 * yOffset, grid.gridWorldSize.y / 2);

            nodeUnderHit = grid.NodeFromWorldPoint(new Vector3(nodePositionX, 0, nodePositionY));

            spawnedBuilding.transform.position = nodeUnderHit.worldPosition + new Vector3(xOffset, 0, -yOffset);
        }
    }

    void RotateFacility()
    {
        if (spawnedBuilding != null)
        {
            spawnedBuilding.transform.Rotate(Vector3.up * 90);
            int xSize = spawnedBuilding.GetComponent<Facility>().sizeX;
            spawnedBuilding.GetComponent<Facility>().sizeX = spawnedBuilding.GetComponent<Facility>().sizeY;
            spawnedBuilding.GetComponent<Facility>().sizeY = xSize;
        }
    }

    void BuildFacility()
    {
        isPlacingBuilding = false;

        float xOffset = (spawnedBuilding.GetComponent<Facility>().sizeX - 1) * grid.nodeRadius;
        float yOffset = (spawnedBuilding.GetComponent<Facility>().sizeY - 1) * grid.nodeRadius;

        float xPosition = spawnedBuilding.transform.position.x - xOffset;
        float yPosition = spawnedBuilding.transform.position.z - yOffset;

        float xPositionFinish = spawnedBuilding.transform.position.x + xOffset;
        float yPositionFinish = spawnedBuilding.transform.position.z + yOffset;
        while (xPosition <= xPositionFinish)
        {
            float yPositionStart = yPosition;
            while(yPosition <= yPositionFinish)
            {
                grid.NodeFromWorldPoint(new Vector3(xPosition,0, yPosition)).walkable = false; //Заносим информацию в клетки под зданием
                yPosition += grid.nodeRadius * 2;
            }
            yPosition = yPositionStart;
            xPosition += grid.nodeRadius * 2;
        }

        spawnedBuilding = null;
    }
}
