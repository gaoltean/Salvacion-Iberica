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
public class ManagerZonaAnimales : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase

    /// <summary>
    /// Sacos de comida necesarios para alimentar
    /// </summary>
    [SerializeField] private int ComidaParaAlimentar;
    [SerializeField] private GameObject PanelGestion;
    [SerializeField] private TextMeshProUGUI TextoComida;
    [SerializeField] private TextMeshProUGUI TextoSuciedad;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase

    private float _porcentajeComida = 100;
    private float _porcentajeSuciedad = 0;

    private bool _limpiezaUrgente = false;
    private bool _alimentacionUrgente = false;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        PanelGestion.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Baja la comida 1% cada 5 segundos
        if(_porcentajeComida > 0) _porcentajeComida -= 0.5f * Time.deltaTime;
        if (_porcentajeComida < 0) _porcentajeComida = 0;
        // Alimentacion urgente
        if (_porcentajeComida < 30) _alimentacionUrgente = true;
        // Baja la limpieza 1% cada 10 segundo
        if (_porcentajeSuciedad < 100) _porcentajeSuciedad += 0.2f * Time.deltaTime;
        if (_porcentajeSuciedad > 100) _porcentajeSuciedad = 100;
        // Limpieza urgente
        if (_porcentajeSuciedad > 70) _limpiezaUrgente = true;

        TextoComida.text = "Comida: " + (int)_porcentajeComida + "%";
        TextoSuciedad.text = "Suciedad: " + (int)_porcentajeSuciedad + "%";
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PanelGestion.activeSelf) PanelGestion.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PanelGestion.SetActive(false);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    public void Alimenta()
    {
        if(GameManager.Instance.GastaComida(ComidaParaAlimentar) && _porcentajeComida < 90) _porcentajeComida = 100;
    }
    public void LimpiaZona() 
    {
        if (_porcentajeSuciedad > 10) _porcentajeSuciedad = 0;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase

    #endregion

} // class ManagerZoanAnimales 
// namespace
