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

    private List<Vector2> initialPoints;

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

        initialPoints = new List<Vector2>();

        for (int i = 0; i < m_Points.Count; i++) {
            initialPoints.Add(m_Points[i]);
        }
    }

    public void ResetPolygon() {
        m_PolyCollider2D.points = initialPoints.ToArray();
    }


    private IEnumerator ResetInterval() {
        canDraw = false;
        yield return new WaitForSeconds(interval);
        canDraw = true;
    }

    protected virtual void Update() {
        cursor.transform.position = new Vector3(cursor.transform.position.x, 0 + MicInput.MicLoudness * 20, 0);

        if (canDraw) {
            StartCoroutine(ResetInterval());
            Vector2 mousePosition = cursor.transform.position;
            
            if (!m_Points.Contains(mousePosition) && m_Points.Count >= 5) {
                m_Points.Insert(0, mousePosition);

                m_Points[m_Points.Count - 2] = new Vector2(mousePosition.x + 1.0f, -30);
                m_Points[m_Points.Count - 1] = new Vector2(mousePosition.x + 0.01f, 0);

                float lastX = m_Points[m_Points.Count - 6].x;

                m_Points[m_Points.Count - 5] = new Vector2(lastX - 10.0f, 0);
                m_Points[m_Points.Count - 4] = new Vector2(lastX - 10.0f, -30);
                m_Points[m_Points.Count - 3] = new Vector2(lastX - 0.0f, -30);

                if (m_Points.Count >= 200) {
                    m_Points.RemoveAt(m_Points.Count - 6);
                }

                m_LineRenderer.positionCount = m_Points.Count;
                m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 1, mousePosition);
                if (m_PolyCollider2D != null && m_AddCollider && m_Points.Count >= 5) {
                    m_PolyCollider2D.points = m_Points.ToArray();
                }
            }
        }
    }

    public void Reset() {
        if (m_LineRenderer != null) {
            m_LineRenderer.positionCount = 0;
        }
        if (m_Points != null) {
            m_Points.Clear();
        }
        if (m_PolyCollider2D != null && m_AddCollider) {
            m_PolyCollider2D.pathCount = 0;

            for (int i = 0; i < initialPoints.Count; i++) {
                m_Points.Add(initialPoints[i]);
            }

            m_PolyCollider2D.points = m_Points.ToArray();
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