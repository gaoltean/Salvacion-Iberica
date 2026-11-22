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
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase

    /// <summary>
    /// Dinero con el que empiezas la partida
    /// </summary>
    [SerializeField] private float DineroInicial = 0;
    [SerializeField] private int ProductosLimpiezaInicial = 10;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Dinero que tienes actualmente
    /// </summary>
    private float _dinero;

    /// <summary>
    /// Productos de limpieza que tienes actualmente
    /// </summary>
    private int _productosLimpieza;
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
    public static GameManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Suma una cantidad a tu dinero actual
    /// </summary>
    /// <param name="cantidad">Cantidad a sumar</param>
    public void GanaDinero(float cantidad)
    {
        _dinero += cantidad;
    }
    /// <summary>
    /// Reduce el dinero una cantidad si tienes dinero suficiente
    /// </summary>
    /// <param name="cantidad">Cantidad a gastar</param>
    /// <returns>True cuando has podido gastar el dinero</returns>
    public bool GastaDinero(float cantidad)
    {
        bool ret = _dinero - cantidad > 0;
        if (ret) _dinero -= cantidad;
        return ret;
    }
    /// <summary>
    /// Gasta 1 de productos de limpieza cuando tienes suficientes
    /// </summary>
    /// <returns>True cuando has podido gastar</returns>
    public bool GastaProductosLimpieza()
    {
        bool ret = _productosLimpieza - 1 > 0;
        if (ret) _productosLimpieza -= 1;
        return ret;
    }
    /// <summary>
    /// Aumenta la cantidad de productos de limpieza cuando tienes dinero suficiente
    /// </summary>
    /// <param name="cantidad">Cantidad a comprar</param>
    /// <param name="precio">Precio de la comida</param>
    /// <returns>True si la compra se ha realizado</returns>
    public bool CompraProductosLimpieza(int cantidad, float precio)
    {
        bool ret = _dinero - precio > 0;
        if (ret) 
        { 
            _productosLimpieza += cantidad; 
            _dinero -= precio;
        }
        return ret;
    }

    public float DimeDinero()
    {
        return _dinero;
    }
    public int DimeProductosLimpieza()
    {
        return _productosLimpieza;
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
        _dinero = DineroInicial;
        _productosLimpieza = ProductosLimpiezaInicial;
    }

    #endregion

} // class GameManager 
// namespace
