using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateHex : MonoBehaviour
{

    [SerializeField] float hexSize;
    [SerializeField] int gridSizeX;
    [SerializeField] int gridSizeY;

    [SerializeField]
    public Material material;

    [SerializeField]
    Vector3 origin;

    [SerializeField]
    bool debugSpheres;
    [SerializeField]
    public Vector3 debugScale;

    // Start is called before the first frame update
    void Start()
    {
        float width = Mathf.Sqrt(3) * hexSize;
        float height = 2 * hexSize;

        float horizonal_spacing = width;
        float vertical_spacing = (3f / 4f) * height;
        
        Debug.Log("origin = " + origin);
        Debug.Log("Horizontal spacing = " + horizonal_spacing);
        Debug.Log("Vertical spacing = " + vertical_spacing);

        for (int row = 0; row < gridSizeY; row++)
        {
            
            Vector3 center = origin;
            center.y += vertical_spacing * row;
            
            for (int column = 0; column < gridSizeX; column++)
            {
                center.x = origin.x + horizonal_spacing * column;
                // we want even gridSizeY offset to slot into the tops of the hexagons
                if (((row + 1) % 2) == 0)
                {
                    Debug.Log("Offseting row " + row);
                    center.x += 0.5f * horizonal_spacing;
                }
                Debug.Log("Hex " + row + ":" + column + " center is: " + center);
                create_hex(center, row + ":" + column);
                
            }
        }
    }
    private void create_hex(Vector3 hex_center,  string name)
    {
        GameObject gameObject = new GameObject("Hex " + name, typeof(MeshFilter), typeof(MeshRenderer));
        gameObject.transform.SetParent(transform, false);
        Transform parentTransform = gameObject.transform;

        Vector3[] vertices = new Vector3[6];
        int[] triangles = new int[12];

        for (int i = 0; i < vertices.Length; i++)
        {
            float angle_deg = 60 * i - 30;
            float angle_rad = Mathf.PI / 180 * angle_deg;
            vertices[i] = new Vector3(
                hex_center.x + hexSize * Mathf.Cos(angle_rad),
                hex_center.y + hexSize * Mathf.Sin(angle_rad)
            );
            if (debugSpheres == true)
                placeDebugMarker(vertices[i], "Vertice " + i, parentTransform);
        }

        triangles[0] = 0;
        triangles[1] = 5;
        triangles[2] = 1;

        triangles[3] = 5;
        triangles[4] = 2;
        triangles[5] = 1;

        triangles[6] = 5;
        triangles[7] = 4;
        triangles[8] = 2;

        triangles[9] = 4;
        triangles[10] = 3;
        triangles[11] = 2;

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        if (debugSpheres == true)
            placeDebugMarker(hex_center, "hexCenter " + name, parentTransform);

    }

    private void placeDebugMarker(Vector3 center, string name, Transform parent)
    {
        GameObject centerMark = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        centerMark.transform.SetParent(parent, false);
        centerMark.transform.localPosition = center;
        centerMark.transform.localScale = debugScale;
        centerMark.name=name;
    }
}
