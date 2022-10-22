using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPiece : BasePiece 
{
    PuzzleBehaviour _context;
    
    


    
    public ArrowPiece(PuzzleBehaviour context, int row, int  column, GameObject gameObject) : base(context, row, column, gameObject){
        _context = context;
        
        
        
    }
    public override void ReceivePower(int directionOfSource)
    {
        
        frontCurrentlyFacing = rotation;
        if(!isRotating)
        {
            Debug.Log($"received power at {row} / {column}");
            isPowered = true;
            _greenLight.SetActive(true);
            TransmitPower(frontCurrentlyFacing);
            Debug.Log($"is powered at {row} / {column}");
            //_context._audioManager.Play("piecePowered");
        }
    }

    public override void TransmitPower(int directionOfTarget)
    {

        
        switch (directionOfTarget)
        {
            case 0:
            for (int i = column-1; i >= 1; i--)
            {
                if(_context.spots[(row, i)].HasPiece)
                {
                    if(!_context.spots[(row, i)].Piece.isPowered  && !_context.spots[(row, i)].Piece.isRotating)
                    {
                        _context.spots[(row, i)].Piece.ReceivePower(directionOfTarget);
                    
                        break;
                    }
                    
                }
                else
                {
                    if(i != 1)
                    {
                        _context.spots[(row, i)].leftLine.SetActive(true);
                    }
                    _context.spots[(row, i)].rightLine.SetActive(true);
                }
                
            }
            break;
            case 1:
            for (int i = row-1; i >= 1; i--)
            {
                if(_context.spots[(i, column)].HasPiece)
                {
                    if(!_context.spots[(i, column)].Piece.isPowered && !_context.spots[(i, column)].Piece.isRotating)
                    {
                        _context.spots[(i, column)].Piece.ReceivePower(directionOfTarget);
                    
                        break;
                    }
                    
                }
                else
                {
                    if(i!= 1)
                    {
                        _context.spots[(i, column)].topLine.SetActive(true);
                    }
                    
                    _context.spots[(i, column)].bottomLine.SetActive(true);
                }
                
            }
            break;
            case 2:
            for (int i = column+1; i <= _context.columns; i++)
            {
                if(_context.spots[(row, i)].HasPiece)
                {
                    if(!_context.spots[(row, i)].Piece.isPowered && !_context.spots[(row, i)].Piece.isRotating)
                    {
                       _context.spots[(row, i)].Piece.ReceivePower(directionOfTarget);
                    
                    break; 
                    }
                    
                }
                else
                {
                    if(i != _context.columns)
                    {
                        _context.spots[(row, i)].rightLine.SetActive(true);
                    }
                    _context.spots[(row, i)].leftLine.SetActive(true);
                    
                }
                
            }
            break;
            case 3:
            for (int i = row+1; i <= _context.rows; i++)
            {
                if(_context.spots[(i, column)].HasPiece)
                {
                    if(!_context.spots[(i, column)].Piece.isPowered && !_context.spots[(i, column)].Piece.isRotating)
                    {
                        _context.spots[(i, column)].Piece.ReceivePower(directionOfTarget);
                        break;
                    }
                    
                    
                    
                }
                else
                {
                    if(i!= _context.rows)
                    {
                        _context.spots[(i, column)].bottomLine.SetActive(true);
                    }
                    
                    _context.spots[(i, column)].topLine.SetActive(true);
                    
                }
                
            }
            break;
        }
        
    }
}
