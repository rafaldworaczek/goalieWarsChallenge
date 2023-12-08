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
    public class LeaguePoland
    {
        private int activeTeams = 0;
        /*team name 
         * gk 
         * shot 
         * index 
         * coinsNeeded 
         * fans/flag down color/stick flags color
         * thirt color 
         * shorts color
         * sock color
         * thirt number color
         * hair
         * flare color
         * player Desc:
                name
                nationallity
                defense skills
                attack skills
                lock/unlock
                price
                face_skinColor_beard_tatto_hair_number


              0 - very light blond
              1 - blond
              2 - light brown (ginger)
              3 - brown (giner)
              4 - light black
              5 - 
         */
        private string[][] teams = new string[][] {
             new string[] {
                 "Poznan Club",
                 "43",
                 "42",
                 "0",
                 "0",
                 "blue|blue|blue",
                 "bluedarkbluearmshirt",
                 "whiteblueshort",
                 "darkbluewhitestripesocks",
                "skin_white_1",
                 "hair_16",
                 "blue",
                //Marek Junior 
                "Marek Junior:Poland:55:55:U:0:f7_s1_b0_t0_hblack9|" +

                //Filip Bednarek - gk
                "Franek Bondurak:Poland:77:49:L:5000:f8_s1_b2_t9_hnohair|" +
                 //Tymoteusz Puchacz
                "Tymek Prochucz:Poland:58:50:L:5000:f9_s3_b2_t7_hblonde2|" +
                 //Ramirez Daniel mid
                "Danek Reimez:Spain:53:55:L:2000:f0_s1_b2_t2_hblack5|" +
                 //Tiba Pedro mid
                "Peter Tobi:Portugal:50:51:L:2300:f1_s1_b2_t7_hblack3|" +
                 //Michał Skóraś att
                "Marcin Skros:Poland:49:61:L:2600:f2_s1_b0_t0_hblonde1|" +
                 //Ishak Mikael att
                "Miki Inaki:Sweden:51:74:L:3500:f3_s2_b2_t5_hblack1",
                "shoebluewhite",
                "glovebluewhite"},
             new string[] {
                 "Warsaw FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 //"green|green|green",
                 "darkgreen|darkgreen|darkgreen",
                 "darkgreengoldstripesshirt",
                 "darggreengoldstripesshort",
                 "darkgreengoldstripessocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Piotr Junior
                "Piotr Junior:Poland:60:60:U:0:f0_s1_b2_t2_hblack8|" +

                 //Artur Boruc gk
                "Arek Barok:Poland:79:63:L:5000:f1_s1_b2_t7_hblack10|" +
                 //Jędrzejczyk Artur def
                "Arek Jeszczyk:Poland:70:51:L:5000:f2_s1_b0_t0_hblack1|" +
                 //Mladenovic Filip def
                "Philipe Modovinc:Serbia:65:52:L:2000:f3_s1_b2_t4_hblack5|" +
                 //Bartosz Kapustka mid
                "Bartosz Krusta:Poland:63:63:L:2300:f4_s1_b0_t0_hblack10|" +
                 //Guimaraes Lopes Rafael att
                "Rafi Lupi:Portugal:51:63:L:2600:f5_s1_b2_t5_hblack3|" +
                 //Pekhart Tomas att
                "Tom Pikerth:Czechia:48:76:L:3500:f6_s3_b2_t10_hblack5",
                 "shoegreen",
                 "gloveblackgreen"
                 },
             new string[] {
                 "Niebiescy",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                 "blueshirt",
                 "whiteshort",
                 "darkbluewhitestripessocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Pablo Junior
                "Tomek Junior:Poland:50:48:U:0:f5_s1_b0_t0_hblack1",
                
                "shoeblackred|shoered",
                "gloveblacblue|glovebluewhite"},
             new string[] {
                 "Zabrze FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                 "whiteshirt",
                 "blueshort",
                 "whiteredsocks",
                 "skin_white_1",
                 "hair_19",
                 "red",
                //Martin Junior
                "Darek Junior:Poland:54:51:U:0:f1_s1_b0_t0_hblack13",
              
                "shoeredwhite|shoebluewhite",
                "gloveredwhite|gloveredwhite"},
             //Levante
             new string[] {
                 "Pasy Krakow",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                 "redwhiteshirt",
                 "whiteshort",
                 "sock_red",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Alejandro Jr
                "Kacper Junior:Poland:53:50:U:0:f8_s1_b0_t0_hblack6",
                   
                "shoered|shoeblackred",
                "glovebluered|gloveblacblue"},
             new string[] {
                 "Czerwoni Krakow",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                 "redshirt",
                 "whiteredstripeshort",
                 "sock_red",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Alvaro Junior
                "Antek Junior:Poland:46:41:U:0:f5_s1_b2_t5_hred4",
                        
                 "shoewhite|shoewhiteblack",
                 "globewhiteblack"},
             new string[] {
                 "Lodz FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|red|red",
                 "whitewithredshirt",
                 "whiteredsocks",
                 "whiteredstripessocks1",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //David Junior
                "Karol Junior:Poland:45:48:U:0:f4_s1_b2_t8_hred3",
              
                "shoeredwhite|shoeblackred",
                "gloveredwhite"},
             new string[] {
                 "Lodz Czerwoni",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "redshirt",
                 "redshort",
                 "sock_red",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Lucas Junior
                "Mariusz Junior:Poland:52:53:U:0:f5_s1_b2_t5_hred4",
           
                "shoeredwhite|shoeblackred",
                "gloveredwhite|gloveredblack"},
             new string[] {
                 "Gdynia FC",
                 "60",
                 "58",
                 "0",
                 "0",
                "yellow|yellowblue|yellowblue",
                "yellowbluestripeshirt",
                "blueyellowstripesshort",
                "yellowsocks",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Mario Junior
                "Piotr Junior:Poland:48:50:U:0:f8_s1_b0_t0_hblonde3",
                "shoeredwhite|shoeblackred",
                "gloveredwhite|gloveredblack"},
             new string[] {
                 "Mielec",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "bluewhireshirt",
                "blueshort",
                "bluebluestripesocks",
                "skin_white_1",
                 "hair_19",
                 "yellow",
                 //Diego Junior
                "Jarek Junior:Poland:50:50:U:0:f8_s1_b0_t0_hred4",
                              
                "shoeyellow|shoeblack",
                "gloveyellow|gloveyellowblack"},
             new string[] {
                 "Lubin",
                 "60",
                 "58",
                 "0",
                 "0",
                "orange|orangeblack|orangeblack",
                "orangeblackshirt",
                "blackorangestripesshort",
                "orangeblackstripesocks",
                "skin_white_1",
                 "hair_19",
                 "yellow",
                //Manuel Junior
                "Jakub Junior:Poland:52:53:U:0:f2_s1_b2_t8_hblack1",                          
                "shoeyellow|shoeblack",
                "gloveyellowwhite"},
             new string[] {
                 "Jaga",
                 "60",
                 "58",
                 "0",
                 "0",
                 "yellow|red|red",
                "redyellowshirt",
                "redshort",
                "redyellowstripessocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Leo Junior
                "Tymon Junior:Poland:55:57:U:0:f9_s1_b2_t6_hblack2",                              
                "shoegreen",
                "glovegreenwhite"},
             new string[] {
                 "Gliwice",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|blue|red",
                "bluedarkbluearmshirt",
                 "redshort",
                 "darkbluesocks1",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Mateo Junior
                "Nikodem Junior:Poland:54:53:U:0:f6_s1_b2_t9_hblack3",             
                "shoebluewhite",
                "glovebluewhite"},
             new string[] {
                 "Czestochowa",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|blue",
                 "redshirt",
                 "blueredstripes",
                 "sock_red",
                "skin_white_1",
                 "hair_19",
                 "red",      
                 //Javier Junior
                "Szymon Junior:Poland:63:59:U:0:f1_s1_b0_t0_hblack4",
                "shoeblackred|shoered",
                "gloveblacblue|gloveredblack"},
             //Osasuna
             new string[] {
                 "Szczecin",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                 "blueredshirt",
                 "darkblueshort",
                 "darkbluesocks1",
                "skin_white_1",
                 "hair_19",
                 "red",   
                //Marcos Junior
                "Ignacy Junior:Poland:56:54:U:0:f0_s2_b0_t0_hred2",
       
                "shoeblackred|shoered",
                "gloveredblack|gloveblacblue"},
             new string[] {
                 "Radom",
                 "60",
                 "58",
                 "0",
                 "0",
                //"green|green|green",
                "darkgreen|darkgreen|darkgreen",
                "whitewithgreenstripeshirt",
                 "greenshorts",
                 "greensocks",
                 "skin_white_1",
                 "hair_19",
                 "blue", 
                 //Alex Junior
                "Marcel Junior:Poland:52:53:U:0:f7_s3_b2_t6_hblack6",           
                "shoewhite",
                "gloveredwhite"},
             new string[] {
                 "Kielce",
                 "60",
                 "58",
                 "0",
                 "0",
                 "yellow|red|red",
                 "yellowtshirt", 
                 "redshort",
                 "sock_red",
                 "skin_white_1",
                 "hair_19",
                 "blue", 
                //Sergio Junior
                "Sergio Junior:Poland:52:51:U:0:f4_s4_b2_t10_hred5",
                
                "shoebluewhite|shoeblue",
                "glovelightbluewhite"},
             //Alaves
             new string[] {
                 "Wroclaw Zieloni",
                 "60",
                 "58",
                 "0",
                 "0",
                "green|darkgreen|darkgreen",
                "greenshirt1", 
                 "greenshorts", 
                 "greensocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Marc Junior
                "Marc Junior:Poland:58:56:U:0:f0_s4_b0_t0_hblack13",

                "shoebluewhite",
                "glovebluewhite"},
             //Real Sociedad
             new string[] {
                 "Gdansk Zieloni",
                 "60",
                 "58",
                 "0",
                 "0",
                "darkgreen|darkgreen|darkgreen",
                "whitegreenstripeshorshirt",
                "greenshorts",
                "greenwhitestripessocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Carlos Junior
                "Carlos Junior:Poland:47:45:U:0:f7_s1_b2_t5_hblack11",
                "shoebluewhite",
                "glovebluewhite"},
            //Betis Sevilla
             new string[] {
                 "Katowice",
                 "60",
                 "58",
                 "0",
                 "0",
                 "yellow|yellowblack|yellowblack",
                 "yellowblackshirt",
                "black",
                "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                //Jorge Junior
                "Jorge Junior:Poland:42:41:U:0:f4_s1_b0_t0_hblack12",
                "shoegreen",
                "glovegreenwhite"},
             new string[] {
                 "Customize Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "FANS_COLOR|FLAG_COLOR1|FLAG_COLOR2",
                 "SHIRT_FILE",
                 "SHORT_FILE",
                 "SOCK_FILE",
                 "SKIN_FILE",
                 "HAIR_FILE",
                 "FLARE_COLOR",
                 //Fabio Junior
                "Juliano Jr:COUNTRY_TO_CHANGE:60:60:U:0:SKIN_HAIR_TO_CHANGE",
                "SHOE_FILE",
                "GLOVES_FILE"}
        };


        public string[][] getTeams()
        {
            return teams;
        }
    }
}

/*
https://www.babynamewizard.com/name-list/spanish-boys-names-most-popular-names-for-boys-in-spain  
LIST OF NOT TAKEN NAMES
Enzo
Antonio
Angel
Gonzalo
Iker
Juan
Eric
Ivan
Ruben
Nicolas
Samuel
Hector
José
Dario
Oliver
Aaron
Adam
Dylan
Jesus
Marco
Alberto
Guillermo
Raul
Francisco
Joel
Erik
Luis
Jaime
Rafael
Asier
Unai
Mohamed
Gael
Oscar
Luca
Andres
Biel
Ismael
Alonso
Pol
Nil
Jan
Rayan
Arnau
Cristian
Saul
Isaac
Santiago
Julen
Joan
Miguel
Aimar
Ignacio
Youssef
Mauro
Enrique
Yago
José
Gerard
Abraham
Noah
Omar
Ibai
Francisco
*/
