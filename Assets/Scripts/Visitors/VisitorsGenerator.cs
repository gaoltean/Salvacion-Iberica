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
public class VisitorsGenerator : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    /// <summary>
    /// Prefab de los visitantes
    /// </summary>
    [SerializeField] private GameObject Visitor;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Tiempo del ultimo spawn
    /// </summary>
    private float _lastVisitorSpawn;
    /// <summary>
    /// Tiempo que hay que esperar entre spawns
    /// </summary>
    private float _TimeToWait;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        //Creamos el primero
        GenerateVisitor();
        _TimeToWait = Random.Range(0.5f, 1.5f);
        _lastVisitorSpawn = Time.time;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Si ha pasado el tiempo correspondiente creamos otro
        if(Time.time-_lastVisitorSpawn > _TimeToWait)
        {
            GenerateVisitor();
            _TimeToWait = Random.Range(0.5f, 1.5f);
            _lastVisitorSpawn = Time.time;
        }
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
    /// <summary>
    /// Metodo encargado de generar visitantes
    /// </summary>
    private void GenerateVisitor()
    {
        GameObject visitor = Instantiate(Visitor, this.transform.position, this.transform.rotation);
    }
    #endregion   

} // class VisitorsGenerator 
// namespace
