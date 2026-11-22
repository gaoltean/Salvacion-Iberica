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
public class PlayerMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    /// <summary>
    /// Velocidad a la que se deplaza el personaje
    /// </summary>
    [SerializeField] private float Speed;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Rigidbody del player
    /// </summary>
    private Rigidbody2D _rb;
    /// <summary>
    /// Componente animator del player
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Sprite render del player
    /// </summary>
    private SpriteRenderer _sprite;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.Log("No hay rigidbody por tanto no se puede mover el player");
        }
        _animator = this.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.Log("No hay Animator por tanto no hay animaciones el player");
        }
        _sprite = this.GetComponent<SpriteRenderer>();
        if (_sprite == null)
        {
            Debug.Log("No hay sprite y no hay animaciones en el player");
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector2 dir = InputManager.Instance.MovementVector;
        Movement(dir);
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
    /// Metodo encargado del movimiento del jugador
    /// </summary>
    /// <param name="dir">direcion del movimiento </param>
    private void Movement(Vector2 dir)
    {
        _rb.linearVelocity = dir * Speed;
        Animations(dir);
    }
    /// <summary>
    /// Metodo que genera las animaciones del personaje
    /// </summary>
    /// <param name="dir">direcion del movimiento</param>
    private void Animations(Vector2 dir)
    {
        _sprite.flipX = false;
        bool dirXNull = dir.x > -0.1 && dir.x < 0.1;
        bool dirYNull = dir.y > -0.1 && dir.y < 0.1;
        if(dirXNull && dirYNull)
        {
            _animator.Play("Ilde");
        }
        //Andar hacia delante
        else if (dir.y < -0.1  && dirXNull)
        {
            _animator.Play("RunF");
        }
        //Andar hacia atras
        else if (dir.y > 0.1 && dirXNull)
        {
            _animator.Play("RunB");
        }
        //Correr hacia atras y derecha
        else if (dir.y > 0.1 && dir.x>0.1)
        {
            _animator.Play("RunLateralB");
        }
        //Correr hacia delante y derecha
        else if (dir.y < -0.1 && dir.x > 0.1)
        {
            _animator.Play("RunLateralF");
        }
        //Correr hacia atras y izquierda
        else if (dir.y > 0.1 && dir.x < -0.1)
        {
            _sprite.flipX = true;
            _animator.Play("RunLateralB");
        }
        //Correr hacia delante y izquierda
        else if (dir.y < -0.1 && dir.x < -0.1)
        {
            _sprite.flipX = true;
            _animator.Play("RunLateralF");
        }
        //Correr izquierda
        else if(dir.x <-0.1 && dirYNull)
        {
            _sprite.flipX = true;
            _animator.Play("RunLateral");
        }
        //Correr derecha
        else if (dir.x > 0.1 && dirYNull)
        {
            _animator.Play("RunLateral");
        }
    }
    #endregion   

} // class PlayerMovement 
// namespace
