//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



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
    [SerializeField] private TextMeshProUGUI TextoLimpiadoresDisponibles;
    [SerializeField] private TextMeshProUGUI TextoCuidadoresDisponibles;
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

    /// <summary>
    /// Lista de componentes de las zonas de tranajadores
    /// </summary>
    private List<WorkerStation> _zonasTrabajadores = new();

    /// <summary>
    /// Lista de Cuadros almacenados
    /// </summary>
    private List<Pictures> _cuadrosAlmacenados = new();
    /// <summary>
    /// Lista de Cuadros mostrados en el Menu
    /// </summary>
    private List<Pictures> _cuadrosMostrados = new();

    /// <summary>
    /// Lista de Cuidadores
    /// </summary>
    private List<WorkerMovement> _listaCuidadores = new();
    /// <summary>
    /// Lista de cuidadores contratados
    /// </summary>
    private List<WorkerMovement> _listaCuidadoresContratados = new();
    /// <summary>
    /// Cantidad de cuidadores contratados actualmentes
    /// </summary>
    private int _cantidadCuidadores = 0;
    private List<WorkerMovement> _listaCuidadoresDisponibles = new();

    /// <summary>
    /// Lista de Limpiadores
    /// </summary>
    private List<WorkerMovement> _listaLimpiadores = new();
    /// <summary>
    /// Lista de limpiadores contratados
    /// </summary>
    private List<WorkerMovement> _listaLimpiadoresContratados = new();
    /// <summary>
    /// Cantidad de limpiadores contratados actualmentes
    /// </summary>
    private int _cantidadLimpiadores = 0;
    private List<WorkerMovement> _listaLimpiadoresDisponibles = new();


    // === Elementos mostrados menu Almacen ===
    private List<GameObject> _elemetosAlmacen = new();
    private List<int> _sacosDeCadaElementoAlmacen = new();
    private List<TextMeshProUGUI> _textoDeCadaElementoAlmacen = new();

    // === Elementos mostrados menu Estados zonas ===
    private List<GameObject> _elementosEstados = new();
    private List<TextMeshProUGUI> _textosDeCadaElementoEstados = new();

    // === Elementos mostrados menu Cuadros ===
    private List<GameObject> _elementosCuadros = new();

    // === Elementos mostrados menu Enviar Empleados
    private List<GameObject> _elementosEnviarEmpleados = new();

    // === Componentes rectTransform de los contenedores
    private RectTransform _contenedorAlmacenRect;
    private RectTransform _contenedorEnviarEmpleadosRect;
    private RectTransform _contenedorEstadosRect;
    private RectTransform _contenedorCuadrosRect;

    private bool _trabajadoresHanCobradoHoy = false;
    private float _gastosHoyEnSalarios = 0.0f;
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
            if (PanelTablet.activeSelf && Time.time > _momentoPuedeCerrarTablet && InputManager.Instance.TabletIsPressed())
            {
                _momentoPuedeAbrirTablet = Time.time + 1.0f;
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
                    TextoCuidadoresDisponibles.text = $"Cuidadores disponibles\n: {_listaCuidadoresDisponibles.Count}/{_cantidadCuidadores}";
                    TextoLimpiadoresDisponibles.text = $"Limpiadores disponibles\n: {_listaLimpiadoresDisponibles.Count}/{_cantidadLimpiadores}";
                    ActualizaContenedoresZonas();
                    ActualizaListaCuidadoresDisponibles();
                    ActualizaListaLimpiadoresDisponibles();
                }
                else if (MenuContratarEmpleados.activeSelf)
                {
                    TextoContratarCuidador.text  = $"Contratados: {_cantidadCuidadores}/{_listaCuidadores.Count}\n Salario: {SalarioDiarioCuidador}";
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

        if (_cantidadCuidadores > 0 || _cantidadLimpiadores > 0)
        {
            // Cobran todos a la vez por la noche
            if (TimeManager.Instance.IsNight())
            {
                if (!_trabajadoresHanCobradoHoy)
                {
                    _trabajadoresHanCobradoHoy = true;
                    GameManager.Instance.PagaTrabajador(SalarioDiarioCuidador * _cantidadCuidadores);
                    GameManager.Instance.PagaTrabajador(SalarioDiarioLimpiador * _cantidadLimpiadores);
                    _gastosHoyEnSalarios = SalarioDiarioCuidador * _cantidadCuidadores + SalarioDiarioLimpiador * _cantidadLimpiadores;
                }
            }
            // Al inicio del dia se resetea el cobro de los trabajadores
            else if (_trabajadoresHanCobradoHoy) _trabajadoresHanCobradoHoy = false;
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
    public float GastosEnSalarios()
    {
        return _gastosHoyEnSalarios;
    }
    public void AddZona(ZonaAnimal zona)
    {
        _zonasCompradas.Add(zona);
        _sacosDeCadaElementoAlmacen.Add(0);
    }
    public void AddCuadro(Pictures Cuadro)
    {
        _cuadrosAlmacenados.Add(Cuadro);
    }
    public void AddWorkerStation(WorkerStation zonaTrabajador)
    {
        _zonasTrabajadores.Add(zonaTrabajador);
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
        if (_cantidadCuidadores < _listaCuidadores.Count)
        {
            WorkerMovement trabajador = _listaCuidadores[_cantidadCuidadores];
            trabajador.gameObject.SetActive(true);
            _listaCuidadoresContratados.Add(trabajador);
            _listaCuidadoresDisponibles.Add(trabajador);
            _cantidadCuidadores++;
        }
    }
    public void ContrataLimpiador()
    {
        if (_cantidadLimpiadores < _listaLimpiadores.Count)
        {
            WorkerMovement trabajador = _listaLimpiadores[_cantidadLimpiadores];
            trabajador.gameObject.SetActive(true);
            _listaLimpiadoresContratados.Add(trabajador);
            _listaLimpiadoresDisponibles.Add(trabajador);
            _cantidadLimpiadores++;
        }
    }
    
    public void CompraYAlmacenaSacos(int cantidad, ZonaAnimal zona)
    {
        if (_zonasCompradas.Contains(zona) && GameManager.Instance.GastaDinero(zona.DimePrecioComida() * cantidad))
        {
            _sacosDeCadaElementoAlmacen[_zonasCompradas.IndexOf(zona)]+= cantidad;
        }
    }

    public int DimeSacosAlmacenados(ZonaAnimal zona)
    {
        if (_zonasCompradas.Contains(zona))
        {
            return _sacosDeCadaElementoAlmacen[_zonasCompradas.IndexOf(zona)];
        }
        else return 0;
    }
    public int TraerSacosAlmacenados(ZonaAnimal zona)
    {
        if (_zonasCompradas.Contains(zona))
        {
            int sacos = _sacosDeCadaElementoAlmacen[_zonasCompradas.IndexOf(zona)];
            _sacosDeCadaElementoAlmacen[_zonasCompradas.IndexOf(zona)] = 0;
            return sacos;
        }
        else return 0;
    }
    public List<ZonaAnimal> DameListaZonasCompradas()
    {
        return _zonasCompradas;
    }
    public List<Pictures> DameListaCuadros()
    {
        return _cuadrosAlmacenados;
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
        MenuEnviarEmpleados.SetActive(false);
        MenuContratarEmpleados.SetActive(false);
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
    public void BorraCuadroVendido(Pictures cuadro)
    {
        _cuadrosAlmacenados.Remove(cuadro);
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
    public void CuadroVendido(Pictures cuadro)
    {
        int indice = _cuadrosMostrados.IndexOf(cuadro);

        QuitaElementoContenedor(_elementosCuadros[indice], ContenedorCuadros);

        _cuadrosMostrados.RemoveAt(indice);
        _elementosCuadros.RemoveAt(indice);
        _cuadrosAlmacenados.Remove(cuadro);
    }

    public void ActualizaListaCuidadoresDisponibles()
    {
        foreach (WorkerMovement trabajador in _listaCuidadoresContratados)
        {
            if (!trabajador.IsWorking() && !_listaCuidadoresDisponibles.Contains(trabajador))
            {
                _listaCuidadoresDisponibles.Add(trabajador);
                TextoCuidadoresDisponibles.text = $"Cuidadores disponibles\n: {_listaCuidadoresDisponibles.Count}/{_cantidadCuidadores}";
                TextoLimpiadoresDisponibles.text = $"Limpiadores disponibles\n: {_listaLimpiadoresDisponibles.Count}/{_cantidadLimpiadores}";
            }
        }
    }
    public void ActualizaListaLimpiadoresDisponibles()
    {
        foreach (WorkerMovement trabajador in _listaLimpiadoresContratados)
        {
            if (!trabajador.IsWorking() && !_listaLimpiadoresDisponibles.Contains(trabajador))
            {
                _listaLimpiadoresDisponibles.Add(trabajador);
                TextoCuidadoresDisponibles.text = $"Cuidadores disponibles\n: {_listaCuidadoresDisponibles.Count}/{_cantidadCuidadores}";
                TextoLimpiadoresDisponibles.text = $"Limpiadores disponibles\n: {_listaLimpiadoresDisponibles.Count}/{_cantidadLimpiadores}";
            }
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
            _textoDeCadaElementoAlmacen[_zonasMostradas.IndexOf(zona)].text = $"Comida en el almacen para\n {zona.name}: {DimeSacosAlmacenados(zona)}\n Precio: {zona.DimePrecioComida()}€";
            _textosDeCadaElementoEstados[_zonasMostradas.IndexOf(zona)].text = $"Estado de {zona.name}:\n Hambre: {zona.PorcentajeHambre()}%   Suciedad: {zona.PorcentajeSuciedad()}\n {zona.HayAnimales()}   Salud: {zona.SaludAnimales()}%";
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
        _elemetosAlmacen.Add(ObjetoTexto);
        _textoDeCadaElementoAlmacen.Add(texto);
    }
    private void AgregaElementoCuadro(Pictures cuadro)
    {
        int AltoElemento = 80;
        int AnchoElemento = 300;
        int FontSize = 15;
        _contenedorCuadrosRect.sizeDelta += new Vector2(0, AltoElemento * 1.5f);

        // Texto
        GameObject ObjetoTexto = new GameObject(cuadro.GetTitle(), typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        ObjetoTexto.transform.SetParent(ContenedorCuadros, false);

        RectTransform rectTexto = ObjetoTexto.GetComponent<RectTransform>();
        rectTexto.sizeDelta = new Vector2(AnchoElemento, AltoElemento);

        TextMeshProUGUI texto = ObjetoTexto.GetComponent<TextMeshProUGUI>();
        texto.text = $"Cuadro: {cuadro.GetTitle()}\nAutor: {cuadro.GetAuthor()}\nPrecio: {cuadro.GetPrice()} €";
        texto.fontStyle = FontStyles.Bold;
        texto.color = Color.black;
        texto.fontSize = FontSize;
        texto.textWrappingMode = TextWrappingModes.Normal;
        texto.horizontalAlignment = HorizontalAlignmentOptions.Left;
        texto.verticalAlignment = VerticalAlignmentOptions.Middle;

        // Guardamos referencia
        _elementosCuadros.Add(ObjetoTexto);
        _cuadrosMostrados.Add(cuadro);
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
        rectbotoncuidador.sizeDelta = new Vector2(AnchoElemento / 3, AltoElemento / 1.5f);

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
            EnviaCuidador(zona);
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
        rectbotonLimpiador.sizeDelta = new Vector2(AnchoElemento / 3, AltoElemento / 1.5f);

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
            EnviaLimpiador(zona);
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
        _textosDeCadaElementoEstados.Add(texto);
    }

    private void QuitaElementoContenedor(GameObject objeto, Transform contenedor)
    {
        contenedor.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, objeto.GetComponent<RectTransform>().sizeDelta.y*1.5f);
        Destroy(objeto);
    }

    private void EnviaCuidador(ZonaAnimal zona)
    {
        int indice = _zonasMostradas.IndexOf(zona);
        WorkerStation zonaTrabajo = _zonasTrabajadores[indice];
        bool yaTieneTrabajador = zonaTrabajo.HayTrabajador();
        int cantidadTrabajadores = _listaCuidadoresDisponibles.Count;
        Vector2 PosicionTrabajo = zonaTrabajo.GetPosition();
        if (!yaTieneTrabajador &&  cantidadTrabajadores > 0)
        {
            Debug.Log($"Cuidador enviado a: {PosicionTrabajo.x} , {PosicionTrabajo.y}");
            int trabajadorEnviado = Random.Range(0, cantidadTrabajadores);
            _listaCuidadoresDisponibles[trabajadorEnviado].Call(zonaTrabajo.GetPosition());
            _listaCuidadoresDisponibles.Remove(_listaCuidadoresDisponibles[trabajadorEnviado]);
            _zonasTrabajadores[indice].TrabajadorEnviado();
        }
    }
    private void EnviaLimpiador(ZonaAnimal zona)
    {
        int indice = _zonasMostradas.IndexOf(zona);
        WorkerStation zonaTrabajo = _zonasTrabajadores[indice];
        bool yaTieneTrabajador = zonaTrabajo.HayTrabajador();
        int cantidadTrabajadores = _listaLimpiadoresDisponibles.Count;
        Vector2 PosicionTrabajo = zonaTrabajo.GetPosition();
        if (!yaTieneTrabajador && cantidadTrabajadores > 0)
        {
            Debug.Log($"Limpiador enviado a: {PosicionTrabajo.x} , {PosicionTrabajo.y}");
            int trabajadorEnviado = Random.Range(0, cantidadTrabajadores);
            _listaLimpiadoresDisponibles[trabajadorEnviado].Call(PosicionTrabajo);
            _listaLimpiadoresDisponibles.Remove(_listaLimpiadoresDisponibles[trabajadorEnviado]);
            zonaTrabajo.TrabajadorEnviado();
        }
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
