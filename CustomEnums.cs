using UnityEngine;

public enum gse_GAMESTATEENUM
{
    //game states
    gse_NONE, gse_TRANS, gse_TITLE, gse_INNER, gse_OUTER, gse_PAUSED, gse_ENDGAME, gse_HONEYSHELF
};

public enum a_ADDRESS
{
    /* the order is important here.  PuzzleBuilder uses the order */

    //none
    NONE = 0,

    //honeycomb address
    //a_A, a_B, a_C, a_D, a_E, a_F, a_G

    //worldgrid address
    a_A, a_AA, a_AB, a_AC, a_AD, a_AE, a_AF, a_AG, // 1-8
    a_B, a_BA, a_BB, a_BC, a_BD, a_BE, a_BF, a_BG, // 9-16
    a_C, a_CA, a_CB, a_CC, a_CD, a_CE, a_CF, a_CG, // 17-24
    a_D, a_DA, a_DB, a_DC, a_DD, a_DE, a_DF, a_DG, // 25-32
    a_E, a_EA, a_EB, a_EC, a_ED, a_EE, a_EF, a_EG, // 33-40
    a_F, a_FA, a_FB, a_FC, a_FD, a_FE, a_FF, a_FG, // 41-48
    a_G, a_GA, a_GB, a_GC, a_GD, a_GE, a_GF, a_GG, // 49-56

    //honeyshelf address
    a_HSA, a_HSB, a_HSC, a_HSD, a_HSE, a_HSF, a_HSG, //57-63

    //beebox A address
    a_BBA0,                                 //64
    a_BBA1, a_BBA2, a_BBA3, a_BBA4,         //65-68
    a_BBA5, a_BBA6, a_BBA7, a_BBA8,         //69-72
    a_BBA9, a_BBA10, a_BBA11, a_BBA12,      //73-76
    a_BBA13, a_BBA14, a_BBA15, a_BBA16,     //77-80

    //beebox B address
    a_BBB0,                                 //81
    a_BBB1, a_BBB2, a_BBB3, a_BBB4,         //82-85
    a_BBB5, a_BBB6, a_BBB7, a_BBB8,         //86-89
    a_BBB9, a_BBB10, a_BBB11, a_BBB12,      //90-93
    a_BBB13, a_BBB14, a_BBB15, a_BBB16,     //94-97

    //beebox C address
    a_BBC0,                                 //98
    a_BBC1, a_BBC2, a_BBC3, a_BBC4,         //99-102
    a_BBC5, a_BBC6, a_BBC7, a_BBC8,         //103-106
    a_BBC9, a_BBC10, a_BBC11, a_BBC12,      //107-110
    a_BBC13, a_BBC14, a_BBC15, a_BBC16,     //111-114

    //beebox D address
    a_BBD0,                                 //115
    a_BBD1, a_BBD2, a_BBD3, a_BBD4,         //116-119
    a_BBD5, a_BBD6, a_BBD7, a_BBD8,         //120-123
    a_BBD9, a_BBD10, a_BBD11, a_BBD12,      //124-127
    a_BBD13, a_BBD14, a_BBD15, a_BBD16      //128-131
}

public enum fv_FACEVALUE
{
    fv_NULL = 0,
    fv_0, fv_1, fv_2, fv_3, fv_4, fv_5, fv_6, fv_7, fv_8, fv_9,     // 1-10
    fv_ADD, fv_SUB, fv_BLANK, fv_EQUALS,                            // 11-14
    fv_INT
}

public enum t_TRI
{
    t_1, t_2, t_3, t_4, t_5, t_6
}

public enum c_ACTCOLOR
{
    c_CENTER, c_PETALS, c_VINE, c_HC_WIRE, c_HC_BACK
}

public enum bt_BUTTONTYPE
{
    bt_NONE, bt_EXIT, bt_RESET, bt_NEW, bt_STICKER, 
    bt_PAUSE, bt_BACK, bt_SAVE, bt_SHELF,
    bt_BBA, bt_BBB, bt_BBC, bt_BBD
}

public enum w_WAVETYPE
{
    w_SIN, w_COS, w_TAN, w_PINGPONG
}


public class CustomEnums : MonoBehaviour
{

}



