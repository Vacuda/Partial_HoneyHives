using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using static bt_BUTTONTYPE;
using static a_ADDRESS;
using static gse_GAMESTATEENUM;

/* This base state handles consistent BeeBox and Button interactions in a GameLevel */
/* Then, it calls specific Left And Right button functions in its child game states */
/* Child Game States = GS_Inner, GS_Outer, GS_HoneyShelf */

public abstract class GS_Base_GameLevel : GameState
{
    protected GameLevel_PlayerController gl_controller;
    protected WorldGrid WorldGridScript;
    protected HoneyShelf HoneyShelfScript;
    protected BackButton BackButtonScript;
    protected HoneySticker HoneyStickerScript;
    protected PauseMenu PauseMenuScript;
    protected BeeBoxCluster BeeBoxClusterScript;
    protected GameCursor GameCursorScript;
    protected VineValidator ValidatorScript;
    protected GameLevel GameLevelScript;

    //temporary reference holders
    protected a_ADDRESS Hovered_Slot;
    protected bt_BUTTONTYPE Hovered_Button;
    protected GameObject PieceInHand;


    /* ADMIN */

    public GS_Base_GameLevel(PlayerController controller, SMachine_GameState machine) : base(controller, machine)
    {
        gl_controller = _controller as GameLevel_PlayerController;

        if (gl_controller != null)
        {
            // successfully cast
            gl_controller = (GameLevel_PlayerController)_controller;
            WorldGridScript = gl_controller.WorldGridScript;
            HoneyShelfScript = gl_controller.HoneyShelfScript;
            BackButtonScript = gl_controller.BackButtonScript;
            HoneyStickerScript = gl_controller.HoneyStickerScript;
            PauseMenuScript = gl_controller.PauseMenuScript;
            BeeBoxClusterScript = gl_controller.BeeBoxClusterScript;
            GameCursorScript = gl_controller.GameCursorScript;
            ValidatorScript = gl_controller.ValidatorScript;
            GameLevelScript = gl_controller.GameLevelScript;
        }
        else
        {
            // cast failed
        }

    }

    public override IEnumerator OnBegin()
    {
        yield break;
    }

    public override IEnumerator OnExit()
    {
        yield break;
    }

    /* INPUTS */

    public override IEnumerator OnLeftButton()
    {
        //update refs
        Update_VitalReferences();

        //start interaction
        GameCursorScript.Interact();

        //if button hovered over
        switch (Hovered_Button)
        {
            case bt_NONE:
                break;

            case bt_EXIT:
                Debug.Log("should quit.");
                Application.Quit();
                yield break;

            case bt_STICKER:
                HoneyStickerScript.InteractWithHoneySticker();
                yield break;

            case bt_PAUSE:
                _machine.SetState(gse_PAUSED);
                yield break;

            case bt_NEW:
                Debug.Log("should be new.");
                SceneManager.LoadScene("GameLevel", LoadSceneMode.Single);
                yield break;

            case bt_BACK:
                _machine.SetState(gse_OUTER);
                yield break;

            case bt_SHELF:
                HoneyShelfScript.Toggle_Shelf();
                _machine.SetState(gse_HONEYSHELF);
                yield break;

            case bt_BBA:
                BeeBoxClusterScript.BeeBoxButton_Pressed(a_BBA0);
                yield break;

            case bt_BBB:
                BeeBoxClusterScript.BeeBoxButton_Pressed(a_BBB0);
                yield break;

            case bt_BBC:
                BeeBoxClusterScript.BeeBoxButton_Pressed(a_BBC0);
                yield break;

            case bt_BBD:
                BeeBoxClusterScript.BeeBoxButton_Pressed(a_BBD0);
                yield break;

            default:
                break;
        }

        //none check
        if (Hovered_Slot == NONE)
        {
            yield break;
        }

        //if Beebox address
        if (BeeBox_Check(Hovered_Slot))
        {
            //find AreaObject
            GameObject AreaObject = GameLevelScript.SlotRefDict[Hovered_Slot];

            //check if occupied
            if (AreaObject.GetComponentInChildren<Piece>() != null)
            {
                //if piece in hand
                if (PieceInHand != null)
                {
                    //trigger negative feedback
                }
                else
                {
                    //condense
                    Piece PieceScript = AreaObject.GetComponentInChildren<Piece>();

                    //check if movable
                    if (PieceScript.IsMovable)
                    {
                        //pick up piece
                        PieceScript.Pickup_Piece();

                        //open area slot on beebox
                        BeeBoxClusterScript.Open_ThisArea(Hovered_Slot);

                        //put piece in hand
                        _controller.Update_PieceInHand(PieceScript.gameObject);

                        //change cursor
                        GameCursorScript.Close();
                    }
                    //not movable
                    else
                    {
                        //trigger negative feedback
                    }
                }
            }
            //not occupied
            else
            {
                //if piece in hand
                if (PieceInHand != null)
                {
                    //put piece down
                    PieceInHand.GetComponent<Piece>().Place_Piece(AreaObject, false);

                    //close area slot on beebox
                    BeeBoxClusterScript.Close_ThisArea(Hovered_Slot);

                    //remove piece from hand
                    _controller.Update_PieceInHand(null);

                    //change cursor
                    GameCursorScript.Normal();
                }
                else
                {
                    //do nothing
                }
            }

            //exit function
            yield break;
        }


        //run child game state specifics - there is a hovered slot
        LeftGameBoardSpecific_OnLeftButton();

        yield break;
    }

    public virtual void LeftGameBoardSpecific_OnLeftButton()
    {
    }

    public override IEnumerator OnRightButton()
    {
        //update refs
        Update_VitalReferences();

        //if piece in hand
        if (PieceInHand != null)
        {
            //condense
            Piece PieceScript = PieceInHand.GetComponent<Piece>();

            //if spinnable
            if (PieceScript.IsSpinnable)
            {
                //rotate piece attached
                PieceScript.Rotate_Piece();
            }
            else
            {
                //trigger negative feedback
                PieceScript.NegativeFeedback_PieceRotation();
            }

            //leave spin function
            yield break;
        }

        /* No Piece In Hand */

        //no Hovered_Slot
        if (Hovered_Slot == NONE)
        {
            //do nothing
            yield break;
        }

        //if BeeBox
        if (BeeBox_Check(Hovered_Slot))
        {
            //find AreaObject
            GameObject AreaObject = GameLevelScript.SlotRefDict[Hovered_Slot];

            //check if occupied
            if (AreaObject.GetComponentInChildren<Piece>() != null)
            {
                //condense
                Piece PieceScript = AreaObject.GetComponentInChildren<Piece>();

                //if spinnable
                if (PieceScript.IsSpinnable)
                {
                    //rotate piece attached
                    PieceScript.Rotate_Piece();
                }
                else
                {
                    //trigger negative feedback
                    PieceScript.NegativeFeedback_PieceRotation();
                }
            }

            //exit spin function
            yield break;
        }

        //run child game state specifics
        RightGameBoardSpecific_OnRightButton();

        yield break;
    }

    public virtual void RightGameBoardSpecific_OnRightButton()
    {
    }

    public bool BeeBox_Check(a_ADDRESS slot)
    {
        //convert to int
        int conversion = (int)slot;

        //if beebox, according to enum order
        if (conversion >= 64)
        {
            return true;
        }

        //not a BeeBox address
        return false;
    }

    public bool HoneyComb_Check(a_ADDRESS slot)
    {
        //convert to int
        int conversion = (int)slot;

        //if honeycomb, according to enum order
        if (conversion % 8 == 1 && conversion < 50)
        {
            return true;
        }

        //not a honeycomb
        return false;
    }

    public bool HoneyShelf_Check(a_ADDRESS slot)
    {
        //convert to int
        int conversion = (int)slot;

        //if honeyshelf honeycomb, according to enum order
        if (conversion >=  57 && conversion <= 63)
        {
            return true;
        }

        //not a honeyshelf honeycomb
        return false;
    }

    public void Update_VitalReferences()
    {
        Hovered_Button = _controller.Hovered_Button;
        Hovered_Slot = _controller.Hovered_Slot;
        PieceInHand = _controller.PieceInHand;
    }
}
