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
    private WorkerMovement _worker;
    private ZonaAnimal Zona;
    private float _timeToWait;
    private float _startTime;
    private bool _hasWorker;
    private bool _finishWork;
    private bool _isCleaner;
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
        _timeToWait = 2f;
        Zona = this.gameObject.GetComponent<ZonaAnimal>();
        if (!Zona)
        {
            Debug.Log("No hay componente zona animal");
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Time.time-_startTime > _timeToWait && _hasWorker)
        {
            if (_isCleaner)
            {
                if (Zona.PorcentajeSuciedad() > 50)
                {
                    Zona.LimpiaZona();
                }
            }
            else
            {
                if (Zona.PorcentajeHambre() > 50)
                {
                    Zona.Alimenta();
                }
            }
            _worker.finish();
            _hasWorker = false;
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
        _worker = collision.GetComponent<WorkerMovement>();
        if (_worker)
        {
            //Lo paramos de andar cuando llega 
            _worker.Stop();
            _isCleaner = _worker.IsCleaner();
            _startTime = Time.time;
            _hasWorker = true;

            
        }
    }
    #endregion   

} // class WorkerStation 
// namespace
