using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPiece : BasePiece
{
    PuzzleBehaviour _context;
    int receiverCurrentlyFacing;
    public TargetPiece(PuzzleBehaviour context, int row, int  column, GameObject gameObject) : base(context, row, column, gameObject){
        _context = context;
        isStatic = true;
        istarget = true;
        receiverCurrentlyFacing = rotation;
    }

    public override void ReceivePower(int directionOfSource)
    {
        if(directionOfSource == receiverCurrentlyFacing)
        {
            isPowered = true;
            _greenLight.SetActive(true);
            Debug.Log("target powered");
        }
    }
}
