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
    public class LeagueItaly
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
         * shoes
         * gloves
         */
        private string[][] teams = new string[][] {
             //atlanta
             new string[] {
                 "Bergamo Blue",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "blackbluestripesshirt",
                 "blackbluestripesshort",
                 "blackbluestripessocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Alessio Junior
                 "Alessio Junior:Italy:60:60:U:0:f3_s1_b0_t0_hblack15|" +

                 //Gollini Pierluigi - gk
                 "Lodovico Goalani:Italy:91:72:L:2000:f6_s1_b0_t0_hblack2|" +
                 //Toloi Rafael - def \\TO DO\\
                 "Rafi Toiono:Italy:88:73:L:5000:f4_s1_b2_t8_hblack6|" +
                 //Malinovsky Ruslan - mid \\TO DO\\
                 "Rui Malnowski:Ukraine:82:82:L:5000:f5_s1_b0_t0_hblack5|" +                
                 //Gosens Robin - def
                 "Rob Gones:Germany:89:85:L:2300:f7_s1_b2_t5_hblack10|" +
                 //Muriel Luis - att
                 "Luigi Murle:Colombia:77:90:L:2600:f8_s4_b2_t4_hblack5|" +
                 //Zapata Duvan - att
                 "Daven Zupeta:Colombia:76:88:L:3500:f9_s5_b0_t0_hblack14",
                 "shoebluewhite",
                 "gloveblacblue"},
             new string[] {
                 "Bologna Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                 "blueredshirt",
                 "darkblueshort",
                 "bluewhitestripesverticalsocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Andrea Junior
                "Andrea Junior:Italy:60:60:U:0:f2_s1_b2_t8_hblack14|" +

                 //Skorupski Łukasz - gk
                "Lukasz Skorupek:Poland:84:61:L:5000:f3_s2_b0_t0_hblack3|" +
                 //Tomiyasu Takehiro - def
                "Teke Toyus:Japan:75:69:L:5000:f9_s1_b0_t0_hblack8|" +
                 //Soriano Roberto - mid
                "Robert Soniaro:Italy:71:81:L:2000:f5_s1_b2_t5_hblack5|" +
                 //Orsolini Riccardo - mid
                "Ricard Osonili:Italy:72:76:L:2300:f6_s1_b0_t0_hblack10|" +
                 //Barrow Musa - att
                "Muas Bawora:Gambia:72:80:L:2600:f7_s5_b0_t0_hblack5|" +
                 //Sansone Nicola -att 
                "Nic Saonesa:Italy:65:73:L:3500:f8_s1_b2_t9_hblack1",
                 "shoebluewhite|shoeblackred",
                "gloveblacblue|gloveredblack"},
             new string[] {
                 "Crotone Town",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                "blueredshirt",
                 "darkblueshort",
                 "blueredstripe",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Antonio Junior
                "Antonio Junior:Italy:60:60:U:0:f9_s1_b2_t6_hred1|" +

                 //Cordaz Alex - gk
                "Aleks Codez:Italy:74:45:L:5000:f1_s2_b0_t0_hblack1|" +
                 //Golemic Vladimir - def
                "Vlodek Gomec:Serbia:72:49:L:2000:f2_s1_b0_t0_hblack5|" +
                 //Reca Arkadiusz - def \\TO DO\\
                "Aro Rekaw:Poland:70:55:L:2300:f3_s2_b0_t0_hblack10|" +
                 //Ounas Adam - mid
                "Adamo Onaso:Algeria:55:54:L:2600:f4_s4_b2_t10_hblack5|" +
                 //Simy - att
                "Ziymy:Nigeria:52:85:L:2600:f5_s5_b2_t8_hblack3|" +
                 //Messias Junior - att \\TO DO TO ZAWODNIK CZY JUNIOR??\\
                "Jun Miasas:Brazil:51:77:L:3500:f6_s4_b2_t3_hblack10",
                "shoeblackred|shoered",
                "gloveblacblue|gloveredblack"},
             new string[] {
                 "Genoa Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                 "redblueshirt",
                 "darkblueshort",
                 "blueredstripedownsocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Davide Junior
                "Davide Junior:Italy:60:60:U:0:f7_s1_b0_t0_hred3|" +

                //Perin Mattia - gk
                "Mati Porien:Italy:80:61:L:2000:f9_s1_b2_t6_hblack9|" +
                //Cristian Zapata - def
                "Zapete Kris:Colombia:75:61:L:5000:f8_s5_b0_t0_hnohair|" +                 
                //Zappacosta Davide - def
                "David Zapoasta:Italy:74:65:L:2300:f0_s1_b2_t2_hblack10|" +
                 //Destro Mattia - att
                "Mat Destor:Italy:63:80:L:2600:f1_s1_b2_t7_hblack7|" +
                 //Pandev Goran - att
                "Goren Pavdev:Macedonia:61:75:L:2600:f2_s1_b2_t8_hblack1|" +
                 //Pjaca Marko - att
                "Mark Pacja:Croatia:63:72:L:3500:f3_s1_b2_t4_hblack5",
                "shoeblackred|shoered",
                "gloveblacblue|gloveredblack"},
             new string[] {
                 "JV Torino",
                 "60",
                 "58",
                 "0",
                 "0",
                 "black|black|black",
                "whiteblackstripesshirt",
                 "whitegoldstripes",
                 "whitegoldsocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Domenico Jr 
                "Domenico Jr:Italy:60:60:U:0:f4_s1_b2_t8_hblack13|" +

                 //Szczesny Wojciech - gk
                "Wojtek Szesnyk:Poland:94:82:L:5000:f5_s1_b0_t0_hblonde3|" +
                 //Buffon Gianluigi - gk
                "Gian Bufen:Italy:94:82:L:2000:f6_s1_b2_t9_hblack10|" +
                 //Chiellini Giorgio - def
                "Giorg Celalini:Italy:95:86:L:2300:f7_s1_b2_t5_hblack3|" +
                 //de Ligt Matthijs - def
                "Mathew Lithg:Netherlands:90:83:L:2600:f8_s1_b0_t0_hblonde2|" +
                 //Ronaldo Cristiano - att \\TO DO rolandus rolanes??\\
                "Chris Rolandes:Portugal:86:97:L:2600:f3_s1_b0_t0_hblack10|" +
                 //Dybala Paulo - att \\TO DO diblae??\\
                "Paul Diblaes:Argentina:83:88:L:3500:f1_s1_b0_t0_hblonde4|" +
                 //Morata Alvaro - att
                "Alvro Moate:Spain:85:93:L:3500:f2_s1_b0_t0_hblack5|" +
                 //Chiesa Federico - att
                "Fedricko Cesa:Italy:84:92:L:3500:f9_s1_b0_t0_hblack8",
                "shoewhiteblack",
                "globewhiteblack|gloveblackwhite"},
             new string[] {
                 "Milan Red",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "blackredstripesshirt",
                 "whiteshort",
                 "whiteredsocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Edoardo Junior 
                "Edoardo Junior:Italy:60:60:U:0:f4_s1_b2_t8_hblack12|" +


                 //Donnarumma Gianluigi - gk
                "Giani Donurama:Italy:93:73:L:5000:f5_s1_b2_t5_hblack3|" +
                 //Hernandez Theo - def
                "Teoh Hanreza:France:86:74:L:2000:f6_s1_b2_t9_hblonde3|" +
                 //Leao Rafael - att
                "Rafi Laoea:Portugal:80:84:L:2300:f7_s5_b0_t0_hblack5|" +
                 //Kessie Franck - mid
                "Frank Kiesse:Ivory Coast:87:88:L:2600:f8_s5_b2_t5_hblack7|" +
                 //Mandzukic Mario - att
                "Marius Macukinc:Croatia:75:87:L:2600:f9_s3_b0_t0_hblack5|" +
                 //Ibrahimovic Zlatan - att
                "Zlaten:Sweden:88:90:L:3500:f0_s2_b2_t4_hblack15",
                "shoeblackred|shoeredwhite",
                "gloveredblack|gloveredwhite"},
             new string[] {
                 "Milan Blue",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blueblack|blueblack",
                "blackbluestripesshirt",
                 "whiteshort",
                 "blueblackstripeupsocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Emanuele Jr
                "Emanuele Jr:Italy:60:60:U:0:f1_s1_b2_t7_hblonde1|" +

                 //Hakimi Achraf - def
                "Afchraf Hamikii:Morocco:90:84:L:5000:f2_s4_b2_t10_hblack10|" +
                 //Handanovic Samir - gk
                "Samri Hadnowick:Slovenia:93:81:L:2000:f3_s2_b2_t5_hblack1|" +
                 //Lukaku Romelu - att
                "Lukukul:Belgium:91:94:L:2300:f4_s5_b2_t4_hnohair|" +
                 //Perisic Ivan - att \\TO DO pracic, parasic??\\
                "Ian Pervcis:Croatia:81:84:L:2600:f5_s1_b2_t5_hblack3|" +
                 //Sanchez Alexis - att
                "Aleks Sonhez:Chile:79:83:L:2600:f6_s4_b0_t0_hblack10|" +
                 //Martinez Lautaro - att
                "Lutro Martinee:Spain:83:90:L:3500:f7_s1_b0_t0_hblack5",
                 "shoebluewhite",
                "glovebluewhite|gloveblacblue"},
             new string[] {
                 "Napoli FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "lightblue|lightblue|lightblue",
                "bluwithblueshirt",
                 "whiteblueshort",
                 "blueblackstripesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Federico Jr
                "Federico Jr:Italy:60:60:U:0:f8_s1_b0_t0_hblack11|" +

                 //Rui Mario - def
                "Mark Ruiia:Portugal:86:73:L:5000:f9_s3_b2_t7_hblack5|" +
                 //Ospina David - gk
                "Davi Opinas:Colombia:90:72:L:2000:f0_s1_b2_t2_hblack1|" +
                 //Insigne Lorenzo - att \\TO DO\\
                "Lorzo Insanes:Italy:76:91:L:2300:f1_s1_b2_t7_hblack3|" +
                 //Lozano Hirving - att
                "Hing Lozro:Mexico:83:90:L:2600:f2_s2_b0_t0_hblack8|" +
                 //Mertens Dries - att
                "Dres Martes:Belgium:78:89:L:2600:f3_s1_b2_t4_hblonde5|" +
                 //Zielinski Piotr - mid
                "Piotr Ziela:Poland:89:88:L:3500:f4_s2_b0_t0_hblack5",
                "shoeblue",
                "glovelightbluewhite"},
             new string[] {
                 "Roma FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claret|claret",
                "darkredwithyellowshirt",
                 "darkredshort",
                 "darkredgreystripesocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Francesco Jr
                "Francesco Jr:Italy:60:60:U:0:f5_s1_b2_t5_hblack9|" +

                 //Pellegrini Lorenzo - mid
                "Lorez Polgrani:Italy:81:78:L:5000:f6_s1_b2_t9_hblack5|" +
                 //Dzeko Edin - att
                "Eden Eko:Bosnia:79:85:L:2000:f7_s3_b2_t6_hblack3|" +
                 //Veretout Jordan - mid \\TO DO vretue??\\
                "Jordi Vretue:France:82:80:L:2000:f8_s1_b2_t9_hblack8|" +
                 //Lopez Pau - gk
                "Pauu Lopiz:Spain:89:70:L:2300:f9_s1_b2_t6_hnohair|" +
                 //Mkhitaryan Henrikh - mid
                "Henrick Mitarian:Armenia:88:88:L:2600:f0_s1_b2_t2_hblack10|" +
                 //Mayoral Borja - att
                "Boja Morales:Spain:71:89:L:2600:f1_s1_b2_t7_hnohair|" +
                 //Mancini Gianluca - def
                "Gianni Macinni:Italy:85:70:L:3500:f2_s1_b0_t0_hblack3",
                "shoered|shoeblackred",
                "gloveredwhite"},
             new string[] {
                 "Sassuolo Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "darkgreen|darkgreen|darkgreen",
                "darkgreendblackstripesshirt",
                 "black",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Franco Junior
                "Franco Junior:Italy:60:60:U:0:f3_s1_b2_t4_hred4|" +

                 //Consigli Andrea - gk
                "Andre Conglisi:Italy:85:63:L:2300:f6_s1_b2_t9_hblack13|" +
                 //Traore Hamed - mid
                "Hamet Atrore:Ivory Coast:75:73:L:5000:f4_s5_b0_t0_hblack3|" +
                 //Djuricic Filip - mid
                "Filip Dinicic:Serbia:74:75:L:2000:f5_s2_b0_t0_hblack7|" +                
                 //Chiriches Vlad - def
                "Vlod Chichas:Romania:78:67:L:2600:f7_s1_b0_t0_hblack1|" +
                 //Caputo Francesco - att
                "France Kuput:Italy:73:83:L:2600:f8_s1_b2_t9_hblack9|" +
                 //Berardi Domenico - att
                "Dominic Boredi:Italy:76:85:L:3500:f9_s1_b2_t6_hblonde4",
                "shoegreen",
                "gloveblackgreen"},
             new string[] {
                 "Torino Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claret|claret",
                "darkredshirt",
                 "whiteredstripeshort",
                 "blackredstripesocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Gabriel Junior
                "Simone Junior:Italy:60:60:U:0:f0_s1_b2_t2_hblack2|" +

                //Sirigu Salvatore - gk
                "Salvo Siguri:Italy:79:60:L:5000:f1_s1_b2_t7_hblack8|" +
                 //Bremer - def
                "Brimre:Brazil:77:59:L:2000:f2_s4_b2_t10_hnohair|" +
                 //Linetty Karol - mid
                "Karol Linoty:Poland:74:69:L:2300:f3_s2_b2_t5_hblack10|" +
                 //Belotti Andrea - att
                "Andre Balloti:Italy:59:70:L:2600:f4_s2_b0_t0_hblack1|" +
                 //Zaza Simone - att
                "Simon Zezea:Italy:61:76:L:2600:f5_s1_b2_t5_hnohair|" +
                 //Lukic Sasa - mid
                "Sase Lucik:Serbia:65:65:L:3500:f6_s1_b0_t0_hblack3",
                "shoeblackred|shoeredwhite",
                "gloveredblack|gloveredwhite"},
             new string[] {
                 "Benevento FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "redyellowshirt",
                 "blackwhitestripesshort",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Giacomo Junior
                "Giacomo Junior:Italy:60:60:U:0:f7_s1_b2_t5_hnohair|" +

                //Montipo Lorenzo - gk
                "Lore Motipom:Italy:79:49:L:2300:f0_s1_b2_t2_hblack3|" +
                //Hetemaj Perparim - mid
                "Parim Hamelaj:Finland:57:55:L:5000:f8_s1_b2_t9_hblack8|" +
                //Glik Kamil - def
                "Kamil Glicek:Poland:75:68:L:2000:f9_s1_b0_t0_hblonde3|" +                
                //Ionita Artur - mid
                "Arthur Noita:Moldova:57:56:L:2600:f1_s1_b2_t7_hred4|" +
                //Lapadula Gianluca - att
                "Gianni Lapulap:Peru:55:71:L:2600:f2_s1_b0_t0_hblack5|" +
                //Caprari Gianluca - att
                "Gian Caparip:Italy:53:70:L:3500:f3_s1_b0_t0_hblack5",
                "shoeblack|shoeblackred",
                "gloveredwhite|gloveredblack"},
             new string[] {
                 "Cagliari FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                 "redblueshirt",
                 "darkblueyellowstripeshort",
                 "blueredstripe",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Giorgio Junior
                "Giorgio Junior:Italy:60:60:U:0:f4_s1_b2_t8_hblack6|" +


                 //Cragno Alessio - gk
                "Aessio Cagnor:Italy:77:55:L:2000:f6_s1_b0_t0_hblack5|" +
                 //Lykogiannis Charalampos - def
                "Lyginisnnios:Greece:64:59:L:5000:f5_s1_b2_t5_hblack3|" +              
                 //Walukiewicz Sebastian - def
                "Seba Walowicz:Poland:69:60:L:2300:f7_s1_b2_t5_hblack10|" +
                 //Joao Pedro - att
                "Pero Jaoo:Brazil:62:87:L:2600:f8_s4_b2_t4_hblack8|" +
                 //Simeone Giovanni att
                "Vanni Semone:Argentina:61:76:L:2600:f9_s3_b2_t7_hblack10|" +
                 //Godin Diego - def
                "Tiago Goin:Uruguay:70:60:L:2600:f0_s1_b2_t2_hblack1|" +
                 //Zappa Gabriele - def
                "Gabriele Zupe:Italy:65:58:L:3500:f1_s1_b0_t0_hblack3",
                "shoeblackred|shoered",
                "gloveblacblue|gloveredblack"},
             new string[] {
                 "Florence Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "purple|purple|purple",
                "purplewithwhiteshirt",
                 "whitepurplestripesshort",
                 "purplewhitesocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Giovanni Jr
                "Giovanni Jr:Italy:60:60:U:0:f2_s1_b0_t0_hblack7|" +

                //Dragowski Bartlomiej - gk
                "Bart Dranowski:Poland:89:62:L:2000:f4_s2_b2_t9_hblack5|" +
                 //Biraghi Cristiano - def
                "Cris Bagir:Italy:70:63:L:5000:f3_s1_b2_t4_hblack10|" +               
                 //Milenkovic Nikola - def
                "Nik Molenovic:Serbia:69:63:L:2300:f5_s2_b0_t0_hblack10|" +
                 //Vlahovic Dusan - att
                "Dusan Vlaocic:Serbia:65:85:L:2600:f6_s1_b0_t0_hblack13|" +
                 //Callejon Jose - att
                "Joe Coleon:Spain:63:81:L:2600:f7_s1_b2_t5_hblack9|" +
                 //Ribery Franck - mid
                "Frank Rabreri:France:85:83:L:3500:f8_s1_b2_t9_hblack3",
                 "shoewhiteblack",
                "gloveblacblue"},
             new string[] {
                 "Verona Town",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|yellowblue|yellowblue",
                "darkblueyellowstripeshirt",
                 "darkblueyellowstripeshort",
                 "blueyellowstripesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Giulio Junior
                "Giulio Junior:Italy:60:60:U:0:f9_s1_b2_t6_hblack8|" +

                //Silvestri Marco - gk
                "Maro Sisri:Italy:83:63:L:2000:f2_s1_b0_t0_hblack5|" +
                //Dawidowicz Pawel - def
                "Pawel Danowicz:Poland:78:65:L:5000:f1_s2_b0_t0_hred3|" +              
                //Tameze Adrien - def \\TO DO\\
                "Adrian Temizer:France:79:64:L:2300:f3_s5_b2_t7_hblack12|" +
                //Zaccagni Mattia - mid
                "Mat Zacangi:Italy:70:70:L:2600:f4_s1_b2_t8_hblack10|" +
                //Kalinic Nikola - att
                "Nik Kavicic:Croatia:65:73:L:2600:f5_s1_b0_t0_hblack1|" +
                //Barak Antonin - mid \\TO DO brasz??\\
                "Anton Brasz:Czechia:76:75:L:3500:f6_s1_b0_t0_hblonde5",
              "shoeblack|shoeyellow",
                "gloveyellowblack|gloveyellowblack"},
             new string[] {
                 "Roma Blue",
                 "60",
                 "58",
                 "0",
                 "0",
                 "lightblue|lightblue|lightblue",
                "lightbluewithwhiteshirt",
                 "whiteshort",
                 "whitelightbluestripesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Giuseppe Jr
                "Giuseppe Jr:Montenegro:60:60:U:0:f7_s1_b0_t0_hred5|" +

                 //Reina Pepe - gk
                "Pep Rona:Spain:89:70:L:2000:f9_s1_b2_t6_hnohair|" +
                 //Marusic Adam - mid
                "Marisic:Montenegro:80:75:L:5000:f8_s1_b0_t0_hblack3|" +                
                 //Luiz Felipe - def
                "Filipe Laiuz:Brazil:88:77:L:2300:f0_s1_b0_t0_hblack5|" +
                 //Alberto Luis - mid
                "Luise Abertino:Spain:81:80:L:2600:f1_s1_b2_t7_hblack8|" +
                 //Caicedo Felipe - att
                "Filipe Cocedon:Ecuador:71:85:L:2600:f2_s5_b2_t3_hblack5|" +
                 //Immobile Ciro - att
                "Cirillo Imollile:Italy:72:93:L:3500:f3_s1_b0_t0_hblonde4",
                "shoeblue",
                "glovelightbluewhite"},
             new string[] {
                 "Parma Town",
                 "60",
                 "58",
                 "0",
                 "0",
                 "yellow|yellowblue|yellowblue",
                "whiteblackshirt",
                 "blackwhitestripeshorwertshort",
                 "whiteblackstripedownsocks",
                "skin_white_1",
                 "hair_19",
                 "yellow",
                //Leonardo Jr
                "Valerius Jr:Italy:60:60:U:0:f4_s1_b0_t0_hblonde4|" +


                //Sepe Luigi - gk
                "Luca Sopes:Italy:79:49:L:5000:f5_s1_b2_t5_hnohair|" +
                //Alves Bruno - def
                "Breno Avels:Portugal:79:70:L:2000:f6_s5_b0_t0_hblack8|" +
                 //Gagliolo Riccardo - def
                "Riardo Galioli:Sweden:75:55:L:2300:f7_s1_b0_t0_hblonde3|" +
                 //Hernani - mid
                "Henarni:Brazil:71:78:L:2600:f8_s4_b2_t4_hblack3|" +
                 //Gervinho - att
                "Gevithon:Ivory Coast:54:70:L:2600:f9_s5_b2_t9_hblack13|" +
                 //Karamoh Yann - att
                "Kamarhon:France:51:72:L:3500:f0_s5_b0_t0_hblack3",
                "shoewhiteblack",
                "globewhiteblack|gloveblackwhite"},
             //Sampodria
             new string[] {
                 "Genoa Blue",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                 "bluetricolourstripesshirt",
                 "whiteshort",
                 "whiteblustripesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Lorenzo Junior
                "Lorenzo Junior:Italy:60:60:U:0:f1_s1_b2_t7_hblack9|" +

                //Emil Audero - gk
                "Emilo Dueroa:Italy:80:61:L:2300:f4_s1_b2_t8_hblack5|" +
                //Bartosz Bereszynski - def
                "Bartek Baszyski:Poland:75:70:L:5000:f2_s1_b2_t8_hblack3|" +
                 //Jakub Jankto - mid
                "Kuba Jungtons:Czechia:76:71:L:2000:f3_s2_b0_t0_hblonde3|" +               
                 //Mikkel Damsgaard - att
                "Michael Mastagat:Denmark:60:69:L:2600:f5_s2_b2_t6_hblonde4|" +
                 //Keita Baldé - att
                "Keith Bold:Senegal:64:71:L:2600:f6_s5_b2_t4_hblack3|" +
                 //Morten Thorsby - mid
                "Mor Tahsbay:Norway:73:72:L:3500:f7_s3_b0_t0_hblonde4",
                "shoebluewhite|shoered",
                "glovebluewhite"},
             new string[] {
                 "La Spezia",
                 "60",
                 "58",
                 "0",
                 "0",
                 "black|black|black",
                "whitewithblack",
                 "black",
                 "blackwhitestripesverticalsocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Luca Junior
                "Luca Junior:Italy:60:60:U:0:f8_s1_b0_t0_hblack2|" +


                 //Provedel Ivan - gk
                "Ivo Povell:Italy:77:55:L:5000:f9_s1_b0_t0_hred4|" +
                 //Chabot Julian - def
                "Julius Ahbott:Germany:75:59:L:2000:f0_s1_b2_t2_hblack3|" +
                 //Pobega Tommaso - mid
                "Tom Obepa:Italy:65:64:L:2600:f1_s1_b2_t7_hred3|" +
                 //Nzola MBala - att
                "Bale Ozolan:Angola:60:76:L:2300:f2_s5_b2_t3_hblack5|" +
                 //Farias Diego - att
                "Diogo Fahias:Brazil:59:69:L:2600:f3_s4_b2_t6_hblack10|" +
                 //Estevez Nahuel - mid
                "Miguel Esvtez:Argentina:65:63:L:3500:f4_s2_b0_t0_hblonde1",
                 "shoeblackred|shoewhiteblack",
                "globewhiteblack|gloveblackwhite"},
             new string[] {
                 "Udine",
                 "60",
                 "58",
                 "0",
                 "0",
                 "black|black|black",
                 "whiteblackshirt1",
                 "whiteshort",
                 "blackwhitestripeupsocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Luigi Junior
                "Luigi Junior:Italy:60:60:U:0:f5_s1_b2_t5_hblonde5|" +


                //Musso Juan - gk
                "Julian Mosson:Argentina:80:59:L:5000:f6_s1_b0_t0_hblack3|" +
                 //Samir - def
                "Sarims:Brazil:75:65:L:2000:f7_s5_b0_t0_hnohair|" +
                 //De Paul Rodrigo - mid
                "Rodri Paulo:Argentina:72:71:L:2300:f8_s1_b0_t0_hblack10|" +
                 //Nestorovski Ilija - att
                "Il Naotovski:Macedonia:61:74:L:2600:f9_s1_b2_t6_hblack1|" +
                 //Okaka Stefano - att
                "Stefan Kaka:Italy:61:73:L:2600:f0_s5_b0_t0_hblack1|" +
                 //Pereyra Roberto - mid
                "Robert Eyara:Argentina:73:72:L:3500:f1_s4_b2_t9_hblack3",
                "shoeblackred|shoewhiteblack",
                "globewhiteblack|gloveblackwhite"},
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
 ITALIAN NAME NOT TAKEN
 Marco
Matteo
Mattia
Michele
Nathan
Nicola
Nicolo
Pietro
Raffaele
Riccardo
Salvatore
Samuel
Simone
Stefano
Thomas
Tommaso
Vincenzo
*/
