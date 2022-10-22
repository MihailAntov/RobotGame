using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public int rows = 3;
    [SerializeField]
    public int columns = 3;
    [SerializeField]
    public float increment = 0.33f;
    [SerializeField]
    GameObject piece1;
    [SerializeField]
    GameObject piece2;
    [SerializeField]
    GameObject piece3;
    [SerializeField]
    GameObject piece4;
    [SerializeField]
    GameObject piece5;
    [SerializeField]
    GameObject piece6;
    [SerializeField]
    GameObject piece7;
    [SerializeField]
    GameObject piece8;
    [SerializeField]
    GameObject piece9;
    [SerializeField]
    GameObject piece10;
    [SerializeField]
    Light mainLights;
    [SerializeField]



    public List<GameObject> startingPieces;

    public BasePiece selectedPiece;
    PlayerInput _playerInput;
    public Vector3 _destination;
    public Quaternion _rotation;
    PlayerStateMachine _playerStateMachine;

    public AudioManager _audioManager;
    public float timeCount = 0;
    public Dictionary<(int, int), Spot> spots = new Dictionary<(int, int), Spot>();
    public bool controlsLocked = false;
    bool highlighting = false;
    bool puzzleCompleted = false;
    public bool powerComingOn = false;

    Vector2 mousePosition;
    GameObject highlightedObject;
    [SerializeField]
    public GameObject horizontalLine;
    [SerializeField]
    public GameObject verticalLine;
    [SerializeField]
    public GameObject greenLight;
    Transform greenLightAtSpot11;
    int layerMask;
    public int delayNeeded;
    public bool isRotating;
    public bool lightsOn;
    List<GameObject> targets = new List<GameObject>();



    void Awake()
    {
        mainLights.intensity = 0;
        RenderSettings.ambientIntensity = 0f;
        RenderSettings.reflectionIntensity = 0f;
        // mainLights.intensity = 1;
        // RenderSettings.ambientIntensity = 1f;
        // RenderSettings.reflectionIntensity = 1f;
        isRotating = false;
        //greenLightAtSpot11 = GameObject.Find("GreenLightAtSpot11").transform;
        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= columns; j++)
            {
                spots.Add((i, j), new Spot(i, j, false, this));


            }
        }

        foreach (Spot spot in spots.Values)
        {
            if (spot.Row != 1)
            {
                spot.topLine = GameObject.Instantiate(verticalLine, verticalLine.transform.position - Vector3.left * (((spot.Column - 1) * increment)) - Vector3.forward * ((spot.Row - 1) * increment) + Vector3.forward * (0.5f * increment), Quaternion.Euler(0, 0, 0));
                spot.topLine.SetActive(false);
            }

            if (spot.Column != 1)
            {

                spot.leftLine = GameObject.Instantiate(horizontalLine, horizontalLine.transform.position - Vector3.left * (((spot.Column - 1) * increment)) - Vector3.forward * ((spot.Row - 1) * increment) + Vector3.left * (0.5f * increment), Quaternion.Euler(0, 0, 0));
                spot.leftLine.SetActive(false);
            }

            if (spot.Row != rows)
            {
                spot.bottomLine = GameObject.Instantiate(verticalLine, verticalLine.transform.position - Vector3.left * (((spot.Column - 1) * increment)) - Vector3.forward * ((spot.Row - 1) * increment), Quaternion.Euler(0, 0, 0));
                spot.bottomLine.SetActive(false);
            }

            if (spot.Column != columns)
            {
                spot.rightLine = GameObject.Instantiate(horizontalLine, horizontalLine.transform.position - Vector3.left * (((spot.Column - 1) * increment)) - Vector3.forward * ((spot.Row - 1) * increment), Quaternion.Euler(0, 0, 0));
                spot.rightLine.SetActive(false);
            }
        }
        Object.Destroy(horizontalLine);
        Object.Destroy(verticalLine);


        if (piece1 != null)
            startingPieces.Add(piece1);
        if (piece2 != null)
            startingPieces.Add(piece2);
        if (piece3 != null)
            startingPieces.Add(piece3);
        if (piece4 != null)
            startingPieces.Add(piece4);
        if (piece5 != null)
            startingPieces.Add(piece5);
        if (piece6 != null)
            startingPieces.Add(piece6);
        if (piece7 != null)
            startingPieces.Add(piece7);
        if (piece8 != null)
            startingPieces.Add(piece8);
        if (piece9 != null)
            startingPieces.Add(piece9);

        AddPieces();
        //AddPiece(1, 3, new ArrowPiece(this, 1, 3), piece1);
        //AddPiece(2, 2, new ArrowPiece(this, 2, 2), piece2);
        // piece1.GetComponent<objectDetails>().row = 1;
        // piece1.GetComponent<objectDetails>().column = 3;
        layerMask = LayerMask.GetMask("Puzzle");
        delayNeeded = (int)(increment * 13);




        _playerInput = new PlayerInput();
        _playerInput.PuzzleControls.MoveDown.started += OnMoveDown;
        _playerInput.PuzzleControls.MoveUp.started += OnMoveUp;
        _playerInput.PuzzleControls.MoveLeft.started += OnMoveLeft;
        _playerInput.PuzzleControls.MoveRight.started += OnMoveRight;
        _playerInput.PuzzleControls.Rotate.started += OnRotate;
        _playerInput.PuzzleControls.Select.started += OnClick;
        _playerStateMachine = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
        _audioManager = GameObject.FindObjectOfType<AudioManager>();
        selectedPiece = spots[(piece1.GetComponent<objectDetails>().row, piece1.GetComponent<objectDetails>().column)].Piece;
        selectedPiece.Object = piece1;
        selectedPiece.row = piece1.GetComponent<objectDetails>().row;
        selectedPiece.column = piece1.GetComponent<objectDetails>().column;

        _destination = selectedPiece.Object.transform.position;



    }
    void AddPieces()
    {
        foreach (GameObject piece in startingPieces)
        {

            Debug.Log("Creating a piece.");
            AddPiece(piece.GetComponent<objectDetails>().row, piece.GetComponent<objectDetails>().column, piece);
            if (piece.GetComponent<objectDetails>().pieceType == "target")
            {
                targets.Add(piece);
            }


        }
    }

    void DeactivateGreenLines()
    {
        foreach (Spot spot in spots.Values)
        {
            if (spot.Row != 1)
            {
                spot.topLine.SetActive(false);
            }

            if (spot.Column != 1)
            {

                spot.leftLine.SetActive(false);
            }

            if (spot.Row != rows)
            {
                spot.bottomLine.SetActive(false);
            }

            if (spot.Column != columns)
            {
                spot.rightLine.SetActive(false);
            }
        }
    }

    public void AddPiece(int row, int column, GameObject gameObject)
    {

        //spot.Piece = piece;
        spots[(row, column)].HasPiece = true;
        spots[(row, column)].Object = gameObject;
        spots[(row, column)].Piece = CreatePiece(gameObject);
        spots[(row, column)].Piece.row = row;
        spots[(row, column)].Piece.column = column;
        spots[(row, column)].Piece.rotation = (int)(gameObject.transform.rotation.eulerAngles.y / 90);


        //gameObject.GetComponent<objectDetails>().row = row;
        //gameObject.GetComponent<objectDetails>().column = column;
    }

    // public void AddPiece(int row, int column, BasePiece piece, GameObject gameObject)
    // {
    //     Spot spot = spots[(row, column)];
    //     //spot.Piece = piece;
    //     spot.HasPiece = true;
    //     spot.Object = gameObject;
    //     spot.Piece = piece;
    //     //gameObject.GetComponent<objectDetails>().row = row;
    //     //gameObject.GetComponent<objectDetails>().column = column;
    // }
    BasePiece CreatePiece(GameObject gameObject)
    {
        switch (gameObject.GetComponent<objectDetails>().pieceType)
        {
            case "arrow":
                return new ArrowPiece(this, gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column, gameObject);
            case "pipe":
                return new PipePiece(this, gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column, gameObject);
            case "source":
                return new SourcePiece(this, gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column, gameObject);
            case "target":
                return new TargetPiece(this, gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column, gameObject);
            default:
                return new ArrowPiece(this, gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column, gameObject);
        }
    }


    void OnMoveDown(InputAction.CallbackContext context)
    {
        if (_playerStateMachine.IsUsingTerminal && !controlsLocked)
        {
            selectedPiece.MoveDown();
        }

    }
    void OnClick(InputAction.CallbackContext context)
    {
        if (highlighting && !controlsLocked && _playerStateMachine.IsUsingTerminal)
        {
            if (!spots[(highlightedObject.GetComponent<objectDetails>().row, highlightedObject.GetComponent<objectDetails>().column)].Piece.isStatic)
            {
                Debug.Log("Click");
                selectedPiece.Object.GetComponent<Outline>().enabled = false;
                selectedPiece = spots[(highlightedObject.GetComponentInParent<objectDetails>().row, highlightedObject.GetComponentInParent<objectDetails>().column)].Piece;
                selectedPiece.Object = highlightedObject;
                selectedPiece.Object.GetComponent<Outline>().enabled = true;
                selectedPiece.row = highlightedObject.GetComponentInParent<objectDetails>().row;
                selectedPiece.column = highlightedObject.GetComponentInParent<objectDetails>().column;

                _destination = highlightedObject.transform.position;
                _rotation = highlightedObject.transform.rotation;
                controlsLocked = true;
                timeCount = delayNeeded - 1;
            }
        }

    }
    void OnMoveUp(InputAction.CallbackContext context)
    {
        if (_playerStateMachine.IsUsingTerminal && !controlsLocked)
        {
            selectedPiece.MoveUp();
        }
    }
    void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (_playerStateMachine.IsUsingTerminal && !controlsLocked)
        {
            selectedPiece.MoveLeft();
        }
    }
    void OnMoveRight(InputAction.CallbackContext context)
    {
        if (_playerStateMachine.IsUsingTerminal && !controlsLocked)
        {
            selectedPiece.MoveRight();
        }
    }
    void OnRotate(InputAction.CallbackContext context)
    {
        if (_playerStateMachine.IsUsingTerminal && !controlsLocked)
        {
            selectedPiece.Rotate();


        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        selectedPiece.Object.transform.position = Vector3.MoveTowards(selectedPiece.Object.transform.position, _destination, 0.08f);
        selectedPiece.Object.transform.rotation = Quaternion.RotateTowards(selectedPiece.Object.transform.rotation, _rotation, 1.48f);
        if (controlsLocked)
        {
            timeCount++;
            Debug.Log(timeCount);

            if (timeCount == 1)
            {

                foreach (GameObject gameObject in startingPieces)
                {
                    spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece.isPowered = false;
                    spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece._greenLight.SetActive(false);

                }
                DeactivateGreenLines();
                foreach (GameObject gameObject in startingPieces)
                {

                    if (spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece != null)
                    {
                        spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece.HandlePower();
                    }

                    //spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece.
                }
                spots[(selectedPiece.row, selectedPiece.column)].HasPiece = true;
                _audioManager.Play("pieceMove");
            }

            // if(isRotating)
            // {
            //     timeCount = 200;
            // }



            if (timeCount > delayNeeded)
            {
                isRotating = false;
                controlsLocked = false;
                _audioManager.Stop("pieceMove");
                timeCount = 0;
                Debug.Log("Unlocking");

                foreach (GameObject gameObject in startingPieces)
                {
                    spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece.isPowered = false;
                    spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece.isRotating = false;
                    spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece._greenLight.SetActive(false);

                }
                DeactivateGreenLines();
                foreach (GameObject gameObject in startingPieces)
                {
                    spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece.HandlePower();
                    //spots[(gameObject.GetComponent<objectDetails>().row, gameObject.GetComponent<objectDetails>().column)].Piece.
                }
                if (selectedPiece.isPowered)
                {
                    _audioManager.Play("piecePowered");
                }

                if (CheckTargets())
                {
                    puzzleCompleted = true;
                    powerComingOn = true;
                    timeCount = 0;

                    //   mainLights.intensity = 1;

                    //      RenderSettings.ambientIntensity = 1;
                    //      RenderSettings.reflectionIntensity = 1;   
                    //      _audioManager.Play("lightsOn");                    

                }
                else
                {
                    puzzleCompleted = false;
                    lightsOn = false;
                    powerComingOn = false;
                    mainLights.intensity = 0f;
                    _audioManager.Stop("ambLight");
                    _audioManager.Play("ambDark");

                    RenderSettings.ambientIntensity = 0.2f;
                    RenderSettings.reflectionIntensity = 0.2f;
                }
            }



        }

        if (!lightsOn && puzzleCompleted)
        {
            timeCount++;
            if (timeCount > 150)
            {
                mainLights.intensity = 1;
                RenderSettings.ambientIntensity = 1;
                RenderSettings.reflectionIntensity = 1;
                _audioManager.Play("lightsOn");
                lightsOn = true;
                powerComingOn = false;
                timeCount = 0;
                _audioManager.Stop("ambDark");
                _audioManager.Play("ambLight");
            }
        }

        if (!controlsLocked)
        {
            Ray selectRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(selectRay, out hit, 500f, layerMask))
            {
                //Debug.Log("piece found - row :"+hit.collider.GetComponent<objectDetails>().row +" column :"+ hit.collider.GetComponent<objectDetails>().column);
                highlightedObject = hit.collider.gameObject;
                highlighting = true;


            }
            else
            {
                highlighting = false;
            }
        }
    }


    public bool CheckTargets()
    {
        foreach (GameObject piece in targets)
        {
            if (!spots[(piece.GetComponent<objectDetails>().row, piece.GetComponent<objectDetails>().column)].Piece.isPowered)
            {
                return false;
            }
        }
        return true;
    }
    private void OnEnable()
    {
        _playerInput.PuzzleControls.Enable();
    }

    private void OnDisable()
    {
        //_playerInput.PuzzleControls.Disable();
    }

    private void OnGUI()
    {

        //GUI.Box(new Rect(20, 50, 250, 25), spots[(1,1)].HasPiece.ToString());
        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= columns; j++)
            {
                //GUI.Box(new Rect(20+150*(j-1), 50f*i, 100, 25),spots[(i, j)].HasPiece? $"{spots[(i,j)].Piece.rotation}"+"/"+$"{spots[(i,j)].Piece.isPowered}" : " ");
            }
        }


        // GUI.Box(new Rect(20, 100, 200, 25),"up"+  selectedPiece.canMove(-1, 0).ToString());
        // GUI.Box(new Rect(20, 150, 200, 25),"right"+  selectedPiece.canMove(0, 1).ToString());
        // GUI.Box(new Rect(20, 200, 200, 25),"highlighting"+  highlighting);
    }
}
public class Spot
{
    public Spot(int row, int column, bool hasPiece, PuzzleBehaviour _context)
    {
        Row = row;
        Column = column;
        HasPiece = hasPiece;




    }
    public int Row;
    public int Column;
    public bool HasPiece;
    public BasePiece Piece;
    public GameObject Object;
    public bool isSubPowered;
    public bool isSubPoweredHorizontally;
    public bool isSubPoweredVertically;
    public GameObject topLine;
    public GameObject bottomLine;
    public GameObject leftLine;
    public GameObject rightLine;

}
