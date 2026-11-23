//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class Tablet : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase

    [SerializeField] private GameObject PanelTablet;
    [SerializeField] private GameObject PanelInicio;
    [SerializeField] private GameObject PanelAlmacen;
    [SerializeField] private GameObject PanelEmpleados;
    [SerializeField] private GameObject PanelEstadoZonas;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static Tablet _instance;

    /// <summary>
    /// Lista de zonas activas
    /// </summary>
    private List<ZonaAnimal> _zonasAnimalesCompradas = new();

    private List<int> _sacosComidaAlmacenados = new();

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de este manager al que ya existe.
            // Esto permitirá al GameManager real mantener su estado interno
            // pero acceder a los elementos de la nueva escena
            // o bien olvidar los de la escena previa de la que venimos
            TransferManagerSetup();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
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
    private void Update()
    {
        if (!PanelTablet.activeSelf && InputManager.Instance.TabletIsPressed())
        {
            PanelTablet.SetActive(true);
            PanelInicio.SetActive(true);
            PanelAlmacen.SetActive(false);
            PanelEmpleados.SetActive(false);
            PanelEstadoZonas.SetActive(false);
        }
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

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static Tablet Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    public void AddZona(ZonaAnimal zona)
    {
        _zonasAnimalesCompradas.Add(zona);
        _sacosComidaAlmacenados.Add(0);
    }
    
    public void CompraYAlmacenaSacos(int cantidad, ZonaAnimal zona)
    {
        if (_zonasAnimalesCompradas.Contains(zona) && GameManager.Instance.GastaDinero(zona.DimePrecioComida() * cantidad))
        {
            _sacosComidaAlmacenados[_zonasAnimalesCompradas.IndexOf(zona)]+= cantidad;
        }
    }

    public int DimeSacosAlmacenados(ZonaAnimal zona)
    {
        if (_zonasAnimalesCompradas.Contains(zona))
        {
            return _sacosComidaAlmacenados[_zonasAnimalesCompradas.IndexOf(zona)];
        }
        else return 0;
    }
    public int TraerSacosAlmacenados(ZonaAnimal zona)
    {
        if (_zonasAnimalesCompradas.Contains(zona))
        {
            int sacos = _sacosComidaAlmacenados[_zonasAnimalesCompradas.IndexOf(zona)];
            _sacosComidaAlmacenados[_zonasAnimalesCompradas.IndexOf(zona)] = 0;
            return sacos;
        }
        else return 0;
    }
    public List<ZonaAnimal> DameListaZonasCompradas()
    {
        return _zonasAnimalesCompradas;
    }
    public void ActivaPanelAlmacen()
    {
        PanelInicio.SetActive(false);
        PanelAlmacen.SetActive(true);
    }
    public void ActivaPanelEmpleados()
    {
        PanelInicio.SetActive(false);
        PanelEmpleados.SetActive(true);
    }
    public void ActivaPanelEstadoZonas()
    {
        PanelInicio.SetActive(false);
        PanelEstadoZonas.SetActive(true);
    }
    public void Volver()
    {
        if (PanelInicio.activeSelf)
        {
            PanelTablet.SetActive(false);
        }
        else if (PanelAlmacen.activeSelf)
        {
            PanelAlmacen.SetActive(false);
            PanelInicio.SetActive(true);
        }
        else if (PanelEmpleados.activeSelf)
        {
            PanelEmpleados.SetActive(false);
            PanelInicio.SetActive(true);
        }
        else if (PanelEstadoZonas.activeSelf)
        {
            PanelEstadoZonas.SetActive(false);
            PanelInicio.SetActive(true);
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    /// <summary>
    /// Metodo que transfiere el setup
    /// </summary>
    private void TransferManagerSetup()
    {
        // De momento no hay que transferir ningún setup
        // a otro manager
    }
    /// <summary>
    /// Metodo que inicializa las variables
    /// </summary>
    private void Init()
    {

    }

    #endregion

} // class Tablet 
// namespace
