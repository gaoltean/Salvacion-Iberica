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
public class AnimalsShopManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase

    [SerializeField] private GameObject TextoGestionar;
    [SerializeField] private GameObject Panel;
    [SerializeField] private Transform Contenedor;
    [SerializeField] private List<ZonaAnimal> ZonasAnimales;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase

    private List<ZonaAnimal> zonasMostradas = new();
    private List<GameObject> elementos = new();

    private float _momentoInicioGestion = 0;
    private float _momentoFinGestion = 0;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        Panel.SetActive(false);
        TextoGestionar.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Panel.activeSelf) ActualizarPanel();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!Panel.activeSelf && !TextoGestionar.activeSelf) TextoGestionar.SetActive(true);
        if (!Panel.activeSelf && InputManager.Instance.Interact && Time.time > _momentoFinGestion + 1)
        {
            _momentoInicioGestion = Time.time;
            Panel.SetActive(true);
            TextoGestionar.SetActive(false);
        }
        if (Panel.activeSelf && InputManager.Instance.Interact && Time.time > _momentoInicioGestion + 1)
        {
            _momentoFinGestion = Time.time;
            Panel.SetActive(false);
            TextoGestionar.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Panel.SetActive(false);
        TextoGestionar.SetActive(false);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    private void ActualizarPanel()
    { 
        foreach (ZonaAnimal zona in ZonasAnimales)
        {
            bool debeAparecer = zona.gameObject.activeInHierarchy && !zona.ContieneAnimales();
            bool estaMostrada = zonasMostradas.Contains(zona);

            if (debeAparecer && !estaMostrada)
            {
                CrearElemento(zona);
            }
            else if (!debeAparecer && estaMostrada)
            {
                int indice = zonasMostradas.IndexOf(zona);

                Destroy(elementos[indice]);

                zonasMostradas.RemoveAt(indice);
                elementos.RemoveAt(indice);
            }
        }
    }
    private void CrearElemento(ZonaAnimal zona)
    {
        GameObject objetoZonaMostrada = new GameObject(zona.name,
            typeof(RectTransform),
            typeof(HorizontalLayoutGroup),
            typeof(LayoutElement)
        );
        objetoZonaMostrada.transform.SetParent(Contenedor, false);

        RectTransform rect = objetoZonaMostrada.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(400, 50);

        HorizontalLayoutGroup layout = objetoZonaMostrada.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 50;
        layout.childAlignment = TextAnchor.MiddleCenter;

        // Texto
        GameObject TextoZonaMostrada = new GameObject(zona.name, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        TextoZonaMostrada.transform.SetParent(objetoZonaMostrada.transform, false);

        RectTransform rectTexto = TextoZonaMostrada.GetComponent<RectTransform>();
        rectTexto.sizeDelta = new Vector2(100, 50);

        TextMeshProUGUI texto = TextoZonaMostrada.GetComponent<TextMeshProUGUI>();
        texto.text = zona.name;
        texto.fontStyle = FontStyles.Bold;
        texto.fontSize = 24;
        texto.textWrappingMode = TextWrappingModes.NoWrap;
        texto.horizontalAlignment = HorizontalAlignmentOptions.Center;
        texto.verticalAlignment = VerticalAlignmentOptions.Middle;

        // Botón
        GameObject BotonZonaMostrada = new GameObject("BotonComprar", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
        BotonZonaMostrada.transform.SetParent(objetoZonaMostrada.transform, false);


        RectTransform rectboton = BotonZonaMostrada.GetComponent<RectTransform>();
        rectboton.sizeDelta = new Vector2(100, 50);

        Button boton = BotonZonaMostrada.GetComponent<Button>();
        boton.onClick.AddListener(() =>
        {
            zona.CompraAnimales();
            ActualizarPanel();
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
        zonasMostradas.Add(zona);
        elementos.Add(objetoZonaMostrada);
    }

    #endregion
} // class AnimalsManager 
// namespace
