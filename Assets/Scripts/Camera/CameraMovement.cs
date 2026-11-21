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
public class CameraMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    /// <summary>
    /// GameObject del Player
    /// </summary>
    [SerializeField] private GameObject Player;

    /// <summary>
    /// Suavizado del movimiento de la camara
    /// </summary>
    [SerializeField] float SmoothSpeed = 5f;

    /// <summary> 
    /// Distancia maxima a la que se adelanta la camara 
    /// </summary>
    [SerializeField] float LookAheadDistance = 2f;

    /// <summary>
    /// Altura fija de la camara
    /// </summary>
    [SerializeField] float VerticalOffset = 1f;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Como de lejos vamos mirando delante
    /// </summary>
    private float _currentLookAhead = 0f;
    /// <summary>
    /// Posicin adelantada a la que hay que llegar
    /// </summary>
    private float _targetLookAhead = 0f;
    /// <summary>
    /// Velocidad que mira hacia delante 
    /// </summary>
    private float _lookAheadVelocity = 0f;
    /// <summary>
    /// Posicion en X de el objeto que sigue
    /// </summary>
    private float _lastTargetX;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        //Calculamos donde esta el player en X
        _lastTargetX = Player.transform.position.x;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void LateUpdate()
    {
        //Direcion de movimiento del player
        float moveDirection = Player.transform.position.x - _lastTargetX;

        // Detectar dirección del movimiento
        if (Mathf.Abs(moveDirection) > 0.01f)
        {
            _targetLookAhead = Mathf.Sign(moveDirection) * LookAheadDistance;
        }

        // Suavizar el adelantamiento
        _currentLookAhead = Mathf.SmoothDamp(
            _currentLookAhead,
            _targetLookAhead,
            ref _lookAheadVelocity,
            0.3f
        );

        // Posición objetivo
        Vector3 targetPosition = new Vector3(
            Player.transform.position.x + _currentLookAhead,
            Player.transform.position.y + VerticalOffset,
            transform.position.z
        );

        // Movimiento suave de cámara
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            SmoothSpeed * Time.deltaTime
        );
        //establecemos la ultima posicion de seguimento en X donde esta el player
        _lastTargetX = Player.transform.position.x;
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

    #endregion

} // class CameraMovement 
// namespace
