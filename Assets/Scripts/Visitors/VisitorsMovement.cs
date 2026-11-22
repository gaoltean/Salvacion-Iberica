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
    private Vector2 dir;
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
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Nos movemos a una velocidad establecida
        Movement(dir);
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
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    /// <summary>
    /// Metodo encargado del movimiento del visitante
    /// </summary>
    /// <param name="dir">direcion del movimiento </param>
    private void Movement(Vector2 dir)
    {
        _rb.linearVelocity = dir * Speed;
    }
    #endregion   

} // class VisitorsMovement 
// namespace
