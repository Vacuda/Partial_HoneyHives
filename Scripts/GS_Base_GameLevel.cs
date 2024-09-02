using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using static bt_BUTTONTYPE;
using static a_ADDRESS;
using static gsn_GAMESTATENAME;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

/* This base state handles consistent BeeBox and Button interactions in a GameLevel */
/* Then, it calls specific Left And Right button functions in its child game states */
/* Child Game States = GS_Inner, GS_Outer, GS_HoneyShelf */

public abstract class GS_Base_GameLevel : GameState
{
    //temps
    GameObject Area_obj;
    Piece Piece_s;


    /* ADMIN */

    public GS_Base_GameLevel(SMachine_GameState machine) : base(machine)
    {
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
        //if button hovered over
        if (Hub._controller.Hovered_Button != bt_NONE)
        {
            Hub.GameCursor_s.Interact_WithButton();

            switch (Hub._controller.Hovered_Button)
            {

                case bt_EXIT:
                    Debug.Log("should quit.");
                    Application.Quit();
                    yield break;

                case bt_STICKER:
                    HoneySticker_Pressed();
                    yield break;

                case bt_PAUSE:
                    _machine.SetState(gsn_PAUSED);
                    yield break;

                case bt_NEW:
                    Debug.Log("should be new.");
                    SceneManager.LoadScene("GameLevel", LoadSceneMode.Single);
                    yield break;

                case bt_BACK:
                    _machine.SetState(gsn_OUTER);
                    yield break;

                case bt_SHELF:
                    Hub.HoneyShelf_s.Toggle_Shelf();
                    yield break;

                case bt_BBA:
                    Hub.BeeBoxCluster_s.BeeBoxButton_Pressed(a_BBA0);
                    yield break;

                case bt_BBB:
                    Hub.BeeBoxCluster_s.BeeBoxButton_Pressed(a_BBB0);
                    yield break;

                case bt_BBC:
                    Hub.BeeBoxCluster_s.BeeBoxButton_Pressed(a_BBC0);
                    yield break;

                case bt_BBD:
                    Hub.BeeBoxCluster_s.BeeBoxButton_Pressed(a_BBD0);
                    yield break;

                case bt_SORT:
                    Hub.BeeBoxCluster_s.SortLeft_PiecesOnBeeBox();
                    yield break;

                default:
                    break;
            }

        }

        //none check
        if (Hub._controller.Hovered_Slot == a_NONE)
        {
            Hub.GameCursor_s.Interact_WithNothing();

            //not honeyshelf, you can break here
            if (Hub._machine.Get_CurrentState() != gsn_HONEYSHELF)
            {
                yield break;
            }

            //Proceeding from here means GS_HoneyShelf can deal with Hovered_Slot being a_NONE
        }

        //if Beebox address
        if (BeeBox_Check(Hub._controller.Hovered_Slot))
        {
            GameObject AreaObject = Hub.GameLevel_s.SlotRefDict[Hub._controller.Hovered_Slot];

            //if occupied
            if (AreaObject.GetComponentInChildren<Piece>() != null)
            {
                //if piece in hand
                if (Hub._controller.PieceInHand != null)
                {

                }
                //no piece in hand
                else
                {
                    Piece Piece_s = AreaObject.GetComponentInChildren<Piece>();

                    //if movable
                    if (Piece_s.IsMovable)
                    {
                        Piece_s.Pickup_Piece();
                        Hub.BeeBoxCluster_s.Open_ThisArea(Hub._controller.Hovered_Slot);
                        Hub._controller.Update_PieceInHand(Piece_s.gameObject);
                        Hub.GameCursor_s.Holding();
                    }
                    //not movable
                    else
                    {

                    }
                }
            }
            //not occupied
            else
            {
                //if piece in hand
                if (Hub._controller.PieceInHand != null)
                {
                    Hub._controller.PieceInHand.GetComponent<Piece>().Place_Piece(AreaObject, false);
                    Hub.BeeBoxCluster_s.Close_ThisArea(Hub._controller.Hovered_Slot);
                    Hub._controller.Update_PieceInHand(null);
                    Hub.GameCursor_s.Normal();
                }
                //no piece in hand
                else
                {

                }
            }

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
        //if piece in hand
        if (Hub._controller.PieceInHand != null)
        {
            Piece Piece_s = Hub._controller.PieceInHand.GetComponent<Piece>();

            //if spinnable
            if (Piece_s.IsSpinnable)
            {
                Piece_s.Rotate_Piece();
            }
            //not spinnable
            else
            {
                Piece_s.NegativeFeedback_PieceRotation();
            }

            yield break;
        }

        /* No Piece In Hand */

        //no Hovered_Slot
        if (Hub._controller.Hovered_Slot == a_NONE)
        {
            yield break;
        }

        //if BeeBox
        if (BeeBox_Check(Hub._controller.Hovered_Slot))
        {
            GameObject AreaObject = Hub.GameLevel_s.SlotRefDict[Hub._controller.Hovered_Slot];

            //check if occupied
            if (AreaObject.GetComponentInChildren<Piece>() != null)
            {
                //condense
                Piece Piece_s = AreaObject.GetComponentInChildren<Piece>();

                //if spinnable
                if (Piece_s.IsSpinnable)
                {
                    //rotate piece attached
                    Piece_s.Rotate_Piece();
                }
                else
                {
                    //trigger negative feedback
                    Piece_s.NegativeFeedback_PieceRotation();
                }
            }

            //exit spin function
            yield break;
        }

        //run child game state specifics
        LeftGameBoardSpecific_OnRightButton();

        yield break;
    }

    public virtual void LeftGameBoardSpecific_OnRightButton()
    {
    }

    /* UTILITIES */

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
        if (conversion >= 57 && conversion <= 63)
        {
            return true;
        }

        //not a honeyshelf honeycomb
        return false;
    }

    private void HoneySticker_Pressed()
    {
        //if piece in hand
        if (Hub._controller.PieceInHand != null)
        {
            return;
        }

        //sticky already
        if (Hub.GameCursor_s.IsCursorSticky())
        {
            Hub.GameCursor_s.Normal();
        }
        //not sticky
        else
        {
            Hub.GameCursor_s.Sticky();
        }
    }
}