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
public class AnimalMovement : MonoBehaviour
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
    private Rigidbody2D _rb;
    private float Speed;
    private float TimeToChange = 2f;
    private float _startTime;
    private Vector2 _dir;
    /// <summary>
    /// Componente animator del animal
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Sprite render del animal
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
        _dir = new Vector2(0, 0);
        _startTime = Time.time;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Time.time - _startTime > TimeToChange)
        {
            GeneraDirAleatoria(ref _dir);
            GeneraVelAleatoria();
            Movement(_dir);
            _startTime = Time.time;
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
    private void Movement(Vector2 dirs)
    {
        _rb.linearVelocity = dirs * Speed;
        Animate(dirs);
    }
     private void GeneraDirAleatoria(ref Vector2 dir)
    {
        Vector2 aleatorio = new Vector2(0, 0);
        int dir1 = Random.Range(0, 2);
        int dir2 = Random.Range(0, 2);
        if (dir1 == 0) //eje X
        {
            if (dir2 == 0)
            {
                aleatorio = new Vector2(1, 0);
            }
            else
            {
                aleatorio = new Vector2(-1, 0);
            }
        }
        else //eje y
        {
            if(dir2 == 0)
            {
                aleatorio = new Vector2(0, 1);
            }
            else
            {
                aleatorio = new Vector2(0, -1);
            }
        }
        if(aleatorio == dir)
        {
            GeneraDirAleatoria( ref dir);
        }
        else
        {
            dir = aleatorio;
        }
       
    }
    private  void GeneraVelAleatoria()
    {
        Speed = Random.Range(1, 4);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Invierte la dirección o genera una nueva al instante
        GeneraDirAleatoria(ref _dir);
        GeneraVelAleatoria();
        Movement(_dir);
        _startTime = Time.time; // Reinicia el temporizador
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
            
            _animator.Play("WalkL");
        }
        //Correr derecha
        else if (dirs.x > 0.1 && dirYNull)
        {
            _animator.Play("WalkL");
            _sprite.flipX = true;
        }
    }
    #endregion   

} // class AnimalMovement 
// namespace
