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
    public class AmericaCupTeams
    {
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
                 "Venezuela",
                 "65", 
                 "66",
                 "0",
                 "0",
                 "red|claret|venezuela",
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
                "Uruguay",
                "81", 
                "80",
                "20",
                "5000",
                "blue|blue|uruguay",
                "lightbluestripes", "black", "blacksocks", "skin_white_19", "hair_18", "blue",
                "Klaus:Chile:85:65:U:0:f2_s1_b0_t0_hblack12",
                "shoeblackred|shoebluewhite",
                "gloveblacblue|gloveblueblue"},                      
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
                "Costa Rica",
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
        };

        public string[][] getTeams()
        {
            return teams;
        }
    }
}
