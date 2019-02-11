using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine2D : MonoBehaviour {

    [SerializeField]
    protected LineRenderer m_LineRenderer;
    [SerializeField]
    protected bool m_AddCollider = false;
    [SerializeField]
    protected EdgeCollider2D m_EdgeCollider2D;
    [SerializeField]
    protected PolygonCollider2D m_PolyCollider2D;
    [SerializeField]
    protected Camera m_Camera;
    protected List<Vector2> m_Points;

    [SerializeField]
    private GameObject cursor;

    [SerializeField]
    private float interval = 0.5f;
    private bool canDraw = true;

    private Vector2 endPoint;
    private Vector2 endPointBase;

    public virtual LineRenderer lineRenderer {
        get {
            return m_LineRenderer;
        }
    }

    public virtual bool addCollider {
        get {
            return m_AddCollider;
        }
    }

    public virtual EdgeCollider2D edgeCollider2D {
        get {
            return m_EdgeCollider2D;
        }
    }

    public virtual List<Vector2> points {
        get {
            return m_Points;
        }
    }

    protected virtual void Awake() {
        if (m_LineRenderer == null) {
            Debug.LogWarning("DrawLine: Line Renderer not assigned, Adding and Using default Line Renderer.");
            CreateDefaultLineRenderer();
        }
        if (m_EdgeCollider2D == null && m_AddCollider) {
            Debug.LogWarning("DrawLine: Edge Collider 2D not assigned, Adding and Using default Edge Collider 2D.");
            CreateDefaultEdgeCollider2D();
        }
        if (m_Camera == null) {
            m_Camera = Camera.main;
        }
        //m_LineRenderer.numCornerVertices = 5;
        //m_Points = new List<Vector2>();
        m_Points = new List<Vector2>(m_PolyCollider2D.points);

        endPoint = new Vector2(-10, -10);
        endPointBase = new Vector2(10, -10);
    }


    private IEnumerator ResetInterval() {
        canDraw = false;
        yield return new WaitForSeconds(interval);
        canDraw = true;
    }

    protected virtual void Update() {
        cursor.transform.position = new Vector3(cursor.transform.position.x, 0 + MicInput.MicLoudness * 20, 0);

        if (Input.GetMouseButtonDown(0)) {
            //Reset();
        }
        //if (Input.GetMouseButton(0) && canDraw) {
        if (canDraw) {
            //Debug.Log("Can draw");
            StartCoroutine(ResetInterval());
            //Vector2 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition = cursor.transform.position;

            //m_Points.Insert
            if (!m_Points.Contains(mousePosition)) {
                m_Points.Insert(0, mousePosition);

                m_Points[m_Points.Count - 2] = new Vector2(mousePosition.x + 0.1f, -20);
                m_Points[m_Points.Count - 1] = new Vector2(mousePosition.x, 0);
                
                if (m_Points.Count >= 100) {
                    m_Points.RemoveAt(m_Points.Count - 5);

                    //m_Points[m_Points.Count - 5] = new Vector2(mousePosition.x + 0.1f, -20);
                    m_Points[m_Points.Count - 4] = new Vector2(mousePosition.x - 20.0f, 0);
                    m_Points[m_Points.Count - 3] = new Vector2(mousePosition.x - 20.0f, -20);
                }

                m_LineRenderer.positionCount = m_Points.Count;
                m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 1, mousePosition);
                if (m_PolyCollider2D != null && m_AddCollider && m_Points.Count > 1) {
                    m_PolyCollider2D.points = m_Points.ToArray();
                }
            }
        }
    }

    protected virtual void Reset() {
        if (m_LineRenderer != null) {
            m_LineRenderer.positionCount = 0;
        }
        if (m_Points != null) {
            m_Points.Clear();
        }
        if (m_PolyCollider2D != null && m_AddCollider) {
            m_PolyCollider2D.pathCount = 0;
        }
    }

    protected virtual void CreateDefaultLineRenderer() {
        m_LineRenderer = gameObject.AddComponent<LineRenderer>();
        m_LineRenderer.positionCount = 0;
        m_LineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        m_LineRenderer.startColor = Color.white;
        m_LineRenderer.endColor = Color.white;
        m_LineRenderer.startWidth = 0.2f;
        m_LineRenderer.endWidth = 0.2f;
        m_LineRenderer.useWorldSpace = true;
    }

    protected virtual void CreateDefaultEdgeCollider2D() {
        m_EdgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
    }

}