//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using TMPro;
using UnityEngine;



/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class ZonaAnimal : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase

    [SerializeField] private GameObject TextoGestionar;
    [SerializeField] private GameObject PanelGestion;
    [SerializeField] private TextMeshProUGUI TextoComida;
    [SerializeField] private TextMeshProUGUI TextoSacosComida;
    [SerializeField] private TextMeshProUGUI TextoSuciedad;
    [SerializeField] private TextMeshProUGUI TextoProductosLimpieza;
    [SerializeField] private GameObject TextoNoAnimales;
    /// <summary>
    /// Sacos de comida que tienes al desbloquear una zona
    /// </summary>
    [SerializeField] private int SacosIniciales = 10;
    /// <summary>
    /// Precio de un saco de comida
    /// </summary>
    [SerializeField] private float PrecioComida = 10.0f;
    /// <summary>
    /// Sacos de comida necesarios para alimentar
    /// </summary>
    [SerializeField] private int ComidaParaAlimentar;
    /// <summary>
    /// Cuanto tarda en gastarse la comida (0 = Por defecto)
    /// </summary>
    [SerializeField] private float DuracionComida = 300;
    /// <summary>
    /// Tiempo que aguantan los animales sin morirse con 100% de hambre
    /// </summary>
    [SerializeField] private float ResistenciaALaInanicion = 300;
    /// <summary>
    /// Cuanto tarda en ensuciarse del todo (0 = Por defecto)
    /// </summary>
    [SerializeField] private float DurazionLimpieza = 600;
    /// <summary>
    /// Dinero necesario para desbloquear animales en la zona
    /// </summary>
    [SerializeField] private float PrecioAnimales;
    /// <summary>
    /// Contenedor de objetos de animales que se desbloquean
    /// </summary>
    [SerializeField] private GameObject SpritesAnimales;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase

    private float _porcentajeHambre = 0;
    private float _porcentajeSuciedad = 0;
    private float _porcentajeMuerte = 0;

    private bool _limpiezaUrgente = false;
    private bool _alimentacionUrgente = false;

    private int _sacosComida = 0;

    private float _disminucionComida = 0;
    private float _disminucionLimpieza = 0;
    private float _disminucionSaludPorHambre = 0;
    private bool _contieneAnimales = false;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _sacosComida = SacosIniciales;
        PanelGestion.SetActive(false);
        TextoGestionar.SetActive(false);
        TextoNoAnimales.SetActive(true);
        SpritesAnimales.SetActive(false);
        _disminucionComida = 100f / DuracionComida;
        _disminucionLimpieza = 100f / DurazionLimpieza;
        _disminucionSaludPorHambre = 100f / ResistenciaALaInanicion;
        TextoGestionar.GetComponent<TextMeshProUGUI>().text = "Gestionar " + this.name + ": 'E'";
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_contieneAnimales)
        {
            // Baja la comida
            if (_porcentajeHambre < 100) _porcentajeHambre += _disminucionComida * Time.deltaTime;
            // Alimentacion urgente
            else if (_porcentajeHambre > 100)
            {
                _porcentajeHambre = 100;
                _alimentacionUrgente = true;
            } 
            // Baja la limpieza
            if (_porcentajeSuciedad < 100) _porcentajeSuciedad += _disminucionLimpieza * Time.deltaTime;

            // Limpieza urgente
            else if (_porcentajeSuciedad > 100)
            {
                _porcentajeSuciedad = 100;
                _limpiezaUrgente = true;
            }
            // Alimentacion urgente
            if (_alimentacionUrgente)
            {
                _porcentajeMuerte += _disminucionSaludPorHambre * Time.deltaTime;
                if (_porcentajeMuerte > 100)
                {
                    _contieneAnimales = false;
                    SpritesAnimales.SetActive(false);
                    _alimentacionUrgente = false;
                    _porcentajeMuerte = 0;
                }
            }
        }
        TextoSacosComida.text = "Sacos de comida: " + _sacosComida;
        TextoProductosLimpieza.text = "Productos de limpieza: " + GameManager.Instance.DimeProductosLimpieza();
        TextoComida.text = "Hambre: " + (int)(_porcentajeHambre) + "%";
        TextoSuciedad.text = "Suciedad: " + (int)_porcentajeSuciedad + "%";
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PanelGestion.activeSelf && !TextoGestionar.activeSelf) TextoGestionar.SetActive(true);
        if (!PanelGestion.activeSelf && InputManager.Instance.Interact)
        {
            PanelGestion.SetActive(true);
            if (TextoNoAnimales.activeSelf != !_contieneAnimales) TextoNoAnimales.SetActive(!_contieneAnimales);
            TextoGestionar.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PanelGestion.SetActive(false);
        TextoGestionar.SetActive(false);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    public void Alimenta()
    {
        if (_porcentajeHambre > 10 && _sacosComida > 0)
        {
            _sacosComida--; 
            _porcentajeHambre = 0;
        }
    }
    public void LimpiaZona() 
    {
        if (_porcentajeSuciedad > 10 && GameManager.Instance.GastaProductosLimpieza()) _porcentajeSuciedad = 0;
    }

    public bool ContieneAnimales()
    {
        return _contieneAnimales;
    }

    public void CompraAnimales()
    {
        _contieneAnimales = true;
        SpritesAnimales.SetActive(true);
    }

    public void CompraComida(int Cantidad)
    {
        if (GameManager.Instance.GastaDinero(PrecioComida * Cantidad)) _sacosComida += Cantidad;
    }
    public int DimeSacosComida()
    {
        return _sacosComida;
    }
    public float DimePrecioAnimales()
    {
        return PrecioAnimales;
    }

    public float DimePrecioComida()
    {
        return PrecioComida;
    }
    public void SalirGestion()
    {
        PanelGestion.SetActive(false);
        TextoGestionar.SetActive(true);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    #endregion

} // class ManagerZoanAnimales 
// namespace
