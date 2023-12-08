using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using GlobalsNS;
using MenuCustomNS;


namespace MenuCustomNS
{
    public class NationalTeams
    {
        private int activeTeams = 0;
        /*team name 
         * gk 
         * shot 
         * index 
         * coinsNeeded 
         * fans/flag color number
         * thirt color 
         * shorts color
         * sock color
         * thirt number color
         * hair
         * flare color
         */
        private string[][] teams = new string[][] {
             new string[] {
                "Bosnia",
                "60",
                "58",
                "0",
                "0",
                "blue|blue|bosnia",
                 "bluewithwhiteshirt",
                "darkbluewhitestripes",
                "darkbluewhitestripessocks",
                "skin_white_1",
                "hair_19",
                "blue",
                "Klaus:Chile:60:58:U:0:f0_s2_b0_t0_hblack4",
                "shoeblue|shoebluewhite",
                "glovebluewhite"},
             new string[] {
                "Estonia", 
                "43", 
                "42", 
                "0", 
                "0",
                "blue|blue|estonia",
                 "blueshirt", "black", "white",
                "skin_white_1", "hair_16", "blue",
                "Klaus:Chile:43:42:U:0:f4_s2_b2_t9_hblack2",
                 "shoebluewhite|shoeblackred",
                 "gloveblacblue|glovebluewhite"},
             new string[] {
                 "Georgia", 
                 "51", 
                 "53", 
                 "0", 
                 "0",
                 "red|red|georgia",
                 "whiteshirt", "redwhitestripesshort", "white",
                "skin_white_1", "hair_16", "red",
                 "Klaus:Chile:51:53:U:0:f2_s1_b2_t8_hblack8",
                 "shoeblackred|shoeredwhite",
                 "gloveredwhite"},
             new string[] {
                 "Iraq", 
                 "51", 
                 "50", 
                 "0", 
                 "0",
                "green|darkgreen|iraq",
                 "whitegreenstripeshirt", "whiteshort", "white",
                "skin_white_1", "hair_4", "green",
                 "Klaus:Chile:85:65:U:0:f3_s2_b0_t0_hblack7",
                 "shoegreen",
                 "glovegreenwhite"},
             new string[] {
                 "Latvia", 
                 "43", 
                 "41", 
                 "0", 
                 "0",
                "red|red|latvia",
                 "darkredshirt", "whiteredstripesshort", "darkredgreystripesocks",
                "skin_white_1", "hair_16", "red",
                 "Klaus:Chile:85:65:U:0:f4_s2_b0_t0_hblonde4",
                 "shoewhite|shoeredwhite",
                 "gloveredwhite"},
             new string[] {
                 "Lithuania", 
                 "43", 
                 "44", 
                 "0", 
                 "0",
                 "yellow|darkgreen|lithuania",
                 "yellowwithgreenshirt", "greenshorts", "yellowsocks",
                "skin_white_1", "hair_19", "yellow",
                 "Klaus:Chile:85:65:U:0:f5_s2_b0_t0_hblack3",
                 "shoeyellow|shoegreen",
                 "glovegreenwhite|gloveyellow|gloveyellowwhite"},
             new string[] {
                 "Montenegro", 
                 "51", 
                 "49", 
                 "0", 
                 "0",
                 "red|red|montenegro",
                 "redshirt", "redshort", "sock_red",
                "skin_white_1", "hair_4", "red",
                 "Klaus:Chile:85:65:U:0:f6_s3_b2_t10_hblack8",
                 "shoeredwhite|shoeblackred",
                 "gloveredwhite"},
             new string[] {
                 "Morocco", 
                 "67", 
                 "65", 
                 "0", 
                 "0",
                 "red|red|morocco",
                "redwithwhiteshirt", "greenwhitestripesshort", "redwhitesocks",
                "skin_white_1", "hair_8", "red",
                 "Klaus:Chile:85:65:U:0:f7_s4_b0_t0_hblack4",
                 "shoegreen|shoeredwhite",
                 "glovegreenwhite"},
             new string[] {
                 "Serbia", 
                 "63", 
                 "67", 
                 "0", 
                 "0",
                 "red|red|serbia",
                "redwithwhiteshirt", "redshort", "sock_red",
                "skin_white_1", "hair_8", "red",
                 "Klaus:Chile:85:65:U:0:f8_s3_b0_t0_hblack5",
                 "shoered|shoeredwhite",
                 "gloveredwhite"},
             new string[] {
                 "Slovakia", 
                 "63", 
                 "61", 
                 "0", 
                 "0",
                 "blue|blue|slovakia",
                "whiteshirt", "whiteshort", "whitebluestripesocks",
                "skin_white_1", "hair_8", "blue",
                 "Klaus:Chile:85:65:U:0:f9_s1_b0_t0_hblack10",
                 "shoebluewhite",
                 "glovebluewhite"},
             new string[] {
                 "Slovenia", 
                 "58", 
                 "53", 
                 "0", 
                 "0",
                 "blue|blue|slovenia",
                "bluewhireshirt", "whiteshort", "whiteblueblackstripessocks",
                "skin_white_1", "hair_8", "blue",
                 "Klaus:Chile:85:65:U:0:f0_s1_b0_t0_hblack9",
                 "shoeblue",
                 "glovebluewhite"},
             new string[] {
                 "Tunisia", 
                 "66", 
                 "65", 
                 "0", 
                 "0",
                 "red|red|tunisia",
                "whiteredshirt", "redshort", "white",
                "skin_white_1", "hair_19", "red",
                 "Klaus:Chile:85:65:U:0:f1_s1_b2_t7_hblack3",
                 "shoeredwhite|shoered",
                 "gloveredwhite"},
             new string[] {
                 "Venezuela", 
                 "65", 
                 "66", 
                 "0", 
                 "0",
                "claret|claret|venezuela",
                "darkredshirt", "darkredshort", "darkredgreystripesocks",
                "skin_white_1", "hair_19", "red",
                 "Klaus:Chile:85:65:U:0:f2_s1_b0_t0_hblack8",
                 "shoeredwhite",
                 "gloveredwhite"},
            new string[] {
                "Argentina", 
                "84", 
                "88", 
                "0", 
                "7120",
                //"lightblue|lightblue|argentina",
                "blue|blue|argentina",
                "lightbluewhitestripesshirt", "whiteblackstripeshort", "lightbluewhirestripessocks",
                "skin_white_1", "hair_19", "blue",
                "Klaus:Chile:85:65:U:0:f3_s1_b0_t0_hblack8",
                "shoeblue",
                "glovelightbluewhite|gloveblueblue"},
            new string[] {
                "Algeria", 
                "65", 
                "62", 
                "1", 
                "0",
                "green|darkgreen|algeria",
                "whitewithgreenshirt1", "whitewithgreenshort", "whitegreensocks",
                "skin_black_8", "hair_4", "green",
                "Klaus:Chile:85:65:U:0:f4_s1_b0_t0_hblack3",
                "shoegreen",
                "glovegreenwhite"},
            new string[] {
                "Brazil", 
                "90", 
                "91", 
                "2", 
                "9000",
                "yellow|yellowblue|brazil",
                "yellowshirt1", 
                "darkblueshort11", 
                "white", 
                "skin_black_8", 
                "hair_5", 
                "yellow",
                "Klaus:Chile:85:65:U:0:f5_s4_b0_t0_hblack10",
                "shoebluewhite|shoeyellow",
                "gloveyellowwhite|gloveyellow"},
            new string[] {
                "Chile", 
                "83", 
                "85", 
                "3", 
                "3000",
                "red|red|chile",
                "redwhitearmshirt", "darkblueshort11", "whitebluestripesocks", "skin_white_2", "hair_6", "red",
                "Klaus:Chile:85:65:U:0:f6_s4_b0_t0_hblack3",
                "shoebluewhite|shoeredwhite",
                "gloveredwhite|glovebluered|glovebluewhite"},
            new string[] {
                "Colombia", 
                "84", 
                "86", 
                "4", 
                "5800",
                "yellow|yellowblue|colombia",
                "yellowwithstripesshirt", "darkblueshort1", "redyellowstripessocks", "skin_white_3", "hair_7", "yellow",
                "Klaus:Chile:85:65:U:0:f7_s1_b0_t0_hblack4",
                "shoeredwhite|shoeblackred",
                "gloveyellow|gloveyellowwhite"},
            new string[] {
                "Austria", 
                "68", 
                "64", 
                "5", 
                "840",
                "red|red|austria",
                "redshirt", "whiteshort", "sock_red", "skin_white_4", "hair_8", "red",
                "Klaus:Chile:85:65:U:0:f0_s2_b2_t4_hred3",
                "shoeblackred|shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "Denmark", 
                "75",
                "69", 
                "6",
                "4810",
                "red|red|denmark",
                "redshirt",
                "whiteredstripeshort",
                "redwhitestripesocks1",
                "skin_white_5",
                "hair_1",
                "red",
                "Klaus:Chile:85:65:U:0:f1_s2_b0_t0_hblonde6",
                "shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "Germany", 
                "93", 
                "87", 
                "7", 
                "9210",
                "white|black|germany",
                "whiteblackstripeshorshirt", 
                "blackwhitestripesshort", 
                "whiteblacstripessocks", 
                "skin_white_6", 
                "hair_8", 
                "white",
                "Klaus:Chile:85:65:U:0:f9_s1_b0_t0_hblack7",
                "shoeblue|shoeblack",
                "globewhiteblack|gloveblackwhite"},
            new string[] {
                "Greece", 
                "59", 
                "65", 
                "8", 
                "0",
                "blue|blue|greece",
                "bluewithwhiteshirt", 
                "darkbluewhitestripes", 
                "whitebluestripesocks", 
                "skin_white_7", 
                "hair_9", 
                "blue",
                "Klaus:Chile:85:65:U:0:f0_s1_b2_t2_hblack6",
                "shoewhiteblack",
                "glovebluewhite"},
            new string[] {
                "France", 
                "95", 
                "95", 
                "9", 
                "9500",
                "blue|blue|france",
                "darkbluedarkbluestripesshirt", 
                "darkblueshort1", 
                "sock_red", 
                "skin_white_8", 
                "hair_10", 
                "blue",
                "Klaus:Chile:85:65:U:0:f1_s1_b0_t0_hblack5",
                "shoeredwhite|shoeblackred",
                "gloveblacblue|glovebluewhite"},
            new string[] {
                "Finland", 
                "49",
                "56", 
                "10",
                "0",
                "blue|blue|finland",
                "whiteblestripevertshirt", "whiteshort", "white", "skin_white_9", "hair_22", "white",
                "Klaus:Chile:85:65:U:0:f2_s2_b0_t0_hblonde3",
                "shoebluewhite|shoeblue",
                "glovebluewhite"},
            new string[] {
                "Hungary", 
                "61", 
                "58", 
                "11", 
                "5",
                "red|red|hungary",
                "redwithwhiteshirt", "whiteredstripeshort", "greenwhitestripessocks", "skin_white_10", "hair_11", "red",
                "Klaus:Chile:85:65:U:0:f3_s2_b0_t0_hblack4",
                "shoegreen",
                "glovegreenwhite|gloveredwhite"},
            new string[] {
                "Iceland", 
                "65", 
                "63", 
                "12", 
                "3410",
                "blue|blue|iceland",
                "blueshirt", "darkblueshort11", "darkblue", "skin_white_11", "hair_24", "blue",
                "Klaus:Chile:85:65:U:0:f4_s2_b2_t9_hblonde5",
                "shoebluewhite",
                "glovebluewhite"},
            new string[] {
                "Italy", 
                "83", 
                "82", 
                "13", 
                "8510",
                "blue|blue|italy",
                "blueshirt", "whitebluestripeshort", "darkblue", "skin_white_12", "hair_12", "blue",
                "Klaus:Chile:85:65:U:0:f5_s1_b0_t0_hblack11",
                "shoeblue|shoebluewhite",
                "glovebluewhite"},
            new string[] {
                "Netherlands", 
                "80", 
                "83", 
                "14", 
                "7232",
                //"orange|orange|netherlands",
                "orange|yellowblue|netherlands",
                "orangeblackstripesshirt", "whiteshort", "orangeblackstripesocks", "skin_white_13", "hair_25", 
                "yellow",
                "Klaus:Chile:85:65:U:0:f6_s3_b2_t10_hblonde4",
                "shoeorange",
                "gloveblackorange"},
            new string[] {
                "Poland", 
                "71", 
                "81", 
                "15", 
                "0",
                "red|poland|poland",
                "whiteredshirt", "redshort", "white", "skin_white_16", "hair_19", "red",
                "Klaus:Chile:85:65:U:0:f7_s1_b0_t0_hblack10",
                "shoeredwhite|shoered",
                "gloveredwhite"},
            new string[] {
                "Belgium", 
                "85", 
                "87",
                "16", 
                "6500",
                "red|red|belgium",
                "redblackstripesshirt", "redblackstripesshort", "redblackstripessocks", "skin_white_15", "hair_19", "red",
                "Klaus:Chile:85:65:U:0:f8_s3_b0_t0_hblack11",
                "shoeblackred|shoered",
                "gloveredwhite|gloveredblack"},
            new string[] {
                "Portugal", 
                "85", 
                "87", 
                "17", 
                "5542",
                "red|red|portugal",
                "darkredshirt", "greenshorts", "redblackstripessocks", "skin_white_16", "hair_15", "red",
                "Klaus:Chile:85:65:U:0:f9_s1_b0_t0_hblack10",
                "shoeblackred|shoegreen",
                "gloveredblack|gloveredwhite|gloveblackgreen"},
            new string[] {
                "Croatia", 
                "85", 
                "87", 
                "18", 
                "7500",
                "red|red|croatia",
                "croatiashirt", "whiteblueshort", "white", "skin_white_17", "hair_16", "red",
                "Klaus:Chile:85:65:U:0:f0_s1_b2_t2_hblack4",
                "shoeredwhite|shoered",
                "gloveredwhite"},
            new string[] {
                "Spain", 
                "85", 
                "85", 
                "19", 
                "9995",
                "red|red|spain",
                "darkredwithyellowshirt", "darkblueyellowstripeshort", "darkblueyellowstripessocks", "skin_white_18", "hair_17", "red",
                "Klaus:Chile:85:65:U:0:f1_s1_b0_t0_hblack13",
                "shoeyellow|shoered",
                "gloveyellowblack|gloveblacblue|gloveblueyellow"},
            new string[] {
                "Uruguay", 
                "81", 
                "80", 
                "20",
                "5000",
                //"lightblue|lightblue|uruguay",
                "blue|blue|uruguay",
                "lightbluestripes", "black", "blacksocks", "skin_white_19", "hair_18", "blue",
                "Klaus:Chile:85:65:U:0:f2_s1_b0_t0_hblack12",
                "shoeblackred|shoebluewhite",
                "gloveblacblue|gloveblueblue"},
            new string[] {
                "Switzerland", 
                "77", 
                "79", 
                "21", 
                "5300",
                "red|red|switzerland",
                "redshirt", "whiteshort", "sock_red", "skin_white_20", "hair_19", "red",
                "Klaus:Chile:85:65:U:0:f3_s2_b2_t5_hblack5",
                "shoeredwhite|shoered",
                "gloveredwhite"},
            new string[] {
                "Sweden", 
                "71", 
                "68", 
                "22", 
                "4100",
                "yellow|yellowblue|sweden",
                "yellowbluestripesshirt", "darkblueyellowstripeshort", "yellowbluestripecsocks", "skin_white_21", "hair_26", "yellow",
                "Klaus:Chile:85:65:U:0:f4_s2_b0_t0_hblonde1",
                "shoeblack|shoebluewhite",
                "gloveblueyellow|gloveyellowblack"},
            new string[] {
                "Mexico", 
                "71", 
                "69", 
                "23", 
                "0",
                "green|darkgreen|mexico",
                "greenwithwhiteshirt", "whiteshort", "darkredgreystripesocks", "skin_white_1","hair20", "green",
                "Klaus:Chile:85:65:U:0:f5_s4_b0_t0_hblack1",
                "shoegreen|shoered",
                "glovegreenwhite"},
            new string[] {
                "Iran", 
                "59", 
                "54", 
                "24", 
                "0",
                "green|darkgreen|iran",
                "whiteredstripeshirt", "whiteredstripeshort", "whiteredstripessocks1", "skin_white_2", "hair_17", "green",
                "Klaus:Chile:85:65:U:0:f6_s1_b0_t0_hblack4",
                "shoeredwhite|shoered",
                "gloveredwhite"},
            new string[] {
                "Peru", 
                "65", 
                "63", 
                "25", 
                "0",
                "red|red|peru",
                "whitewithredshirt", "whiteshort", "whiteredstripesocks1", "skin_black_18", "hair_4", "red",
                "Klaus:Chile:85:65:U:0:f7_s4_b2_t7_hblack5",
                "shoeredwhite|shoered",
                "gloveredwhite"},
            new string[] {
                "Senegal", 
                "71", 
                "72", 
                "26", 
                "2600",
                "green|darkgreen|senegal",
                "greenshirt1", "greenshorts", "greensocks", "skin_black_1", "hair_5", "green",
                "Klaus:Chile:85:65:U:0:f8_s5_b2_t5_hblack15",
                "shoegreen",
                "glovegreenwhite|gloveblackgreen"},
            new string[] {
                "Ukraine", 
                "71", 
                "62", 
                "27", 
                "1760",
                "yellow|yellowblue|ukraine",
                "yellowbluestripesshirt1", "yellowshort", "yellowsocks", "skin_white_3", "hair_26", "yellow",
                "Klaus:Chile:85:65:U:0:f9_s3_b0_t0_hblonde2",
                "shoeyellow|shoeblack",
                "gloveyellow|gloveyellowblack"},
            new string[] {
                "Romania", 
                "59", 
                "59", 
                "28", 
                "250",
                "yellow|yellowblue|romania",
                "yellowbluestripeshirt", "yellowredstripesshort", "yellowsocks",
                "skin_white_3", "hair_23", "yellow",
                "Klaus:Chile:85:65:U:0:f0_s1_b2_t2_hblack5",
                "shoeblue|shoeblack",
                "gloveyellow|gloveyellowblack"},
            new string[] {
                "Japan", 
                "63", 
                "59", 
                "29", 
                "900",
                "blue|blue|japan",
                "blestripesshirt1", "darkblueredstropedn", "darkblueredstripes",
                "skin_white_5", "hair_23", "blue",
                "Klaus:Chile:85:65:U:0:f1_s2_b0_t0_hblack4",
                "shoeredwhite|shoered",
                "gloveredwhite"},
            new string[] {
                "USA", 
                "61", 
                "63", 
                "30", 
                "2100",
                "blue|blue|usa",
                "whiteredstripeshirt", "darkblueshort11", "whiteredbluestripessocks", "skin_white_1", "hair_29", "white",
                "Klaus:Chile:61:63:U:0:f2_s1_b0_t0_hblack9",
                "shoeblue",
                "glovebluewhite"},
            new string[] {
                "South Korea", 
                "63", 
                "56", 
                "31", 
                "760",
                "red|red|southkorea",
                "redblackstripesshirt", "redblackstripesshort", "redblackstripesocks1", "skin_white_5", "hair_11", "white",
                "Klaus:Chile:63:56:U:0:f9_s3_b0_t0_hblack4",
                "shoeblackred|shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "Turkey", 
                "63", 
                "56", 
                "32", 
                "2300",
                "red|red|turkey",
                "redshirt", "redshort", "white", "skin_white_2", "hair_12", "red",
                "Klaus:Chile:63:56:U:0:f4_s1_b0_t0_hblack5",
                "shoeredwhite|shoered",
                "gloveredwhite"},
            new string[] {
                "Czechia", 
                "63", 
                "61", 
                "33", 
                "0",
                "red|red|czechia",
                "redshirt", "darkblueshort11", "sock_red",
                "skin_white_3", "hair_19", "red",
                "Klaus:Chile:63:61:U:0:f5_s2_b0_t0_hblonde5",
                "shoeblue|shoebluewhite",
                "glovebluewhite|glovebluered"},
            new string[] {
                "Australia", 
                "59", 
                "61", 
                "34", 
                "0",
                //"orange|orange|australia",
                "orange|yellowblue|australia",
                "yellowtshirt", "yellowshort", "yellowsocks",
                "skin_white_4", "hair_13", "yellow",
                "Klaus:Chile:59:61:U:0:f6_s1_b2_t9_hblack5",
                "shoeblack|shoeyellow",
                "gloveyellow|gloveyellowwhite"},
            new string[] {
                "Russia", 
                "64", 
                "67", 
                "35", 
                "0",
                "red|red|russia",
                "redwithwhiteshirt", "whiteredstripesshort", "redwhitesocks", "skin_white_6", "hair_28", "red",
                "Klaus:Chile:85:65:U:0:f7_s1_b0_t0_hred4",
                "shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "Bulgaria", 
                "59", 
                "49", 
                "36", 
                "0",
                "green|darkgreen|bulgaria",
                "whitewithgreenstripeshirt", "greenshorts", "whiteredsocks", "skin_white_7", "hair_23", "green",
                "Klaus:Chile:85:65:U:0:f5_s1_b0_t0_hblack4",
                "shoegreen",
                "glovegreenwhite|gloveblackgreen"},
            new string[] {
                "Saudi Arabia", 
                "59", 
                "49", 
                "37", 
                "450",
                "green|darkgreen|saudiarabia",
                "greenshirt1", "greenshorts", "greensocks",
                "skin_black_9", "hair_19", "green",
                "Klaus:Chile:85:65:U:0:f9_s4_b0_t0_hblack5",
                "shoegreen",
                "gloveblackgreen|glovegreenwhite"},
            new string[] {
                "Nigeria", 
                "55", 
                "51", 
                "38", 
                "3320",
                "green|darkgreen|nigeria",
                "greenshirt1", "greenshorts", "greensocks",
                "skin_black_16", "hair_15", "green",
                "Klaus:Chile:85:65:U:0:f0_s5_b2_t6_hblack6",
                "shoegreen",
                "gloveblackgreen|glovegreenwhite"},
            new string[] {
                "China", 
                "45", 
                "51", 
                "39", 
                "0",
                "red|red|china",
                "darkredwithyellowshirt", "whiteshort", "redyellowstripessocks",
                "skin_black_5", "hair_10", "red",
                "Klaus:Chile:85:65:U:0:f9_s3_b0_t0_hblack7",
                "shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "Cameroon", 
                "65",
                "71", 
                "40", 
                "5120",
                "green|darkgreen|cameroon",
                "greenshirt1", "redshort", "yellowsocks",
                "skin_black_3", "hair_19", "yellow",
                "Klaus:Chile:85:65:U:0:f2_s5_b0_t0_hblack8",
                "shoeblack|shoegreen|shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "Costarica", 
                "61", 
                "60", 
                "41", 
                "0",
                "red|red|costarica",
                "redshirt", "darkblueshort11", "whitebluestripesocks",
                "skin_white_10", "hair_16", "red",
                "Klaus:Chile:85:65:U:0:f3_s4_b0_t0_hblack9",
                "shoebluewhite",
                "glovebluered|glovebluewhite"},
            new string[] {
                "Ecuador", 
                "63", 
                "65", 
                "42", 
                "2212",
                "yellow|yellowblue|ecuador",
                "yellowbluearmshirt", "darkblueshort", "darkbluesocks1",
                "skin_black_15", "hair_5", "yellow",
                "Klaus:Chile:85:65:U:0:f5_s4_b0_t0_hblack15",
                "shoebluewhite|shoeyellow",
                "gloveblueyellow|gloveblacblue"},
            new string[] {
                "Ghana", 
                "65", 
                "69", 
                "43", 
                "3500",
                "red|red|ghana",
                "whiteshirt", "whiteshort", "whitegreysocks",
                "skin_black_11", "hair_6", "white",
                "Klaus:Chile:85:65:U:0:f5_s5_b2_t8_hblack11",
                "shoewhiteblack",
                "globewhiteblack"},
            new string[] {
                "Honduras", 
                "59", 
                "65", 
                "44", 
                "0",
                "blue|blue|honduras",
                "whiteblueshirt", "whiteshort", "white",
                "skin_white_11", "hair_7", "blue",
                "Klaus:Chile:85:65:U:0:f6_s5_b0_t0_hblack12",
                "shoebluewhite",
                "glovebluewhite"},
            new string[] {
                "Ivory Coast", 
                "71", 
                "65", 
                "45", 
                "2884", 
                //"orange|orange|ivorycoast",
                "orange|yellowblue|ivorycoast",
                "orangeblackstripesshirt", "oragngeshort", "orangesocks",
                "skin_black_12", "hair_8", "yellow",
                "Klaus:Chile:85:65:U:0:f7_s5_b2_t8_hblack13",
                "shoeorange",
                "gloveblackorange"},
            new string[] {
                "India", 
                "39", 
                "41", 
                "46", 
                "0",
                "blue|blue|india",
                "blueshirt", "darkblueshort11", "darkblue",
                "skin_black_8", "hair_9", "blue",
                "Klaus:Chile:85:65:U:0:f8_s4_b0_t0_hblack4",
                "shoebluewhite",
                "glovebluewhite"},
            new string[] {
                "Qatar", 
                "54", 
                "55", 
                "47", 
                "0",
                "red|red|qatar",
                "darkredshirt", "darkredshort", "darkredgreystripesocks",
                "skin_white_15", "hair_4", "red",
                "Klaus:Chile:85:65:U:0:f9_s3_b2_t7_hblack5",
                "shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "England", 
                "87", 
                "85", 
                "48", 
                "6130",
                "white|red|england",
                "whiteblueshirt", "darkblueredstropedn", "whiteredstripessocks1",
                "skin_white_16", "hair_16", "white",
                "Klaus:Chile:85:65:U:0:f0_s2_b0_t0_hred4",
                "shoeredwhite|shoebluewhite",
                "glovebluewhite"},
            new string[] {
                "Egypt", 
                "55", 
                "57", 
                "49", 
                "0",
                "red|red|egypt",
                "redshirt", "whiteshort", "blacksocks",
                "skin_black_18", "hair_6", "red",
                "Klaus:Chile:85:65:U:0:f2_s4_b0_t0_hblack14",
                "shoeblackred|shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "Panama", 
                "57", 
                "61", 
                "50", 
                "0",
                "red|red|panama",
                "redshirt", "redyellowstripeshort", "redyellowstripessocks",
                "skin_black_15", "hair_13", "red",
                "Klaus:Chile:85:65:U:0:f2_s4_b0_t0_hblack14",
                "shoeredwhite|shoered",
                "gloveredwhite"},
            new string[] {
                "Norway", 
                "51", 
                "52", 
                "51", 
                "0",
                "red|red|norway",
                "redblueshirt1", "whiteshort", "blueredstripedownsocks",
                "skin_white_21", "hair_1", "red",
                "Klaus:Chile:85:65:U:0:f3_s2_b0_t0_hblonde5",
                "shoebluewhite|shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "Paraguay", 
                "67", 
                "68", 
                "52", 
                "1890",
                "red|red|paraguay",
                "redwhitestripesshirt1", "bluewhitestripe", "darkbluewhitestripessocks",
                "skin_white_14", "hair_7", "blue",
                "Klaus:Chile:85:65:U:0:f4_s4_b2_t10_hblack13",
                "shoebluewhite|shoeredwhite",
                "gloveredwhite|glovebluered|glovebluewhite"},
            new string[] {
                "Jamaica", 
                "55", 
                "59", 
                "53", 
                "0",
                "yellow|darkgreen|jamaica",
                "yellowtshirt", "yellowshort", "yellowgreenstripesocks",
                "skin_black_4", "hair_8", "yellow",
                "Klaus:Chile:85:65:U:0:f5_s5_b0_t0_hblack3",
                "shoegreen",
                "gloveyellow"},
            new string[] {
                "Canada", 
                "55", 
                "59", 
                "54", 
                "0",
                "red|red|canada",
                "redshirt", "redshort", "sock_red",
                "skin_white_21", "hair_20", "red",
                "Klaus:Chile:85:65:U:0:f6_s3_b0_t0_hred3",
                "shoeredwhite",
                "gloveredwhite"},
            new string[] {
                "Albania", 
                "55", 
                "56", 
                "56", 
                "0",
                "red|red|albania",
                "redwithblackshirt", "black", "sock_red",
                "skin_white_20", "hair_15", "red",
                "Klaus:Chile:85:65:U:0:f7_s1_b0_t0_hblack4",
                "shoeblackred|shoeredwhite",
                "gloveredblack"},
            new string[] {
                "Bolivia", 
                "46", 
                "42", 
                "57", 
                "75",
                "darkgreen|darkgreen|bolivia",
                "greenshirt1", "greenshorts", "darkredgreystripesocks",
                "skin_black_9", "hair_11", "green",
                "Klaus:Chile:85:65:U:0:f8_s4_b2_t4_hblack5",
                "shoered|shoegreen",
                "gloveblackgreen"},
            new string[] {
                "Belarus", 
                "45", 
                "43", 
                "58", 
                "0",
                "red|red|belarus",
                "whiteshirt", "whiteshort", "white",
                "skin_white_18", "hair_24", "red",
                "Klaus:Chile:85:65:U:0:f9_s3_b2_t7_hblonde3",
                "shoeblue|shoered",
                "globewhiteblack"},
            new string[] {
                "Ireland", 
                "53", 
                "52", 
                "59", 
                "0",
                "green|darkgreen|ireland",
                "greenshirt1", "greenshorts", "greensocks",
                "skin_white_17", "hair_8", "green",
                "Klaus:Chile:85:65:U:0:f0_s2_b0_t0_hblack10",
                "shoegreen",
                "glovegreenwhite|gloveblackgreen"},
            new string[] {
                "Israel", 
                "45",
                "43", 
                "60", 
                "0",
                "blue|blue|israel",
                "whiteshirt", "lightblueshort", "white",
                "skin_white_16", "hair_10", "blue",
                "Klaus:Chile:85:65:U:0:f1_s2_b0_t0_hblack4",
                "shoeblue",
                "gloveblueblue|glovebluewhite"},
             new string[] {
                 "South Africa", 
                 "55",
                 "59", 
                 "34", 
                 "0",
                 "yellow|darkgreen|southafrica",
                "yellowtshirt", "greenshorts", "yellowsocks",
                "skin_black_4", "hair_13", "yellow",
                 "Klaus:Chile:85:65:U:0:f2_s5_b0_t0_hblack11",
                 "shoegreen|shoeblack",
                 "gloveyellow|gloveblackgreen"},
             new string[] {
                 "Kazakhstan", 
                 "48", 
                 "50", 
                 "2", 
                 "0",
                 "yellow|yellowblue|kazakhstan",
                 "yellowbluestripesshirt", "yellowbluestripesshort", "yellowbluestripecsocks", "skin_white_16", "hair_25", "yellow",
                 "Klaus:Chile:85:65:U:0:f4_s2_b2_t9_hblonde4",
                 "shoeblue|shoeyellow",
                 "gloveblueyellow|gloveyellow"},
             new string[] {
                 "San Marino", 
                 "39", 
                 "40", 
                 "2", 
                 "0",
                 "blue|blue|sanmarino",
                "lightbluestripes", "lightblueshort", "bluewhitestripedownsocks", "skin_white_18", "hair_23", "blue",
                 "Klaus:Chile:85:65:U:0:f4_s1_b0_t0_hblack7",
                 "shoeblue",
                 "gloveblueblue|glovelightbluewhite"},
             new string[] {
                 "Domin. Republic", 
                 "41", 
                 "41", 
                 "2", 
                 "0",
                 "blue|blue|domin.republic",
                "redbluearmshirt", "darkblueshort11", "darkblue", "skin_black_8", "hair_5", "blue",
                 "Klaus:Chile:85:65:U:0:f0_s4_b2_t5_hblack10",
                 "shoebluewhite",
                 "glovebluewhite"},
             new string[] {
                 "Scotland", 
                 "63", 
                 "60", 
                 "13", 
                 "0",
                 "blue|blue|scotland",
                "bluewithwhiteshirt", "darkbluewhitestripes", "darkbluewhitestripessocks", "skin_white_12", "hair_12", "blue",
                 "Klaus:Chile:85:65:U:0:f5_s2_b0_t0_hred2",
                 "shoebluewhite",
                 "glovebluewhite"},
             new string[] {
                 "Wales", 
                 "73", 
                 "74", 
                 "19", 
                 "0",
                 "red|red|wales",
                "redshirt", "redyellowstripeshort", "redyellowstripessocks", "skin_white_18", "hair_17", "red",
                 "Klaus:Chile:85:65:U:0:f6_s3_b2_t10_hblack8",
                 "shoeredwhite|shoeblackred",
                 "gloveredwhite"},
             new string[] {
                 "Armenia", 
                 "50", 
                 "49", 
                 "19", 
                 "0",
                 "red|red|armenia",
                "darkredshirt", "darkredshort", "darkredgreystripesocks", "skin_white_18", "hair_28", "red",
                 "Klaus:Chile:85:65:U:0:f7_s1_b0_t0_hblack4",
                 "shoered",
                 "gloveredwhite"},
             new string[] {
                 "Cyprus", 
                 "50", 
                 "49", 
                 "19",
                 "0",
                 "blue|blue|cyprus",
               "blueshirt", "darkblueshort11", "darkblue", "skin_white_18", "hair_10", "blue",
                 "Klaus:Chile:85:65:U:0:f8_s1_b0_t0_hblack5",
                 "shoebluewhite",
                 "glovebluewhite"},
             new string[] {
                 "Northern Ireland", 
                 "55", 
                 "54", 
                 "0", 
                 "0",
                 "green|darkgreen|northernireland",
                "greenshirt1", "whitegreenstripesshort", "greenwhitestripessocks",
                "skin_white_1", "hair_5", "green",
                 "Klaus:Chile:85:65:U:0:f9_s3_b0_t0_hblack6",
                 "shoegreen",
                 "gloveyellow|gloveblackgreen"}
        };

        public string[][] getTeams()
        {
            return teams;
        }

        /*public NationalTeams()
        {
            activeTeams = teams.Length;
            this.sortTeams();
            for (int i = 0; i < teams.Length; i++)
                teams[i][3] = i.ToString();
        }

        public NationalTeams(double levelFactor)
        {
            activeTeams = teams.Length;
            this.sortTeams();

            for (int i = 0; i < teams.Length; i++)
                teams[i][3] = i.ToString();

            if (levelFactor == 1.0) return;

            double res;
            for (int i = 0; i < teams.Length; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    if (j == 1)
                        res = ((double)(int.Parse(teams[i][j]))) * levelFactor / 1.5;
                    else
                        res = ((double)(int.Parse(teams[i][j]))) * levelFactor;

                    res = (int)res;
                    teams[i][j] = res.ToString();
                }
            }
        }

        public bool swapElements(int row1, int row2)
        {
            if (row1 >= activeTeams
                    || row2 >= activeTeams
                    || activeTeams < 2
                    || row1 < 0
                    || row2 < 0)
                return false;

            string[] tmpRow = teams[row2];
            teams[row2] = teams[row1];
            teams[row1] = tmpRow;
            activeTeams--;
            return true;
        }

        public int getMaxTeams()
        {
            return teams.Length;
        }

        public int getMaxActiveTeams()
        {
            return this.activeTeams;
        }

        public static int sortTeamsComparator(string[] team1, string[] team2)
        {
            string time1 = team1[0];
            string time2 = team2[0];

            return time1.CompareTo(time2);
        }

        private void sortTeams()
        {
            Array.Sort(this.teams, sortTeamsComparator);
        }

        public string[] getTeamByIndex(int index)
        {
            if (PlayerPrefs.HasKey(teams[index][0] + "_defense"))
            {
                int defense = PlayerPrefs.GetInt(teams[index][0] + "_defense");
                teams[index][1] = defense.ToString();
            }

            if (PlayerPrefs.HasKey(teams[index][0] + "_attack"))
            {
                int attack = PlayerPrefs.GetInt(teams[index][0] + "_attack");
                teams[index][2] = attack.ToString();
            }

            return teams[index];
        }

        public string[] getTeamByName(string name)
        {
            for (int i = 0; i < teams.Length; i++)
            {
                if (teams[i][0].Equals(name))
                    return teams[i];
            }

            return null;
        }

        public int getTeamIndexByName(string name)
        {
            for (int i = 0; i < teams.Length; i++)
            {
                if (teams[i][0].Equals(name))
                    return i;
            }

            return -1;
        }

        public string getTshirtColorByTeamIndex(int index)
        {     
            return teams[index][6];
        }

        public string getShortsColorByTeamIndex(int index)
        {         
            return teams[index][7];
        }

        public string getSocksColorByTeamIndex(int index)
        {            
            return teams[index][8];
        }

        public string getSkinColorByTeamIndex(int index)
        {
            return teams[index][9];
        }

        public string getHairColorByTeamIndex(int index)
        {
            return teams[index][10];
        }

        public string getFlareColorByTeamIndex(int index)
        {
            return teams[index][11];
        }

        public void addTeamtoPrefabs(int coins)
        {
            int coinsNeeded = 0;

            for (int i = 0; i < teams.Length; i++)
            {
                coinsNeeded = int.Parse(teams[i][4]);

                if (coinsNeeded == coins)
                {
                    string teamName = Regex.Replace(teams[i][0], "\\s+", "");

                    if (!PlayerPrefs.HasKey(teamName))
                    {
                        PlayerPrefs.SetInt(teamName, 1);
                        PlayerPrefs.Save();
                    }
                }
            }
        }

        public List<int> getJustUnlockedTeamsIndexes()
        {
            string teamName = "";
            int coinsNeeded = 0;
            int coinsGathered = Globals.coins;
            List<int> teamsIndexses = new List<int>();

            for (int i = 0; i < teams.Length; i++)
            {
                teamName = Regex.Replace(teams[i][0], "\\s+", "");
                coinsNeeded = int.Parse(teams[i][4]);

                if (!PlayerPrefs.HasKey(teamName) &&
                    coinsGathered >= coinsNeeded)
                {
                    teamsIndexses.Add(i);
                }
            }

            return teamsIndexses;
        }

        public void saveJustUnlockedTeamsToPrefabs()
        {
            List<int> teamIndexses = getJustUnlockedTeamsIndexes();
            foreach (int teamIdx in teamIndexses)
            {
                string teamName = Regex.Replace(teams[teamIdx][0], "\\s+", "");
                PlayerPrefs.SetInt(teamName, 1);
                PlayerPrefs.Save();
            }
        }

        public bool isAnyNewTeamUnclocked()
        {
            return (getJustUnlockedTeamsIndexes().Count > 0 ? true : false);
        }*/
    }
}
