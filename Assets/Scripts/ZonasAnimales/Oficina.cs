//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using TMPro.EditorUtilities;



/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class Oficina : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase

    [SerializeField] private GameObject TextoGestionar;
    [SerializeField] private GameObject PanelGestion;
    [SerializeField] private GameObject InicioGestion;

    [SerializeField] private GameObject ComprarAnimales;
    [SerializeField] private Transform ContenedorComprarAnimales;

    [SerializeField] private GameObject ComprarUtiles;
    [SerializeField] private GameObject BotonesComprarUtiles;
    [SerializeField] private float PrecioProductosLimpieza;
    [SerializeField] private TextMeshProUGUI TextoStockLimpieza;
    [SerializeField] private GameObject ComprarComida;
    [SerializeField] private Transform ContenedorComprarComida;

    [SerializeField] private GameObject ConstruirZonas;
    [SerializeField] private int PrecioTienda = 0;
    [SerializeField] private int PrecioBaño = 0;
    [SerializeField] private TextMeshProUGUI TextoComprarTienda;
    [SerializeField] private TextMeshProUGUI TextoComprarBaño;
    [SerializeField] private List<GameObject> ZonasTiendas;
    [SerializeField] private List<GameObject> ZonasBaños;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase

    private List<ZonaAnimal> _zonasMostradasCompraAnimales = new();

    private List<GameObject> _elementosCompraAnimales = new();

    private List<ZonaAnimal> _zonasMostradasCompraComida = new();
    private List<GameObject> _elementosCompraComida = new();


    private List<GameObject> _zonasOcultasTiendas = new();
    private List<GameObject> _zonasOcultasBaños = new();

    private int _tiendasMaxDisponibles = 0;
    private int _bañosMaxDisponibles = 0;

    private RectTransform _contenedorComidaRect;
    private RectTransform _contenedorAnimalesRect;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        InicioGestion.SetActive(false);
        ComprarAnimales.SetActive(false);
        ConstruirZonas.SetActive(false);
        ComprarUtiles.SetActive(false);
        ComprarComida.SetActive(false);
        PanelGestion.SetActive(false);
        TextoGestionar.SetActive(false);

        _tiendasMaxDisponibles = ZonasTiendas.Count;
        _bañosMaxDisponibles = ZonasBaños.Count;

        foreach (GameObject tienda in ZonasTiendas)
        {
            _zonasOcultasTiendas.Add(tienda);
            tienda.SetActive(false);
        }
        foreach (GameObject baño in ZonasBaños)
        {
            _zonasOcultasBaños.Add(baño);
            baño.SetActive(false);
        }

        _contenedorAnimalesRect = ContenedorComprarAnimales.GetComponent<RectTransform>();
        _contenedorComidaRect = ContenedorComprarComida.GetComponent<RectTransform>();

        TextoStockLimpieza.text = "Stock: " + GameManager.Instance.DimeProductosLimpieza() + "\n" + "Precio: " + PrecioProductosLimpieza + "€";
        TextoComprarTienda.text = "Compradas:" + (_tiendasMaxDisponibles - _zonasOcultasTiendas.Count) + "/" + ZonasTiendas.Count + "\n" + "Precio: " + PrecioTienda + "€";
        TextoComprarBaño.text = "Comprados:" + (_bañosMaxDisponibles - _zonasOcultasBaños.Count) + "/" + ZonasBaños.Count + "\n" + "Precio:" + PrecioBaño + "€";
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (ComprarAnimales.activeSelf) ActualizaCompraAnimales();
        else if (ComprarComida.activeSelf) ActualizaCompraComida();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!ComprarAnimales.activeSelf && !TextoGestionar.activeSelf) TextoGestionar.SetActive(true);
        if (!PanelGestion.activeSelf && InputManager.Instance.InteractIsPressed())
        {
            PanelGestion.SetActive(true);
            InicioGestion.SetActive(true);
            TextoGestionar.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        InicioGestion.SetActive(false);
        ComprarAnimales.SetActive(false);
        ConstruirZonas.SetActive(false);
        ComprarUtiles.SetActive(false);
        ComprarComida.SetActive(false);
        PanelGestion.SetActive(false);
        TextoGestionar.SetActive(false);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    public void ActivaCompraAnimales()
    {
        InicioGestion.SetActive(false);
        ComprarAnimales.SetActive(true);
    }
    public void ActivaCompraUtiles()
    {
        InicioGestion.SetActive(false);
        ComprarUtiles.SetActive(true);
        BotonesComprarUtiles.SetActive(true);
    }
    public void ActivaConstruirZonas()
    {
        InicioGestion.SetActive(false);
        ConstruirZonas.SetActive(true);
    }
    public void ActivaCompraComida()
    {
        BotonesComprarUtiles.SetActive(false);
        ComprarComida.SetActive(true);
    }
    public void CompraLimpieza()
    {
        GameManager.Instance.CompraProductosLimpieza(1, PrecioProductosLimpieza);
        TextoStockLimpieza.text = "Stock: " + GameManager.Instance.DimeProductosLimpieza() + "\n" + "Precio: " + PrecioProductosLimpieza + "€";
    }
    public void ConstruyeTienda()
    {
        if (_zonasOcultasTiendas.Count > 0 && GameManager.Instance.GastaDinero(PrecioTienda))
        {
            int tiendaComprada = Random.Range(0, _zonasOcultasTiendas.Count);
            _zonasOcultasTiendas[tiendaComprada].SetActive(true);
            _zonasOcultasTiendas.RemoveAt(tiendaComprada);

            TextoComprarTienda.text = "Compradas:" + (_tiendasMaxDisponibles - _zonasOcultasTiendas.Count) + "/" + ZonasTiendas.Count + "\n" + "Precio: " + PrecioTienda + "€";
        }
    }
    public void ConstruyeBaño()
    {
        if (_zonasOcultasBaños.Count > 0 && GameManager.Instance.GastaDinero(PrecioBaño))
        {
            int bañoComprado = Random.Range(0, _zonasOcultasBaños.Count);
            _zonasOcultasBaños[bañoComprado].SetActive(true);
            _zonasOcultasBaños.RemoveAt(bañoComprado);

            TextoComprarBaño.text = "Comprados:" + (_bañosMaxDisponibles - _zonasOcultasBaños.Count) + "/" + ZonasBaños.Count + "\n" + "Precio:" + PrecioBaño + "€";
        }
    }
    public void GoBack()
    {
        if (InicioGestion.activeSelf)
        {
            InicioGestion.SetActive(false);
            ComprarAnimales.SetActive(false);
            ConstruirZonas.SetActive(false);
            ComprarUtiles.SetActive(false);
            ComprarComida.SetActive(false);
            PanelGestion.SetActive(false);
            TextoGestionar.SetActive(true);
        }
        if (ComprarAnimales.activeSelf)
        {
            ComprarAnimales.SetActive(false);
            InicioGestion.SetActive(true);
        }
        else if (ComprarUtiles.activeSelf)
        {
            if (ComprarComida.activeSelf)
            {
                ComprarComida.SetActive(false);
                BotonesComprarUtiles.SetActive(true);
            }
            else
            {
                ComprarUtiles.SetActive(false);
                InicioGestion.SetActive(true);
            }
        }
        else if (ConstruirZonas.activeSelf)
        {
            ConstruirZonas.SetActive(false);
            InicioGestion.SetActive(true);
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    private void ActualizaCompraAnimales()
    { 
        foreach (ZonaAnimal zona in Tablet.Instance.DameListaZonasCompradas())
        {
            bool debeAparecer = !zona.ContieneAnimales();
            bool estaMostrada = _zonasMostradasCompraAnimales.Contains(zona);

            if (debeAparecer && !estaMostrada)
            {
                CrearElementoCompraAnimales(zona);
            }
            else if (!debeAparecer && estaMostrada)
            {
                int indice = _zonasMostradasCompraAnimales.IndexOf(zona);

                Destroy(_elementosCompraAnimales[indice]);

                _zonasMostradasCompraAnimales.RemoveAt(indice);
                _elementosCompraAnimales.RemoveAt(indice);
            }
        }
    }

    private void ActualizaCompraComida()
    {
        foreach (ZonaAnimal zona in Tablet.Instance.DameListaZonasCompradas())
        {
            bool estaMostrada = _zonasMostradasCompraComida.Contains(zona);

            if (!estaMostrada)
            {
                CrearElementoCompraComida(zona);
            }
        }
    }
    /// <summary>
    /// Crea un elemento con texto de nombre de zona y precio de animales
    /// y un boton para comprar los animales y mete el elemento en la lista
    /// </summary>
    /// <param name="zona">Zona asociada</param>
    private void CrearElementoCompraAnimales(ZonaAnimal zona)
    {
        int AltoElemento = 50;
        int AnchoElemento = 200;
        _contenedorAnimalesRect.sizeDelta += new Vector2(0, AltoElemento * 1.5f);

        GameObject objetoZonaMostrada = new GameObject(zona.name,
            typeof(RectTransform),
            typeof(HorizontalLayoutGroup),
            typeof(LayoutElement)
        );
        objetoZonaMostrada.transform.SetParent(ContenedorComprarAnimales, false);

        RectTransform rect = objetoZonaMostrada.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(AnchoElemento, AltoElemento);

        HorizontalLayoutGroup layout = objetoZonaMostrada.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 20;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.childAlignment = TextAnchor.MiddleCenter;

        // Texto
        GameObject TextoZonaMostrada = new GameObject(zona.name, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        TextoZonaMostrada.transform.SetParent(objetoZonaMostrada.transform, false);

        RectTransform rectTexto = TextoZonaMostrada.GetComponent<RectTransform>();
        rectTexto.sizeDelta = new Vector2(AnchoElemento/2, AltoElemento);

        TextMeshProUGUI texto = TextoZonaMostrada.GetComponent<TextMeshProUGUI>();
        texto.text = zona.name + "\n" + "Precio: " + zona.DimePrecioAnimales() + "€";
        texto.fontStyle = FontStyles.Bold;
        texto.fontSize = 18;
        texto.textWrappingMode = TextWrappingModes.NoWrap;
        texto.horizontalAlignment = HorizontalAlignmentOptions.Center;
        texto.verticalAlignment = VerticalAlignmentOptions.Middle;

        // Botón
        GameObject BotonZonaMostrada = new GameObject("BotonComprar", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
        BotonZonaMostrada.transform.SetParent(objetoZonaMostrada.transform, false);


        RectTransform rectboton = BotonZonaMostrada.GetComponent<RectTransform>();
        rectboton.sizeDelta = new Vector2(AnchoElemento/2, AltoElemento);

        Button boton = BotonZonaMostrada.GetComponent<Button>();
        boton.transition = Selectable.Transition.ColorTint;
        ColorBlock colores = boton.colors;
        colores.normalColor = new Color32(0, 195, 255, 255);
        colores.highlightedColor = new Color32(152, 231, 255, 255);
        colores.selectedColor = new Color32(152, 231, 255, 255);
        colores.pressedColor = new Color32(0, 143, 183, 255);
        colores.disabledColor = new Color32(0, 143, 183, 255);
        boton.colors = colores;

        boton.onClick.AddListener(() =>
        {
            zona.CompraAnimales();
            ActualizaCompraAnimales();
        });

        // Texto del botón
        GameObject textoBoton = new GameObject("ComprarAnimales", typeof(RectTransform), typeof(TextMeshProUGUI));
        textoBoton.transform.SetParent(BotonZonaMostrada.transform, false);

        TMP_Text text = textoBoton.GetComponent<TMP_Text>();
        text.text = "Comprar" + "\n" + "animales";
        text.fontSize = 18;
        text.fontStyle = FontStyles.Bold;
        text.color = Color.black;
        text.horizontalAlignment = HorizontalAlignmentOptions.Center;
        text.verticalAlignment = VerticalAlignmentOptions.Middle;
        // Guardar referencias
        _zonasMostradasCompraAnimales.Add(zona);
        _elementosCompraAnimales.Add(objetoZonaMostrada);
    }

    /// <summary>
    /// Crea un elemento con texto de nombre de zona, cantidad de comida y precio de la comida
    /// y un boton para comprar comida y mete el elemento en la lista de elementos de compra comida
    /// </summary>
    /// <param name="zona">Zona asociada</param>
    private void CrearElementoCompraComida(ZonaAnimal zona)
    {
        int AltoElemento = 80;
        int AnchoElemento = 200;
        _contenedorComidaRect.sizeDelta += new Vector2(0, AltoElemento * 1.5f);
        GameObject objetoZonaMostrada = new GameObject(zona.name,
            typeof(RectTransform),
            typeof(HorizontalLayoutGroup),
            typeof(LayoutElement)
        );
        objetoZonaMostrada.transform.SetParent(ContenedorComprarComida, false);

        RectTransform rect = objetoZonaMostrada.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(AnchoElemento, AltoElemento);

        HorizontalLayoutGroup layout = objetoZonaMostrada.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 20;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.childAlignment = TextAnchor.MiddleCenter;

        // Texto
        GameObject TextoZonaMostrada = new GameObject(zona.name, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        TextoZonaMostrada.transform.SetParent(objetoZonaMostrada.transform, false);

        RectTransform rectTexto = TextoZonaMostrada.GetComponent<RectTransform>();
        rectTexto.sizeDelta = new Vector2(AnchoElemento/2, AltoElemento);

        TextMeshProUGUI texto = TextoZonaMostrada.GetComponent<TextMeshProUGUI>();
        texto.text = zona.name + ":\n" + "En almacen: " + Tablet.Instance.TraerSacosAlmacenados(zona) + "\n" + "En el recinto: " + zona.DimeSacosComida() + "\n" + "Precio: " + zona.DimePrecioComida() + "€";
        texto.fontStyle = FontStyles.Bold;
        texto.fontSize = 15;
        texto.textWrappingMode = TextWrappingModes.NoWrap;
        texto.horizontalAlignment = HorizontalAlignmentOptions.Center;
        texto.verticalAlignment = VerticalAlignmentOptions.Middle;

        // Botón
        GameObject BotonZonaMostrada = new GameObject("BotonComprar", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
        BotonZonaMostrada.transform.SetParent(objetoZonaMostrada.transform, false);


        RectTransform rectboton = BotonZonaMostrada.GetComponent<RectTransform>();
        rectboton.sizeDelta = new Vector2(AnchoElemento/2, AltoElemento);

        Button boton = BotonZonaMostrada.GetComponent<Button>();
        boton.transition = Selectable.Transition.ColorTint;
        ColorBlock colores = boton.colors;
        colores.normalColor = new Color32(0, 195, 255, 255);
        colores.highlightedColor = new Color32(152, 231, 255, 255);
        colores.selectedColor = new Color32(152, 231, 255, 255);
        colores.pressedColor = new Color32(0, 143, 183, 255);
        colores.disabledColor = new Color32(0, 143, 183, 255);
        boton.colors = colores;

        boton.onClick.AddListener(() =>
        {
            Tablet.Instance.CompraYAlmacenaSacos(1, zona);
            texto.text = zona.name + "\n" + "En almacen: " + Tablet.Instance.DimeSacosAlmacenados(zona) + "\n" + "En el recinto: " + zona.DimeSacosComida() + "\n" + "Precio: " + zona.DimePrecioComida() + "€";
        });

        // Texto del botón
        GameObject textoBoton = new GameObject("ComprarComida", typeof(RectTransform), typeof(TextMeshProUGUI));
        textoBoton.transform.SetParent(BotonZonaMostrada.transform, false);

        TMP_Text text = textoBoton.GetComponent<TMP_Text>();
        text.text = "Comprar" + "\n" + "Comida";
        text.fontSize = 18;
        text.fontStyle = FontStyles.Bold;
        text.color = Color.black;
        text.horizontalAlignment = HorizontalAlignmentOptions.Center;
        text.verticalAlignment = VerticalAlignmentOptions.Middle;
        // Guardar referencias
        _zonasMostradasCompraComida.Add(zona);
        _elementosCompraComida.Add(objetoZonaMostrada);
    }
    #endregion
} // class AnimalsManager 
// namespace
