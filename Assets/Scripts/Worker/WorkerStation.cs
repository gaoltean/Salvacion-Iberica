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
public class WorkerStation : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Trabajador que ha entrado en la estacion
    /// </summary>
    private WorkerMovement _worker;
    /// <summary>
    /// Componente de la zonaAnimal
    /// </summary>
    private ZonaAnimal _zonaAnimal;
    /// <summary>
    /// Momento en el que inicio el trabajo
    /// </summary>
    private float _startTime;
    /// <summary>
    /// True cuando hay un trabajador
    /// </summary>
    private bool _hasWorker;
    private WorkerMovement _workerSent;
    /// <summary>
    /// True cuando hay un trabajador en camino
    /// </summary>
    private bool _excpectsWorker;
    /// <summary>
    /// True cuando el tranajador es limpiador
    /// </summary>
    private bool _cleaner;
    /// <summary>
    /// Tiempo que tarda el trabajador en completar el trabajo
    /// </summary>
    private float _duracionTrabajo;
    /// <summary>
    /// Tiempo que tarda el trabajador en comprobar si necesita trabajar la zona o no
    /// </summary>
    private float _duracionComprobacion = 1.0f;
    /// <summary>
    /// Indica si la zona necesita el trabajador
    /// </summary>
    private bool _zoneNeedsWorker;

    private Vector2 _zonePosition = Vector2.zero;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _hasWorker = false;
        _duracionTrabajo = 2f;
        _zonaAnimal = this.gameObject.GetComponent<ZonaAnimal>();
        if (!_zonaAnimal)
        {
            Debug.Log("No hay componente zona animal");
        }
        _zonePosition = this.transform.position;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_hasWorker)
        {
            if (_zoneNeedsWorker && Time.time - _startTime > _duracionTrabajo)
            {
                if (_cleaner)
                {
                    _zonaAnimal.LimpiaZona();
                }
                else
                {
                    _zonaAnimal.Alimenta();
                }
                _worker.finish();
                _hasWorker = false;
                _zoneNeedsWorker = false;
                _excpectsWorker = false;
            }
            else if (!_zoneNeedsWorker && Time.time - _startTime > _duracionComprobacion)
            {
                _worker.finish();
                _hasWorker = false;
                _excpectsWorker = false;
            }
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    
    public void TrabajadorEnviado(WorkerMovement trabajador)
    {
        _workerSent = trabajador;
        _excpectsWorker = true;
    }
    public bool HayTrabajador()
    {
        return _excpectsWorker;
    }
    public Vector2 GetPosition()
    {
        return _zonePosition;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (_excpectsWorker && !_hasWorker)
        {
            _worker = collision.GetComponent<WorkerMovement>();
            if (!_worker || _worker != _workerSent) return;

            _cleaner = _worker.IsCleaner();
            _zoneNeedsWorker = _cleaner && _zonaAnimal.NecesitaLimpieza() || !_cleaner && _zonaAnimal.NecesitaAlimentar();
            // Lo paramos de andar cuando llega
            _startTime = Time.time;
            _hasWorker = true;
            _duracionTrabajo = _worker.DuracionDelTrabajo();
        }
    }
    #endregion   

} // class WorkerStation 
// namespace
