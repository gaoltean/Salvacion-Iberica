//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class TimeManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    [SerializeField] private float TiempoDia;
    [SerializeField] private float LuzDia;
    [SerializeField] private float LuzNoche;
    [SerializeField] private float TiempoCambio;
    [SerializeField] private GameObject PanelFinDia;
    [SerializeField] private GameObject BotonFinDia;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    private static TimeManager _instance;
    private bool _dia;
    private bool _cambio;
    private float _startTime;
    private Light2D _luz;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    protected void Awake()
    {
        if (_instance != null)
        {
            //Si no somos la instancia nos transferimos
            TransferManagerSetup();

            //Nos destruimos
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        } // if-else somos instancia nueva o no.
    }
    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _luz = this.gameObject.GetComponent<Light2D>();
        _dia = true;
        _cambio = false;
        _startTime = Time.time;
        _luz.intensity = LuzDia;
        PanelFinDia.SetActive(false);
        BotonFinDia.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_dia && Time.time - _startTime > TiempoDia && !_cambio)
        {
            Debug.Log("Dia");
            _cambio = true;
            _startTime = Time.time;
        }
        if (_dia && _cambio && Time.time - _startTime > TiempoCambio)
        {
            Debug.Log("cambio a noche");
            _luz.intensity -= 0.01f;
            _startTime = Time.time;
            if (_luz.intensity <= LuzNoche)
            {
                _luz.intensity = LuzNoche;
                _dia = false;
                _cambio = false;
                _startTime = Time.time;
                BotonFinDia.SetActive(true);
            }
        }
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static TimeManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }
    /// <summary>
    /// Nos dice si hay instancia
    /// </summary>
    /// <returns></returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }
    /// <summary>
    /// Nos indica si es dia
    /// </summary>
    /// <returns></returns>
    public bool IsDay()
    {
        return !_cambio;
    }
    public bool IsNight()
    {
        return !_dia;
    }
    public void ChangeToDay()
    {
        _dia = true;
        _startTime = Time.time;
        _cambio = false;
        _luz.intensity = LuzDia;
        PanelFinDia.SetActive(false);
    }
    public void ActivaPanelFinDia()
    {
        BotonFinDia.SetActive(false);
        PanelFinDia.SetActive(true);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    private void Init()
    {
        // De momento no hay nada que inicializar
    }

    private void TransferManagerSetup()
    {
        // De momento no hay que transferir ningún setup
    }
    #endregion

} // class TimeManager 
// namespace
