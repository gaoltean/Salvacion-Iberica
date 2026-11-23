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
public class WcStop : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    /// <summary>
    /// Tiempo de espera dentro del baño
    /// </summary>
    [SerializeField] private float TimeToWait;
    /// <summary>
    /// Tiempo que se tarda en limpiar el baño
    /// </summary>
    [SerializeField] private float TimeToClean;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Movimiento del visitante
    /// </summary>
    private VisitorsMovement _visitor;
    /// <summary>
    /// Bool que indica si tenemos visitante
    /// </summary>
    private bool _hasVisitor;
    /// <summary>
    /// Bool que indica si esta sucio
    /// </summary>
    private bool _dirty;
    /// <summary>
    /// Tiempo en el que se empieza a contar
    /// </summary>
    private float _startTime;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _dirty = false;
        _hasVisitor = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Realiza el tiempo de espera
        if(_hasVisitor && Time.time - _startTime > TimeToWait)
        {
            _visitor.gameObject.SetActive(true);
            _hasVisitor = false;
            _startTime = Time.time;
            _dirty = true;
        }
        else
        {
            //realiza el tiempo de limpieza
            if(_dirty && Time.time -_startTime > TimeToClean)
            {
                _dirty = false;
            }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_hasVisitor && !_dirty)
        {
            _visitor = collision.GetComponent<VisitorsMovement>();
            if (_visitor)
            {
                if (_visitor.WC()) //Si va al baño lo metemos
                {
                    _visitor.gameObject.SetActive(false);
                    _hasVisitor = true;
                    _startTime = Time.time;
                }

            }
        }
       
    }
    #endregion   

} // class WcStop 
// namespace
