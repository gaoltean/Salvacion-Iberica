//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using UnityEngine;



/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    /// <summary>
    /// Gameobject del panel de pausa
    /// </summary>
    [SerializeField] private GameObject PanelPause;
    /// <summary>
    /// La entidad que tiene a todos los objetos que se tienen que pausar
    /// </summary>
    [SerializeField] private GameObject ObjetsToPause;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
            Init();
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static LevelManager Instance
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
    /// cierre para evitar usar el LevelManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }
    /// <summary>
    /// Cambia el boton seleccionado al del panel de pausa y activa el panel
    /// </summary>
    public void ChangeToPause()
    {
        //Activamos el Pause Screen
        PanelPause.SetActive(true);
        //Pausamos a los objetos
        ObjetsToPause.SetActive(false);
    }
    /// <summary>
    /// Vuelve a activar el juego
    /// </summary>
    public void ChangeToUnPause()
    {
        //Activamos el Pause Screen
        PanelPause.SetActive(false);
        //Pausamos a los objetos
        ObjetsToPause.SetActive(true);
    }
    /// <summary>
    /// Cierra la build
    /// </summary>
    public void Exit()
    {
        // Debug.Log("Cerrando juego");
        Application.Quit();
    }
    public void ChangeScene(int index)
    {
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    }
    #endregion

        // ---- MÉTODOS PRIVADOS ----
        #region Métodos Privados
        // Documentar cada método que aparece aquí con ///<summary>
        // Estos métodos se nombren en formato PascalCase
        /// <summary>
        /// Dispara la inicialización.
        /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar
    }
    #endregion

} // class LevelManager 
// namespace
