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
public class ParkEntry : MonoBehaviour
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
    /// <summary>
    /// Movimiento de los visitantes
    /// </summary>
    private VisitorsMovement _visitor;
    /// <summary>
    /// Precio de la entrada
    /// </summary>
    private float _entrancePrice = 2;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    /// <summary>
    /// Metodo para cambiar el precio de la entrada al parque
    /// </summary>
    /// <param name="entrance">Nuevo precio de la entrada</param>
    public void SetEntrancePrice(float entrance)
    {
        _entrancePrice = entrance;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _visitor = collision.GetComponent<VisitorsMovement>();
        if (_visitor)
        {
            //Pagamos la entrada
            _visitor.Pay(_entrancePrice);
        }
    }
    #endregion   

} // class ParkEntry 
// namespace
