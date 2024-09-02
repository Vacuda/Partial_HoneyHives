using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static a_ADDRESS;
using static fv_FACEVALUE;
using System;

public class BlockHouse : MonoBehaviour
{
    //these need to change when a new template is added
    int FourBlockTemplates = 2;
    int SixBlockTemplates = 5;


    //@@@@ I think I can make this non-static.  Then, you can call it similar to other things, Hub.BlockHouse_s  / BlockHouse.
    //I only made it like this so I can call it from anywhere.  Now, with Hub, I can do that.

    //@@@@ I should put temp variables up here.


    /* BUILD HONEYCOMBS */

    public void Get_Seven_Solvable_HoneyCombs(ref List<List<Info_Slot>> hc_list, Info_PuzConfig config)
    {
        Fill_FaceValues(ref hc_list);
    }

    void Fill_FaceValues(ref List<List<Info_Slot>> hc_list)
    {
        //Output of solutions
        StreamWriter writer = new StreamWriter("LastSolution.txt");

        //declarations
        int hc_counter = 1;
        fv_FACEVALUE[] four_block;
        fv_FACEVALUE[] six_block;

        //loop honeycombs
        for (int i = 0; i <= 6; i++)
        {
            //output honeycomb address
            writer.WriteLine("");
            writer.WriteLine("++++++++++++++++");
            writer.WriteLine((a_ADDRESS)hc_counter);
            writer.WriteLine("++++++++++++++++");
            writer.WriteLine("");

            /* flower 1 */
            {
                four_block = Get_FourBlock();

                //change facevalue
                hc_list[i][ConvertToIndex(a_G)].fslot_6 = four_block[0];
                hc_list[i][ConvertToIndex(a_G)].fslot_3 = four_block[1];
                hc_list[i][ConvertToIndex(a_F)].fslot_6 = four_block[2];
                hc_list[i][ConvertToIndex(a_F)].fslot_3 = four_block[3];

                //record solution for debug
                Record_Solution(ref writer, four_block);
            }

            /* flower 2 */
            {
                six_block = Get_SixBlock();

                //change facevalue
                hc_list[i][ConvertToIndex(a_E)].fslot_6 = six_block[0];
                hc_list[i][ConvertToIndex(a_E)].fslot_3 = six_block[1];
                hc_list[i][ConvertToIndex(a_D)].fslot_6 = six_block[2];
                hc_list[i][ConvertToIndex(a_D)].fslot_3 = six_block[3];
                hc_list[i][ConvertToIndex(a_C)].fslot_6 = six_block[4];
                hc_list[i][ConvertToIndex(a_C)].fslot_3 = six_block[5];

                //record solution for debug
                Record_Solution(ref writer, six_block);
            }

            /* flower 3 */
            {
                four_block = Get_FourBlock();

                //change facevalue
                hc_list[i][ConvertToIndex(a_B)].fslot_6 = four_block[0];
                hc_list[i][ConvertToIndex(a_B)].fslot_3 = four_block[1];
                hc_list[i][ConvertToIndex(a_A)].fslot_6 = four_block[2];
                hc_list[i][ConvertToIndex(a_A)].fslot_3 = four_block[3];

                //record solution for debug
                Record_Solution(ref writer, four_block);
            }

            /* flower 4 */
            {
                four_block = Get_FourBlock();

                //change facevalue
                hc_list[i][ConvertToIndex(a_E)].fslot_5 = four_block[0];
                hc_list[i][ConvertToIndex(a_E)].fslot_2 = four_block[1];
                hc_list[i][ConvertToIndex(a_G)].fslot_5 = four_block[2];
                hc_list[i][ConvertToIndex(a_G)].fslot_2 = four_block[3];

                //record solution for debug
                Record_Solution(ref writer, four_block);
            }

            /* flower 5 */
            {
                six_block = Get_SixBlock();

                //change facevalue
                hc_list[i][ConvertToIndex(a_B)].fslot_5 = six_block[0];
                hc_list[i][ConvertToIndex(a_B)].fslot_2 = six_block[1];
                hc_list[i][ConvertToIndex(a_D)].fslot_5 = six_block[2];
                hc_list[i][ConvertToIndex(a_D)].fslot_2 = six_block[3];
                hc_list[i][ConvertToIndex(a_F)].fslot_5 = six_block[4];
                hc_list[i][ConvertToIndex(a_F)].fslot_2 = six_block[5];

                //record solution for debug
                Record_Solution(ref writer, six_block);
            }

            /* flower 6 */
            {
                four_block = Get_FourBlock();

                //change facevalue
                hc_list[i][ConvertToIndex(a_A)].fslot_5 = four_block[0];
                hc_list[i][ConvertToIndex(a_A)].fslot_2 = four_block[1];
                hc_list[i][ConvertToIndex(a_C)].fslot_5 = four_block[2];
                hc_list[i][ConvertToIndex(a_C)].fslot_2 = four_block[3];

                //record solution for debug
                Record_Solution(ref writer, four_block);
            }

            /* flower 7 */
            {
                four_block = Get_FourBlock();

                //change facevalue
                hc_list[i][ConvertToIndex(a_B)].fslot_4 = four_block[0];
                hc_list[i][ConvertToIndex(a_B)].fslot_1 = four_block[1];
                hc_list[i][ConvertToIndex(a_E)].fslot_4 = four_block[2];
                hc_list[i][ConvertToIndex(a_E)].fslot_1 = four_block[3];

                //record solution for debug
                Record_Solution(ref writer, four_block);
            }

            /* flower 8 */
            {
                six_block = Get_SixBlock();

                //change facevalue
                hc_list[i][ConvertToIndex(a_A)].fslot_4 = six_block[0];
                hc_list[i][ConvertToIndex(a_A)].fslot_1 = six_block[1];
                hc_list[i][ConvertToIndex(a_D)].fslot_4 = six_block[2];
                hc_list[i][ConvertToIndex(a_D)].fslot_1 = six_block[3];
                hc_list[i][ConvertToIndex(a_G)].fslot_4 = six_block[4];
                hc_list[i][ConvertToIndex(a_G)].fslot_1 = six_block[5];

                //record solution for debug
                Record_Solution(ref writer, six_block);
            }

            /* flower 9 */
            {
                four_block = Get_FourBlock();

                //change facevalue
                hc_list[i][ConvertToIndex(a_C)].fslot_4 = four_block[0];
                hc_list[i][ConvertToIndex(a_C)].fslot_1 = four_block[1];
                hc_list[i][ConvertToIndex(a_F)].fslot_4 = four_block[2];
                hc_list[i][ConvertToIndex(a_F)].fslot_1 = four_block[3];

                //record solution for debug
                Record_Solution(ref writer, four_block);
            }

            //increment hc_counter
            hc_counter += 8;
        }

        writer.WriteLine("===========");
        writer.WriteLine("");

        //close output of solutions
        writer.Close();
    }


    /* FOUR BLOCKS */

    fv_FACEVALUE[] Get_FourBlock()
    {
        //get rand
        int rand = Random.Range(1, FourBlockTemplates + 1); //inclusive, exclusive

        switch (rand)
        {
            case 1: return FourBlock_AllRandom_ZeroEquals();
            case 2: return FourBlock_OneOrThreeEquals();
            default: return FourBlock_AllRandom_ZeroEquals();
        }
    }

    fv_FACEVALUE[] FourBlock_AllRandom_ZeroEquals()
    {
        /* All values are random */

        //create block to return
        fv_FACEVALUE[] block = new fv_FACEVALUE[4];

        //loop block
        for (int i = 0; i <= 3; i++)
        {
            block[i] = GetRandomFaceValue();
        }

        return block;
    }

    fv_FACEVALUE[] FourBlock_OneOrThreeEquals()
    {
        /* Left Side Equals Right Side */

        //create block to return
        fv_FACEVALUE[] block = new fv_FACEVALUE[4];

        //find common left/right facevalue
        fv_FACEVALUE commonvalue = GetRandomFaceValue(true);

        //larger left
        if (Roll_EitherOr())
        {
            //right side set
            block[2] = fv_EQUALS;
            block[3] = commonvalue;

            // 0 is commonvalue
            if (Roll_EitherOr())
            {
                //set 0
                block[0] = commonvalue;

                //set 1                                 // c _ = c
                {
                    /* fv_BLANK works everywhere here */

                    // 0 _ = 0      //fv_BLANK and fv_0
                    // 3 _ = 3      //only fv_BLANK
                    // + _ = +      //only fv_BLANK
                    // - _ = -      //only fv_BLANK
                    // b _ = b      //only fv_BLANK
                    // = _ = =      //only fv_BLANK

                    //set 1;
                    block[1] = fv_BLANK;

                    //edge handle
                    if (commonvalue == fv_0)
                    {
                        if (Roll_EitherOr())
                        {
                            //v_o also works
                            block[1] = fv_0;
                        }
                    }

                }
            }
            // 1 is commonvalue
            else
            {
                //set 0                                 // _ c = c
                {
                    /* fv_BLANK works everywhere here */

                    // _ 0 = 0      //fv_0, fv_ADD, fv_SUB, fv_BLANK
                    // _ 3 = 3      //fv_0, fv_ADD, fv_BLANK
                    // _ + = +      //only fv_BLANK
                    // _ - = -      //only fv_BLANK
                    // _ b = b      //only fv_BLANK
                    // _ = = =      //only fv_BLANK

                    //set 0;
                    block[0] = fv_BLANK;

                    //edge handle
                    if (commonvalue == fv_0)
                    {
                        //get val(4)
                        int rand = Random.Range(1, 5); //inclusive, exclusive

                        if (rand == 1)
                        {
                            block[0] = fv_0;
                        }
                        else if (rand == 2)
                        {
                            block[0] = fv_ADD;
                        }
                        else if (rand == 3)
                        {
                            block[0] = fv_SUB;
                        }

                        //rand = 4, stays as fv_BLANK
                    }

                    //edge handle
                    if (bIsDigitMinusZero(commonvalue))
                    {
                        //get val(3)
                        int rand = Random.Range(1, 4); //inclusive, exclusive

                        if (rand == 1)
                        {
                            block[0] = fv_0;
                        }
                        else if (rand == 2)
                        {
                            block[0] = fv_ADD;
                        }

                        //rand = 3, stays as fv_BLANK
                    }

                }

                //set 1
                block[1] = commonvalue;
            }
        }
        //larger right
        else
        {
            //left side set
            block[0] = commonvalue;
            block[1] = fv_EQUALS;

            // 2 is commonvalue
            if (Roll_EitherOr())
            {
                //set 2
                block[2] = commonvalue;

                //set 3                                 // c = c _
                {
                    /* fv_BLANK works everywhere here */

                    // 0 = 0 _      //fv_BLANK and fv_0
                    // 3 = 3 _      //only fv_BLANK
                    // + = + _      //only fv_BLANK
                    // - = - _      //only fv_BLANK
                    // b = b _      //only fv_BLANK
                    // = = = _      //only fv_BLANK

                    //set 3
                    block[3] = fv_BLANK;

                    //edge handle
                    if (commonvalue == fv_0)
                    {
                        if (Roll_EitherOr())
                        {
                            //v_o also works
                            block[3] = fv_0;
                        }
                    }
                }
            }
            // 3 is commonvalue
            else
            {
                //set 2                                 // c = _ c
                {
                    /* fv_BLANK works everywhere here */

                    // 0 = _ 0      //fv_0, fv_ADD, fv_SUB, fv_BLANK
                    // 3 = _ 3      //fv_0, fv_ADD, fv_BLANK
                    // + = _ +      //only fv_BLANK
                    // - = _ -      //only fv_BLANK
                    // b = _ b      //only fv_BLANK
                    // = = _ =      //only fv_BLANK

                    //set 2;
                    block[2] = fv_BLANK;

                    //edge handle
                    if (commonvalue == fv_0)
                    {
                        //get val(4)
                        int rand = Random.Range(1, 5); //inclusive, exclusive

                        if (rand == 1)
                        {
                            block[2] = fv_0;
                        }
                        else if (rand == 2)
                        {
                            block[2] = fv_ADD;
                        }
                        else if (rand == 3)
                        {
                            block[2] = fv_SUB;
                        }

                        //rand = 4, stays as fv_BLANK
                    }

                    //edge handle
                    if (bIsDigitMinusZero(commonvalue))
                    {
                        //get val(3)
                        int rand = Random.Range(1, 4); //inclusive, exclusive

                        if (rand == 1)
                        {
                            block[2] = fv_0;
                        }
                        else if (rand == 2)
                        {
                            block[2] = fv_ADD;
                        }

                        //rand = 3, stays as fv_BLANK
                    }

                }

                //set 3
                block[3] = commonvalue;
            }
        }


        return block;
    }


    /* SIX BLOCKS */

    fv_FACEVALUE[] Get_SixBlock()
    {
        //return SixBlock_AllRandom_ZeroEquals();
        //return SixBlock_NoMath_OneEquals_OneAndFourSplit();
        //return SixBlock_NoMath_OneEquals_TwoAndThreeSplit();
        //return SixBlock_YesMath_Subtraction_OneEquals_AllDigits();
        //return SixBlock_YesMath_Addition_OneEquals_AllDigits();

        //get rand 1 - 5
        int rand = Random.Range(1, SixBlockTemplates + 1); //inclusive, exclusive

        switch (rand)
        {
            case 1: return SixBlock_AllRandom_ZeroEquals();
            case 2: return SixBlock_NoMath_OneEquals_OneAndFourSplit();
            case 3: return SixBlock_NoMath_OneEquals_TwoAndThreeSplit();
            case 4: return SixBlock_YesMath_Subtraction_OneEquals_AllDigits();
            case 5: return SixBlock_YesMath_Addition_OneEquals_AllDigits();
            default: return SixBlock_AllRandom_ZeroEquals();
        }
    }

    fv_FACEVALUE[] SixBlock_AllRandom_ZeroEquals()
    {
        /* All values are random */

        //create block to return
        fv_FACEVALUE[] block = new fv_FACEVALUE[6];

        //loop block
        for (int i = 0; i <= 5; i++)
        {
            block[i] = GetRandomFaceValue();
        }

        return block;
    }

    fv_FACEVALUE[] SixBlock_NoMath_OneEquals_OneAndFourSplit()
    {
        /* One Side Equals Four Side */
        /* Later, I Determine Left Or Right */

        //create fourblock
        fv_FACEVALUE[] fourblock = new fv_FACEVALUE[4];

        //find the one facevalue   - @@@@ this needs to be weighted towards digits
        fv_FACEVALUE onefacevalue = GetRandomFaceValue(true);

        //if digit
        if (bIsDigit(onefacevalue))
        {
            //get rand, 0 - 3
            int rand = Random.Range(0, 4); //inclusive, exclusive

            //place digit
            fourblock[rand] = onefacevalue;

            //fill, to the right, with blanks
            for (int i = rand + 1; i <= 3; i++)
            {
                fourblock[i] = fv_BLANK;
            }

            //fill, to the left, with blanks or +
            for (int i = rand - 1; i >= 0; i--)
            {
                if (Roll_EitherOr())
                {
                    fourblock[i] = fv_BLANK;
                }
                else
                {
                    fourblock[i] = fv_ADD;
                }
            }

            /* This is finished, but I'm going to add a chance for it to have two minus signs */

            // if digit placed at 2 or 3
            if (rand >= 2)
            {
                //get high chance to leave this code
                if (Random.value > 0.9f)
                {
                    //do nothing, move on
                }
                else
                {
                    // if digit placed at 2
                    if (rand == 2)
                    {
                        fourblock[0] = fv_SUB;
                        fourblock[1] = fv_SUB;
                    }
                    // if digit placed at 3
                    else
                    {
                        //make all sub
                        fourblock[0] = fv_SUB;
                        fourblock[1] = fv_SUB;
                        fourblock[2] = fv_SUB;

                        //get rand, 0 - 2
                        int newrand = Random.Range(0, 3); //inclusive, exclusive

                        //change newrand back to fv_ADD
                        fourblock[newrand] = fv_ADD;
                    }
                }
            }
        }
        //if NOT digit
        else
        {
            //if fv_ADD
            if (onefacevalue == fv_ADD)
            {
                fourblock = Get_BlankedFourBlock_WithThisValueInserted(fv_ADD);
            }
            //if fv_SUB
            if (onefacevalue == fv_SUB)
            {
                fourblock = Get_BlankedFourBlock_WithThisValueInserted(fv_SUB);
            }
            //if fv_BLANK
            if (onefacevalue == fv_BLANK)
            {
                fourblock = Get_BlankedFourBlock_WithThisValueInserted(fv_BLANK);
            }
            //if fv_EQUALS
            if (onefacevalue == fv_EQUALS)
            {
                fourblock = Get_BlankedFourBlock_WithThisValueInserted(fv_EQUALS);
            }
        }

        //create block to return
        fv_FACEVALUE[] block = new fv_FACEVALUE[6];

        //place onefacevalue on left or right
        if (Roll_EitherOr())
        {
            block[0] = onefacevalue;
            block[1] = fv_EQUALS;
            block[2] = fourblock[0];
            block[3] = fourblock[1];
            block[4] = fourblock[2];
            block[5] = fourblock[3];
        }
        else
        {
            block[0] = fourblock[0];
            block[1] = fourblock[1];
            block[2] = fourblock[2];
            block[3] = fourblock[3];
            block[4] = fv_EQUALS;
            block[5] = onefacevalue;
        }

        //return block
        return block;
    }

    fv_FACEVALUE[] SixBlock_NoMath_OneEquals_TwoAndThreeSplit()
    {
        /* Two Side Equals Three Side, No Math */
        /* Later, I Determine Left Or Right */

        //get random facevalues
        fv_FACEVALUE firstvalue = GetRandomFaceValue();
        fv_FACEVALUE secondvalue = GetRandomFaceValue();

        //create twoblock
        fv_FACEVALUE[] twoblock = new fv_FACEVALUE[2];

        //fill twoblock
        twoblock[0] = firstvalue;
        twoblock[1] = secondvalue;

        //create threeblock
        fv_FACEVALUE[] threeblock = new fv_FACEVALUE[3];

        //initialize blocktype
        int blocktype;

        //fill threeblock
        {
            //get blocktype 1 - 3
            blocktype = Random.Range(1, 4); //inclusive, exclusive

            switch (blocktype)
            {
                case 1:
                    threeblock[0] = firstvalue;
                    threeblock[1] = secondvalue;
                    threeblock[2] = fv_BLANK;
                    break;
                case 2:
                    threeblock[0] = firstvalue;
                    threeblock[1] = fv_BLANK;
                    threeblock[2] = secondvalue;
                    break;
                case 3:
                    threeblock[0] = fv_BLANK;
                    threeblock[1] = firstvalue;
                    threeblock[2] = secondvalue;
                    break;
                default:
                    threeblock[0] = fv_NULL;
                    threeblock[1] = fv_NULL;
                    threeblock[2] = fv_NULL;
                    break;
            }
        }

        //make alterations maybe
        {
            // 7 8 = ? 7 8 , non-zero first
            if (bIsDigitMinusZero(firstvalue) && blocktype == 3)
            {
                //get rand 1 - 3
                int rand = Random.Range(1, 4); //inclusive, exclusive

                switch (rand)
                {
                    case 1:
                        threeblock[0] = fv_0;
                        break;
                    case 2:
                        threeblock[0] = fv_ADD;
                        break;
                    case 3:
                        threeblock[0] = fv_BLANK; //no change
                        break;
                    default:
                        break;
                }
            }

            // - 3 =
            if ((firstvalue == fv_SUB) && bIsDigit(secondvalue))
            {
                // - 0
                if (secondvalue == fv_0)
                {
                    //perhaps change threeblock
                    Place_SingleValue_InThreeBlock(ref threeblock, secondvalue);
                }
                // - 3
                else
                {
                    //get rand 1 - 5
                    int rand = Random.Range(1, 6); //inclusive, exclusive

                    //use rand
                    switch (rand)
                    {
                        case 1: // - _ 3
                            threeblock[0] = fv_SUB;
                            threeblock[1] = fv_BLANK;
                            threeblock[2] = secondvalue;
                            break;
                        case 2: // - 3 _
                            threeblock[0] = fv_SUB;
                            threeblock[1] = secondvalue;
                            threeblock[2] = fv_BLANK;
                            break;
                        case 3: // _ - 3
                            threeblock[0] = fv_BLANK;
                            threeblock[1] = fv_SUB;
                            threeblock[2] = secondvalue;
                            break;
                        case 4: // + - 3
                            threeblock[0] = fv_ADD;
                            threeblock[1] = fv_SUB;
                            threeblock[2] = secondvalue;
                            break;
                        case 5: // - + 3
                            threeblock[0] = fv_SUB;
                            threeblock[1] = fv_ADD;
                            threeblock[2] = secondvalue;
                            break;
                        default:
                            break;
                    }
                }
            }

            // + 8 =    OR   0 8 =    OR   _ 8 =
            if ((firstvalue == fv_ADD || firstvalue == fv_0 || firstvalue == fv_BLANK) && bIsDigit(secondvalue))
            {
                //perhaps change threeblock
                Place_SingleValue_InThreeBlock(ref threeblock, secondvalue);
            }

            // 9 _ =
            if (bIsDigit(firstvalue) && secondvalue == fv_BLANK)
            {
                //perhaps change threeblock
                Place_SingleValue_InThreeBlock(ref threeblock, firstvalue);
            }
        }

        /* Got both blocks made */
        /* Decide Left and Right, and combine */

        //create sixblock to return
        fv_FACEVALUE[] sixblock = new fv_FACEVALUE[6];

        //if left
        if (Roll_EitherOr())
        {
            sixblock[0] = threeblock[0];
            sixblock[1] = threeblock[1];
            sixblock[2] = threeblock[2];
            sixblock[3] = fv_EQUALS;
            sixblock[4] = twoblock[0];
            sixblock[5] = twoblock[1];
        }
        //if right
        else
        {
            sixblock[0] = twoblock[0];
            sixblock[1] = twoblock[1];
            sixblock[2] = fv_EQUALS;
            sixblock[3] = threeblock[0];
            sixblock[4] = threeblock[1];
            sixblock[5] = threeblock[2];
        }

        //return sixblock
        return sixblock;
    }

    fv_FACEVALUE[] SixBlock_YesMath_Subtraction_OneEquals_AllDigits()
    {
        /* Minus Side Equals TWO Int Side */
        /* int - int = int int */
        /* Later, I Determine Left Or Right */
        if (Roll_EitherOr())
        {
            //create threeblock
            fv_FACEVALUE[] threeblock = new fv_FACEVALUE[3];

            //fill threeblock
            threeblock[0] = GetRandomDigitValue();
            threeblock[1] = fv_SUB;
            threeblock[2] = GetRandomDigitValue();

            //create twoblock
            fv_FACEVALUE[] twoblock = new fv_FACEVALUE[2];

            //get result
            int firstint = Hub.VineValidator_s.Get_IntFromFaceValue(threeblock[0]);
            int secondint = Hub.VineValidator_s.Get_IntFromFaceValue(threeblock[2]);
            int result = firstint - secondint;

            //if positive, non-zero
            if (result > 0)
            {
                //get rand 1 - 4
                int rand = Random.Range(1, 5); //inclusive, exclusive

                switch (rand) // 09, _9, 9_, +9
                {
                    case 1:
                        twoblock[0] = fv_0;
                        twoblock[1] = Hub.VineValidator_s.Get_FaceValue(result);
                        break;
                    case 2:
                        twoblock[0] = fv_BLANK;
                        twoblock[1] = Hub.VineValidator_s.Get_FaceValue(result);
                        break;
                    case 3:
                        twoblock[0] = Hub.VineValidator_s.Get_FaceValue(result);
                        twoblock[1] = fv_BLANK;
                        break;
                    case 4:
                        twoblock[0] = fv_ADD;
                        twoblock[1] = Hub.VineValidator_s.Get_FaceValue(result);
                        break;
                    default:
                        twoblock[0] = fv_NULL;
                        twoblock[1] = fv_NULL;
                        break;
                }
            }

            // if zero
            if (result == 0)
            {
                //get rand 1 - 5
                int rand = Random.Range(1, 6); //inclusive, exclusive

                switch (rand) //_0, 0_, 00, +0, -0
                {
                    case 1:
                        twoblock[0] = fv_BLANK;
                        twoblock[1] = fv_0;
                        break;
                    case 2:
                        twoblock[0] = fv_0;
                        twoblock[1] = fv_BLANK;
                        break;
                    case 3:
                        twoblock[0] = fv_0;
                        twoblock[1] = fv_0;
                        break;
                    case 4:
                        twoblock[0] = fv_ADD;
                        twoblock[1] = fv_0;
                        break;
                    case 5:
                        twoblock[0] = fv_SUB;
                        twoblock[1] = fv_0;
                        break;
                    default:
                        twoblock[0] = fv_NULL;
                        twoblock[1] = fv_NULL;
                        break;
                }
            }

            // if negative
            if (result < 0)
            {
                // - (absolute value of result)
                twoblock[0] = fv_SUB;
                twoblock[1] = Hub.VineValidator_s.Get_FaceValue(Mathf.Abs(result));
            }

            /* Got both blocks made */
            /* Decide Left and Right, and combine */

            //create sixblock to return
            fv_FACEVALUE[] sixblock = new fv_FACEVALUE[6];

            //if left
            if (Roll_EitherOr())
            {
                sixblock[0] = threeblock[0];
                sixblock[1] = threeblock[1];
                sixblock[2] = threeblock[2];
                sixblock[3] = fv_EQUALS;
                sixblock[4] = twoblock[0];
                sixblock[5] = twoblock[1];
            }
            //if right
            else
            {
                sixblock[0] = twoblock[0];
                sixblock[1] = twoblock[1];
                sixblock[2] = fv_EQUALS;
                sixblock[3] = threeblock[0];
                sixblock[4] = threeblock[1];
                sixblock[5] = threeblock[2];
            }

            //return sixblock
            return sixblock;
        }

        /* Minus Side Equals ONE Int Side */
        /* int - _ int = int */
        /* Later, I Determine Left Or Right */
        else
        {
            //create fourblock
            fv_FACEVALUE[] fourblock = new fv_FACEVALUE[4];

            //get oneblock
            fv_FACEVALUE oneblock = GetRandomDigitValue();

            //get int result
            int result = Hub.VineValidator_s.Get_IntFromFaceValue(oneblock);

            //get first value - range from result to (result + 9)
            int firstvalue = Random.Range(result, result + 9 + 1); //inclusive, exclusive

            //get second value based on firstvalue and result
            int secondvalue = firstvalue - result;

            //fill fourblock
            {
                //if double digit
                if (firstvalue >= 10)
                {
                    fourblock[0] = fv_1;
                    fourblock[1] = Hub.VineValidator_s.Get_FaceValue(firstvalue - 10); // first value minus 10
                    fourblock[2] = fv_SUB;
                    fourblock[3] = Hub.VineValidator_s.Get_FaceValue(secondvalue);
                }
                else
                {
                    //get rand, 1 - 8
                    int rand = Random.Range(1, 9); //inclusive, exclusive

                    //use rand
                    switch (rand)
                    {

                        case 1: // 1 - 0 2
                            fourblock[0] = Hub.VineValidator_s.Get_FaceValue(firstvalue);
                            fourblock[1] = fv_SUB;
                            fourblock[2] = fv_0;
                            fourblock[3] = Hub.VineValidator_s.Get_FaceValue(secondvalue);
                            break;
                        case 2: // 1 - + 2
                            fourblock[0] = Hub.VineValidator_s.Get_FaceValue(firstvalue);
                            fourblock[1] = fv_SUB;
                            fourblock[2] = fv_ADD;
                            fourblock[3] = Hub.VineValidator_s.Get_FaceValue(secondvalue);
                            break;
                        case 3: // 1 - _ 2
                            fourblock[0] = Hub.VineValidator_s.Get_FaceValue(firstvalue);
                            fourblock[1] = fv_SUB;
                            fourblock[2] = fv_BLANK;
                            fourblock[3] = Hub.VineValidator_s.Get_FaceValue(secondvalue);
                            break;
                        case 4: // 1 - 2 _
                            fourblock[0] = Hub.VineValidator_s.Get_FaceValue(firstvalue);
                            fourblock[1] = fv_SUB;
                            fourblock[2] = Hub.VineValidator_s.Get_FaceValue(secondvalue);
                            fourblock[3] = fv_BLANK;
                            break;
                        case 5: // 1 _ - 2
                            fourblock[0] = Hub.VineValidator_s.Get_FaceValue(firstvalue);
                            fourblock[1] = fv_BLANK;
                            fourblock[2] = fv_SUB;
                            fourblock[3] = Hub.VineValidator_s.Get_FaceValue(secondvalue);
                            break;
                        case 6: // 0 1 - 2
                            fourblock[0] = fv_0;
                            fourblock[1] = Hub.VineValidator_s.Get_FaceValue(firstvalue);
                            fourblock[2] = fv_SUB;
                            fourblock[3] = Hub.VineValidator_s.Get_FaceValue(secondvalue);
                            break;
                        case 7: // + 1 - 2
                            fourblock[0] = fv_ADD;
                            fourblock[1] = Hub.VineValidator_s.Get_FaceValue(firstvalue);
                            fourblock[2] = fv_SUB;
                            fourblock[3] = Hub.VineValidator_s.Get_FaceValue(secondvalue);
                            break;
                        case 8: // _ 1 - 2
                            fourblock[0] = fv_BLANK;
                            fourblock[1] = Hub.VineValidator_s.Get_FaceValue(firstvalue);
                            fourblock[2] = fv_SUB;
                            fourblock[3] = Hub.VineValidator_s.Get_FaceValue(secondvalue);
                            break;
                        default:
                            break;
                    }
                }
            }

            /* Got both blocks made */
            /* Decide Left and Right, and combine */

            //create sixblock to return
            fv_FACEVALUE[] sixblock = new fv_FACEVALUE[6];

            //if left
            if (Roll_EitherOr())
            {
                sixblock[0] = fourblock[0];
                sixblock[1] = fourblock[1];
                sixblock[2] = fourblock[2];
                sixblock[3] = fourblock[3];
                sixblock[4] = fv_EQUALS;
                sixblock[5] = oneblock;
            }
            //if right
            else
            {
                sixblock[0] = oneblock;
                sixblock[1] = fv_EQUALS;
                sixblock[2] = fourblock[0];
                sixblock[3] = fourblock[1];
                sixblock[4] = fourblock[2];
                sixblock[5] = fourblock[3];
            }

            //return sixblock
            return sixblock;
        }
    }

    fv_FACEVALUE[] SixBlock_YesMath_Addition_OneEquals_AllDigits()
    {
        /* Plus Side Equals TWO Int Side */
        /* int + int = int int */
        /* Later, I Determine Left Or Right */
        if (Roll_EitherOr())
        {
            //create threeblock
            fv_FACEVALUE[] threeblock = new fv_FACEVALUE[3];

            //fill threeblock
            threeblock[0] = GetRandomDigitValue();
            threeblock[1] = fv_ADD;
            threeblock[2] = GetRandomDigitValue();

            //create twoblock
            fv_FACEVALUE[] twoblock = new fv_FACEVALUE[2];

            //get result
            int firstint = Hub.VineValidator_s.Get_IntFromFaceValue(threeblock[0]);
            int secondint = Hub.VineValidator_s.Get_IntFromFaceValue(threeblock[2]);
            int result = firstint + secondint;

            //if result ten or over
            if (result >= 10)
            {
                //first is 1
                twoblock[0] = fv_1;

                //second is result - 10, converted to facevalue
                twoblock[1] = Hub.VineValidator_s.Get_FaceValue(result - 10);
            }

            //else if result positive, non-zero, less than 10
            else if (result > 0)
            {
                //get rand 1 - 4
                int rand = Random.Range(1, 5); //inclusive, exclusive

                switch (rand) // 09, _9, 9_, +9
                {
                    case 1:
                        twoblock[0] = fv_0;
                        twoblock[1] = Hub.VineValidator_s.Get_FaceValue(result);
                        break;
                    case 2:
                        twoblock[0] = fv_BLANK;
                        twoblock[1] = Hub.VineValidator_s.Get_FaceValue(result);
                        break;
                    case 3:
                        twoblock[0] = Hub.VineValidator_s.Get_FaceValue(result);
                        twoblock[1] = fv_BLANK;
                        break;
                    case 4:
                        twoblock[0] = fv_ADD;
                        twoblock[1] = Hub.VineValidator_s.Get_FaceValue(result);
                        break;
                    default:
                        twoblock[0] = fv_NULL;
                        twoblock[1] = fv_NULL;
                        break;
                }
            }

            //if result 0
            if (result == 0)
            {
                //get rand 1 - 5
                int rand = Random.Range(1, 6); //inclusive, exclusive

                switch (rand) //_0, 0_, 00, +0, -0
                {
                    case 1:
                        twoblock[0] = fv_BLANK;
                        twoblock[1] = fv_0;
                        break;
                    case 2:
                        twoblock[0] = fv_0;
                        twoblock[1] = fv_BLANK;
                        break;
                    case 3:
                        twoblock[0] = fv_0;
                        twoblock[1] = fv_0;
                        break;
                    case 4:
                        twoblock[0] = fv_ADD;
                        twoblock[1] = fv_0;
                        break;
                    case 5:
                        twoblock[0] = fv_SUB;
                        twoblock[1] = fv_0;
                        break;
                    default:
                        twoblock[0] = fv_NULL;
                        twoblock[1] = fv_NULL;
                        break;
                }
            }

            /* Got both blocks made */
            /* Decide Left and Right, and combine */

            //create sixblock to return
            fv_FACEVALUE[] sixblock = new fv_FACEVALUE[6];

            //if left
            if (Roll_EitherOr())
            {
                sixblock[0] = threeblock[0];
                sixblock[1] = threeblock[1];
                sixblock[2] = threeblock[2];
                sixblock[3] = fv_EQUALS;
                sixblock[4] = twoblock[0];
                sixblock[5] = twoblock[1];
            }
            //if right
            else
            {
                sixblock[0] = twoblock[0];
                sixblock[1] = twoblock[1];
                sixblock[2] = fv_EQUALS;
                sixblock[3] = threeblock[0];
                sixblock[4] = threeblock[1];
                sixblock[5] = threeblock[2];
            }

            //return sixblock
            return sixblock;
        }

        /* Plus Side Equals ONE Int Side */
        /* int + _ int = int */
        /* Later, I Determine Left Or Right */
        else
        {
            //create fourblock
            fv_FACEVALUE[] fourblock = new fv_FACEVALUE[4];

            //get oneblock
            fv_FACEVALUE oneblock = GetRandomDigitValue();

            //get int result
            int result = Hub.VineValidator_s.Get_IntFromFaceValue(oneblock);

            //get first value - range from (result - 9) to result
            int firstvalue = Random.Range(result - 9, result + 1); //inclusive, exclusive

            //get second value based on firstvalue and result
            int secondvalue = result - firstvalue;

            //fill fourblock
            {
                //declare left and right
                int left;
                int right;

                //first is left
                if (Roll_EitherOr())
                {
                    left = firstvalue;
                    right = secondvalue;
                }
                //second is left
                else
                {
                    left = secondvalue;
                    right = firstvalue;
                }

                //if one is negative
                if (left < 0 || right < 0)
                {
                    //left negative
                    if (left < 0)
                    {
                        fourblock[0] = fv_SUB;
                        fourblock[1] = Hub.VineValidator_s.Get_FaceValue(Mathf.Abs(left));
                        fourblock[2] = fv_ADD;
                        fourblock[3] = Hub.VineValidator_s.Get_FaceValue(right);
                    }
                    //right negative
                    else
                    {
                        fourblock[0] = Hub.VineValidator_s.Get_FaceValue(left);
                        fourblock[1] = fv_ADD;
                        fourblock[2] = fv_SUB;
                        fourblock[3] = Hub.VineValidator_s.Get_FaceValue(Mathf.Abs(right));
                    }
                }
                //no negatives
                else
                {
                    //left is twoblock
                    if (Roll_EitherOr())
                    {
                        //get rand, 1 - 4
                        int rand = Random.Range(1, 5); //inclusive, exclusive

                        //use rand
                        switch (rand)
                        {
                            case 1: // 0 9
                                fourblock[0] = fv_0;
                                fourblock[1] = Hub.VineValidator_s.Get_FaceValue(left);
                                break;
                            case 2: // _ 9
                                fourblock[0] = fv_BLANK;
                                fourblock[1] = Hub.VineValidator_s.Get_FaceValue(left);
                                break;
                            case 3: // 9 _
                                fourblock[0] = Hub.VineValidator_s.Get_FaceValue(left);
                                fourblock[1] = fv_BLANK;
                                break;
                            case 4: // + 9
                                fourblock[0] = fv_ADD;
                                fourblock[1] = Hub.VineValidator_s.Get_FaceValue(left);
                                break;
                            default:
                                break;
                        }

                        //fill right
                        fourblock[2] = fv_ADD;
                        fourblock[3] = Hub.VineValidator_s.Get_FaceValue(right);
                    }
                    //right is twoblock
                    else
                    {
                        //fill left
                        fourblock[0] = Hub.VineValidator_s.Get_FaceValue(left);
                        fourblock[1] = fv_ADD;

                        //get rand, 1 - 5
                        int rand = Random.Range(1, 6); //inclusive, exclusive

                        //use rand
                        switch (rand)
                        {
                            case 1: // 0 9
                                fourblock[2] = fv_0;
                                fourblock[3] = Hub.VineValidator_s.Get_FaceValue(right);
                                break;
                            case 2: // _ 9
                                fourblock[2] = fv_BLANK;
                                fourblock[3] = Hub.VineValidator_s.Get_FaceValue(right);
                                break;
                            case 3: // 9 _
                                fourblock[2] = Hub.VineValidator_s.Get_FaceValue(right);
                                fourblock[3] = fv_BLANK;
                                break;
                            case 4: // + 9
                                fourblock[2] = fv_ADD;
                                fourblock[3] = Hub.VineValidator_s.Get_FaceValue(right);
                                break;
                            case 5: // - - 9 DOUBLE NEGATIVE IS PLUS
                                fourblock[1] = fv_SUB;
                                fourblock[2] = fv_SUB;
                                fourblock[3] = Hub.VineValidator_s.Get_FaceValue(right);
                                break;
                            default:
                                break;
                        }

                    }

                }
            }

            /* Got both blocks made */
            /* Decide Left and Right, and combine */

            //create sixblock to return
            fv_FACEVALUE[] sixblock = new fv_FACEVALUE[6];

            //if left
            if (Roll_EitherOr())
            {
                sixblock[0] = fourblock[0];
                sixblock[1] = fourblock[1];
                sixblock[2] = fourblock[2];
                sixblock[3] = fourblock[3];
                sixblock[4] = fv_EQUALS;
                sixblock[5] = oneblock;
            }
            //if right
            else
            {
                sixblock[0] = oneblock;
                sixblock[1] = fv_EQUALS;
                sixblock[2] = fourblock[0];
                sixblock[3] = fourblock[1];
                sixblock[4] = fourblock[2];
                sixblock[5] = fourblock[3];
            }

            //return sixblock
            return sixblock;
        }
    }



    /* UTILITIES */

    public fv_FACEVALUE GetRandomFaceValue(bool bIncludeEquals = false)
    {
        // facevalue enum limit
        int limit = 13;

        //if including equals
        if (bIncludeEquals)
        {
            limit = 14;
        }

        //get rand
        int rand = Random.Range(1, limit + 1); //inclusive, exclusive

        //cast to facevalue
        return (fv_FACEVALUE)rand;
    }

    public fv_FACEVALUE GetRandomDigitValue()
    {
        //get rand index for enum digits
        int rand = Random.Range(1, 11); //inclusive, exclusive

        //cast to facevalue
        return (fv_FACEVALUE)rand;
    }

    public int ConvertToIndex(a_ADDRESS address)
    {
        switch (address)
        {
            case a_A: return 0;
            case a_B: return 1;
            case a_C: return 2;
            case a_D: return 3;
            case a_E: return 4;
            case a_F: return 5;
            case a_G: return 6;
            default: return 0;
        }
    }

    public bool Roll_EitherOr()
    {
        return (Random.value > 0.5f);
    }

    bool bIsDigitMinusZero(fv_FACEVALUE val)
    {
        //cast
        int valcast = (int)val;

        //if val is fv_1 -> fv_9
        if (valcast >= 2 && valcast <= 10)
        {
            return true;
        }

        return false;
    }

    bool bIsDigit(fv_FACEVALUE val)
    {
        //cast
        int valcast = (int)val;

        //if val is fv_0 -> fv_9
        if (valcast >= 1 && valcast <= 10)
        {
            return true;
        }

        return false;
    }

    fv_FACEVALUE[] Get_BlankedFourBlock_WithThisValueInserted(fv_FACEVALUE val)
    {
        //create block to return
        fv_FACEVALUE[] block = new fv_FACEVALUE[4] { fv_BLANK, fv_BLANK, fv_BLANK, fv_BLANK };

        //get rand
        int rand = Random.Range(0, 4); //inclusive, exclusive

        //add val
        block[rand] = val;

        //return
        return block;

    }

    void Place_SingleValue_InThreeBlock(ref fv_FACEVALUE[] threeblock, fv_FACEVALUE singlevalue)
    {
        //declare rand
        int rand;

        //find rand based on if 0
        {
            //if 0
            if (singlevalue == 0)
            {
                //get rand 1 - 19
                rand = Random.Range(1, 20); //inclusive, exclusive
            }
            else
            {
                //get rand 1 - 13
                rand = Random.Range(1, 14); //inclusive, exclusive
            }
        }

        //use rand
        switch (rand)
        {
            /* Digit first */

            case 1: // 8 _ _
                threeblock[0] = singlevalue;
                threeblock[1] = fv_BLANK;
                threeblock[2] = fv_BLANK;
                break;

            /* Digit second */

            case 2: // _ 8 _
                threeblock[0] = fv_BLANK;
                threeblock[1] = singlevalue;
                threeblock[2] = fv_BLANK;
                break;
            case 3: // + 8 _
                threeblock[0] = fv_ADD;
                threeblock[1] = singlevalue;
                threeblock[2] = fv_BLANK;
                break;
            case 4: // 0 8 _
                threeblock[0] = fv_0;
                threeblock[1] = singlevalue;
                threeblock[2] = fv_BLANK;
                break;

            /* Digit third */

            case 5: // 0 0 8
                threeblock[0] = fv_0;
                threeblock[1] = fv_0;
                threeblock[2] = singlevalue;
                break;
            case 6: // 0 _ 8
                threeblock[0] = fv_0;
                threeblock[1] = fv_BLANK;
                threeblock[2] = singlevalue;
                break;
            case 7: // + 0 8
                threeblock[0] = fv_ADD;
                threeblock[1] = fv_0;
                threeblock[2] = singlevalue;
                break;
            case 8: // + + 8
                threeblock[0] = fv_ADD;
                threeblock[1] = fv_ADD;
                threeblock[2] = singlevalue;
                break;
            case 9: // + _ 8
                threeblock[0] = fv_ADD;
                threeblock[1] = fv_BLANK;
                threeblock[2] = singlevalue;
                break;
            case 10: // _ 0 8
                threeblock[0] = fv_BLANK;
                threeblock[1] = fv_0;
                threeblock[2] = singlevalue;
                break;
            case 11: // _ + 8
                threeblock[0] = fv_BLANK;
                threeblock[1] = fv_ADD;
                threeblock[2] = singlevalue;
                break;
            case 12: // _ _ 8
                threeblock[0] = fv_BLANK;
                threeblock[1] = fv_BLANK;
                threeblock[2] = singlevalue;
                break;
            case 13: // - - 8 (two minus)
                threeblock[0] = fv_SUB;
                threeblock[1] = fv_SUB;
                threeblock[2] = singlevalue;
                break;

            /*  Additional possibilities if singlevalue is 0 */

            case 14: // - 0 0
                threeblock[0] = fv_SUB;
                threeblock[1] = fv_0;
                threeblock[2] = fv_0;
                break;
            case 15: // - 0 _
                threeblock[0] = fv_SUB;
                threeblock[1] = fv_0;
                threeblock[2] = fv_BLANK;
                break;
            case 16: // - + 0
                threeblock[0] = fv_SUB;
                threeblock[1] = fv_ADD;
                threeblock[2] = fv_0;
                break;
            case 17: // - _ 0
                threeblock[0] = fv_SUB;
                threeblock[1] = fv_BLANK;
                threeblock[2] = fv_0;
                break;
            case 18: // + - 0
                threeblock[0] = fv_ADD;
                threeblock[1] = fv_SUB;
                threeblock[2] = fv_0;
                break;
            case 19: // _ - 0
                threeblock[0] = fv_BLANK;
                threeblock[1] = fv_SUB;
                threeblock[2] = fv_0;
                break;

            default:
                break;
        }
    }

    void Record_Solution(ref StreamWriter writer, fv_FACEVALUE[] block)
    {
        writer.WriteLine("===========");

        //if six_block
        if (block.Length > 4)
        {
            writer.WriteLine(Convert_FaceValueToString(block[0]) + " " +
                                Convert_FaceValueToString(block[1]) + " " +
                                Convert_FaceValueToString(block[2]) + " " +
                                Convert_FaceValueToString(block[3]) + " " +
                                Convert_FaceValueToString(block[4]) + " " +
                                Convert_FaceValueToString(block[5])
                            );
        }
        //if four_block
        else
        {
            writer.WriteLine(Convert_FaceValueToString(block[0]) + " " +
                                Convert_FaceValueToString(block[1]) + " " +
                                Convert_FaceValueToString(block[2]) + " " +
                                Convert_FaceValueToString(block[3])
                );
        }
    }

    string Convert_FaceValueToString(fv_FACEVALUE fv)
    {
        /*  This one is slightly different that the LevelBuilder converter.  Blank is represented by an underscore */


        switch (fv)
        {
            case fv_0:
                return "0";
            case fv_1:
                return "1";
            case fv_2:
                return "2";
            case fv_3:
                return "3";
            case fv_4:
                return "4";
            case fv_5:
                return "5";
            case fv_6:
                return "6";
            case fv_7:
                return "7";
            case fv_8:
                return "8";
            case fv_9:
                return "9";
            case fv_ADD:
                return "+";
            case fv_SUB:
                return "-";
            case fv_BLANK:
                return "_";
            case fv_EQUALS:
                return "=";
            default:
                return "?";

        }
    }
}