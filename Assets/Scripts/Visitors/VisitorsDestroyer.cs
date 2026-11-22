//---------------------------------------------------------
// Destuye los Visitantes que pasan por su trigger
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using UnityEngine;
using System;


/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class VisitorsDestroyer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    [SerializeField] private GameObject VisitorGenerator;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Movimiento del visitante
    /// </summary>
    private VisitorsMovement _visitor;
    private VisitorsGenerator _generator;
    private float _actualfelicidad;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _actualfelicidad = 50;
        _generator = VisitorGenerator.GetComponent<VisitorsGenerator>();
        if (!_generator)
        {
            Debug.Log("No hay generador asignado");
        }
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Miramos si es un visitante para destruirlo
        _visitor = collision.GetComponent<VisitorsMovement>();
        if (_visitor){
            float felicidad = _visitor.GetFelicidad();
            Debug.Log(_actualfelicidad);
            if(Math.Abs(_actualfelicidad - felicidad) >= 10)
            {
                _actualfelicidad = felicidad;
                _generator.SetFame(_actualfelicidad);
            }

            Destroy(collision.gameObject);
        }
    }
    #endregion   

} // class VisitorsDestroyer 
// namespace
