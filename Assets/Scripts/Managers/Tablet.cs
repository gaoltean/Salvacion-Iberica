//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;



/// <summary>
/// Tablet de gestion para ver y gestionar algunos datos del parque
/// </summary>
public class Tablet : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase

    [SerializeField] private GameObject PanelTablet;
    [SerializeField] private GameObject MenuInicio;
    [SerializeField] private GameObject MenuAlmacen;
    [SerializeField] private GameObject MenuEmpleados;
    [SerializeField] private GameObject MenuEstados;
    [SerializeField] private GameObject MenuCuadros;
    [SerializeField] private Transform ContenedorAlmacen;
    [SerializeField] private Transform ContenedorEmpleados;
    [SerializeField] private Transform ContenedorEstados;
    [SerializeField] private Transform ContenedorCuadros;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static Tablet _instance;

    private float _momentoPuedeCerrarTablet = 0.0f;
    private float _momentoPuedeAbrirTablet = 0.0f;

    /// <summary>
    /// Lista de zonas activas
    /// </summary>
    private List<ZonaAnimal> _zonasCompradas = new();

    /// <summary>
    /// Lista de zonas mostradas en los menus
    /// </summary>
    private List<ZonaAnimal> _zonasMostradas = new();

    private List<GameObject> _elementosAlmacen = new();
    private List<TextMeshProUGUI> _textosElementosAlmacen = new();

    private List<GameObject> _elementosEstados = new();
    private List<TextMeshProUGUI> _textosElementosEstados = new();

    private List<int> _sacosComidaAlmacenados = new();
    private List<Pictures> _cuadrosAlmacenados = new();
    private List<Pictures> _cuadrosMostrados = new();
    private List<GameObject> _elementosCuadros = new();

    private RectTransform _contenedorAlmacenRect;
    private RectTransform _contenedorEmpleadosRect;
    private RectTransform _contenedorEstadosRect;
    private RectTransform _contenedorCuadrosRect;

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
    private void Start()
    {
        _contenedorAlmacenRect = ContenedorAlmacen.GetComponent<RectTransform>();
        _contenedorEmpleadosRect = ContenedorEmpleados.GetComponent<RectTransform>();
        _contenedorEstadosRect = ContenedorEstados.GetComponent<RectTransform>();
        _contenedorCuadrosRect = ContenedorCuadros.GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (!PanelTablet.activeSelf && Time.time > _momentoPuedeAbrirTablet && InputManager.Instance.TabletIsPressed())
        {
            PanelTablet.SetActive(true);
            MenuInicio.SetActive(true);
            MenuAlmacen.SetActive(false);
            MenuEmpleados.SetActive(false);
            MenuEstados.SetActive(false);
            MenuCuadros.SetActive(false);
            _momentoPuedeCerrarTablet = Time.time + 1.0f;
        }
        else
        {
            if (Time.time > _momentoPuedeCerrarTablet && InputManager.Instance.TabletIsPressed())
            {
                _momentoPuedeAbrirTablet = _momentoPuedeCerrarTablet + 1.0f;
                PanelTablet.SetActive(false);
            }
            else if (MenuAlmacen.activeSelf)
            {
                ActualizaContenedoresZonas();
            }
            else if (MenuEmpleados.activeSelf)
            {
                ActualizaContenedorEmpleados();
            }
            else if (MenuEstados.activeSelf)
            {
                ActualizaContenedoresZonas();
            }else if (MenuCuadros.activeSelf)
            {
                ActualizaContenerdorCuadros();
            }
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
        _zonasCompradas.Add(zona);
        _sacosComidaAlmacenados.Add(0);
    }
    public void AddCuadro(Pictures Cuadro)
    {
        _cuadrosAlmacenados.Add(Cuadro);
    }
    
    public void CompraYAlmacenaSacos(int cantidad, ZonaAnimal zona)
    {
        if (_zonasCompradas.Contains(zona) && GameManager.Instance.GastaDinero(zona.DimePrecioComida() * cantidad))
        {
            _sacosComidaAlmacenados[_zonasCompradas.IndexOf(zona)]+= cantidad;
        }
    }

    public int DimeSacosAlmacenados(ZonaAnimal zona)
    {
        if (_zonasCompradas.Contains(zona))
        {
            return _sacosComidaAlmacenados[_zonasCompradas.IndexOf(zona)];
        }
        else return 0;
    }
    public int TraerSacosAlmacenados(ZonaAnimal zona)
    {
        if (_zonasCompradas.Contains(zona))
        {
            int sacos = _sacosComidaAlmacenados[_zonasCompradas.IndexOf(zona)];
            _sacosComidaAlmacenados[_zonasCompradas.IndexOf(zona)] = 0;
            return sacos;
        }
        else return 0;
    }
    public List<ZonaAnimal> DameListaZonasCompradas()
    {
        return _zonasCompradas;
    }
    public void ActivaMenuAlmacen()
    {
        MenuInicio.SetActive(false);
        MenuAlmacen.SetActive(true);
    }
    public void ActivaMenuEmpleados()
    {
        MenuInicio.SetActive(false);
        MenuEmpleados.SetActive(true);
    }
    public void ActivaMenuEstados()
    {
        MenuInicio.SetActive(false);
        MenuEstados.SetActive(true);
    }
    public void ActivaMenuCuadros()
    {
        MenuInicio.SetActive(false);
        MenuCuadros.SetActive(true);
    }
    public void VolverSalir()
    {
        if (MenuInicio.activeSelf)
        {
            PanelTablet.SetActive(false);
        }
        else if (MenuAlmacen.activeSelf)
        {
            MenuAlmacen.SetActive(false);
            MenuInicio.SetActive(true);
        }
        else if (MenuEmpleados.activeSelf)
        {
            MenuEmpleados.SetActive(false);
            MenuInicio.SetActive(true);
        }
        else if (MenuEstados.activeSelf)
        {
            MenuEstados.SetActive(false);
            MenuInicio.SetActive(true);
        }
        else if (MenuCuadros.activeSelf)
        {
            MenuCuadros.SetActive(false);
            MenuInicio.SetActive(true);
        }
    }
    public int CantidadZonasCompradas()
    {
        return _zonasCompradas.Count;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    private void ActualizaContenedoresZonas()
    {
        if (_zonasCompradas.Count != _zonasMostradas.Count)
        {
            foreach (ZonaAnimal zona in _zonasCompradas)
            {
                if (!_zonasMostradas.Contains(zona))
                {
                    AgregaElementoAlmacen(zona);
                    AgregaElementoEstados(zona);
                    _zonasMostradas.Add(zona);
                }
            }
        }
        foreach (ZonaAnimal zona in _zonasMostradas)
        {
            _textosElementosAlmacen[_zonasMostradas.IndexOf(zona)].text = $"Comida en el almacen para\n {zona.name}: {DimeSacosAlmacenados(zona)}\n Precio: {zona.DimePrecioComida()}€";
            _textosElementosEstados[_zonasMostradas.IndexOf(zona)].text = $"Estado de {zona.name}:\n Hambre: {zona.PorcentajeHambre()}%   Suciedad: {zona.PorcentajeSuciedad()}\n {zona.HayAnimales()}   Salud: {zona.SaludAnimales()}%";
        }
    }

    private void ActualizaContenedorEmpleados()
    {
       
    }
    private void ActualizaContenerdorCuadros()
    {
        if (_cuadrosAlmacenados.Count != _cuadrosMostrados.Count)
        {
            foreach (Pictures Cuadro in _cuadrosAlmacenados)
            {
                if (!_cuadrosMostrados.Contains(Cuadro))
                {
                    AgregaElementoCuadro(Cuadro);
                    _cuadrosMostrados.Add(Cuadro);
                }
            }
        }
    }

    private void AgregaElementoAlmacen(ZonaAnimal zona)
    {
        int AltoElemento = 80;
        int AnchoElemento = 300;
        _contenedorAlmacenRect.sizeDelta += new Vector2(0, AltoElemento * 1.5f);

        // Texto
        GameObject ObjetoTexto = new GameObject(zona.name, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        ObjetoTexto.transform.SetParent(ContenedorAlmacen, false);

        RectTransform rectTexto = ObjetoTexto.GetComponent<RectTransform>();
        rectTexto.sizeDelta = new Vector2(AnchoElemento, AltoElemento);

        TextMeshProUGUI texto = ObjetoTexto.GetComponent<TextMeshProUGUI>();
        texto.text = "Comida en el almacen para " + zona.name + ":" + DimeSacosAlmacenados(zona) + "\n" + "Precio: " + zona.DimePrecioComida() + "€";
        texto.fontStyle = FontStyles.Bold;
        texto.fontSize = 15;
        texto.textWrappingMode = TextWrappingModes.NoWrap;
        texto.horizontalAlignment = HorizontalAlignmentOptions.Center;
        texto.verticalAlignment = VerticalAlignmentOptions.Middle;

        // Guardamos referencia
        _elementosAlmacen.Add(ObjetoTexto);
        _textosElementosAlmacen.Add(texto);
    }
    private void AgregaElementoCuadro(Pictures cuadro)
    {
        int AltoElemento = 80;
        int AnchoElemento = 300;
        _contenedorCuadrosRect.sizeDelta += new Vector2(0, AltoElemento * 1.5f);

        // Texto
        GameObject ObjetoTexto = new GameObject(cuadro.GetTitle(), typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        ObjetoTexto.transform.SetParent(ContenedorCuadros, false);

        RectTransform rectTexto = ObjetoTexto.GetComponent<RectTransform>();
        rectTexto.sizeDelta = new Vector2(AnchoElemento, AltoElemento);

        TextMeshProUGUI texto = ObjetoTexto.GetComponent<TextMeshProUGUI>();
        texto.text = $"{cuadro.GetTitle()}: Autor: {cuadro.GetAuthor()}\n Precio: {cuadro.GetPrice()}  €";
        texto.fontStyle = FontStyles.Bold;
        texto.fontSize = 10;
        texto.textWrappingMode = TextWrappingModes.NoWrap;
        texto.horizontalAlignment = HorizontalAlignmentOptions.Center;
        texto.verticalAlignment = VerticalAlignmentOptions.Middle;

        // Guardamos referencia
        _elementosCuadros.Add(ObjetoTexto);
    }
    private void CreaElementoContenedorEmpleados()
    {

    }
    private void AgregaElementoEstados(ZonaAnimal zona)
    {
        int AltoElemento = 80;
        int AnchoElemento = 300;
        _contenedorEstadosRect.sizeDelta += new Vector2(0, AltoElemento * 1.5f);

        // Texto
        GameObject ObjetoTexto = new GameObject(zona.name, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        ObjetoTexto.transform.SetParent(ContenedorEstados, false);

        RectTransform rectTexto = ObjetoTexto.GetComponent<RectTransform>();
        rectTexto.sizeDelta = new Vector2(AnchoElemento, AltoElemento);

        TextMeshProUGUI texto = ObjetoTexto.GetComponent<TextMeshProUGUI>();
        texto.text = $"Estado de {zona.name}:\n Hambre: {zona.PorcentajeHambre()}%   Suciedad: {zona.PorcentajeSuciedad()}\n {zona.HayAnimales()}   Salud: {zona.SaludAnimales()}%";
        texto.fontStyle = FontStyles.Bold;
        texto.fontSize = 15;
        texto.textWrappingMode = TextWrappingModes.NoWrap;
        texto.horizontalAlignment = HorizontalAlignmentOptions.Center;
        texto.verticalAlignment = VerticalAlignmentOptions.Middle;

        // Guardamos referencia
        _elementosEstados.Add(ObjetoTexto);
        _textosElementosEstados.Add(texto);
    }
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
