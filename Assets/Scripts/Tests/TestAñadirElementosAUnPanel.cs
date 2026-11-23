//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using TMPro;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class TestAñadirElementosAUnPanel : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    [SerializeField] private Transform ContenedorElementos;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase

    private int _cantidadPrueba = 0;
    private RectTransform _contenedorRect;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _contenedorRect = ContenedorElementos.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    public void AgregaElemento()
    {
        int AltoObjeto = 60;
        int AnchoObjeto = 200;
        _contenedorRect.sizeDelta += new Vector2(0, 1.5f * AltoObjeto);
        GameObject objetoPrueba = new GameObject("ElementoPrueba",
            typeof(RectTransform),
            typeof(HorizontalLayoutGroup),
            typeof(LayoutElement)
        );
        objetoPrueba.transform.SetParent(ContenedorElementos, false);

        RectTransform rect = objetoPrueba.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(AnchoObjeto, AltoObjeto);

        HorizontalLayoutGroup layout = objetoPrueba.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 20;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.childAlignment = TextAnchor.MiddleCenter;

        // Texto
        GameObject TextoPrueba = new GameObject("TextoPrueba", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        TextoPrueba.transform.SetParent(objetoPrueba.transform, false);

        RectTransform rectTexto = TextoPrueba.GetComponent<RectTransform>();
        rectTexto.sizeDelta = new Vector2(AnchoObjeto/2, AltoObjeto);

        TextMeshProUGUI texto = TextoPrueba.GetComponent<TextMeshProUGUI>();
        texto.text = "Texto Prueba" + "\n" + "Cantidad: " + _cantidadPrueba;
        texto.fontStyle = FontStyles.Bold;
        texto.fontSize = 15;
        texto.textWrappingMode = TextWrappingModes.NoWrap;
        texto.horizontalAlignment = HorizontalAlignmentOptions.Center;
        texto.verticalAlignment = VerticalAlignmentOptions.Middle;

        // Botón
        GameObject BotonPrueba = new GameObject("BotonPrueba", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
        BotonPrueba.transform.SetParent(objetoPrueba.transform, false);

        RectTransform rectBoton = BotonPrueba.GetComponent<RectTransform>();
        rectBoton.sizeDelta = new Vector2(AnchoObjeto/2, AltoObjeto);

        Button boton = BotonPrueba.GetComponent<Button>();
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
            _cantidadPrueba++;
            texto.text = "Texto Prueba" + "\n" + "Cantidad: " + _cantidadPrueba;
        });

        // Texto del botón
        GameObject textoBoton = new GameObject("TextoBotonPrueba", typeof(RectTransform), typeof(TextMeshProUGUI));
        textoBoton.transform.SetParent(BotonPrueba.transform, false);

        TMP_Text text = textoBoton.GetComponent<TMP_Text>();
        text.text = "Añadir" + "\n" + "Cantidad";
        text.fontSize = 18;
        text.fontStyle = FontStyles.Bold;
        text.color = Color.black;
        text.horizontalAlignment = HorizontalAlignmentOptions.Center;
        text.verticalAlignment = VerticalAlignmentOptions.Middle;
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    #endregion   

} // class TestAñadirElementosAUnPanel 
// namespace
