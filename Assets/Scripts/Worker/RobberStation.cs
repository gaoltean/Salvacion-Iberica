//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class RobberStation : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    /// <summary>
    /// Tiempo que tarda en robar
    /// </summary>
    [SerializeField] private float TimeToWaitRobber;
    /// <summary>
    /// Tiempo que hay que esperar para robar de nuevo
    /// </summary>
    [SerializeField] private float TimeToWait;
    /// <summary>
    /// Lista de los cuadros robables
    /// </summary>
    [SerializeField] private List<Pictures> Cuadros;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Movimiento del ladron
    /// </summary>
    private RobbersMovement _robber;
    /// <summary>
    /// Tiempo de inicio del contador
    /// </summary>
    private float _startTime;
    /// <summary>
    /// Indica si tiene un ladron robando
    /// </summary>
    private bool _hasRobber;
    /// <summary>
    /// Indica si puede tener un ladron
    /// </summary>
    private bool _canHaveRobber;
    /// <summary>
    /// Numero de cuadros que quedan
    /// </summary>
    private int _numItems = 0;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        //Contamos el numero de cuadros
        _canHaveRobber = true;
        foreach (Pictures cuadro in Cuadros)
        {
            _numItems++;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_hasRobber)
        {
            //Realizamos el tiempo de espera de robar
            if(Time.time -_startTime > TimeToWaitRobber)
            {
                _robber.gameObject.SetActive(true);
                _robber.Back();
                _hasRobber = false;
                _startTime = Time.time;
                _canHaveRobber = false;
                if(_numItems > 0)
                {
                    Tablet.Instance.AddCuadro(Cuadros[_numItems-1]);
                    Cuadros.Remove(Cuadros[_numItems-1]);
                    _numItems--;
                }
                
            }
        }
        else 
        {
            //Realizamos el tiempo de espera para robar de nuevo
            if(!_canHaveRobber && Time.time -_startTime > TimeToWait)
            {
                _canHaveRobber = true;
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
        if (!_hasRobber && _canHaveRobber)
        {
            //Si podemos cogemos a uno nuevo
            _robber = collision.GetComponent<RobbersMovement>();
            if (_robber)
            {
                _hasRobber = true;
                _robber.gameObject.SetActive(false);
                _startTime = Time.time;
               
            }
        }
        
    }
    #endregion   

} // class RobberStation 
// namespace
