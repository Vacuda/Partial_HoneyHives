using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info_Slot
{
    //MEMBERS

    public a_ADDRESS Address;

    public bool IsMovable;
    public bool IsSpinnable;
    public bool Stored_IsMovable;
    public bool Stored_IsSpinnable;

    public bool IsStuck;
    public bool IsDeactivated;

    public int Current_TriOffset;
    public fv_FACEVALUE fslot_1;
    public fv_FACEVALUE fslot_2;
    public fv_FACEVALUE fslot_3;
    public fv_FACEVALUE fslot_4;
    public fv_FACEVALUE fslot_5;
    public fv_FACEVALUE fslot_6;


    //CONSTRUCTOR
    public Info_Slot(   a_ADDRESS address, 
                        bool movable,
                        bool spinnable,
                        bool stored_movable,
                        bool stored_spinnable,
                        bool stuck,
                        bool deactivated,
                        int current_TriOffset,
                        fv_FACEVALUE fslot_1,
                        fv_FACEVALUE fslot_2,
                        fv_FACEVALUE fslot_3,
                        fv_FACEVALUE fslot_4,
                        fv_FACEVALUE fslot_5,
                        fv_FACEVALUE fslot_6)
    {
        this.Address = address;
        this.IsMovable = movable;
        this.IsSpinnable = spinnable;
        this.Stored_IsMovable = stored_movable;
        this.Stored_IsSpinnable = stored_spinnable;
        this.IsStuck = stuck;
        this.IsDeactivated = deactivated;
        this.Current_TriOffset = current_TriOffset;
        this.fslot_1 = fslot_1;
        this.fslot_2 = fslot_2;
        this.fslot_3 = fslot_3;
        this.fslot_4 = fslot_4;
        this.fslot_5 = fslot_5;
        this.fslot_6 = fslot_6;
    }

    //CONSTRUCTOR
    public Info_Slot(a_ADDRESS address)
    {
        this.Address = address;
    }
}
