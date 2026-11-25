//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using System;
using UnityEngine;



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
    /// Posicion en el trabajo
    /// </summary>
    private Vector2 _posEnElTrabajo;
    /// <summary>
    /// Direccion para volver al camino despues del trabajo
    /// </summary>
    private Vector2 _dirVueltaCamino;
    private Vector2 _dirPorCaminoAntesDeTrabajo;

    private bool ReachX;
    private bool ReachAproximateX;
    private bool ReachY;
    private bool ReachAproximateY;
    private bool _vaPorCamino;
    private bool _zonaEncontrada;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        if (Cleaner) Tablet.Instance.AddLimpiadorContratable(this);
        else Tablet.Instance.AddCuidadorContratable(this);
        this.gameObject.SetActive(false);

        _rb = this.GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.Log("No hay rigidbody por tanto no se puede mover el Trabajdor");
        }
        _animator = this.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.Log("No hay Animator por tanto no hay animaciones el Trabajador");
        }
        _sprite = this.GetComponent<SpriteRenderer>();
        if (_sprite == null)
        {
            Debug.Log("No hay sprite y no hay animaciones en el trabajdor");
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
        // Cuando no está parado trabajando
        if (!_paradoTrabajar)
        {
            // 1. Movimiento normal mientras busca la zona
            if (!_zonaEncontrada)
            {
                Movement(_dir);

                if (_llamadoTrabajar)
                {
                    float dx = transform.position.x - _posicionTrabajo.x;
                    float dy = transform.position.y - _posicionTrabajo.y;

                    ReachX = Mathf.Abs(dx) <= 1f;
                    ReachY = Mathf.Abs(dy) <= 1f;

                    ReachAproximateY = Mathf.Abs(dy) <= 5f;
                    ReachAproximateX = Mathf.Abs(dx) <= 6f;

                    if (ReachX && ReachAproximateY)
                    {
                        _dirPorCaminoAntesDeTrabajo = _dir;

                        // Dirección para regresar al camino
                        if (Mathf.Abs(dy) > 0.001f)
                            _dirVueltaCamino = new Vector2(0f, Mathf.Sign(dy));
                        else if (Mathf.Abs(dx) > 0.001f)
                            _dirVueltaCamino = new Vector2(Mathf.Sign(dx), 0f);
                        else
                            _dirVueltaCamino = Vector2.zero;

                        _dir = -_dirVueltaCamino;
                        _zonaEncontrada = true;
                    }
                    else if (ReachY && ReachAproximateX)
                    {
                        _dirPorCaminoAntesDeTrabajo = _dir;

                        // Dirección para regresar al camino
                        if (Mathf.Abs(dx) > 0.001f)
                            _dirVueltaCamino = new Vector2(Mathf.Sign(dx), 0f);
                        else if (Mathf.Abs(dy) > 0.001f)
                            _dirVueltaCamino = new Vector2(0f, Mathf.Sign(dy));
                        else
                            _dirVueltaCamino = Vector2.zero;

                        _dir = -_dirVueltaCamino;
                        _zonaEncontrada = true;
                    }
                }
            }
            // 2. Aproximación a la posición de trabajo
            else if (_llamadoTrabajar)
            {
                Movement(_dir);

                ReachX = Mathf.Abs(transform.position.x - _posicionTrabajo.x) <= 1f;
                ReachY = Mathf.Abs(transform.position.y - _posicionTrabajo.y) <= 1f;

                if (ReachX && ReachY)
                {
                    Movement(Vector2.zero);

                    _posEnElTrabajo = transform.position;
                    _paradoTrabajar = true;
                    _trabajoTerminado = false;
                    AnimateIlde(-_dirVueltaCamino);
                }
            }
        }
        // 3. Cuando está parado y ha terminado de trabajar
        else if (_trabajoTerminado)
        {
            float distancia = Vector2.Distance(transform.position, _posEnElTrabajo);

            // Si ya está suficientemente alejado, recuperar movimiento normal
            if (distancia > 1f || _dirVueltaCamino == Vector2.zero)
            {
                _dir = _dirPorCaminoAntesDeTrabajo;

                _paradoTrabajar = false;
                _llamadoTrabajar = false;
                _zonaEncontrada = false;
                _trabajoTerminado = false;

                _dirVueltaCamino = Vector2.zero;
            }
            else
            {
                // Seguir alejándose del punto de trabajo
                Movement(_dirVueltaCamino);
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

    public bool IsWorking()
    {
        return _llamadoTrabajar;
    }
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
            if (!_llamadoTrabajar)
            {
                Vector2 nuevaDir;
                int direccion = UnityEngine.Random.Range(0, 4);

                if (direccion == 0) nuevaDir = Vector2.up;
                else if (direccion == 1) nuevaDir = Vector2.down;
                else if (direccion == 2) nuevaDir = Vector2.left;
                else nuevaDir = Vector2.right;

                // Si el nuevo movimiento es en el mismo eje, cambiarla por 2 vez para evitar movimientos repetitivos
                if ((_dir.x != 0 && nuevaDir.x != 0) || (_dir.y != 0 && nuevaDir.y != 0))
                {
                    direccion = UnityEngine.Random.Range(0, 4);

                    if (direccion == 0) nuevaDir = Vector2.up;
                    else if (direccion == 1) nuevaDir = Vector2.down;
                    else if (direccion == 2) nuevaDir = Vector2.left;
                    else nuevaDir = Vector2.right;
                }
                _dir = nuevaDir;
            }
            else
            {
                float dx = _posicionTrabajo.x - transform.position.x;
                float dy = _posicionTrabajo.y - transform.position.y;

                if (Mathf.Abs(dx) > 1f && Mathf.Abs(dy) < 5f)
                {
                    _dir = new Vector2(Mathf.Sign(dx), 0);
                }
                else if (Mathf.Abs(dy) > 1f)
                {
                    _dir = new Vector2(0, Mathf.Sign(dy));
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
            _animator.Play("IldeR");
        }
    }
    #endregion   

} // class WorkerMovement 
// namespace
