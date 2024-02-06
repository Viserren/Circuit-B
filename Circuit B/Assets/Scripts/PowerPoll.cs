using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPoll : MonoBehaviour
{
    [SerializeField] Vector3 _area;
    [SerializeField] bool _isSphere;
    LineRenderer _lineRenderer;
    GameObject _player;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _dropScale;
    [SerializeField] Transform _startPoint;
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, _startPoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _area.x / 2, _layerMask);
        if (hitColliders.Length == 1)
        {
            _player = hitColliders[0].gameObject;
            Vector3 direction = _player.transform.position - transform.position;
            int distance = Mathf.CeilToInt(Vector3.Distance(_player.transform.position, transform.position));
            _lineRenderer.positionCount = distance + 1;
            for (int i = 0; i < distance; i++)
            {
                float d = (float)i / (float)distance;
                _lineRenderer.SetPosition(i, _startPoint.position + (direction * d) + (Vector3.down * direction.magnitude * _dropScale) * Mathf.Sin(d * Mathf.PI));
            }
            _lineRenderer.enabled = _lineRenderer != null ? true : false;
            _lineRenderer?.SetPosition(distance, _player.transform.position);
        }
        else
        {
            _player = null;
            _lineRenderer.enabled = _lineRenderer != null ? false : false;
        }
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = Color.green;

        if (_isSphere)
        {
            Gizmos.DrawWireSphere(Vector3.zero, _area.x/2);
        }
        else
        {
            Gizmos.DrawWireCube(Vector3.zero, _area);
        }
    }
}
