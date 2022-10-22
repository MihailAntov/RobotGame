using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePiece 
{
    PuzzleBehaviour _context;
    Vector3 _destination; 
    Spot currentSpot;
    public int row;
    public int column;
    Spot targetSpot;
    public bool canRotate = true;
    public int rotation;
    public bool isPowered = false;
    public int frontCurrentlyFacing;
    public int backCurrentlyFacing;
    public int backReceiverCurrentlyFacing;
    public int frontReceiverCurrentlyFacing;
    public bool isStatic = false;
    public bool isSource = false;
    public bool istarget = true;
    public bool isRotating = false;
    public GameObject _gameObject;
    public enum Directions {up, down, left, right}
    public Dictionary<string, int> directions = new Dictionary<string, int>(){{"down", 3}, {"up", 1}, {"left", 0}, {"right", 2}};
    public GameObject _greenLight;
    public BasePiece(PuzzleBehaviour context, int row, int  column, GameObject gameObject)
    {
        _context = context;
        rotation = (int)(gameObject.transform.rotation.eulerAngles.y/90);
        _gameObject = gameObject;
        _greenLight = GameObject.Instantiate(_context.greenLight, gameObject.transform.position+Vector3.up*_context.increment +gameObject.transform.right*(-(_context.increment / 2f)), gameObject.transform.rotation, gameObject.transform);
        _greenLight.SetActive(false);
        
    }
    public bool canMoveTo(int rowChange, int columnChange)
    {
        if((row+rowChange<= _context.rows) && (row + rowChange >= 1) && (column+columnChange <= _context.columns) && (column + columnChange >= 1))
        {
            if(!_context.spots[(row+rowChange, column + columnChange)].HasPiece)
            {
                return true;
            }
        }
       return false;
    }
    public virtual void MoveUp()
    {
        if(canMoveTo(-1, 0))
        {
            _context._destination = Object.transform.position + Vector3.forward * _context.increment;
            _context.controlsLocked = true;
            _context.spots[(row, column)].HasPiece = false;
            //_context.spots[(row-1, column)].HasPiece = true;
            _context.spots[(row-1, column)].Piece = _context.spots[(row, column)].Piece;
            _context.spots[(row, column)].Piece = null;
            
            row -= 1;
            
            _context.selectedPiece.Object.GetComponent<objectDetails>().row-=1;

        }
        
    }
    public virtual void MoveDown()
    {
        if(canMoveTo(1, 0))
        {
            _context._destination = Object.transform.position + Vector3.back * _context.increment;
        _context.controlsLocked = true;
        _context.spots[(row, column)].HasPiece = false;
            //_context.spots[(row+1, column)].HasPiece = true;

            _context.spots[(row+1, column)].Piece = _context.spots[(row, column)].Piece;
            _context.spots[(row, column)].Piece = null;

            row += 1;
            _context.selectedPiece.Object.GetComponent<objectDetails>().row+=1;
        }
        
    }
    public virtual void MoveLeft()
    {
        if(canMoveTo(0, -1))
        {
            _context._destination = Object.transform.position + Vector3.left * _context.increment;
        _context.controlsLocked = true;
        _context.spots[(row, column)].HasPiece = false;
            //_context.spots[(row, column-1)].HasPiece = true;

            _context.spots[(row, column-1)].Piece = _context.spots[(row, column)].Piece;
            _context.spots[(row, column)].Piece = null;

            column -= 1;
            _context.selectedPiece.Object.GetComponent<objectDetails>().column-=1;
        }
        
    }
    public virtual void MoveRight()
    {
        if (canMoveTo(0, 1))
        {
            _context._destination = Object.transform.position + Vector3.right * _context.increment;
        _context.controlsLocked = true;
        _context.spots[(row, column)].HasPiece = false;
            //_context.spots[(row, column+1)].HasPiece = true;

            _context.spots[(row, column+1)].Piece = _context.spots[(row, column)].Piece;
            _context.spots[(row, column)].Piece = null;

            column += 1;
            _context.selectedPiece.Object.GetComponent<objectDetails>().column+=1;
        }
        
    }
    public virtual void Rotate()
    {
       if(canRotate)
       {
           _context._rotation = Object.transform.rotation * Quaternion.Euler(0, 90f,0);
        _context.controlsLocked = true; 
        isRotating = true;
        _context.selectedPiece.rotation++;
        if(_context.selectedPiece.rotation == 4)
        {
            _context.selectedPiece.rotation = 0;
        }
        //_context.spots[(_context.selectedPiece.row, _context.selectedPiece.column)].HasPiece = false;
       }
        
    }
    public virtual void ReceivePower(int directionOfSource)
    {

    }

    public virtual void TransmitPower(int directionOfTarget)
    {

    }
    public virtual void HandlePower()
    {
        // Debug.Log($"Entering handle power from {row} / {column}");
        // if (row == _context.powerRow1 && column == _context.powerColumn1)
        // {
        //     ReceivePower(directions[_context.powerSourceDirection1]);
        // }
        // if (row == _context.powerRow2 && column == _context.powerColumn2)
        // {
        //     ReceivePower(directions[_context.powerSourceDirection2]);
        // }
        
    }
    public GameObject Object;
    
}
