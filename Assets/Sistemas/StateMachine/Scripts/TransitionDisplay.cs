using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StateMachine
{
    public class TransitionDisplay : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] int minPoints = 4;
        [SerializeField] int maxPoints = 16;
        [SerializeField] Vector3 offset;
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] CanvasGroup dropdownCG;
        [SerializeField] SpriteRenderer arrowhead;

        [SerializeField, HideInInspector] LineRenderer line;
        [SerializeField, HideInInspector] MeshCollider meshCollider;
        Vector3[] points;
        bool targetsMoved = false;
        Vector3 endPosition;
        Vector3 startPosition;

        public StateTransitionPort FromPort { get; set; }
        public StateTransitionPort ToPort { get; set; }
        public TMP_Dropdown Dropdown => dropdown;
        public Transition Transition { get; set; }
        public Vector3 EndPosition
        {
            get => endPosition;
            set => (targetsMoved, endPosition) = (true, value);
        }
        public Vector3 StartPosition
        {
            get => startPosition;
            set => (targetsMoved, startPosition) = (true, value);
        }

        void Start()
        {
            points = new Vector3[maxPoints];
            dropdownCG = dropdown.GetComponent<CanvasGroup>();
            dropdown.ClearOptions();
            dropdown.AddOptions(Input.Patterns.Select(i => ((InputPattern)i).ToString()).ToList());
            dropdown.SetValueWithoutNotify((int)InputPattern.Horizontal);
            dropdown.onValueChanged.AddListener(value =>
            {
                Transition.InputPattern = (InputPattern)value;
            });
        }

        void OnValidate()
        {
            line = GetComponent<LineRenderer>();
            meshCollider = GetComponent<MeshCollider>();
        }

        void Update()
        {
            var startMoved = FromPort?.transform ? FromPort.transform.hasChanged : false;
            var endMoved = ToPort?.transform ? ToPort.transform.hasChanged : false;
            targetsMoved |= startMoved | endMoved;

            if (!targetsMoved)
                return;

            var p0 = (FromPort?.transform ? FromPort.transform.position : StartPosition) + offset;
            var p3 = (ToPort?.transform ? ToPort.transform.position : EndPosition) + offset;
            var lerp05 = Vector3.Lerp(p0, p3, 0.05f);
            var lerp95 = Vector3.Lerp(p0, p3, 0.95f);
            var p1 = lerp95.x * Vector3.right + lerp05.y * Vector3.up;
            var p2 = lerp05.x * Vector3.right + lerp95.y * Vector3.up;

            var distance = (int)Vector3.Distance(p0, p3);
            var samples = Mathf.Clamp(distance, minPoints, maxPoints);

            for (int s = 0; s < samples; s++)
            {
                var t = (float)s / (samples - 1);
                var a = Vector3.Lerp(p0, p1, t);
                var b = Vector3.Lerp(p1, p2, t);
                var c = Vector3.Lerp(p2, p3, t);
                var d = Vector3.Lerp(a, b, t);
                var e = Vector3.Lerp(b, c, t);
                var f = Vector3.Lerp(d, e, t);

                points[s] = f;
            }

            line.positionCount = samples;
            line.SetPositions(points);

            var mesh = new Mesh();
            line.BakeMesh(mesh, true);
            meshCollider.sharedMesh = mesh;

            (dropdown.transform.parent as RectTransform).anchoredPosition3D = (samples % 2) switch
            {
                1 => points[samples / 2],
                _ => (points[(samples - 1) / 2] + points[samples / 2]) / 2,
            };

            var direction = FromPort.Type switch
            {
                StateTransitionPort.PortType.Entry => points[0] - points[2],
                _ => points[samples - 1] - points[samples - 3]
            };
            var euler = new Vector3
            {
                z = Mathf.Rad2Deg * Mathf.Atan2(-direction.x, direction.y)
            };
            var position = FromPort.Type switch
            {
                StateTransitionPort.PortType.Entry => points[0],
                _ => points[samples - 1]
            };

            arrowhead.transform.eulerAngles = euler;
            arrowhead.transform.position = position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var state = dropdownCG.blocksRaycasts;
            dropdownCG.blocksRaycasts = !state;
            dropdownCG.alpha = state == true ? 0 : 1;
        }

        void LateUpdate()
        {
            targetsMoved = false;
            if (FromPort?.transform)
                FromPort.transform.hasChanged = false;
            if (ToPort?.transform)
                ToPort.transform.hasChanged = false;
        }
    }
}