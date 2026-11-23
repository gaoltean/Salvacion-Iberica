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
public class RobberStation : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    [SerializeField] private float TimeToWaitRobber;
    [SerializeField] private float TimeToWait;
    [SerializeField] private Pictures cuadro;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    private RobbersMovement _robber;
    private float _startTime;
    private bool _hasRobber;
    private bool _canHaveRobber;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _canHaveRobber = true;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_hasRobber)
        {
            if(Time.time -_startTime > TimeToWaitRobber)
            {
                _robber.gameObject.SetActive(true);
                _robber.Back();
                _hasRobber = false;
                _startTime = Time.time;
                _canHaveRobber = false;
                Tablet.Instance.AddCuadro(cuadro);
            }
        }
        else 
        {
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
