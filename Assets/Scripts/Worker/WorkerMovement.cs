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
    /// <summary>
    /// Direccion del trabajador
    /// </summary>
    private Vector2 dir;
    /// <summary>
    /// Componente animator del Trabajador
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Sprite render del Trabajador
    /// </summary>
    private SpriteRenderer _sprite;
    /// <summary>
    /// Nos dicen si nos han llamado
    /// </summary>
    private bool _call;
    /// <summary>
    /// Transform del player
    /// </summary>
    private Transform _transform;
    private Vector2 _posToReach;
    private bool _finish;
    private bool _stop;
    private Vector2 _inicialPos;
    private bool ReachX;
    private bool ReachY;
    private bool izq;
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
        _call = false;
        _finish = false;
        _stop = false;
        izq = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Esperamos parados en la oficina 
        if (!_call)
        {
            Walk();
             //Vector2 pause = new Vector2(0,0);
            //Movement(pause);
            Vector2 look = new Vector2(0,-1);
            //AnimateIlde(look);
        }
        else if (_call && !_stop)//Nos dicen donde ir y trabajamos
        {
            Work();
        }
        else if(_call && _finish )//Volvemos a la ofi
        {
            MoveToInicalPos();
        }

        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
   public void Call(Vector2 dir)
   {
        _call = true;
        _posToReach = dir;
       
   }
    public void Call1()
    {
        _call = true;
        _posToReach = new Vector2(-19.7f, 6f);
    }
    public void finish()
    {
        _finish = true;
    }
    public void Stop()
    {
        _stop = true;

        Vector2 stop = new Vector2(0, 0);
        Movement(stop);
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
    private void Movement(Vector2 dirs)
    {
        _rb.linearVelocity = dirs * Speed;
    }
    private void AnimateIlde(Vector2 direction)
    {
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
    private void Work()
    {
        Vector2 dir = new Vector2(0, 0);
        ReachX = Math.Abs(_transform.position.x - _posToReach.x) <= 1;
        ReachY = Math.Abs(_transform.position.y - _posToReach.y) <= 1;
        if (!ReachX)
        {

            if (_posToReach.x > _transform.position.x)
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
                if (_posToReach.y > _transform.position.y)
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
    }
    private void MoveToInicalPos()
    {
        Vector2 dir = new Vector2(0, 0);
        ReachY = Math.Abs(_inicialPos.y-_transform.position.y) <= 2.8;
        if (!ReachY)
        {
            //Movemos la y
            if (_inicialPos.y > _transform.position.y)
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
        if (ReachY)
        {
            _stop = false;
            _call = false;
            _finish = false;
        }
        Movement(dir);
    }
    private void Walk()
    {
        Vector2 dir = new Vector2(0, 0);
        if (_transform.position.x < _inicialPos.x + 10 && !izq)
        {
            //Esta a derecha
            Debug.Log("derecha");
            dir = new Vector2(1, 0);
        }
        else if(_transform.position.x >= _inicialPos.x + 10 && !izq)
        {
            izq = true;
            dir = new Vector2(-1, 0);
            Debug.Log("cambio a izqu");
        }
        else if(_transform.position.x > _inicialPos.x - 10 && izq)
        {
            //Esta a izquierda
            Debug.Log("izqu");
            dir = new Vector2(-1, 0);
        }
        else if (_transform.position.x <= _inicialPos.x - 10 && izq)
        {
            izq = false;
            dir = new Vector2(1, 0);
            Debug.Log("cambio a drech");
        }
        Movement(dir);

    }
    #endregion   

} // class WorkerMovement 
// namespace
