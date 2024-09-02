using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static a_ADDRESS;
using static t_TRI;
using static fv_FACEVALUE;
using static va_VINEAXIS;
using System.Diagnostics;

public struct VineBlock
{
    public VineBlock(fv_FACEVALUE val, int intval)
    {
        this.Value = val;
        this.IntValue = intval;
    }

    public fv_FACEVALUE Value;
    public int IntValue;
}

public class VineValidator : MonoBehaviour
{
    //public GameObject RefHolder;

    //public Dictionary<a_ADDRESS, GameObject> SlotRefDict;
    //Dictionary<va_VINEAXIS, List<fv_FACEVALUE>> BulbAnswerDict;

    public void Validate_AllHoneyCombs()
    {
        Validate_ThisHoneyComb(a_A);
        Validate_ThisHoneyComb(a_B);
        Validate_ThisHoneyComb(a_C);
        Validate_ThisHoneyComb(a_D);
        Validate_ThisHoneyComb(a_E);
        Validate_ThisHoneyComb(a_F);
        Validate_ThisHoneyComb(a_G);
    }

    public void Validate_ThisHoneyComb(a_ADDRESS HoneyCombAddress)
    {
        //condense
        HoneyComb ThisHoneyComb = Hub.SlotRefDict[HoneyCombAddress].GetComponent<HoneyComb>();

        //finalized check
        if (ThisHoneyComb.IsHoneyComb_Finalized())
        {
            return;
        }

        //get truncated ref dictionary
        Dictionary<a_ADDRESS, GameObject> HoneySlotRefDict = ThisHoneyComb.HoneySlotRefDict;
        Dictionary<va_VINEAXIS, Vine> VineRefDict = ThisHoneyComb.VineRefDict;

        //start counter
        int ValidVineCounter = 0;

        //loop through nine vines
        for (int i = 1; i <= 9; i++)
        {
            //if vine is good
            if (Validate_ThisVine(HoneySlotRefDict, VineRefDict, (va_VINEAXIS)i))
            {
                Hub.SlotRefDict[HoneyCombAddress].transform.GetChild(i + 7).GetComponent<Vine>().Activate_Flower();
                Change_EqualSigns(HoneySlotRefDict, (va_VINEAXIS)i, true);
                ValidVineCounter++;
            }
            //if vine is bad
            else
            {
                Hub.SlotRefDict[HoneyCombAddress].transform.GetChild(i + 7).GetComponent<Vine>().Deactivate_Flower();
                Change_EqualSigns(HoneySlotRefDict, (va_VINEAXIS)i, false);
            }
        }

        if (Hub.TitleLevel_s || Hub.TutorialLevel_s)
        {
            return;
        }

        //if all 9 Flowers are valid
        if (ValidVineCounter == 9)
        {
            Hub.GameLevel_s.Trigger_HoneyCombValidation(ThisHoneyComb, true);
        }
        else
        {
            Hub.GameLevel_s.Trigger_HoneyCombValidation(ThisHoneyComb, false);
        }
    }


    public bool DoesThisValidate(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, ref string ans)
    {
        //find amount of equals in values
        int EqualsAmount = Get_AmountOfEqualsValues(val1, val2, val3, val4);

        //validate according to EqualsAmount
        switch (EqualsAmount)
        {
            case 0:
                Scrub_TheseBlocks_NoEquals(val1, val2, val3, val4, ref ans);
                return true;
            case 1:
                return Validate_With_1_Equals(val1, val2, val3, val4, ref ans);
            case 2:
                return false;
            case 3:
                return Validate_With_3_Equals(val1, val2, val3, val4, ref ans);
            case 4:
                return false;
            default:
                return false;
        }
    }

    public bool DoesThisValidate(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, fv_FACEVALUE val5, fv_FACEVALUE val6, ref string ans)
    {
        //find amount of equals in values
        int EqualsAmount = Get_AmountOfEqualsValues(val1, val2, val3, val4, val5, val6);

        //validate according to EqualsAmount
        switch (EqualsAmount)
        {
            case 0:
                Scrub_TheseBlocks_NoEquals(val1, val2, val3, val4, val5, val6, ref ans);
                return true;
            case 1:
                return Validate_With_1_Equals(val1, val2, val3, val4, val5, val6, ref ans);
            case 2:
                return Validate_With_2_Equals(val1, val2, val3, val4, val5, val6, ref ans);
            case 3:
                return Validate_With_3_Equals(val1, val2, val3, val4, val5, val6, ref ans);
            case 4:
                return false;
            case 5:
                return Validate_With_5_Equals(val1, val2, val3, val4, val5, val6, ref ans);
            case 6:
                return false;
            default:
                return false;
        }
    }

    private bool Validate_With_1_Equals(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, ref string ans)
    {
        //separate into Left and Right lists
        //remove blanks
        //remove zeros
        //merge ints
        //remove plus signs and minus signs, but not do math
        //do minus sign math
        //do plus sign math
        //check each side for equality

        //put values into list
        List<VineBlock> AllValues = new List<VineBlock>();
        AllValues.Add(new VineBlock(val1, 0));
        AllValues.Add(new VineBlock(val2, 0));
        AllValues.Add(new VineBlock(val3, 0));
        AllValues.Add(new VineBlock(val4, 0));

        //create a left and right side list
        List<VineBlock> Left_Values = new List<VineBlock>();
        List<VineBlock> Right_Values = new List<VineBlock>();

        // runner to interpret left or right side of equation
        bool SideTrigger = false;

        //loop all 4 values, also remove blanks
        foreach (VineBlock vblock in AllValues)
        {
            //set equals trigger
            if (vblock.Value == fv_EQUALS)
            {
                SideTrigger = true;
                continue;
            }

            // skip, if blank
            if (vblock.Value == fv_BLANK)
            {
                continue;
            }

            //left side
            if (SideTrigger == false)
            {
                //add to left
                Left_Values.Add(vblock);
            }
            //right side
            if (SideTrigger == true)
            {
                //add to right
                Right_Values.Add(vblock);
            }
        }

        Scrub_PossiblyEliminate_Zeros(ref Left_Values);
        Scrub_PossiblyEliminate_Zeros(ref Right_Values);

        Scrub_PossiblyMerge_Ints(ref Left_Values);
        Scrub_PossiblyMerge_Ints(ref Right_Values);

        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref Left_Values);
        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref Right_Values);

        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref Left_Values);
        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref Right_Values);

        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref Left_Values);
        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref Right_Values);

        /* SCRUBBING DONE */

        /* Apply Answer */
        {
            //left
            for (int i = 0; i < Left_Values.Count; i++)
            {
                if (Left_Values[i].Value == fv_INT)
                {
                    ans += Left_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(Left_Values[i].Value);
                }
            }

            //equals
            ans += Hub.LevelBuilder_s.Convert_FaceValueToString(fv_EQUALS);

            //right
            for (int i = 0; i < Right_Values.Count; i++)
            {
                if (Right_Values[i].Value == fv_INT)
                {
                    ans += Right_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(Right_Values[i].Value);
                }
            }
        }

        /* Check If Equal Now */

        //if equal side counts
        if (Right_Values.Count == Left_Values.Count)
        {
            //loop indexes
            for (int i = 0; i < Right_Values.Count; i++)
            {
                //if no match
                if (Right_Values[i].Value != Left_Values[i].Value)
                {
                    return false;
                }
                //if both are fv_INT
                if (Right_Values[i].Value == fv_INT)
                {
                    //if IntValues don't match
                    if (Right_Values[i].IntValue != Left_Values[i].IntValue)
                    {
                        return false;
                    }
                }
            }

            /* Everything matches */

            return true;
        }

        //safety
        return false;
    }

    private bool Validate_With_1_Equals(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, fv_FACEVALUE val5, fv_FACEVALUE val6, ref string ans)
    {
        //separate into Left and Right lists
        //remove blanks
        //remove zeros
        //merge ints
        //remove plus signs and minus signs, but not do math
        //do minus sign math
        //do plus sign math
        //check each side for equality

        //put values into list
        List<VineBlock> AllValues = new List<VineBlock>();
        AllValues.Add(new VineBlock(val1, 0));
        AllValues.Add(new VineBlock(val2, 0));
        AllValues.Add(new VineBlock(val3, 0));
        AllValues.Add(new VineBlock(val4, 0));
        AllValues.Add(new VineBlock(val5, 0));
        AllValues.Add(new VineBlock(val6, 0));

        //create a left and right side list
        List<VineBlock> Left_Values = new List<VineBlock>();
        List<VineBlock> Right_Values = new List<VineBlock>();

        // runner to interpret left or right side of equation
        bool SideTrigger = false;

        //loop all 6 values, also remove blanks
        foreach (VineBlock vblock in AllValues)
        {
            //set equals trigger
            if (vblock.Value == fv_EQUALS)
            {
                SideTrigger = true;
                continue;
            }

            // skip, if blank
            if (vblock.Value == fv_BLANK)
            {
                continue;
            }

            //left side
            if (SideTrigger == false)
            {
                //add to left
                Left_Values.Add(vblock);
            }
            //right side
            if (SideTrigger == true)
            {
                //add to right
                Right_Values.Add(vblock);
            }
        }


        Scrub_PossiblyEliminate_Zeros(ref Left_Values);
        Scrub_PossiblyEliminate_Zeros(ref Right_Values);

        Scrub_PossiblyMerge_Ints(ref Left_Values);
        Scrub_PossiblyMerge_Ints(ref Right_Values);

        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref Left_Values);
        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref Right_Values);

        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref Left_Values);
        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref Right_Values);

        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref Left_Values);
        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref Right_Values);

        //Debug.Log("LeftValues------ ");
        //foreach (VineBlock block in Left_Values)
        //{
        //    if (block.Value == fv_INT)
        //    {
        //        Debug.Log("LeftValues: int " + block.IntValue);
        //        continue;
        //    }
        //    Debug.Log("LeftValues: " + block.Value);
        //}
        //Debug.Log("RightValues------ ");
        //foreach (VineBlock block in Right_Values)
        //{
        //    if (block.Value == fv_INT)
        //    {
        //        Debug.Log("RightValues: int " + block.IntValue);
        //        continue;
        //    }
        //    Debug.Log("RightValues: " + block.Value);
        //}


        /* SCRUBBING DONE */

        /* Apply Answer */
        {
            //left
            for (int i = 0; i < Left_Values.Count; i++)
            {
                if (Left_Values[i].Value == fv_INT)
                {
                    ans += Left_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(Left_Values[i].Value);
                }
            }

            //equals
            ans += Hub.LevelBuilder_s.Convert_FaceValueToString(fv_EQUALS);

            //right
            for (int i = 0; i < Right_Values.Count; i++)
            {
                if (Right_Values[i].Value == fv_INT)
                {
                    ans += Right_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(Right_Values[i].Value);
                }
            }
        }

        /* Check If Equal Now */

        //if equal side counts
        if (Right_Values.Count == Left_Values.Count)
        {
            //loop indexes
            for (int i = 0; i < Right_Values.Count; i++)
            {
                //if no match
                if (Right_Values[i].Value != Left_Values[i].Value)
                {
                    return false;
                }
                //if both are fv_INT
                if (Right_Values[i].Value == fv_INT)
                {
                    //if IntValues don't match
                    if (Left_Values[i].IntValue != Right_Values[i].IntValue)
                    {
                        return false;
                    }
                }
            }

            /* Everything matches */

            return true;
        }

        //safety
        return false;
    }

    private bool Validate_With_2_Equals(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, fv_FACEVALUE val5, fv_FACEVALUE val6, ref string ans)
    {
        /* Three blocks need to equal each other */

        //put values into list
        List<VineBlock> AllValues = new List<VineBlock>();
        AllValues.Add(new VineBlock(val1, 0));
        AllValues.Add(new VineBlock(val2, 0));
        AllValues.Add(new VineBlock(val3, 0));
        AllValues.Add(new VineBlock(val4, 0));
        AllValues.Add(new VineBlock(val5, 0));
        AllValues.Add(new VineBlock(val6, 0));

        //create a A list, B list, and C list
        List<VineBlock> A_Values = new List<VineBlock>();
        List<VineBlock> B_Values = new List<VineBlock>();
        List<VineBlock> C_Values = new List<VineBlock>();

        // runner to interpret which list to put it in
        int ListTrigger = 0;

        //loop all 6 values, also remove blanks
        foreach (VineBlock vblock in AllValues)
        {
            //set equals trigger
            if (vblock.Value == fv_EQUALS)
            {
                ListTrigger++;
                continue;
            }

            // skip, if blank
            if (vblock.Value == fv_BLANK)
            {
                continue;
            }

            // A-List
            if (ListTrigger == 0)
            {
                //add to A-List
                A_Values.Add(vblock);
            }

            // B-List
            if (ListTrigger == 1)
            {
                //add to B-List
                B_Values.Add(vblock);
            }

            // C-List
            if (ListTrigger == 2)
            {
                //add to C-List
                C_Values.Add(vblock);
            }
        }

        //scrub each list
        {
            //A-List
            if (A_Values.Count > 1)
            {
                Scrub_PossiblyEliminate_Zeros(ref A_Values);
                Scrub_PossiblyMerge_Ints(ref A_Values);
                Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref A_Values);
                Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref A_Values);
                Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref A_Values);
            }
            else
            {
                Scrub_PossiblyMerge_Ints(ref A_Values);
            }

            //B-List
            if (B_Values.Count > 1)
            {
                Scrub_PossiblyEliminate_Zeros(ref B_Values);
                Scrub_PossiblyMerge_Ints(ref B_Values);
                Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref B_Values);
                Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref B_Values);
                Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref B_Values);
            }
            else
            {
                Scrub_PossiblyMerge_Ints(ref B_Values);
            }

            //C-List
            if (C_Values.Count > 1)
            {
                Scrub_PossiblyEliminate_Zeros(ref C_Values);
                Scrub_PossiblyMerge_Ints(ref C_Values);
                Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref C_Values);
                Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref C_Values);
                Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref C_Values);
            }
            else
            {
                Scrub_PossiblyMerge_Ints(ref C_Values);
            }

        }

        /* SCRUBBING DONE */

        /* Apply Answer */
        {
            //A
            for (int i = 0; i < A_Values.Count; i++)
            {
                if (A_Values[i].Value == fv_INT)
                {
                    ans += A_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(A_Values[i].Value);
                }
            }

            //equals
            ans += Hub.LevelBuilder_s.Convert_FaceValueToString(fv_EQUALS);

            //B
            for (int i = 0; i < B_Values.Count; i++)
            {
                if (B_Values[i].Value == fv_INT)
                {
                    ans += B_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(B_Values[i].Value);
                }
            }

            //equals
            ans += Hub.LevelBuilder_s.Convert_FaceValueToString(fv_EQUALS);

            //C
            for (int i = 0; i < C_Values.Count; i++)
            {
                if (C_Values[i].Value == fv_INT)
                {
                    ans += C_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(C_Values[i].Value);
                }
            }
        }

        /* Check If Equal Now */

        //if equal counts
        if (A_Values.Count == B_Values.Count && A_Values.Count == C_Values.Count)
        {
            //loop indexes
            for (int i = 0; i < A_Values.Count; i++)
            {
                //if no match between all three
                if (A_Values[i].Value != B_Values[i].Value || A_Values[i].Value != C_Values[i].Value)
                {
                    return false;
                }
                //if all are fv_INT
                if (A_Values[i].Value == fv_INT)
                {
                    //if IntValues don't match
                    if (A_Values[i].IntValue != B_Values[i].IntValue || A_Values[i].IntValue != C_Values[i].IntValue)
                    {
                        return false;
                    }
                }
            }

            /* Everything matches */

            return true;
        }

        //safety
        return false;
    }

    private bool Validate_With_3_Equals(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, fv_FACEVALUE val5, fv_FACEVALUE val6, ref string ans)
    {
        /* Of the 3, the middle equals is the divider.  Side equals are just symbols */

        //separate into Left and Right lists
        //remove blanks
        //remove zeros
        //merge ints
        //remove plus signs and minus signs, but not do math
        //do minus sign math
        //do plus sign math
        //check each side for equality

        //put values into list
        List<VineBlock> AllValues = new List<VineBlock>();
        AllValues.Add(new VineBlock(val1, 0));
        AllValues.Add(new VineBlock(val2, 0));
        AllValues.Add(new VineBlock(val3, 0));
        AllValues.Add(new VineBlock(val4, 0));
        AllValues.Add(new VineBlock(val5, 0));
        AllValues.Add(new VineBlock(val6, 0));

        //create a left and right side list
        List<VineBlock> Left_Values = new List<VineBlock>();
        List<VineBlock> Right_Values = new List<VineBlock>();

        // runner to interpret left or right side of equation
        int EqualsTicker = 0;

        //loop all 6 values, also remove blanks
        foreach (VineBlock vblock in AllValues)
        {
            //increment ticker
            if (vblock.Value == fv_EQUALS)
            {
                EqualsTicker++;
            }

            // skip, if blank
            if (vblock.Value == fv_BLANK)
            {
                continue;
            }

            //before middle
            if (EqualsTicker < 2)
            {
                //add to left
                Left_Values.Add(vblock);
            }
            //after middle
            if (EqualsTicker > 2)
            {
                //add to right
                Right_Values.Add(vblock);
            }
            //at middle
            if (EqualsTicker == 2)
            {
                //increment ticker again, sending to right side now
                EqualsTicker++;
            }
        }

        Scrub_PossiblyEliminate_Zeros(ref Left_Values);
        Scrub_PossiblyEliminate_Zeros(ref Right_Values);

        Scrub_PossiblyMerge_Ints(ref Left_Values);
        Scrub_PossiblyMerge_Ints(ref Right_Values);

        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref Left_Values);
        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref Right_Values);

        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref Left_Values);
        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref Right_Values);

        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref Left_Values);
        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref Right_Values);


        /* SCRUBBING DONE */

        /* Apply Answer */
        {
            //left
            for (int i = 0; i < Left_Values.Count; i++)
            {
                if (Left_Values[i].Value == fv_INT)
                {
                    ans += Left_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(Left_Values[i].Value);
                }
            }

            //equals
            ans += Hub.LevelBuilder_s.Convert_FaceValueToString(fv_EQUALS);

            //right
            for (int i = 0; i < Right_Values.Count; i++)
            {
                if (Right_Values[i].Value == fv_INT)
                {
                    ans += Right_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(Right_Values[i].Value);
                }
            }
        }

        /* Check If Equal Now */

        //if equal side counts
        if (Right_Values.Count == Left_Values.Count)
        {
            //loop indexes
            for (int i = 0; i < Right_Values.Count; i++)
            {
                //if no match
                if (Right_Values[i].Value != Left_Values[i].Value)
                {
                    return false;
                }
                //if both are fv_INT
                if (Right_Values[i].Value == fv_INT)
                {
                    //if IntValues don't match
                    if (Left_Values[i].IntValue != Right_Values[i].IntValue)
                    {
                        return false;
                    }
                }
            }

            /* Everything matches */

            return true;
        }

        //safety
        return false;
    }

    private bool Validate_With_3_Equals(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, ref string ans)
    {
        /* Of the 3, the middle equals is the divider.  Side equals are just symbols */

        //separate into Left and Right lists
        //remove blanks
        //remove zeros
        //merge ints
        //remove plus signs and minus signs, but not do math
        //do minus sign math
        //do plus sign math
        //check each side for equality

        //put values into list
        List<VineBlock> AllValues = new List<VineBlock>();
        AllValues.Add(new VineBlock(val1, 0));
        AllValues.Add(new VineBlock(val2, 0));
        AllValues.Add(new VineBlock(val3, 0));
        AllValues.Add(new VineBlock(val4, 0));

        //create a left and right side list
        List<VineBlock> Left_Values = new List<VineBlock>();
        List<VineBlock> Right_Values = new List<VineBlock>();

        // runner to interpret left or right side of equation
        int EqualsTicker = 0;

        //loop all 4 values, also remove blanks
        foreach (VineBlock vblock in AllValues)
        {
            //increment ticker
            if (vblock.Value == fv_EQUALS)
            {
                EqualsTicker++;
            }

            // skip, if blank
            if (vblock.Value == fv_BLANK)
            {
                continue;
            }

            //before middle
            if (EqualsTicker < 2)
            {
                //add to left
                Left_Values.Add(vblock);
            }
            //after middle
            if (EqualsTicker > 2)
            {
                //add to right
                Right_Values.Add(vblock);
            }
            //at middle
            if (EqualsTicker == 2)
            {
                //increment ticker again, sending to right side now
                EqualsTicker++;
            }
        }

        Scrub_PossiblyEliminate_Zeros(ref Left_Values);
        Scrub_PossiblyEliminate_Zeros(ref Right_Values);

        Scrub_PossiblyMerge_Ints(ref Left_Values);
        Scrub_PossiblyMerge_Ints(ref Right_Values);

        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref Left_Values);
        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref Right_Values);

        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref Left_Values);
        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref Right_Values);

        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref Left_Values);
        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref Right_Values);


        /* SCRUBBING DONE */

        /* Apply Answer */
        {
            //left
            for (int i = 0; i < Left_Values.Count; i++)
            {
                if (Left_Values[i].Value == fv_INT)
                {
                    ans += Left_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(Left_Values[i].Value);
                }
            }

            //equals
            ans += Hub.LevelBuilder_s.Convert_FaceValueToString(fv_EQUALS);

            //right
            for (int i = 0; i < Right_Values.Count; i++)
            {
                if (Right_Values[i].Value == fv_INT)
                {
                    ans += Right_Values[i].IntValue.ToString();
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(Right_Values[i].Value);
                }
            }
        }

        /* Check If Equal Now */

        //if equal side counts
        if (Right_Values.Count == Left_Values.Count)
        {
            //loop indexes
            for (int i = 0; i < Right_Values.Count; i++)
            {
                //if no match
                if (Right_Values[i].Value != Left_Values[i].Value)
                {
                    return false;
                }
                //if both are fv_INT
                if (Right_Values[i].Value == fv_INT)
                {
                    //if IntValues don't match
                    if (Left_Values[i].IntValue != Right_Values[i].IntValue)
                    {
                        return false;
                    }
                }
            }

            /* Everything matches */

            return true;
        }

        //safety
        return false;
    }

    private bool Validate_With_5_Equals(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, fv_FACEVALUE val5, fv_FACEVALUE val6, ref string ans)
    {
        // the one non-equal has to be a fv_BLANK
        if (val1 == fv_BLANK || val2 == fv_BLANK || val3 == fv_BLANK || val4 == fv_BLANK || val5 == fv_BLANK || val6 == fv_BLANK)
        {
            //apply answer
            ans = "=====";

            return true;
        }

        return false;
    }

    private bool Validate_ThisVine(Dictionary<a_ADDRESS, GameObject> HoneySlotRefDict, Dictionary<va_VINEAXIS, Vine> VineRefDict, va_VINEAXIS axis)
    {
        /*
         This finds each value that is needed, with a correct offset and sends it off for validation to 
        an overloaded function that will validate 4 values or 6.
         */

        //usables
        fv_FACEVALUE val1;
        fv_FACEVALUE val2;
        fv_FACEVALUE val3;
        fv_FACEVALUE val4;
        fv_FACEVALUE val5;
        fv_FACEVALUE val6;

        string bulb_ans = string.Empty;

        switch (axis)
        {
            case va_1:
                {
                    //null check
                    if (HoneySlotRefDict[a_G].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_F].GetComponentInChildren<Piece>() == null)
                    {
                        return false;
                    }

                    val1 = HoneySlotRefDict[a_G].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_6);
                    val2 = HoneySlotRefDict[a_G].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_3);
                    val3 = HoneySlotRefDict[a_F].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_6);
                    val4 = HoneySlotRefDict[a_F].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_3);

                    //if bulb activated
                    if (VineRefDict[axis].IsBulbValidated)
                    {
                        DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                        return bulb_ans == VineRefDict[axis].bulb_answer;
                    }

                    return DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                }
            case va_2:
                {
                    //null check
                    if (HoneySlotRefDict[a_E].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_D].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_C].GetComponentInChildren<Piece>() == null)
                    {
                        return false;
                    }

                    val1 = HoneySlotRefDict[a_E].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_6);
                    val2 = HoneySlotRefDict[a_E].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_3);
                    val3 = HoneySlotRefDict[a_D].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_6);
                    val4 = HoneySlotRefDict[a_D].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_3);
                    val5 = HoneySlotRefDict[a_C].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_6);
                    val6 = HoneySlotRefDict[a_C].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_3);

                    //if bulb activated
                    if (VineRefDict[axis].IsBulbValidated)
                    {
                        DoesThisValidate(val1, val2, val3, val4, val5, val6, ref bulb_ans);
                        return bulb_ans == VineRefDict[axis].bulb_answer;
                    }

                    return DoesThisValidate(val1, val2, val3, val4, val5, val6, ref bulb_ans);
                }
            case va_3:
                {
                    //null check
                    if (HoneySlotRefDict[a_B].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_A].GetComponentInChildren<Piece>() == null)
                    {
                        return false;
                    }

                    val1 = HoneySlotRefDict[a_B].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_6);
                    val2 = HoneySlotRefDict[a_B].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_3);
                    val3 = HoneySlotRefDict[a_A].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_6);
                    val4 = HoneySlotRefDict[a_A].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_3);

                    //if bulb activated
                    if (VineRefDict[axis].IsBulbValidated)
                    {
                        DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                        return bulb_ans == VineRefDict[axis].bulb_answer;
                    }

                    return DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                }
            case va_4:
                {
                    //null check
                    if (HoneySlotRefDict[a_E].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_G].GetComponentInChildren<Piece>() == null)
                    {
                        return false;
                    }

                    val1 = HoneySlotRefDict[a_E].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_5);
                    val2 = HoneySlotRefDict[a_E].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_2);
                    val3 = HoneySlotRefDict[a_G].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_5);
                    val4 = HoneySlotRefDict[a_G].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_2);

                    //if bulb activated
                    if (VineRefDict[axis].IsBulbValidated)
                    {
                        DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                        return bulb_ans == VineRefDict[axis].bulb_answer;
                    }

                    return DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                }
            case va_5:
                {
                    //null check
                    if (HoneySlotRefDict[a_B].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_D].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_F].GetComponentInChildren<Piece>() == null)
                    {
                        return false;
                    }

                    val1 = HoneySlotRefDict[a_B].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_5);
                    val2 = HoneySlotRefDict[a_B].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_2);
                    val3 = HoneySlotRefDict[a_D].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_5);
                    val4 = HoneySlotRefDict[a_D].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_2);
                    val5 = HoneySlotRefDict[a_F].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_5);
                    val6 = HoneySlotRefDict[a_F].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_2);

                    //if bulb activated
                    if (VineRefDict[axis].IsBulbValidated)
                    {
                        DoesThisValidate(val1, val2, val3, val4, val5, val6, ref bulb_ans);
                        return bulb_ans == VineRefDict[axis].bulb_answer;
                    }

                    return DoesThisValidate(val1, val2, val3, val4, val5, val6, ref bulb_ans);
                }
            case va_6:
                {
                    //null check
                    if (HoneySlotRefDict[a_A].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_C].GetComponentInChildren<Piece>() == null)
                    {
                        return false;
                    }

                    val1 = HoneySlotRefDict[a_A].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_5);
                    val2 = HoneySlotRefDict[a_A].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_2);
                    val3 = HoneySlotRefDict[a_C].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_5);
                    val4 = HoneySlotRefDict[a_C].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_2);

                    //if bulb activated
                    if (VineRefDict[axis].IsBulbValidated)
                    {
                        DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                        return bulb_ans == VineRefDict[axis].bulb_answer;
                    }

                    return DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                }
            case va_7:
                {
                    //null check
                    if (HoneySlotRefDict[a_B].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_E].GetComponentInChildren<Piece>() == null)
                    {
                        return false;
                    }

                    val1 = HoneySlotRefDict[a_B].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_4);
                    val2 = HoneySlotRefDict[a_B].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_1);
                    val3 = HoneySlotRefDict[a_E].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_4);
                    val4 = HoneySlotRefDict[a_E].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_1);

                    //if bulb activated
                    if (VineRefDict[axis].IsBulbValidated)
                    {
                        DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                        return bulb_ans == VineRefDict[axis].bulb_answer;
                    }

                    return DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                }
            case va_8:
                {
                    //null check
                    if (HoneySlotRefDict[a_A].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_D].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_G].GetComponentInChildren<Piece>() == null)
                    {
                        return false;
                    }

                    val1 = HoneySlotRefDict[a_A].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_4);
                    val2 = HoneySlotRefDict[a_A].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_1);
                    val3 = HoneySlotRefDict[a_D].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_4);
                    val4 = HoneySlotRefDict[a_D].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_1);
                    val5 = HoneySlotRefDict[a_G].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_4);
                    val6 = HoneySlotRefDict[a_G].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_1);

                    //if bulb activated
                    if (VineRefDict[axis].IsBulbValidated)
                    {
                        DoesThisValidate(val1, val2, val3, val4, val5, val6, ref bulb_ans);
                        return bulb_ans == VineRefDict[axis].bulb_answer;
                    }

                    return DoesThisValidate(val1, val2, val3, val4, val5, val6, ref bulb_ans);
                }
            case va_9:
                {
                    //null check
                    if (HoneySlotRefDict[a_C].GetComponentInChildren<Piece>() == null || HoneySlotRefDict[a_F].GetComponentInChildren<Piece>() == null)
                    {
                        return false;
                    }

                    val1 = HoneySlotRefDict[a_C].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_4);
                    val2 = HoneySlotRefDict[a_C].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_1);
                    val3 = HoneySlotRefDict[a_F].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_4);
                    val4 = HoneySlotRefDict[a_F].GetComponentInChildren<Piece>().Get_FaceValue_WithOffset(t_1);

                    //if bulb activated
                    if (VineRefDict[axis].IsBulbValidated)
                    {
                        DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                        return bulb_ans == VineRefDict[axis].bulb_answer;
                    }

                    return DoesThisValidate(val1, val2, val3, val4, ref bulb_ans);
                }
            default:
                return false;
        }
    }

    public void Change_EqualSigns(Dictionary<a_ADDRESS, GameObject> HoneySlotRefDict, va_VINEAXIS axis, bool activation)
    {
        //usables
        Piece Piece_s;

        switch (axis)
        {
            case va_1:
                {
                    if (HoneySlotRefDict[a_G].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_G].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_6, activation);
                        Piece_s.Alter_EqualSign(t_3, activation);
                    }

                    if (HoneySlotRefDict[a_F].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_F].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_6, activation);
                        Piece_s.Alter_EqualSign(t_3, activation);
                    }

                    break;
                }
            case va_2:
                {
                    if (HoneySlotRefDict[a_E].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_E].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_6, activation);
                        Piece_s.Alter_EqualSign(t_3, activation);
                    }

                    if (HoneySlotRefDict[a_D].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_D].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_6, activation);
                        Piece_s.Alter_EqualSign(t_3, activation);
                    }

                    if (HoneySlotRefDict[a_C].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_C].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_6, activation);
                        Piece_s.Alter_EqualSign(t_3, activation);
                    }

                    break;
                }
            case va_3:
                {
                    if (HoneySlotRefDict[a_B].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_B].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_6, activation);
                        Piece_s.Alter_EqualSign(t_3, activation);
                    }

                    if (HoneySlotRefDict[a_A].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_A].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_6, activation);
                        Piece_s.Alter_EqualSign(t_3, activation);
                    }

                    break;
                }
            case va_4:
                {
                    if (HoneySlotRefDict[a_E].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_E].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_5, activation);
                        Piece_s.Alter_EqualSign(t_2, activation);
                    }

                    if (HoneySlotRefDict[a_G].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_G].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_5, activation);
                        Piece_s.Alter_EqualSign(t_2, activation);
                    }

                    break;
                }
            case va_5:
                {
                    if (HoneySlotRefDict[a_B].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_B].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_5, activation);
                        Piece_s.Alter_EqualSign(t_2, activation);
                    }

                    if (HoneySlotRefDict[a_D].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_D].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_5, activation);
                        Piece_s.Alter_EqualSign(t_2, activation);
                    }

                    if (HoneySlotRefDict[a_F].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_F].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_5, activation);
                        Piece_s.Alter_EqualSign(t_2, activation);
                    }

                    break;
                }
            case va_6:
                {
                    if (HoneySlotRefDict[a_A].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_A].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_5, activation);
                        Piece_s.Alter_EqualSign(t_2, activation);
                    }

                    if (HoneySlotRefDict[a_C].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_C].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_5, activation);
                        Piece_s.Alter_EqualSign(t_2, activation);
                    }

                    break;
                }
            case va_7:
                {
                    if (HoneySlotRefDict[a_B].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_B].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_4, activation);
                        Piece_s.Alter_EqualSign(t_1, activation);
                    }

                    if (HoneySlotRefDict[a_E].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_E].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_4, activation);
                        Piece_s.Alter_EqualSign(t_1, activation);
                    }

                    break;
                }
            case va_8:
                {
                    if (HoneySlotRefDict[a_A].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_A].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_4, activation);
                        Piece_s.Alter_EqualSign(t_1, activation);
                    }

                    if (HoneySlotRefDict[a_D].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_D].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_4, activation);
                        Piece_s.Alter_EqualSign(t_1, activation);
                    }

                    if (HoneySlotRefDict[a_G].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_G].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_4, activation);
                        Piece_s.Alter_EqualSign(t_1, activation);
                    }

                    break;
                }
            case va_9:
                {
                    if (HoneySlotRefDict[a_C].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_C].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_4, activation);
                        Piece_s.Alter_EqualSign(t_1, activation);
                    }

                    if (HoneySlotRefDict[a_F].GetComponentInChildren<Piece>() != null)
                    {
                        Piece_s = HoneySlotRefDict[a_F].GetComponentInChildren<Piece>();
                        Piece_s.Alter_EqualSign(t_4, activation);
                        Piece_s.Alter_EqualSign(t_1, activation);
                    }

                    break;
                }
            default:
                break;
        }
    }

    public string Get_BulbAnswer(int honeycomb_index, va_VINEAXIS vine_axis, ref List<List<Info_Slot>> hc_list)
    {
        string answer = "";

        //usables
        fv_FACEVALUE val1;
        fv_FACEVALUE val2;
        fv_FACEVALUE val3;
        fv_FACEVALUE val4;
        fv_FACEVALUE val5;
        fv_FACEVALUE val6;

        switch (vine_axis)
        {
            case va_1:
                val1 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_G)].fslot_6;
                val2 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_G)].fslot_3;
                val3 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_F)].fslot_6;
                val4 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_F)].fslot_3;
                DoesThisValidate(val1, val2, val3, val4, ref answer);
                break;
            case va_2:
                val1 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_E)].fslot_6;
                val2 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_E)].fslot_3;
                val3 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_D)].fslot_6;
                val4 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_D)].fslot_3;
                val5 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_C)].fslot_6;
                val6 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_C)].fslot_3;
                DoesThisValidate(val1, val2, val3, val4, val5, val6, ref answer);
                break;
            case va_3:
                val1 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_B)].fslot_6;
                val2 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_B)].fslot_3;
                val3 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_A)].fslot_6;
                val4 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_A)].fslot_3;
                DoesThisValidate(val1, val2, val3, val4, ref answer);
                break;
            case va_4:
                val1 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_E)].fslot_5;
                val2 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_E)].fslot_2;
                val3 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_G)].fslot_5;
                val4 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_G)].fslot_2;
                DoesThisValidate(val1, val2, val3, val4, ref answer);
                break;
            case va_5:
                val1 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_B)].fslot_5;
                val2 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_B)].fslot_2;
                val3 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_D)].fslot_5;
                val4 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_D)].fslot_2;
                val5 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_F)].fslot_5;
                val6 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_F)].fslot_2;
                DoesThisValidate(val1, val2, val3, val4, val5, val6, ref answer);
                break;
            case va_6:
                val1 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_A)].fslot_5;
                val2 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_A)].fslot_2;
                val3 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_C)].fslot_5;
                val4 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_C)].fslot_2;
                DoesThisValidate(val1, val2, val3, val4, ref answer);
                break;
            case va_7:
                val1 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_B)].fslot_4;
                val2 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_B)].fslot_1;
                val3 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_E)].fslot_4;
                val4 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_E)].fslot_1;
                DoesThisValidate(val1, val2, val3, val4, ref answer);
                break;
            case va_8:
                val1 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_A)].fslot_4;
                val2 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_A)].fslot_1;
                val3 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_D)].fslot_4;
                val4 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_D)].fslot_1;
                val5 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_G)].fslot_4;
                val6 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_G)].fslot_1;
                DoesThisValidate(val1, val2, val3, val4, val5, val6, ref answer);
                break;
            case va_9:
                val1 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_C)].fslot_4;
                val2 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_C)].fslot_1;
                val3 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_F)].fslot_4;
                val4 = hc_list[honeycomb_index][Hub.BlockHouse_s.ConvertToIndex(a_F)].fslot_1;
                DoesThisValidate(val1, val2, val3, val4, ref answer);
                break;
            default:
                break;
        }

        return answer;
    }

    //Utilities

    private void Scrub_TheseBlocks_NoEquals(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, ref string ans)
    {
        //put values into list
        List<VineBlock> AllValues = new List<VineBlock>();
        AllValues.Add(new VineBlock(val1, 0));
        AllValues.Add(new VineBlock(val2, 0));
        AllValues.Add(new VineBlock(val3, 0));
        AllValues.Add(new VineBlock(val4, 0));

        Scrub_PossiblyEliminate_Spaces(ref AllValues);
        Scrub_PossiblyEliminate_Zeros(ref AllValues);
        Scrub_PossiblyMerge_Ints(ref AllValues);
        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref AllValues);
        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref AllValues);
        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref AllValues);

        /* SCRUBBING DONE */

        /* Apply Answer */
        {
            for (int i = 0; i < AllValues.Count; i++)
            {
                if (AllValues[i].Value == fv_INT)
                {
                    ans += AllValues[i].IntValue.ToString();
                }
                else if (AllValues[i].Value == fv_BLANK)
                {
                    //skip
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(AllValues[i].Value);
                }
            }
        }
    }

    private void Scrub_TheseBlocks_NoEquals(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, fv_FACEVALUE val5, fv_FACEVALUE val6, ref string ans)
    {
        //put values into list
        List<VineBlock> AllValues = new List<VineBlock>();
        AllValues.Add(new VineBlock(val1, 0));
        AllValues.Add(new VineBlock(val2, 0));
        AllValues.Add(new VineBlock(val3, 0));
        AllValues.Add(new VineBlock(val4, 0));
        AllValues.Add(new VineBlock(val5, 0));
        AllValues.Add(new VineBlock(val6, 0));

        Scrub_PossiblyEliminate_Spaces(ref AllValues);
        Scrub_PossiblyEliminate_Zeros(ref AllValues);
        Scrub_PossiblyMerge_Ints(ref AllValues);
        Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref AllValues);
        Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref AllValues);
        Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref AllValues);

        /* SCRUBBING DONE */

        /* Apply Answer */
        {
            for (int i = 0; i < AllValues.Count; i++)
            {
                if (AllValues[i].Value == fv_INT)
                {
                    ans += AllValues[i].IntValue.ToString();
                }
                else if (AllValues[i].Value == fv_BLANK)
                {
                    //skip
                }
                else
                {
                    ans += Hub.LevelBuilder_s.Convert_FaceValueToString(AllValues[i].Value);
                }
            }
        }
    }

    private void Scrub_PossiblyMerge_Ints(ref List<VineBlock> VBlocks)
    {
        int IntValue = 1;
        int DigitTracker = 0;

        //loop Values
        for (int i = 0; i < VBlocks.Count; i++)
        {
            //if digit
            if (IsDigit(VBlocks[i].Value))
            {

                //first time
                if (DigitTracker == 0)
                {
                    //set to this first digit
                    IntValue = Get_IntFromFaceValue(VBlocks[i].Value);

                    //increment
                    DigitTracker++;
                }
                //not first time
                else
                {
                    //IntValue = 10 * previous + new digit
                    IntValue = (10 * IntValue) + Get_IntFromFaceValue(VBlocks[i].Value);

                    //increment
                    DigitTracker++;
                }
            }
            //if not digit and there's stored int
            else if (DigitTracker != 0)
            {
                //get prev index
                int PrevIndex = i - 1;

                //remove all ints that make up IntValue
                while (DigitTracker > 0)
                {
                    //remove
                    VBlocks.RemoveAt(PrevIndex);

                    //decrement index and DigitTracker
                    PrevIndex--;
                    DigitTracker--;

                    //decrement current runner to stay on track
                    i--;
                }

                //PrevIndex and current runner needs to reverse it's last decrement above to be right
                PrevIndex++;
                i++;

                //add IntValue fv_INT block
                VBlocks.Insert(PrevIndex, new VineBlock(fv_INT, IntValue));
            }
        }

        /* After looping entire List */

        //if it ended on a digit that still needs to be packaged
        if (DigitTracker != 0)
        {
            //get End index
            int EndIndex = VBlocks.Count - 1;

            //remove all ints that make up IntValue
            while (DigitTracker > 0)
            {
                //remove
                VBlocks.RemoveAt(EndIndex);

                //decrement index and DigitTracker
                EndIndex--;
                DigitTracker--;
            }

            //CopiedIndex needs to reverse it's last decrement above to be right
            EndIndex++;

            //add IntValue fv_INT block
            VBlocks.Insert(EndIndex, new VineBlock(fv_INT, IntValue));
        }
    }

    private void Scrub_PossiblyEliminate_MinusAndPlusSigns_NoMathx5(ref List<VineBlock> VBlocks)
    {
        /* 
        This needs to be done 5 times to accomodate +-+-+9.  If not, not enough +/- are converted into the fv_INT
        */

        //do atleast five times
        for (int times = 0; times < 5; times++)
        {
            //loop Values by index
            for (int i = 0; i < VBlocks.Count; i++)
            {
                //if plus
                if (VBlocks[i].Value == fv_ADD)
                {
                    //safety -check if last index
                    if (i + 1 == VBlocks.Count)
                    {
                        //it stays as symbol
                        continue;
                    }

                    //digit on right
                    if (IsDigit(VBlocks[i + 1].Value))
                    {
                        //safety -check if first index
                        if (i - 1 == -1)
                        {
                            //it is deleted, just a positive indicator
                            VBlocks.RemoveAt(i);

                            //need to deincrement index because of removal
                            i--;

                            continue;
                        }
                        //check prior index, if  not digit
                        if (!IsDigit(VBlocks[i - 1].Value))
                        {
                            //it is deleted, just a positive indicator
                            VBlocks.RemoveAt(i);

                            //need to deincrement index because of removal
                            i--;

                            continue;
                        }
                        //prior digit
                        else
                        {
                            /* WONT DO MATH YET */
                        }
                    }
                }

                //if minus
                if (VBlocks[i].Value == fv_SUB)
                {
                    //safety -check if first index
                    if (i == 0)
                    {
                        //safety -check if at end
                        if (i + 1 >= VBlocks.Count)
                        {
                            //just a minus
                            continue;
                        }
                        //to right, int
                        if (VBlocks[i + 1].Value == fv_INT)
                        {
                            //store OldInt
                            int OldInt = VBlocks[i + 1].IntValue;

                            //remove old fv_INT block
                            VBlocks.RemoveAt(i + 1);

                            //remove minus block
                            VBlocks.RemoveAt(i);

                            //multiply by -1
                            OldInt = OldInt * -1;

                            //build new VBlock
                            VineBlock NewVineBlock = new VineBlock(fv_INT, OldInt);

                            //add new VBlock
                            VBlocks.Insert(i, NewVineBlock);

                            //removed two, added one
                            //no need to change i
                        }
                    }
                    //not at first index
                    else
                    {
                        //if digit on left
                        if (VBlocks[i - 1].Value == fv_INT)
                        {
                            //safety -check if at end
                            if (i + 1 >= VBlocks.Count)
                            {
                                //stays symbol
                                continue;
                            }

                            // digit - digit
                            if (VBlocks[i + 1].Value == fv_INT)
                            {
                                //do nothing, no math yet
                            }
                        }
                        //no fv_INT on left
                        else
                        {
                            //safety -check if at end
                            if (i + 1 >= VBlocks.Count)
                            {
                                //just a minus
                                continue;
                            }
                            //to right, int
                            if (VBlocks[i + 1].Value == fv_INT)
                            {
                                //store OldInt
                                int OldInt = VBlocks[i + 1].IntValue;

                                //remove old fv_INT block
                                VBlocks.RemoveAt(i + 1);

                                //remove minus block
                                VBlocks.RemoveAt(i);

                                //multiply by -1
                                OldInt = OldInt * -1;

                                //build new VBlock
                                VineBlock NewVineBlock = new VineBlock(fv_INT, OldInt);

                                //add new VBlock
                                VBlocks.Insert(i, NewVineBlock);

                                //removed two, added one
                                //no need to change i

                            }
                        }
                    }
                }
            }
        }
    }

    private void Scrub_PossiblyEliminate_MinusSigns_OnlyMath(ref List<VineBlock> VBlocks)
    {
        //loop Values by index
        for (int i = 0; i < VBlocks.Count; i++)
        {
            //if minus
            if (VBlocks[i].Value == fv_SUB)
            {
                //safety -check if first index
                if (i == 0)
                {
                    //safety -check if at end
                    if (i + 1 >= VBlocks.Count)
                    {
                        //just a minus
                        continue;
                    }
                    if (VBlocks[i + 1].Value == fv_INT)
                    {
                        //store OldInt
                        int OldInt = VBlocks[i + 1].IntValue;

                        //remove old fv_INT block
                        VBlocks.RemoveAt(i + 1);

                        //remove minus block
                        VBlocks.RemoveAt(i);

                        //multiply by -1
                        OldInt = OldInt * -1;

                        //build new VBlock
                        VineBlock NewVineBlock = new VineBlock(fv_INT, OldInt);

                        //add new VBlock
                        VBlocks.Insert(i, NewVineBlock);

                        //removed two, added one
                        //no need to change i

                    }
                }
                //not at first index
                else
                {
                    //if digit on left
                    if (VBlocks[i - 1].Value == fv_INT)
                    {
                        //safety -check if at end
                        if (i + 1 >= VBlocks.Count)
                        {
                            //stays symbol
                            continue;
                        }

                        // digit - digit
                        if (VBlocks[i + 1].Value == fv_INT)
                        {
                            //get new int
                            int NewInt = (VBlocks[i - 1].IntValue) - (VBlocks[i + 1].IntValue);

                            //remove right block
                            VBlocks.RemoveAt(i + 1);

                            //remove minus block
                            VBlocks.RemoveAt(i);

                            //remove left block
                            VBlocks.RemoveAt(i - 1);

                            //build new VBlock
                            VineBlock NewVineBlock = new VineBlock(fv_INT, NewInt);

                            //removed 3, added 1, so need to decrement i
                            i--;

                            //safety - check to see if new index is okay
                            if (i < VBlocks.Count)
                            {
                                //add new VBlock
                                VBlocks.Insert(i, NewVineBlock);
                            }
                            // index unsafe, add to end
                            else
                            {
                                //add new VBlock
                                VBlocks.Add(NewVineBlock);
                            }
                        }
                    }
                    //no fv_INT on left
                    else
                    {
                        //safety -check if at end
                        if (i + 1 >= VBlocks.Count)
                        {
                            //just a minus
                            continue;
                        }
                        if (VBlocks[i + 1].Value == fv_INT)
                        {
                            //store OldInt
                            int OldInt = VBlocks[i + 1].IntValue;

                            //remove old fv_INT block
                            VBlocks.RemoveAt(i + 1);

                            //remove minus block
                            VBlocks.RemoveAt(i);

                            //multiply by -1
                            OldInt = OldInt * -1;

                            //build new VBlock
                            VineBlock NewVineBlock = new VineBlock(fv_INT, OldInt);

                            //@@@@ new saftery here


                            //add new VBlock
                            VBlocks.Insert(i, NewVineBlock);

                            //removed two, added one
                            //no need to change i

                        }
                    }


                }


            }
        }
    }

    private void Scrub_PossiblyEliminate_PlusSigns_OnlyMath(ref List<VineBlock> VBlocks)
    {
        //loop Values by index
        for (int i = 0; i < VBlocks.Count; i++)
        {
            //if plus
            if (VBlocks[i].Value == fv_ADD)
            {
                //safety -check if last index
                if (i + 1 == VBlocks.Count)
                {
                    //it stays as symbol
                    continue;
                }

                //digit on right
                if (IsDigit(VBlocks[i + 1].Value))
                {
                    //check prior index, if digit
                    if (IsDigit(VBlocks[i - 1].Value))
                    {
                        //get new int
                        int NewInt = (VBlocks[i - 1].IntValue) + (VBlocks[i + 1].IntValue);

                        //remove right block
                        VBlocks.RemoveAt(i + 1);

                        //remove minus block
                        VBlocks.RemoveAt(i);

                        //remove left block
                        VBlocks.RemoveAt(i - 1);

                        //build new VBlock
                        VineBlock NewVineBlock = new VineBlock(fv_INT, NewInt);

                        //removed 3, added 1, so need to decrement i
                        i--;

                        //safety - check to see if new index is okay
                        if (i < VBlocks.Count)
                        {
                            //add new VBlock
                            VBlocks.Insert(i, NewVineBlock);
                        }
                        // index unsafe, add to end
                        else
                        {
                            //add new VBlock
                            VBlocks.Add(NewVineBlock);
                        }

                    }

                }
            }
        }
    }

    private void Scrub_PossiblyEliminate_Zeros(ref List<VineBlock> VBlocks)
    {
        //loop Values by index
        for (int i = 0; i < VBlocks.Count; i++)
        {
            //if zero
            if (VBlocks[i].Value == fv_0)
            {
                //safety -check if last index
                if (i + 1 == VBlocks.Count)
                {
                    continue;
                }

                //digit on right
                if (IsDigit(VBlocks[i + 1].Value))
                {
                    //safety -check if first index
                    if (i - 1 == -1)
                    {
                        //delete the zero
                        VBlocks.RemoveAt(i);

                        //need to deincrement index because of removal
                        i--;

                        continue;
                    }
                    //check prior index, if not digit
                    if (!IsDigit(VBlocks[i - 1].Value))
                    {
                        //delete the zero
                        VBlocks.RemoveAt(i);

                        //need to deincrement index because of removal
                        i--;
                    }
                }
            }
        }
    }

    private void Scrub_PossiblyEliminate_Spaces(ref List<VineBlock> VBlocks)
    {
        //loop Values by index
        for (int i = 0; i < VBlocks.Count; i++)
        {
            //if zero
            if (VBlocks[i].Value == fv_BLANK)
            {
                //delete the zero
                VBlocks.RemoveAt(i);

                //need to deincrement index because of removal
                i--;
            }
        }
    }

    private int Get_AmountOfEqualsValues(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4)
    {
        int EqualsAmount = 0;

        if (val1 == fv_EQUALS) { EqualsAmount++; }
        if (val2 == fv_EQUALS) { EqualsAmount++; }
        if (val3 == fv_EQUALS) { EqualsAmount++; }
        if (val4 == fv_EQUALS) { EqualsAmount++; }

        return EqualsAmount;
    }

    private int Get_AmountOfEqualsValues(fv_FACEVALUE val1, fv_FACEVALUE val2, fv_FACEVALUE val3, fv_FACEVALUE val4, fv_FACEVALUE val5, fv_FACEVALUE val6)
    {
        int EqualsAmount = 0;

        if (val1 == fv_EQUALS) { EqualsAmount++; }
        if (val2 == fv_EQUALS) { EqualsAmount++; }
        if (val3 == fv_EQUALS) { EqualsAmount++; }
        if (val4 == fv_EQUALS) { EqualsAmount++; }
        if (val5 == fv_EQUALS) { EqualsAmount++; }
        if (val6 == fv_EQUALS) { EqualsAmount++; }

        return EqualsAmount;
    }

    public int Get_IntFromFaceValue(fv_FACEVALUE val)
    {
        return (int)val - 1;
    }

    public fv_FACEVALUE Get_FaceValue(int val)
    {
        return (fv_FACEVALUE)(val + 1);
    }

    private bool IsDigit(fv_FACEVALUE val)
    {
        switch (val)
        {
            case fv_0:
                return true;
            case fv_1:
                return true;
            case fv_2:
                return true;
            case fv_3:
                return true;
            case fv_4:
                return true;
            case fv_5:
                return true;
            case fv_6:
                return true;
            case fv_7:
                return true;
            case fv_8:
                return true;
            case fv_9:
                return true;
            case fv_INT:
                return true;
            default:
                return false;
        }
    }

    public void Run_SpecificTestValidate()
    {
        fv_FACEVALUE val1 = fv_ADD;
        fv_FACEVALUE val2 = fv_ADD;
        fv_FACEVALUE val3 = fv_SUB;
        fv_FACEVALUE val4 = fv_ADD;
        fv_FACEVALUE val5 = fv_SUB;
        fv_FACEVALUE val6 = fv_6;

        Debug.Log("++++++++++++++++++");

        string ans = string.Empty;

        bool result = DoesThisValidate(val1, val2, val3, val4, val5, val6, ref ans);
        //bool result = DoesThisValidate(val1, val2, val3, val4, ref ans);


        Debug.Log("Test validation: " + result);
        Debug.Log("Test answer: " + ans);
        Debug.Log("++++++++++++++++++");


    }
}