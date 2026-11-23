//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using UnityEngine;
using System;


/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class RobbersMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    [SerializeField] private float Speed;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Rigidbody del trabajador
    /// </summary>
    private Rigidbody2D _rb;
    private Transform _transform;
    private bool _rob;
    private Vector2 _posOfi;
    private Vector2 _posSalida;
    private Vector2 _posInter;
    private Vector2 _salidaOfi;
    private bool _salida;
    private bool _inter;
    private bool _exit;
    private bool ReachX;
    private bool ReachY;
    private bool _back;
    private bool _office;
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
            Debug.Log("No hay rigidbody por tanto no se puede mover el Trabajdor");
        }
        _transform = this.GetComponent<Transform>();
        if(_transform == null)
        {
            Debug.Log("No hay transform");
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
        _rob = false;
        _salida = false;
        _posInter = new Vector2(-0.5f, 3);
        _posSalida = new Vector2(-0.5f, -5);
        _posOfi = new Vector2(-6, 6);
        _salidaOfi = new Vector2(-6.7f, 2.8f);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_rob)
        {
            //vamos hasta la intersecion
            if (!_salida)
            {
                _salida = ReachPos(_salidaOfi);
            }
            else if (!_inter)
            {
               _inter = ReachPos(_posInter);
            }
            else if(!_exit)
            {
                _exit = ReachPos(_posSalida);
            }
            if(_exit && _inter && _salida)
            {
                _rob = false;
                _inter = false;
                _exit = false;
                _salida = false;
            }
        }else if (_back)
        {
            if (!_inter)
            {
                _inter = ReachPos(_posInter);
               // Debug.Log("llegando a inter");
            }else if(!_office)
            {
              _office= ReachPos(_posOfi);
                //Debug.Log("llegando a offi");
            }
            if(_office && _inter)
            {
                _inter = false;
                _office = false;
                _back = false;
            }
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    public void Rob()
    {
        _rob = true;
    }
    public void Back()
    {
        _back = true;
        _rob = false;
        _inter = false;
        _salida = false;
        _exit = false;
    }
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
    private bool ReachPos(Vector2 pos)
    {
        Vector2 dir = new Vector2(0, 0);
        ReachX = Math.Abs(_transform.position.x - pos.x) <= 0.5;
        ReachY = Math.Abs(_transform.position.y - pos.y) <= 0.5;
        if (!ReachX)
        {

            if (pos.x > _transform.position.x)
            {
                //Esta a derecha
                dir = new Vector2(1, 0);
            }
            else
            {
                //Esta a izquierda
                dir = new Vector2(-1, 0);
            }
        }
        if (!ReachY && ReachX)
        {
            if (!ReachY)
            {
                //Movemos la y
                if (pos.y > _transform.position.y)
                {
                    //Esta a arriba
                    dir = new Vector2(0, 1);
                }
                else
                {
                    //Esta a abajo
                    dir = new Vector2(0, -1);
                }
            }

        }
        Movement(dir);
        return ReachX && ReachY;
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

} // class Robbers 
// namespace
