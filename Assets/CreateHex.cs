using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHex : MonoBehaviour
{

    [SerializeField] float hexSize;
    [SerializeField] int rowSize;
    [SerializeField] int rows;

    [SerializeField]
    public Material material;

    [SerializeField]
    public Vector3 markerScale;

    // Start is called before the first frame update
    void Start()
    {
        float width = Mathf.Sqrt(3) * hexSize;
        float height = 2 * hexSize;

        float horizonal_spacing = width;
        float vertical_spacing = (3f / 4f) * height;

        //we use negative values so when we add the values for row 1 it doesnt
        //move off of 0,0
        Vector3 origin = new Vector3(0, 0, 0);
        
        Debug.Log("origin = " + origin);
        Debug.Log("Horizontal spacing = " + horizonal_spacing);
        Debug.Log("Vertical spacing = " + vertical_spacing);

        for (int row = 0; row < rows; row++)
        {
            
            Vector3 center = origin;
            center.y += vertical_spacing * row;
            
            for (int column = 0; column < rowSize; column++)
            {
                center.x = horizonal_spacing * column;
                // we want even rows offset to slot into the tops of the hexagons
                if (((row + 1) % 2) == 0)
                {
                    Debug.Log("Offseting row " + row);
                    center.x += 0.5f * horizonal_spacing;
                }
                Debug.Log("Hex " + row + ":" + column + " center is: " + center);
                create_hex(center, hexSize, row + ":" + column);
                
            }
        }
    }
    private void create_hex(Vector3 hex_center, float hex_size, string name)
    {
        GameObject gameObject = new GameObject("Hex " + name, typeof(MeshFilter), typeof(MeshRenderer));
        gameObject.transform.SetParent(transform, false);
        Transform parentTransform = gameObject.transform;

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[12];

        for (int i = 0; i < vertices.Length; i++)
        {
            float angle_deg = 60 * i - 30;
            float angle_rad = Mathf.PI / 180 * angle_deg;
            vertices[i] = new Vector3(
                hex_center.x + hex_size * Mathf.Cos(angle_rad),
                hex_center.y + hex_size * Mathf.Sin(angle_rad)
            );
            uv[i] = new Vector2(vertices[i].x, vertices[i].y);
            placeMarker(vertices[i], "Vertice " + i, parentTransform);
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
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        placeMarker(hex_center, "hexCenter " + name, parentTransform);

    }

    private void placeMarker(Vector3 center, string name, Transform parent)
    {
        GameObject centerMark = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        centerMark.transform.SetParent(parent, false);
        centerMark.transform.localPosition = center;
        centerMark.transform.localScale = markerScale;
        centerMark.name=name;
    }
}
