//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject MenuEstados;
    [SerializeField] private GameObject MenuCuadros;
    [SerializeField] private GameObject MenuEmpleados;
    [SerializeField] private GameObject MenuInicioEmpleados;
    [SerializeField] private GameObject MenuEnviarEmpleados;
    [SerializeField] private GameObject MenuContratarEmpleados;
    [SerializeField] private float SalarioDiarioCuidador;
    [SerializeField] private float SalarioDiarioLimpiador;
    [SerializeField] private TextMeshProUGUI TextoContratarLimpiador;
    [SerializeField] private TextMeshProUGUI TextoContratarCuidador;
    [SerializeField] private Transform ContenedorAlmacen;
    [SerializeField] private Transform ContenedorEnviarEmpleados;
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
    private List<int> _sacosComidaAlmacenados = new();
    private List<TextMeshProUGUI> _textosElementosAlmacen = new();

    private List<GameObject> _elementosEstados = new();
    private List<TextMeshProUGUI> _textosElementosEstados = new();

    private List<Pictures> _cuadrosAlmacenados = new();
    private List<Pictures> _cuadrosMostrados = new();
    private List<GameObject> _elementosCuadros = new();

    private List<WorkerMovement> _listaCuidadores = new();
    private List<WorkerMovement> _listaLimpiadores = new();
    private int _cantCuidadores = 0;
    private int _cantidadLimpiadores = 0;
    private List<WorkerMovement> _listaCuidadoresContratados = new();
    private List<WorkerMovement> _listaLimpiadoresContratados = new();
    private List<GameObject> _elementosEnviarEmpleados = new();

    private RectTransform _contenedorAlmacenRect;
    private RectTransform _contenedorEnviarEmpleadosRect;
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
        _contenedorEnviarEmpleadosRect = ContenedorEnviarEmpleados.GetComponent<RectTransform>();
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
                if (MenuEnviarEmpleados.activeSelf)
                {
                    ActualizaContenedoresZonas();
                }
                else if (MenuContratarEmpleados.activeSelf)
                {
                    TextoContratarCuidador.text  = $"Contratados: {_cantCuidadores}/{_listaCuidadores.Count}\n Salario: {SalarioDiarioCuidador}";
                    TextoContratarLimpiador.text = $"Contratados: {_cantidadLimpiadores}/{_listaLimpiadores.Count}\n Salario: {SalarioDiarioLimpiador}";
                }
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
    
    public void AddLimpiadorContratable(WorkerMovement trabajador)
    {
        _listaLimpiadores.Add(trabajador);
    }
    public void AddCuidadorContratable(WorkerMovement trabajador)
    {
        _listaCuidadores.Add(trabajador);
    }
    public void ContrataCuidador()
    {
        if (_cantCuidadores < _listaCuidadores.Count)
        {
            WorkerMovement trabajador = _listaCuidadores[_cantCuidadores];
            trabajador.gameObject.SetActive(true);
            _listaCuidadoresContratados.Add(trabajador);
            _cantCuidadores++;
        }
    }
    public void ContrataLimpiador()
    {
        if (_cantidadLimpiadores < _listaLimpiadores.Count)
        {
            WorkerMovement trabajador = _listaLimpiadores[_cantidadLimpiadores];
            trabajador.gameObject.SetActive(true);
            _listaLimpiadoresContratados.Add(trabajador);
            _cantidadLimpiadores++;
        }
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
    public int CantidadZonasCompradas()
    {
        return _zonasCompradas.Count;
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
        MenuInicioEmpleados.SetActive(true);
    }
    public void ActivaMenuContratarEmpleados()
    {
        MenuInicioEmpleados.SetActive(false);
        MenuContratarEmpleados.SetActive(true);
    }
    public void ActivaMenuEnviarEmpleados()
    {
        MenuInicioEmpleados.SetActive(false);
        MenuEnviarEmpleados.SetActive(true);
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
            if (MenuEnviarEmpleados.activeSelf)
            {
                MenuEnviarEmpleados.SetActive(false);
                MenuInicioEmpleados.SetActive(true);
            }
            else if (MenuContratarEmpleados.activeSelf)
            {
                MenuContratarEmpleados.SetActive(false);
                MenuInicioEmpleados.SetActive(true);
            }
            else
            {
                MenuEmpleados.SetActive(false);
                MenuInicio.SetActive(true);
            }
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
                    AgregaElementoEmpleados(zona);
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
    private void AgregaElementoEmpleados(ZonaAnimal zona)
    {
        int AltoElemento = 70;
        int AnchoElemento = 300;
        int FontSize = 18;

        _contenedorEnviarEmpleadosRect.sizeDelta += new Vector2(0, AltoElemento * 1.5f);

        GameObject objetoEnviarEmpleados = new GameObject(zona.name,
            typeof(RectTransform),
            typeof(HorizontalLayoutGroup),
            typeof(LayoutElement)
        );
        objetoEnviarEmpleados.transform.SetParent(ContenedorEnviarEmpleados, false);

        RectTransform rect = objetoEnviarEmpleados.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(AnchoElemento, AltoElemento);

        HorizontalLayoutGroup layout = objetoEnviarEmpleados.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 20;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.childAlignment = TextAnchor.MiddleCenter;

        // Texto
        GameObject TextoZonaMostrada = new GameObject(zona.name, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        TextoZonaMostrada.transform.SetParent(objetoEnviarEmpleados.transform, false);

        RectTransform rectTexto = TextoZonaMostrada.GetComponent<RectTransform>();
        rectTexto.sizeDelta = new Vector2(AnchoElemento / 3, AltoElemento);

        TextMeshProUGUI texto = TextoZonaMostrada.GetComponent<TextMeshProUGUI>();
        texto.text = "Enviar a:\n"+ zona.name;
        texto.fontStyle = FontStyles.Bold;
        texto.fontSize = FontSize;
        texto.textWrappingMode = TextWrappingModes.Normal;
        texto.horizontalAlignment = HorizontalAlignmentOptions.Center;
        texto.verticalAlignment = VerticalAlignmentOptions.Middle;

        // Botón
        GameObject BotonEnviarCuidador = new GameObject("BotonCuidador", typeof(RectTransform), typeof(UnityEngine.UI.Image), typeof(Button), typeof(LayoutElement));
        BotonEnviarCuidador.transform.SetParent(objetoEnviarEmpleados.transform, false);


        RectTransform rectbotoncuidador = BotonEnviarCuidador.GetComponent<RectTransform>();
        rectbotoncuidador.sizeDelta = new Vector2(AnchoElemento / 3, AltoElemento);

        Button botoncuidador = BotonEnviarCuidador.GetComponent<Button>();
        botoncuidador.transition = Selectable.Transition.ColorTint;
        ColorBlock colores = botoncuidador.colors;
        colores.normalColor = new Color32(0, 195, 255, 255);
        colores.highlightedColor = new Color32(152, 231, 255, 255);
        colores.selectedColor = new Color32(152, 231, 255, 255);
        colores.pressedColor = new Color32(0, 143, 183, 255);
        colores.disabledColor = new Color32(0, 143, 183, 255);
        botoncuidador.colors = colores;

        botoncuidador.onClick.AddListener(() =>
        {
            
        });

        // Texto del botón
        GameObject textoBotonCuidador = new GameObject("Cuidador", typeof(RectTransform), typeof(TextMeshProUGUI));
        textoBotonCuidador.transform.SetParent(BotonEnviarCuidador.transform, false);

        TMP_Text textCuidador = textoBotonCuidador.GetComponent<TMP_Text>();
        textCuidador.text = "Cuidador";
        textCuidador.fontSize = FontSize;
        textCuidador.fontStyle = FontStyles.Bold;
        textCuidador.color = Color.white;
        textCuidador.horizontalAlignment = HorizontalAlignmentOptions.Center;
        textCuidador.verticalAlignment = VerticalAlignmentOptions.Middle;
        // Botón
        GameObject BotonEnviarLimpiador = new GameObject("BotonLimpiador", typeof(RectTransform), typeof(UnityEngine.UI.Image), typeof(Button), typeof(LayoutElement));
        BotonEnviarLimpiador.transform.SetParent(objetoEnviarEmpleados.transform, false);


        RectTransform rectbotonLimpiador = BotonEnviarLimpiador.GetComponent<RectTransform>();
        rectbotonLimpiador.sizeDelta = new Vector2(AnchoElemento / 3, AltoElemento);

        Button botonLimpiador = BotonEnviarLimpiador.GetComponent<Button>();
        botonLimpiador.transition = Selectable.Transition.ColorTint;
        colores = botonLimpiador.colors;
        colores.normalColor = new Color32(0, 195, 255, 255);
        colores.highlightedColor = new Color32(152, 231, 255, 255);
        colores.selectedColor = new Color32(152, 231, 255, 255);
        colores.pressedColor = new Color32(0, 143, 183, 255);
        colores.disabledColor = new Color32(0, 143, 183, 255);
        botonLimpiador.colors = colores;

        botonLimpiador.onClick.AddListener(() =>
        {

        });

        // Texto del botón
        GameObject textoBotonLimpiador = new GameObject("Limpiador", typeof(RectTransform), typeof(TextMeshProUGUI));
        textoBotonLimpiador.transform.SetParent(BotonEnviarLimpiador.transform, false);

        TMP_Text textLimpiador = textoBotonLimpiador.GetComponent<TMP_Text>();
        textLimpiador.text = "Limpiador";
        textLimpiador.fontSize = FontSize;
        textLimpiador.fontStyle = FontStyles.Bold;
        textLimpiador.color = Color.white;
        textLimpiador.horizontalAlignment = HorizontalAlignmentOptions.Center;
        textLimpiador.verticalAlignment = VerticalAlignmentOptions.Middle;
        
        // Guardar referencias
        _elementosEnviarEmpleados.Add(objetoEnviarEmpleados);
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
