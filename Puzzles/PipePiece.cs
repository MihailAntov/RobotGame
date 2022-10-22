using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePiece : BasePiece
{
   PuzzleBehaviour _context;
   public PipePiece(PuzzleBehaviour context, int row, int column, GameObject gameObject): base(context, row, column, gameObject)
   {
      _context = context;
   }

    public override void ReceivePower(int directionOfSource)
    {
        backReceiverCurrentlyFacing = 1 + rotation;
        frontReceiverCurrentlyFacing = 0 + rotation;
        backCurrentlyFacing = 3 + rotation;
        frontCurrentlyFacing = 2 + rotation;
        if(backCurrentlyFacing>3)
        {
            backCurrentlyFacing -= 4;
        }
        if(frontCurrentlyFacing>3)
        {
            frontCurrentlyFacing -= 4;
        }

        if(backReceiverCurrentlyFacing == directionOfSource)
        {
            isPowered = true;
            TransmitPower(frontCurrentlyFacing);
            Debug.Log($"is powered at {row} / {column}");
        }
         if(frontReceiverCurrentlyFacing == directionOfSource)
        {
            isPowered = true;
            TransmitPower(backCurrentlyFacing);
            Debug.Log($"is powered at {row} / {column}");
        }

        Debug.Log($"received power at {row} / {column}");
    }
    public override void TransmitPower(int directionOfTarget)
    {
        switch (directionOfTarget)
        {
            case 0:
               if(column > 1)
               {
                  if(_context.spots[(row, column - 1)].HasPiece)
                  {
                     _context.spots[(row, column - 1)].Piece.ReceivePower(directionOfTarget);
                  }
                  
               }
            break;
            case 1:
            if(row > 1)
               {
                  if(_context.spots[(row -1, column)].HasPiece)
                  {
                     _context.spots[(row -1, column)].Piece.ReceivePower(directionOfTarget);
                  }
                  
               }
            break;
            case 2:
            if(column < _context.columns)
               {
                  if(_context.spots[(row, column + 1)].HasPiece)
                  {
                     _context.spots[(row, column + 1)].Piece.ReceivePower(directionOfTarget);
                  }
                  
               }
            break;
            case 3:
            if(row < _context.rows)
               {
                  if(_context.spots[(row+1, column)].HasPiece)
                  {
                     _context.spots[(row+1, column)].Piece.ReceivePower(directionOfTarget);
                  }
                  
               }
            break;
        }
    }
}
