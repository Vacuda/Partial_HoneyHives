using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static t_TRI;

public class Piece : MonoBehaviour
{
    //REFERENCES
    public Camera MainCamera;
    public PlayerController Controller;
    private PlayerControls Controls;
    public PieceGlobals pg;
    public ColorChanger colorchanger;
    public MaterialChanger materialchanger;
    public WorldGrid WorldGridScript;
    public GameLevel game_level;
    public GameObject PF_StickyGlob;
    public Renderer piece_rend;
    public MoverBase mover_base;
    public BeeBoxCluster BeeBoxClusterScript;

    //MEMBERS

    public int Current_TriOffset = 0;
    public fv_FACEVALUE fslot_1;
    public fv_FACEVALUE fslot_2;
    public fv_FACEVALUE fslot_3;
    public fv_FACEVALUE fslot_4;
    public fv_FACEVALUE fslot_5;
    public fv_FACEVALUE fslot_6;

    public bool IsMovable;
    public bool IsSpinnable;
    public bool IsStuck = false;
    public bool IsDeactivated = false;
    public bool IsInHand = false;
    public bool ActiveSpin = false;
    public float TargetSpinTotal = 0.0f;
    public GameObject DestinationArea;

    //coded nomove animation
    private bool ActiveNoMoveFeedBack = false;
    private float time_start;
    private float timechange;
    private float percentage_complete;
    private float new_z;
    private Vector3 altered_position;

    public bool Stored_IsMovable;
    public bool Stored_IsSpinnable;

    private SortingGroup sorting_component;
    private Transform FaceValueTransform;
    private GameObject StickyGlobObject;

    private Animator anim;

    public void Awake()
    {
        sorting_component = gameObject.GetComponent<SortingGroup>();
        FaceValueTransform = gameObject.transform.Find("VisualPiece").transform.Find("FaceValues").transform;
        anim = gameObject.transform.Find("VisualPiece").GetComponent<Animator>();
        piece_rend = gameObject.transform.Find("VisualPiece").GetComponent<MeshRenderer>();
        mover_base = gameObject.GetComponent<MoverBase>();
    }

    //ADMIN

    public void Start()
    {
        Controls = Controller.Get_Controls();

        altered_position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void FixedUpdate()
    {  
        if (IsInHand)
        {
            Move_Piece_ToMousePointerPosition();
        }

        if (ActiveSpin)
        {
            Handle_PieceRotation();
        }

        if (ActiveNoMoveFeedBack)
        {
            Handle_NoMoveFeedBack();
        }
    }


//ACTIONS

    public void Rotate_Piece()
    {
        //increment
        Current_TriOffset++;
 
        //possible reset
        if(Current_TriOffset > 5)
        {
            Current_TriOffset = Current_TriOffset - 6;
        }

        //add to targetspin
        TargetSpinTotal += 60.0f;

        ActiveSpin = true; 
    }

    public void Pickup_Piece()
    {
        //place in hand
        IsInHand = true;

        //remove parenting
        gameObject.transform.parent = null;

        //scale object down so it can get closer
        gameObject.transform.localScale = pg.Scale_GrabbedPiece;

        //reset rotation
        Reset_Rotation();

        //change sorting layer
        Change_SortingLayer_ToFront();

        //
    }

    public void Place_Piece(GameObject LocationObject, bool IsPlacedOnWorldGrid)
    {
        //if on WorldGrid
        if (IsPlacedOnWorldGrid)
        {
            //scale back object to original
            Change_Scale_ToWorldGrid();

            //change sorting layer
            Change_SortingLayer_ToWorldGrid();
        }
        else
        {
            //scale back object to original
            Change_Scale_ToBeeBox();

            //change sorting layer
            Change_SortingLayer_ToBeeBox();
        }

        //set position to LocationObject
        gameObject.transform.position = LocationObject.transform.position;

        //set piece parent to LocationObject
        gameObject.transform.parent = LocationObject.transform;

        //remove from in hand status
        IsInHand = false;
    }

    public void Place_Piece_AfterMovement()
    {
        //on beebox
        { 
            //scale back object to original
            Change_Scale_ToBeeBox();

            //change sorting layer
            Change_SortingLayer_ToBeeBox();
        }

        //set position to LocationObject
        gameObject.transform.position = DestinationArea.transform.position;

        //set piece parent to LocationObject
        gameObject.transform.parent = DestinationArea.transform;


    }

    private void Move_Piece_ToMousePointerPosition()
    {
        //get mouse position
        Vector2 MousePosition = Controls.General.Mouse_PointerLocation.ReadValue<Vector2>();

        //convert MousePosition to Vector 3
        Vector3 MouseVector3 = new Vector3(MousePosition.x, MousePosition.y, pg.GrabbedPiece_LengthAwayFromCamera);

        //get WorldPosition
        Vector3 WorldPosition = MainCamera.ScreenToWorldPoint(MouseVector3);

        //set to new position
        gameObject.transform.position = WorldPosition;
    }

    private void Handle_PieceRotation()
    {
        //@@@@ Need to add deltatime



        //if in hand
        if (IsInHand)
        {
            /* Don't need to unparent and reattach */

            float NewRotateAmount = Mathf.Lerp(0.0f, TargetSpinTotal, pg.SpinSpeed);

            // 60 = 60 - 2

            TargetSpinTotal -= NewRotateAmount;

            //rotate
            gameObject.transform.Rotate(0.0f, 0.0f, NewRotateAmount);

            //loop children
            foreach (Transform child in FaceValueTransform)
            {
                //rotate clockwise, negative
                child.transform.Rotate(0.0f, 0.0f, -NewRotateAmount);
            }
        }
        else
        {
            //store this piece (My Parent's Parent)
            GameObject HoneySlotParent = gameObject.transform.parent.gameObject;

            //detach from parent
            gameObject.transform.parent = null;

            float NewRotateAmount = Mathf.Lerp(0.0f, TargetSpinTotal, pg.SpinSpeed);

            //Debug.Log(NewRotateAmount);
            // 60 = 60 - 2

            TargetSpinTotal -= NewRotateAmount;

            //rotate
            gameObject.transform.Rotate(0.0f, 0.0f, NewRotateAmount);



            //loop children
            foreach (Transform child in FaceValueTransform)
            {
                //rotate clockwise, negative
                child.transform.Rotate(0.0f, 0.0f, -NewRotateAmount);
            }

            //re-attach to parent
            gameObject.transform.parent = HoneySlotParent.transform;

        }

        //if spinning almost done
        if(TargetSpinTotal <= 0.003f)
        {
            //stop spinning
            ActiveSpin = false;

            //Small amount of rotation still exists here, but it is not lost in the TargetSpinTotal.
            //Next time it spins, it adds 60 to the TargetSpinTotal.
            //So, 60 + small amount
            //There's no build up of small amounts of spin not done.  It carries over.
        }
    }

    private void Handle_NoMoveFeedBack()
    {
        /* This is a coded animation to bring a piece out and back in.  Only changing the z of altered_position*/

        //get time change
        timechange = Time.time - time_start;

        //recalculate precentage
        percentage_complete = timechange / pg.nomove_anim_duration;

        //else if tree
        {
            //coming out section
            if(percentage_complete < 0.1)
            {
                //alter percentage complete for this section
                percentage_complete *= 10.0f;

                //get new_z
                new_z = Mathf.Lerp(0.0f, -0.06f, percentage_complete);
            }
            //plateau section
            else if(percentage_complete < 0.3f)
            {
                //get new_z
                new_z = -0.06f;
            }
            //resetting section
            else if(percentage_complete < 1.0f)
            {
                //alter percentage complete for this section
                percentage_complete = (percentage_complete - 0.3f) / 0.7f;

                //get new_z
                new_z = Mathf.Lerp(-0.06f, 0.0f, percentage_complete);
            }
            else
            {
                //reset
                new_z = 0.0f;

                //stop feedback
                ActiveNoMoveFeedBack = false;
            }
        }

        //change altered_position
        altered_position.z = new_z;

        //set local position to altered_position
        anim.gameObject.transform.localPosition = altered_position;
    }

    public void Deactivate_Piece()
    {

        //stop ability to spin
        IsSpinnable = false;

        //stop ability to move
        IsMovable = false;

        //if already deactivated
        if (!IsDeactivated)
        {
            //set to deactive
            IsDeactivated = true;

            //deactivate piece material
            materialchanger.MaterialChange_Lerp(gameObject, true);
        }
    }

    public void MakeSticky_Piece()
    {
        //store
        Stored_IsMovable = IsMovable;
        Stored_IsSpinnable = IsSpinnable;

        //make stuck
        IsStuck = true;

        //freeze ability to spin
        IsSpinnable = false;

        //freeze ability to move
        IsMovable = false;

        //deactivate piece material
        materialchanger.Sticky_MaterialChange_Lerp(gameObject, true);

        //spawn StickyGlob
        Spawn_StickyGlob();

    }

    public void UnStick_Piece()
    {
        //unstick
        IsStuck = false;

        Debug.Log(IsStuck);

        //reverse spin stuck, if needed
        IsSpinnable = Stored_IsSpinnable;

        //reverse move stuck, if needed
        IsMovable = Stored_IsMovable;

        //deactivate piece material
        materialchanger.Sticky_MaterialChange_Lerp(gameObject, false);

        //delete glob
        Destroy_StickyGlob();
    }
    public bool Sticky_Possible()
    {
        if (IsStuck)
        {
            UnStick_Piece();

            Debug.Log("unstick");

            return true;
        }

        if (IsMovable || IsSpinnable)
        {

            //if active
            if (!IsDeactivated)
            {
                MakeSticky_Piece();

                return true;

            }
        }

        return false;
    }

    public void SetTo_Deactivated()
    {
        IsDeactivated = true;
    }

    public void SetSettledRotation()
    {
        //When a piece is copied after a finalization, it may be in the throws of a rotation
        //This function completes the rotation so it will start fixed, and not in the middle of a rotation

        if (ActiveSpin)
        {
            //set rotation to remaining TargetSpinTotal
            gameObject.transform.Rotate(0.0f, 0.0f, TargetSpinTotal);

            //loop children
            foreach (Transform child in FaceValueTransform)
            {
                //rotate clockwise, negative
                child.transform.Rotate(0.0f, 0.0f, -TargetSpinTotal);
            }

            //reset TargetSpinTotal
            TargetSpinTotal = 0.0f;

            //reset ActiveSpin
            ActiveSpin = false;
        }
    }


    //public void StartingPlacementOfPiece_OnBeeBox()
    //{
    //    //find open area on beebox
    //    a_ADDRESS open_area_slot_address = BeeBoxClusterScript.Find_OpenAreaSlot();

    //    //close area on beebox
    //    BeeBoxClusterScript.Close_ThisArea(open_area_slot_address);

    //    //reset rotation
    //    Reset_Rotation();

    //    //place
    //    Place_Piece(game_level.SlotRefDict[open_area_slot_address], false);
    //}

    public void NegativeFeedback_PieceRotation()
    {
        //get state info
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        //if in default state
        if (info.IsName("state_Default"))
        {
            //play animation
            anim.Play("state_NoSpin");

            return;
        }

        //if already spinning
        if (info.IsName("state_NoSpin"))
        {
            //reset local rotation
            anim.gameObject.transform.localRotation = pg.reset_quat;

            //play from beginning
            anim.Play("state_NoSpin", 0, 0.0f);

            return;
        }
    }

    public void NegativeFeedback_PiecePickUp()
    {
        //set new start time
        time_start = Time.time;

        //if already going
        if (ActiveNoMoveFeedBack)
        {
            //reset
            anim.gameObject.transform.localPosition = pg.reset_pos;
        }
        else
        {
            //turn on coded animation
            ActiveNoMoveFeedBack = true;
        }
    }

    public void Spawn_StickyGlob()
    {
        //make glob
        StickyGlobObject = Instantiate(PF_StickyGlob);

        //find this piece's location
        Transform Location = gameObject.transform;

        //set position of glob to piece
        StickyGlobObject.transform.position = Location.position;

        //set parent to piece
        StickyGlobObject.transform.parent = gameObject.transform.Find("VisualPiece").transform;
    }

    public void Destroy_StickyGlob()
    {

        StickyGlobObject.GetComponent<StickyGlob>().FadeOut();

        ////make glob
        //Destroy(StickyGlobObject);

        Debug.Log("deleted");
    }

    public void UnlockedPiece_InitiateTravel_ToBeeBox()
    {
        //scale back object to original
        Change_Scale_ToBeeBox();

        //change sorting layer
        Change_SortingLayer_ToBeeBox();

        Vector3 lava = gameObject.transform.position;


        Debug.Log(MainCamera.transform.position);


        Vector3 taco = Vector3.Lerp(lava, MainCamera.transform.position, 0.82f);

        gameObject.transform.parent = null;


        mover_base.Change_Position_Instant(taco);

        Debug.Log(lava);


        Debug.Log("at newly");

        //reset rotation
        Reset_Rotation();


        //find open area on beebox
        a_ADDRESS open_area_slot_address = BeeBoxClusterScript.Find_OpenAreaSlot();

        //close area on beebox
        BeeBoxClusterScript.Close_ThisArea(open_area_slot_address);





        //find destination
        DestinationArea = game_level.SlotRefDict[open_area_slot_address];

        //Debug.Log(DestinationArea.transform.position);

        //set destination for movement
        mover_base.Change_TargetPosition(DestinationArea.transform.position);

        //initiate movement
        mover_base.Activate_Move();
    }

    public void DuplicatedPiece_InitiateTravel_ToBeeBox(float sequence_finish_time)
    {
        //scale back object to original
        Change_Scale_ToBeeBox();

        //change sorting layer
        Change_SortingLayer_ToBeeBox();

        Vector3 lava = gameObject.transform.position;


        Debug.Log(MainCamera.transform.position);


        Vector3 taco = Vector3.Lerp(lava, MainCamera.transform.position, 0.87f);


        

        mover_base.Change_Position_Instant(taco);

        Debug.Log(lava);


        Debug.Log("at newly");

        //settle rotation
        SetSettledRotation();

        //find open area on beebox
        a_ADDRESS open_area_slot_address = BeeBoxClusterScript.Find_OpenAreaSlot();

        //close area on beebox
        BeeBoxClusterScript.Close_ThisArea(open_area_slot_address);





        //find destination
        DestinationArea = game_level.SlotRefDict[open_area_slot_address];

        //Debug.Log(DestinationArea.transform.position);

        //set destination for movement
        mover_base.Change_TargetPosition(DestinationArea.transform.position);

        //initiate movement
        mover_base.Activate_Move();



    }


    //UTILITIES

    public fv_FACEVALUE Get_FaceValue_WithOffset(t_TRI tri)
    {
        /* 
        I need the current value in this triangle.
        Take into account the offset and give me the fv_FACEVALUE there.
        */

        //convert tri to IntTri
        int IntTri = Convert_TriToInt(tri);

        //find offseted new int that corresponds to the face value we need due to spinning
        int Offseted_NewInt = PossibleSpinReset(IntTri - Current_TriOffset);

        //switch based off of current offset int 1-6
        switch (Offseted_NewInt)
        {
            case 1:
                return fslot_1;
            case 2:
                return fslot_2;
            case 3:
                return fslot_3;
            case 4:
                return fslot_4;
            case 5:
                return fslot_5;
            case 6:
                return fslot_6;
            default:
                return fslot_1;
        }

      



    }

    int PossibleSpinReset(int integer)
    {
        if(integer < 1)
        {
            return integer + 6;
        }
        else
        {
            return integer;
        }
    }

    int Convert_TriToInt(t_TRI tri)
    {
        switch (tri)
        {
            case t_1:
                return 1;
            case t_2:
                return 2;
            case t_3:
                return 3;
            case t_4:
                return 4;
            case t_5:
                return 5;
            case t_6:
                return 6;
            default:
                return 0;
        }
    }

    public void Reset_Rotation()
    {
        //reset x and y rotation
        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.eulerAngles.z);
    }

    public void Change_SortingLayer_ToWorldGrid()
    {
        sorting_component.sortingLayerID = game_level.ID_sl_WorldGrid;
    }

    public void Change_SortingLayer_ToBeeBox()
    {
        sorting_component.sortingLayerID = game_level.ID_sl_BeeBox;
    }

    public void Change_SortingLayer_ToHoneyShelf()
    {
        sorting_component.sortingLayerID = game_level.ID_sl_HoneyShelf;
    }

    public void Change_SortingLayer_ToFront()
    {
        sorting_component.sortingLayerID = game_level.ID_sl_Cursor;
    }

    public void Change_Scale_ToHoneyJar()
    {
        //Debug.Log("changing scale" + pg.Scale_HoneyJarPiece);
        gameObject.transform.localScale = pg.Scale_HoneyJarPiece;
    }

    public void Change_Scale_ToHoneyShelf()
    {
        gameObject.transform.localScale = pg.Scale_HoneyShelfPiece;
    }

    public void Change_Scale_ToBeeBox()
    {
        gameObject.transform.localScale = pg.Scale_BeeBoxPiece;
    }

    public void Change_Scale_ToWorldGrid()
    {
        gameObject.transform.localScale = pg.Scale_WorldGridPiece;
    }

    //t_TRI Convert_IntToTri(int interger)
    //{
    //    switch (interger)
    //    {
    //        case 1:
    //            return TRI_1;
    //        case 2:
    //            return TRI_2;
    //        case 3:
    //            return TRI_3;
    //        case 4:
    //            return TRI_4;
    //        case 5:
    //            return TRI_5;
    //        case 6:
    //            return TRI_6;
    //        default:
    //            return TRI_1;
    //    }
    //}

}
