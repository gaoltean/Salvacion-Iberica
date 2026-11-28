//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Nombre del juego
// Jam "Cultura Abierta" 9 edicion
//---------------------------------------------------------

// Añadir aquí el resto de directivas using
using UnityEngine;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;



/// <summary>
/// Descripción de qué es y para qué sirve
/// </summary>
public class VisitorsGenerator : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos públicos y de inspector en formato PascalCase
    /// <summary>
    /// Prefab de los visitantes
    /// </summary>
    [SerializeField] private GameObject Visitor1;
    [SerializeField] private GameObject Visitor2;
    [SerializeField] private GameObject Visitor3;
    [SerializeField] private GameObject Visitor4;
    [SerializeField] private GameObject Visitor5;
    [SerializeField] private GameObject Visitor6;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // Los atributos privados en formato _camelCase
    /// <summary>
    /// Tiempo del ultimo spawn
    /// </summary>
    private float _lastVisitorSpawn;
    /// <summary>
    /// Tiempo que hay que esperar entre spawns
    /// </summary>
    private float _TimeToWait;
    private float _maxTime;
    private float _minTime;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        //Creamos el primero
        GenerateVisitor();
        _minTime = 2.5f;
        _maxTime = 6;
        _TimeToWait = Random.Range(_minTime, _maxTime);
        _lastVisitorSpawn = Time.time;
       
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Si ha pasado el tiempo correspondiente creamos otro
        if(Time.time-_lastVisitorSpawn > _TimeToWait && !TimeManager.Instance.IsNight())
        {
            GenerateVisitor();
            _TimeToWait = Random.Range(_minTime, _maxTime);
            _lastVisitorSpawn = Time.time;
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    public void SetFame(float cant)
    {
        //Debug.Log(cant);
        if(cant > 90)
        {
            _minTime = 0.5f;
            _maxTime = 2;
        }else if(cant > 70)
        {
            _minTime = 1.5f;
            _maxTime = 4;

        }
        else if(cant > 50)
        {
            _minTime = 2.5f;
            _maxTime = 6;
        }
        else if (cant > 30)
        {
            _minTime = 4;
            _maxTime = 8;
        }
        else
        {
            _minTime = 5;
            _maxTime = 10;
        }
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí con ///<summary>
    // Estos métodos se nombren en formato PascalCase
    /// <summary>
    /// Metodo encargado de generar visitantes
    /// </summary>
    private void GenerateVisitor()
    {
        if (TimeManager.Instance.IsDay())
        {
            int visitors = Random.Range(1, 7);
            switch (visitors)
            {
                case 1: GameObject visitor1 = Instantiate(Visitor1, this.transform.position, this.transform.rotation);break;
                case 2: GameObject visitor2 = Instantiate(Visitor2, this.transform.position, this.transform.rotation);break;
                case 3: GameObject visitor3 = Instantiate(Visitor3, this.transform.position, this.transform.rotation);break;
                case 4: GameObject visitor4 = Instantiate(Visitor4, this.transform.position, this.transform.rotation);break;
                case 5: GameObject visitor5 = Instantiate(Visitor5, this.transform.position, this.transform.rotation);break;
                case 6: GameObject visitor6 = Instantiate(Visitor6, this.transform.position, this.transform.rotation);break;
            }
        }
       
    }
    #endregion   

} // class VisitorsGenerator 
// namespace
