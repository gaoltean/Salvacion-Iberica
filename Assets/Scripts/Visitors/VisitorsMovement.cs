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
public class VisitorsMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    /// <summary>
    /// Velocidad a la que se deplaza el visitante
    /// </summary>
    [SerializeField] private float Speed;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Rigidbody del visitante
    /// </summary>
    private Rigidbody2D _rb;
    /// <summary>
    /// Direccion del visitante
    /// </summary>
    private Vector2 dir;
    /// <summary>
    /// Indica si ha pagado la entrada
    /// </summary>
    private bool _hasPaid;
    /// <summary>
    /// Indica si se pausa
    /// </summary>
    private bool _paused;
    /// <summary>
    /// Felicidad del visitante
    /// </summary>
    private float _felicidad;
    /// <summary>
    /// Componente animator del visitante
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Sprite render del visitante
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
            Debug.Log("No hay rigidbody por tanto no se puede mover el visitante");
        }
        dir = new Vector2(0, 1);
        _hasPaid = false;
        _paused = false;
        _felicidad = 100;
        _animator = this.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.Log("No hay Animator por tanto no hay animaciones el visitor");
        }
        _sprite = this.GetComponent<SpriteRenderer>();
        if (_sprite == null)
        {
            Debug.Log("No hay sprite y no hay animaciones en el visitor");
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Nos movemos a una velocidad establecida
        if (!_paused)
        {
            Movement(dir);
            Updatefelicidad();
        }
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    public void changeDir(bool intersect)
    {
        if (intersect)
        {
            int dir1 = Random.Range(0, 2);
            int dir2 = Random.Range(0, 2);
            if (dir1 == 0)
            {
                if (dir2 == 0) dir = new Vector2(0, 1);
                else dir = new Vector2(0, -1);
            }
            else
            {
                if (dir2 == 0) dir = new Vector2(1, 0);
                else dir = new Vector2(-1, 0);
            }
        }
        else
        {
            dir.y = -dir.y;
            dir.x = -dir.x;
        }
    }
   
    public void Pay( float entrance)
    {
        if (!_hasPaid)
        {
            GameManager.Instance.GanaDinero(entrance);
            //Añadir si es muy cara no entrar
            _hasPaid = true;
        }
    }
    public void PauseVisitor(bool pause)
    {
        _paused = pause;
    }
    public float GetFelicidad()
    {
        return _felicidad;
    }
    public void AumentaFelicidad(float cant)
    {
        if(_felicidad+ cant > 100)
        {
            _felicidad = 100;
        }
        else
        {
            _felicidad += cant;
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    /// <summary>
    /// Metodo encargado del movimiento del visitante
    /// </summary>
    /// <param name="dir">direcion del movimiento </param>
    private void Movement(Vector2 dirs)
    {
        _rb.linearVelocity = dirs * Speed;
        Animate(dirs);
    }
    private void Updatefelicidad()
    {
        //Baja  un 1% cada 5 sec
        if(_felicidad > 0)
        {
            _felicidad -= 0.5f * Time.deltaTime;
        }
        
    }
    private void Animate(Vector2 dirs)
    {
        _sprite.flipX = false;
        bool dirXNull = dirs.x > -0.1 && dirs.x < 0.1;
        bool dirYNull = dirs.y > -0.1 && dirs.y < 0.1;
        //Andar hacia delante
        if (dirs.y < -0.1 && dirXNull)
        {
            _animator.Play("WalkF");
        }
        //Andar hacia atras
        else if (dirs.y > 0.1 && dirXNull)
        {
            _animator.Play("WalkB");
        }
        //Correr izquierda
        else if (dirs.x < -0.1 && dirYNull)
        {
            _sprite.flipX = true;
            _animator.Play("WalkL");
        }
        //Correr derecha
        else if (dirs.x > 0.1 && dirYNull)
        {
            _animator.Play("WalkL");
        }
    }
    #endregion   

} // class VisitorsMovement 
// namespace
