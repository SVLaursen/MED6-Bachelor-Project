using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private List<string> collisionTags;
    [SerializeField] private GameObject playerCharacter;
    public  GameObject playerAvatar;

    [Header("Haptics")]
    [SerializeField] private bool useHaptics;
    [SerializeField] private HapticFeedbackComponent hapticComponent;
    
    [Header("Auditory")]
    [SerializeField] private bool useAuditory;
    [SerializeField] private AuditoryFeedbackComponent auditoryComponent;

    [Header("Visual")]
    [SerializeField] private bool useVisual;
    [SerializeField] private VisualFeedbackComponent visualComponent;

    private List<FeedbackComponent> _loadedComponents;

    public GameObject PlayerCharacter => playerCharacter;
    public HapticFeedbackComponent HapticComponent => hapticComponent;
    public VisualFeedbackComponent VisualComponent => visualComponent;
    public AuditoryFeedbackComponent AuditoryComponent => auditoryComponent;
    public bool UseHaptics => useHaptics;
    public bool UseAuditory => useAuditory;

    public static ModalController Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        //Setting volume to 0 so that it is not enabled by default.
        AudioListener.volume = 0f;

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        _loadedComponents = new List<FeedbackComponent>();

        if (useHaptics)
            _loadedComponents.Add(hapticComponent);
        if (useAuditory)
            _loadedComponents.Add(auditoryComponent);
        if (useVisual)
            _loadedComponents.Add(visualComponent);
    }

    private void Start()
    {
        if (_loadedComponents.Count > 0)
            foreach (var component in _loadedComponents)
            {
                Debug.Log("Loading components...");
                StartCoroutine(ComponentLoading(component));
            }        
    }

    public void OnTouchEnter(OVRInput.Controller controller, Collider collision)
    {
        if (_loadedComponents.Count > 0)
            foreach (var component in _loadedComponents)
                component.OnEnter(controller, collision);
    }

    public void OnTouchStay(OVRInput.Controller controller, Collider collision)
    {
        if (_loadedComponents.Count > 0)
            foreach (var component in _loadedComponents)
                component.OnStay(controller, collision);
    }

    public void OnTouchExit(OVRInput.Controller controller, Collider collision)
    {
        if (_loadedComponents.Count > 0)
            foreach (var component in _loadedComponents)
                component.OnExit(controller, collision);
    }

    private IEnumerator ComponentLoading(FeedbackComponent component)
    {
        while (!component.Loaded)
        {
            component.Init(collisionTags);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
