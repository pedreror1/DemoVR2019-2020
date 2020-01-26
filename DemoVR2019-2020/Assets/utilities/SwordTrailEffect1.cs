using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SwordTrailEffect1 : MonoBehaviour
{
    public bool enableSlash;
    [SerializeField] private bool isDebug = true;
    [SerializeField] private Transform weaponStart;
    [SerializeField] private Transform weaponEnd;
    [SerializeField] private int MaxMeshNumber = 10;
    [Range(1, 100)] [SerializeField] int catMullIterations = 10;
    private Mesh mesh; 
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> UV = new List<Vector2>();
    private List<int> triangles = new List<int>();
    private List<Vector3> startPoints = new List<Vector3>();
    private List<Vector3> endPoints = new List<Vector3>();
    private List<Vector3> catmullromStartPositions = new List<Vector3>();
    private List<Vector3> catmullromEndPositions = new List<Vector3>();
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }    
    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            Gizmos.color = Color.green;
            for (int i = 1; i < catmullromStartPositions.Count; i++)
            {
                    Gizmos.DrawLine(catmullromStartPositions[i - 1], catmullromStartPositions[i]);
                    Gizmos.DrawLine(catmullromEndPositions[i - 1], catmullromEndPositions[i]);
                 
            }
        }
    }
    Vector3 catmull(float t, Vector3 p0,Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return  0.5f * ( (2*p1)+(-p0+p2) *t+(2*p0-5*p1+4*p2-p3)*(t*t)+(-p0+3*p1-3*p2+p3)*(t*t*t));
    }       
     public void updatemaxMesh(UnityEngine.UI.Slider s)
    {
        MaxMeshNumber = (int)s.value;
        s.GetComponentInChildren<UnityEngine.UI.Text>().text = s.value.ToString();
    }
    public void updateminDist(UnityEngine.UI.Slider s)
    {
        mindist = (int)s.value;
        s.GetComponentInChildren<UnityEngine.UI.Text>().text = s.value.ToString();
    }
    void CreateCatMullMesh()
    {
        mesh.Clear();
        vertices.Clear();
        UV.Clear();
        triangles.Clear();
        float UvIndex = 0f;
        int TrianglesIndex = 0;
        for (int i = 0; i < MaxMeshNumber; i++)
        {
            vertices.AddRange(new Vector3[]
              {
                   transform.InverseTransformPoint(catmullromStartPositions[i]),  transform.InverseTransformPoint(catmullromEndPositions[i]),transform.InverseTransformPoint(catmullromStartPositions[i + 1]), 
                   transform.InverseTransformPoint(catmullromStartPositions[i + 1]),transform.InverseTransformPoint(catmullromEndPositions[i]),transform.InverseTransformPoint(catmullromEndPositions[i + 1])
              }); 
            triangles.AddRange(new int[] { TrianglesIndex, TrianglesIndex+1, TrianglesIndex+2, TrianglesIndex+3,TrianglesIndex+4, TrianglesIndex+5 });         
            UV.AddRange(new Vector2[] { new Vector2(UvIndex,0f),new Vector2(UvIndex,1f),new Vector2(UvIndex+1f/MaxMeshNumber,0f),new Vector2(UvIndex+1f/MaxMeshNumber,0f),
                                                         new Vector2(UvIndex,1f),new Vector2(UvIndex+1f/MaxMeshNumber,1f)});
            TrianglesIndex +=6;
            UvIndex +=1f/MaxMeshNumber;
        }  

        mesh.vertices = vertices.ToArray();
        mesh.uv = UV.ToArray();
        mesh.triangles = triangles.ToArray();     
    }
    public float mindist = 0f;
    [Range(1,99)]
    public int resetSpeed = 25;
    private void 　Update()
    {       
        if(enableSlash)
        {           
            if (startPoints.Count >= MaxMeshNumber+1)
            {
                startPoints.RemoveRange(0,resetSpeed);
                endPoints.RemoveRange(0,resetSpeed);               
            }
            if (catmullromEndPositions.Count >= MaxMeshNumber + 1)
            {              
                catmullromStartPositions.RemoveRange(0, catMullIterations);
                catmullromEndPositions.RemoveRange(0,   catMullIterations);
            }

            if (endPoints.Count>2 && Vector3.Distance(endPoints[endPoints.Count - 2], weaponEnd.position) > mindist)
            {

                endPoints.Add(weaponEnd.position);
                startPoints.Add(weaponStart.position);
            }
            else if(endPoints.Count<=2)
            {
                endPoints.Add(weaponEnd.position);
                startPoints.Add(weaponStart.position);
            }
            
            if (startPoints.Count>=4)
            {
                for(float j=0f;j<1f;j+=1f/(float)catMullIterations)
                {
                    int i = startPoints.Count;
                    catmullromStartPositions.Add( catmull(j, startPoints[i - 4], startPoints[i - 3], startPoints[i - 2], startPoints[i - 1]));
                    catmullromEndPositions.Add(catmull(j, endPoints[i - 4], endPoints[i - 3], endPoints[i - 2], endPoints[i - 1]));
                }                
            }         
            if (catmullromStartPositions.Count >= MaxMeshNumber+1 )
            {
                CreateCatMullMesh();
            }
        }
        else
        {
            mesh.Clear();
            vertices.Clear();
            UV.Clear();
            triangles.Clear();
            startPoints.Clear();
            endPoints.Clear();
            catmullromEndPositions.Clear();
            catmullromStartPositions.Clear();
        }
    }


     


}
