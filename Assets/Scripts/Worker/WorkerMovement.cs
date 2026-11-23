//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using System;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEditor.PlayerSettings;



/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class WorkerMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase

    [SerializeField] private Vector2 DireccionInicial;
    [SerializeField] private float Speed;
    [SerializeField] private bool Cleaner;
    [SerializeField] private float DuracionTrabajo = 3.0f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Rigidbody del trabajador
    /// </summary>
    private Rigidbody2D _rb;
    /// <summary>
    /// Componente transform del trabajador
    /// </summary>
    private Transform _transform;
    /// <summary>
    /// Componente animator del Trabajador
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Sprite render del Trabajador
    /// </summary>
    private SpriteRenderer _sprite;

    /// <summary>
    /// Direccion actual del trabajador
    /// </summary>
    private Vector2 _dir;
    /// <summary>
    /// Nos dicen si nos han llamado
    /// </summary>
    private bool _llamadoTrabajar;
    /// <summary>
    /// Indica si te has terminado de trabajar
    /// </summary>
    private bool _trabajoTerminado;
    /// <summary>
    /// Indica si te has parado para trabajar
    /// </summary>
    private bool _paradoTrabajar;

    /// <summary>
    /// Posicion de la zona en la que tiene que trabajar
    /// </summary>
    private Vector2 _posicionTrabajo;

    /// <summary>
    /// Posicion del camino justo antes de entrar a la zona de trabajo
    /// </summary>
    private Vector2 _posCaminoTrabajo;
    /// <summary>
    /// Direccion para volver al camino despues del trabajo
    /// </summary>
    private Vector2 _dirVueltaCamino;

    private bool ReachX;
    private bool ReachAproximateX;
    private bool ReachY;
    private bool ReachAproximateY;
    private bool _vaPorCamino;
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
        //_animator = this.GetComponent<Animator>();
        //if (_animator == null)
        //{
        //    Debug.Log("No hay Animator por tanto no hay animaciones el Trabajador");
        //}
        _sprite = this.GetComponent<SpriteRenderer>();
        if (_sprite == null)
        {
            Debug.Log("No hay sprite y no hay animaciones en el trabajdor");
        }
        _transform = this.GetComponent<Transform>();
        if (!_transform)
        {
            Debug.Log("No hay transform");
        }
        _llamadoTrabajar = false;
        _trabajoTerminado = false;
        _paradoTrabajar = false;
        _vaPorCamino = true;
        _dir = DireccionInicial;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Cuando no esta parado ni al camino
        if(!_paradoTrabajar)
        {
            // Movimiento
            Movement(_dir);
            // Parada a trabajar
            if (_llamadoTrabajar)
            {
                ReachX = Math.Abs(_transform.position.x - _posicionTrabajo.x) <= 1;
                ReachY = Math.Abs(_transform.position.y - _posicionTrabajo.y) <= 1;
                ReachAproximateX = Math.Abs(_transform.position.x - _posicionTrabajo.x) <= 4;
                ReachAproximateY = Math.Abs(_transform.position.y - _posicionTrabajo.y) <= 4;

                if (ReachX && ReachAproximateY)
                {
                    _posCaminoTrabajo = _transform.position;
                    _dirVueltaCamino = new Vector2(0, _transform.position.y - _posicionTrabajo.y);
                    _paradoTrabajar = true;
                    _trabajoTerminado = false;
                    //Vector2 pause = new Vector2(0,0);
                    //Movement(pause);
                    Vector2 look = new Vector2(0, _posicionTrabajo.y - _transform.position.y);
                    AnimateIlde(look);
                }
                else if (ReachY && ReachAproximateX)
                {
                    _posCaminoTrabajo = _transform.position;
                    _dirVueltaCamino = new Vector2(0, _transform.position.x - _posicionTrabajo.x);
                    _paradoTrabajar = true;
                    _trabajoTerminado = false;
                }
                AnimateIlde(-_dirVueltaCamino);
            }
        }
        // Cuando esta parado y ha terminado de trabajar
        else 
        {
            if (_trabajoTerminado)
            {
                _vaPorCamino = Vector2.Distance(_transform.position, _posCaminoTrabajo) < 0.5f;
                if (!_vaPorCamino)
                {
                    Movement(_dirVueltaCamino);
                }
                else
                {
                    _paradoTrabajar = false;
                }
            }
            else
            {
                Movement(new Vector2(0, 0));
            } 
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    /// <summary>
    /// Indica al trabajador que debe ir a trabajar
    /// </summary>
    /// <param name="posLlamadoTrabajar">Zona a la que debe llegar el trabajador</param>
    public void Call(Vector2 posLlamadoTrabajar)
    {
         _llamadoTrabajar = true;
         _posicionTrabajo = posLlamadoTrabajar;
        
    }
    public void Call1()
    {
        _llamadoTrabajar = true;
        _posicionTrabajo = new Vector2(-19.7f, 6f);
    }
    public void finish()
    {
        _trabajoTerminado = true;
    }
    public void Stop()
    {
        _paradoTrabajar = true;

        Vector2 stop = new Vector2(0, 0);
        Movement(stop);
    }
    public bool IsCleaner()
    {
        return Cleaner;
    }
    public float DuracionDelTrabajo()
    {
        return DuracionTrabajo;
    }
    public void ChangeDir(bool intersection)
    {
        // Cuando es un fin de camino cambia a la direccion opuesta
        if (!intersection)
        {
            _dir.x = - _dir.x;
            _dir.y = - _dir.y;
        }
        // Cuando es interseccion
        else 
        {
            // Cuando no buscamos trabajo
            if (!_llamadoTrabajar)
            {
                // 1 de cada 3 veces cambiamos de direccion
                bool cambiaDireccion = UnityEngine.Random.Range(0, 3) == 0;
                if (cambiaDireccion)
                {
                    // Cambia en x
                    if (UnityEngine.Random.Range(0, 2) == 1)
                    {
                        int nuevaDir = UnityEngine.Random.Range(0, 2);
                        if (nuevaDir == 0) nuevaDir = -1;
                        _dir.x = nuevaDir;
                        _dir.y = 0;
                    }
                    // Cambia en y
                    else
                    {
                        int nuevaDir = UnityEngine.Random.Range(0, 2);
                        if (nuevaDir == 0) nuevaDir = -1;
                        _dir.y = nuevaDir;
                        _dir.x = 0;
                    }
                }
            }
            else
            {
                bool mejorCambiarX = Math.Abs(_transform.position.x - _posicionTrabajo.x) > Math.Abs(_transform.position.y - _posicionTrabajo.y);
                if (mejorCambiarX)
                {
                    if (_posicionTrabajo.x > _transform.position.x) _dir = new Vector2(1, 0);
                    else _dir = new Vector2(-1, 0);
                }
                else
                {
                    if (_posicionTrabajo.y > _transform.position.y) _dir = new Vector2(0, 1);
                    else _dir = new Vector2(0, -1);
                }
            }
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    
    /// <summary>
    /// Metodo encargado del movimiento del Trabajador
    /// </summary>
    /// <param name="dir">direcion del movimiento </param>
    private void Movement(Vector2 dir)
    {
        _rb.linearVelocity = dir * Speed;
        Animate(dir);
    }
    private void Animate(Vector2 LookDir)
    {
        if (!_animator) return;
        _sprite.flipX = false;
        bool dirXNull = LookDir.x > -0.1 && LookDir.x < 0.1;
        bool dirYNull = LookDir.y > -0.1 && LookDir.y < 0.1;
        //Andar hacia delante
        if (LookDir.y < -0.1 && dirXNull)
        {
            _animator.Play("WalkF");
        }
        //Andar hacia atras
        else if (LookDir.y > 0.1 && dirXNull)
        {
            _animator.Play("WalkB");
        }
        //Correr izquierda
        else if (LookDir.x < -0.1 && dirYNull)
        {
            _sprite.flipX = true;
            _animator.Play("WalkL");
        }
        //Correr derecha
        else if (LookDir.x > 0.1 && dirYNull)
        {
            _animator.Play("WalkL");
        }
    }
    private void AnimateIlde(Vector2 direction)
    {
        if (!_animator) return;
        _sprite.flipX = false;
        bool dirXNull = direction.x > -0.1 && direction.x < 0.1;
        bool dirYNull = direction.y > -0.1 && direction.y < 0.1;
        //Andar hacia delante
        if (direction.y < -0.1 && dirXNull)
        {
            _animator.Play("IldeF");
        }
        //Andar hacia atras
        else if (direction.y > 0.1 && dirXNull)
        {
            _animator.Play("IldeB");
        }
        //Correr izquierda
        else if (direction.x < -0.1 && dirYNull)
        {
            _sprite.flipX = true;
            _animator.Play("IldeL");
        }
        //Correr derecha
        else if (direction.x > 0.1 && dirYNull)
        {
            _animator.Play("IldeL");
        }
    }
    #endregion   

} // class WorkerMovement 
// namespace
