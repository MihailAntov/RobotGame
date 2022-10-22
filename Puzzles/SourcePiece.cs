using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourcePiece : BasePiece
{
    PuzzleBehaviour _context;
    int currentlyFacing;
    public SourcePiece(PuzzleBehaviour context, int row, int column, GameObject gameObject) : base(context, row, column, gameObject)
    {
        _context = context;
        isStatic = true;
        isSource = true;
        currentlyFacing = rotation;

    }
    public override void HandlePower()
    {
        isPowered = true;
        _greenLight.SetActive(true);
        switch (currentlyFacing)
        {


            case 0:
                for (int i = column - 1; i >= 1; i--)
                {
                    if (_context.spots[(row, i)].HasPiece)
                    {
                        if (!_context.spots[(row, i)].Piece.isPowered)
                        {
                            _context.spots[(row, i)].Piece.ReceivePower(currentlyFacing);
                            break;
                        }

                    }
                    else
                    {
                        if(i!= 1)
                        {
                            _context.spots[(row, i)].leftLine.SetActive(true);
                        }
                        _context.spots[(row, i)].rightLine.SetActive(true);
                    }
                }
                break;
            case 1:

                for (int i = row - 1; i >= 1; i--)
                {
                    if (_context.spots[(i, column)].HasPiece)
                    {
                        if (!_context.spots[(row, i)].Piece.isPowered )
                        {
                            _context.spots[(i, column)].Piece.ReceivePower(currentlyFacing);
                            break;
                        }

                    }
                    else
                    {
                        if(i!= 1)
                        {
                            _context.spots[(row, i)].topLine.SetActive(true);
                        }
                        _context.spots[(row, i)].bottomLine.SetActive(true);
                    }
                }
                break;
            case 2:
                for (int i = column + 1; i <= _context.columns; i++)
                {
                    if (_context.spots[(row, i)].HasPiece)
                    {
                        if (!_context.spots[(row, i)].Piece.isPowered ) 
                        {
                            _context.spots[(row, i)].Piece.ReceivePower(currentlyFacing);
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
                for (int i = row + 1; i <= _context.rows; i++)
                {
                    if (_context.spots[(i, column)].HasPiece)
                    {
                        if (!_context.spots[(row, i)].Piece.isPowered )
                        {
                            _context.spots[(i, column)].Piece.ReceivePower(currentlyFacing);
                            break;
                        }

                    }
                    else
                    {
                        if(i != _context.rows)
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
