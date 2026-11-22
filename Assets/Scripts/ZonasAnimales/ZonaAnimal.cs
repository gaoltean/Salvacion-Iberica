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
    [SerializeField] private float DuracionComida = 0;
    /// <summary>
    /// Cuanto tarda en ensuciarse del todo (0 = Por defecto)
    /// </summary>
    [SerializeField] private float DurazionLimpieza = 0;
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

    private float _momentoInicioGestion = 0;
    private float _momentoFinGestion = 0;

    private float _porcentajeHambre = 0;
    private float _porcentajeSuciedad = 0;

    private bool _limpiezaUrgente = false;
    private bool _alimentacionUrgente = false;

    private int _sacosComida = 0;

    private float _disminucionComida = 0.5f;
    private float _disminucionLimpieza = 0.2f;
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
        if(SpritesAnimales != null) SpritesAnimales.SetActive(false);
        if (DuracionComida != 0) _disminucionComida = 100f / DuracionComida;
        if (DurazionLimpieza != 0) _disminucionLimpieza = 100f / DurazionLimpieza;
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
            if (_porcentajeHambre > 100) _porcentajeHambre = 100;
            // Baja la limpieza
            if (_porcentajeSuciedad < 100) _porcentajeSuciedad += _disminucionLimpieza * Time.deltaTime;
            if (_porcentajeSuciedad > 100) _porcentajeSuciedad = 100;
            // Alimentacion urgente
            if (_porcentajeHambre > 70) _alimentacionUrgente = true;
            // Limpieza urgente
            if (_porcentajeSuciedad > 70) _limpiezaUrgente = true;
        }
        TextoSacosComida.text = "Sacos de comida: " + _sacosComida;
        TextoProductosLimpieza.text = "Productos de limpieza: " + GameManager.Instance.DimeProductosLimpieza();
        TextoComida.text = "Hambre: " + (int)(_porcentajeHambre) + "%";
        TextoSuciedad.text = "Suciedad: " + (int)_porcentajeSuciedad + "%";
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PanelGestion.activeSelf && !TextoGestionar.activeSelf) TextoGestionar.SetActive(true);
        if (!PanelGestion.activeSelf && InputManager.Instance.Interact && Time.time > _momentoFinGestion + 1)
        {
            _momentoInicioGestion = Time.time;
            PanelGestion.SetActive(true);
            TextoGestionar.SetActive(false);
        }
        if (PanelGestion.activeSelf && InputManager.Instance.Interact && Time.time > _momentoInicioGestion + 1)
        {
            _momentoFinGestion = Time.time;
            PanelGestion.SetActive(false);
            TextoGestionar.SetActive(true);
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
        if(SpritesAnimales != null) SpritesAnimales.SetActive(true);
        TextoNoAnimales.SetActive(false);
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
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    #endregion

} // class ManagerZoanAnimales 
// namespace
