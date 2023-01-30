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
    public class ChampCupTeams
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
         */
        private string[][] teams = new string[][] {
             new string[] {
                 "Warsaw FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "green|green|green",
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
            //sporting lisbona
            new string[] {
                "Lisboa Greens",
                "59",
                "65",
                "8",
                "0",
                "green|green|green",
                "whitegreenstripeshorshirt",
                "black",
                "greenwhitestripessocks",
                "skin_white_7",
                "hair_9",
                "blue",
                //Junior
                "Adriano Junior:Portugal:60:60:U:0:f4_s1_b2_t8_hblack8|" +

                //Adan Antonio gk
                "Anti Anada:Spain:86:70:L:5000:f5_s1_b2_t5_hblack5|" +
                 //Coates Sebastian def
                "Sebos Critos:Uruguay:82:72:L:5000:f6_s1_b2_t9_hblack4|" +
                 //Santos Nuno att
                "Ninio Santi:Portugal:63:78:L:2000:f7_s1_b0_t0_hblack3|" +
                 //Matheus Nunes mid
                "Matteo:Brazil:78:74:L:2300:f8_s1_b0_t0_hblack1|" +
                 //Pedro Goncalves mid
                "Peter Grinels:Portugal:63:84:L:2600:f9_s1_b2_t6_hblack10|" +
                 //Tomas Tiago att
                "Jake Tomies:Portugal:63:79:L:3500:f0_s1_b0_t0_hblack8",
                "shoebluewhite",
                "glovebluewhite"},
            new string[] {
                "Lisboa Reds",
                "59",
                "65",
                "8",
                "0",
                "red|red|red",
                "redblackstripesshirt",
                "whiteblackstripeshort",
                "redblackstripessocks",
                "skin_white_7",
                "hair_9",
                "blue",
                //Junior
                "Marcio Junior:Portugal:60:60:U:0:f1_s1_b0_t0_hblack7|" +

                //Vlachodimos Odisseas gk
                "Odsi Vrachlodis:Greece:83:68:L:5000:f2_s1_b0_t0_hblack3|" +
                 //Grimaldo Alex def
                "Adi Garmedol:Spain:85:71:L:5000:f3_s1_b2_t4_hblack3|" +
                 //Pizzi mid
                "Puzo:Portugal:83:82:L:2000:f4_s1_b2_t8_hblack5|" +
                 //Silva Rafa mid
                "Rafi Salver:Portugal:77:75:L:2300:f5_s1_b2_t5_hblack8|" +
                 //Waldschmidt Gian-Luca att
                "Lucas Wramot:Germany:61:79:L:2600:f6_s1_b2_t9_hred3|" +
                 //Seferovic Haris att
                "Hary Surfolic:Switzerland:60:84:L:3500:f7_s3_b2_t6_hblack1",
                "shoeredwhite|shoeblackred",
                "gloveredblack|gloveredwhite"},
             new string[] {
                 "Paris Club",
                 "51",
                 "50",
                 "0",
                 "0",
                "red|rednavyblue|rednavyblue",
                "darkbluestripehor",
                "darkblueshort",
                "darkblueredstripes",
                "skin_white_1",
                 "hair_4",
                 "green",

                 //Junior
                "Adrien Junior:France:60:60:U:0:f8_s1_b2_t9_hblack6|" +

                //Navas Keylor gk
                "Kali Nevris:Costa Rica:95:80:L:5000:f9_s1_b0_t0_hblonde1|" +
                //Kimpembe Presnel def
                "Parsi Krambo:France:92:90:L:5000:f1_s5_b2_t10_hblack5|" +
                //Marquinhos def
                "Marcoso:Brazil:91:89:L:5000:f2_s1_b0_t0_hblack8|" +
                //Draxler Julian mid
                "Jules Darxlorer:Germany:91:90:L:2000:f3_s2_b0_t0_hblack3|" +
                 //Di Maria Angel mid
                "Angelo Morita:Argentina:93:92:L:2300:f4_s1_b0_t0_hblack3|" +
                 //Marco Verratti - mid
                "Mark Venari:Italy:92:92:L:2300:f5_s2_b0_t0_hblack4|" +
                 //Mbappe Kylian - att
                "Kali Mupae:France:89:97:L:2600:f6_s5_b0_t0_hblack6|" +
                 //Neymar - att
                "Neuman:Brazil:80:96:L:3500:f7_s4_b2_t7_hblack5",
                //Mauro Icardi - att
                //"Marco Acandir:Argentina:83:93:L:3500:f0_s1_b2_t2_hblack3",
                "shoebluewhite",
                "gloveblacblue|glovebluewhite"},
             new string[] {
                 "Lyon Club",
                 "59",
                 "65",
                 "8",
                 "0",
                "white|claretdarkblue|claretdarkblue",
                "whitewithblueshirt",
                 "whiteredstripesshort",
                 "whiteredstripessocks1",
                 "skin_white_7",
                 "hair_9",
                 "blue",
                 //Arthur Junior
                "Alexandre Jr:France:60:60:U:0:f9_s1_b0_t0_hblonde1|" +

                //Lopes Anthony gk
                "Anti Lupaes:Portugal:85:70:L:5000:f0_s1_b2_t2_hblack1|" +
                 //Depay Memphis att
                "Mefi Drapoe:Netherlands:70:89:L:5000:f1_s4_b2_t9_hblack3|" +
                //Ekambi Karl Toko att
                "Carlo Eekundia:Cameroon:70:85:L:2000:f2_s5_b2_t3_hnohair|" +
                 //Paqueta Lucas mid
                "Luca Pikesa:Brazil:85:81:L:2300:f3_s4_b2_t6_hblack8|" +
                 //Marcelo def
                "Mancrulo:Brazil:83:74:L:2600:f4_s5_b0_t0_hblack3|" +
                 //Aouar Houssem mid
                "Hasi Aruaro:France:79:78:L:3500:f5_s2_b0_t0_hblack5",
                "shoebluewhite|shoeredwhite",
                "glovebluewhite|gloveredwhite"},
             new string[] {
                 "Marseille FC",
                 "59",
                 "65",
                 "8",
                 "0",
                "lightblue|lightblue|lightblue",
                "whitewithblueshirt",
                 "whiteshort",
                 "whitebluesocks",
                 "skin_white_7",
                 "hair_9",
                 "blue",

                 //Raphael Junior
                "Raphael Junior:France:60:60:U:0:f9_s1_b0_t0_hblack5|" +

                //Mandanda Steve gk
                "Stiv Mendrana:France:83:68:L:5000:f7_s5_b0_t0_hblack3|" +
                 //Alvaro González def
                "Avluro:Spain:78:65:L:5000:f8_s1_b2_t9_hblack10|" +
                 //Gueye Pape mid
                "Pat Gaja:France:71:70:L:2000:f9_s5_b0_t0_hblack3|" +
                 //Payet Dimitri mid
                "Demetr Pujot:France:70:87:L:2300:f0_s5_b2_t6_hblack15|" +
                 //Thauvin Florian att
                "Flori Traviun:France:68:81:L:2600:f1_s1_b0_t0_hblack5|" +
                 //Arkadiusz Milik - att
                "Arek Mika:Poland:70:83:L:3500:f2_s1_b0_t0_hblack10",
                "shoebluewhite",
                "glovebluewhite"},
             new string[] {
                 "Porto Blue",
                 "59",
                 "65",
                 "8",
                 "0",
                "blue|blue|blue",
                "whitebluestripesshirt",
                 "blueshort",
                 "whitebluestripesocks",
                 "skin_white_7",
                 "hair_9",
                 "blue",
                 //Jose Junior
                "Jose Junior:Portugal:60:60:U:0:f3_s1_b0_t0_hblack4|" +

                //Marchesin Agustin gk
                "August Mehenisn:Argentina:85:73:L:5000:f4_s1_b0_t0_hblack3|" +
                 //Pepe def
                "Popo:Portugal:89:76:L:5000:f5_s1_b2_t5_hnohair|" +
                 //Taremi Mehdi att
                "Meh Aeramit:Iran:65:86:L:2000:f6_s1_b2_t9_hblack5|" +
                 //João Mário Neto Lopes mid
                "Mario Lupiero:Portugal:79:82:L:2300:f7_s3_b2_t6_hblack10|" +
                 //Sergio Oliveira mid
                "Serho Liverast:Portugal:83:82:L:2600:f8_s1_b2_t9_hblack3|" +
                //Marega Moussa att
                "Maso Mugeara:Mali:70:82:L:3500:f9_s5_b2_t9_hblack1",
                "shoebluewhite",
                "glovebluewhite"},
             new string[] {
                 "Amsterdam FC",
                 "59",
                 "65",
                 "8",
                 "0",
                "red|red|red",
                "whiteredshirt1",
                 "whiteredstripeshort",
                 "whiteredstripessocks1",
                 "skin_white_7",
                 "hair_9",
                 "blue",
                 //Daan Junior
                "Daan Junior:Netherlands:60:60:U:0:f0_s1_b2_t2_hblonde2|" +

                //Onana Andre gk
                "Andi Ananad:Cameroon:87:70:L:5000:f1_s5_b0_t0_hblack14|" +
                 //Martinez Lisandro def
                "Lis Artonezm:Argentina:85:70:L:5000:f2_s2_b0_t0_hblack10|" +
                 //Gravenberch Ryan mid
                "Ruud Raberhg:Netherlands:82:80:L:2000:f3_s5_b0_t0_hblack3|" +
                 //Klaassen Davy mid
                "Dave Senaklas:Netherlands:80:85:L:2300:f4_s2_b0_t0_hblonde1|" +
                 //Haller Sebastien att
                "Sebi Ahlerh:Ivory Coast:69:83:L:2600:f5_s1_b2_t5_hblack3|" +
                 //Tadic Dusan att
                "Duson Daticv:Serbia:70:89:L:3500:f6_s3_b0_t0_hblack10",
                "shoeredwhite|shoered",
                 "gloveredwhite"},
             //Celtic
             new string[] {
                 "Glasgow Greens",
                 "59",
                 "65",
                 "8",
                 "0",
                "green|green|green",
                "whitegreenstripeshorshirt",
                 "whitewithgreenshort",
                 "whitegreensocks",
                 "skin_white_7",
                 "hair_9",
                 "blue",
                 //Jock Junior
                "Jock Junior:Scotland:60:60:U:0:f7_s1_b0_t0_hblack3|" +

                //Bain Scott gk
                "Scot Aian:Scotland:75:59:L:5000:f8_s1_b2_t9_hblack3|" +
                 //Ajer Kristoffer def
                "Chris Jajed:Norway:76:65:L:5000:f9_s3_b0_t0_hblonde2|" +
                 //Christie Ryan mid
                "Chris Ray:Scotland:69:67:L:2000:f0_s2_b0_t0_hblack10|" +
                 //McGregor Callum mid
                "Greg Cal:Scotland:70:67:L:2300:f1_s1_b0_t0_hblack5|" +
                 //Elyounoussi Mohamed - att
                "Moha Yononasyo:Norway:64:77:L:2600:f2_s2_b0_t0_hblack5|" +
                 //Edouard Odsonne att
                "Odso Guard:France:60:81:L:3500:f3_s5_b2_t7_hblack5",
                "shoegreen",
                "gloveblackgreen|glovegreenwhite"},
             new string[] {
                 "Donetsk Team",
                 "59",
                 "65",
                 "8",
                 "0",
                 "orange|orangeblack|orangeblack",
                 "orangeblackstripesshirt",
                 "black",
                 "orangeblackstripesocks",
                 "skin_white_7",
                 "hair_9",
                 "blue",
                 //Dmytro Junior
                "Dmytro Junior:Ukraine:60:60:U:0:f4_s1_b2_t8_hblonde3|" +

                //Trubin Anatolii gk
                "Anat Urnin:Ukraine:85:65:L:5000:f5_s2_b0_t0_hblack3|" +
                 //Dodo def
                "Dudulinvo:Brazil:79:70:L:5000:f6_s4_b2_t3_hblack1|" +
                 //Maycon - mid
                "Raycron:Brazil:77:75:L:2000:f7_s4_b2_t7_hblack3|" +
                 //Alan Patrick - mid
                "Pat Lanala:Brazil:76:75:L:2300:f8_s4_b0_t0_hblack3|" +
                 //Moraes Junior att
                "Marolet:Ukraine:69:78:L:2600:f9_s3_b2_t7_hblack5|" +
                 //Solomon Manor att
                "Mano Olsons:Israel:70:80:L:3500:f0_s1_b2_t2_hblack10",
                "shoeorange",
                "gloveblackorange"},
             new string[] {
                 "Eindhoven Team",
                 "59",
                 "65",
                 "8",
                 "0",
                "red|red|red",
                "redwhitestripes",
                "blacredstripeshort",
                "whiteredstripessocks1",
                "skin_white_7",
                "hair_9",
                "blue",
                 //Walter Junior
                "Walter Junior:Netherlands:60:60:U:0:f1_s1_b2_t7_hblonde5|" +

                //Mvogo Yvon gk
                "Von Vmogvom:Switzerland:77:55:L:5000:f2_s5_b2_t3_hblack1|" +
                 //Boscagli Olivier - def
                "Oli Oshalig:France:75:67:L:5000:f3_s2_b0_t0_hblack5|" +
                 //Max Philipp def
                "Phil Maxwells:Germany:73:62:L:2000:f4_s1_b2_t8_hblonde1|" +
                 //Gotze Mario mid
                "Marion Gozteg:Germany:85:84:L:2300:f5_s1_b0_t0_hblonde2|" +
                 //Zahavi Eran att
                "Erva Vehazii:Israel:65:81:L:2600:f6_s3_b2_t10_hblack3|" +
                 //Malen Donyell att
                "Davey Amenl:Netherlands:63:85:L:3500:f7_s1_b2_t5_hblack5",
                "shoeredwhite|shoeblackred",
                "gloveredwhite|gloveredblack"},
             new string[] {
                 "Piraeus Club",
                 "59",
                 "65",
                 "8",
                 "0",
                "red|red|red",
                "redwhitestripes",
                "redyellowstripeshort",
                "redyellowstripessocks",
                "skin_white_7",
                "hair_9",
                "blue",
                 //Nikos Junior
                "Nikos Junior:Greece:60:60:U:0:f8_s1_b0_t0_hblack1|" +

                //Jose Sa gk
                "Joseph:Portugal:75:50:L:5000:f9_s1_b2_t6_hred3|" +
                 //Semedo Ruben def
                "Rubin Emesdor:Netherlands:75:56:L:5000:f0_s5_b2_t6_hblack13|" +
                 //M'Vila Yann mid
                "Ian Ovinlla:France:69:67:L:2000:f1_s5_b2_t10_hblack3|" +
                 //Fortounis Konstantinos mid
                "Konstan Four:Greece:65:63:L:2300:f2_s1_b2_t8_hblack5|" +
                 //Masouras Georgios att
                "Greg Asosar:Greece:57:78:L:2600:f3_s1_b2_t4_hblack10|" +
                 //El Arabi Youssef att
                "Joe Rabira:Morocco:61:84:L:3500:f4_s5_b0_t0_hblack1",
                "shoered",
                "gloveredwhite"},
             new string[] {
                 "Brugge FC",
                 "59",
                 "65",
                 "8",
                 "0",
                "blue|blueblack|blueblack",
                "blackbluestripesshirt",
                 "blackbluestripesshort",
                 "blackbluestripessocks",
                 "skin_white_7",
                 "hair_9",
                 "blue",
                 //Noah Junior
                "Jan Junior:Belgium:60:60:U:0:f5_s1_b2_t5_hblack2|" +

                //Mignolet Simon gk
                "Sim Onoletm:Belgium:73:48:L:5000:f6_s1_b2_t9_hblack5|" +
                 //Mechele Brandon def
                "Brand Ehellems:Panama:70:55:L:5000:f7_s1_b2_t5_hblack3|" +
                 //Vormer Ruud mid
                "Roy Ovmerv:Netherlands:70:70:L:2000:f8_s3_b2_t10_hblack5|" +
                 //Vanaken Hans mid
                "Han Anksent:Belgium:75:75:L:2300:f9_s3_b0_t0_hblack3|" +
                 //Dost Bas att
                "Baz Osdst:Netherlands:55:73:L:2600:f0_s1_b2_t2_hnohair|" +
                 //Lang Noa att
                "Noah Nulg:Netherlands:67:82:L:3500:f1_s1_b0_t0_hred2",
                "shoeblue",
                "gloveblacblue"},
             new string[] {
                 "Zagreb City",
                 "59",
                 "65",
                 "8",
                 "0",
                "blue|blue|blue",
                "bluewithwhiteshirt",
                "darkbluewhitestripes",
                "darkbluewhitestripessocks",
                "skin_white_7",
                "hair_9",
                "blue",
                //Marko Junior
                "Marko Junior:Croatia:60:60:U:0:f2_s1_b0_t0_hblack3|" +

                 //Livakovic Dominik gk
                "Domi Avakovinc:Croatia:79:49:L:5000:f3_s1_b0_t0_hblack4|" +
                 //Theophile-Catherine Kevin def
                "Kev Theodor:France:68:53:L:5000:f4_s5_b2_t4_hnohair|" +
                 //Gvardiol Josko def
                "Juke Guarand:Croatia:70:54:L:2000:f5_s1_b2_t5_hblack4|" +
                 //Ivanusec Luka mid
                "Lukas Invasic:Croatia:61:60:L:2300:f6_s3_b0_t0_hblack9|" +
                 //Majer Lovro mid
                "Loro Muhler:Croatia:65:63:L:2600:f7_s1_b2_t5_hred5|" +
                 //Gavranovic Mario att
                "Marius Grovic:Switzerland:58:77:L:2600:f8_s3_b0_t0_hblack10|" +
                 //Orsic Mislav att
                "Miso Oarkic:Croatia:59:78:L:3500:f9_s1_b0_t0_hblack3",
                "shoebluewhite",
                "glovebluewhite"},
             new string[] {
                "Istanbul FC",
                 "59",
                 "65",
                 "8",
                 "0",
                "claret|claretorange|claretorange",
                "darkredwithyellowshirt",
                "redyellowstripeshort",
                "redwithyellow",
                "skin_white_7",
                "hair_9",
                "blue",
                //Mustafa Junior
                "Mustafa Junior:Turkey:60:60:U:0:f0_s1_b2_t2_hblack4|" +

                 //Muslera Fernando gk
                "Fernand Useram:Uruguay:82:58:L:5000:f1_s1_b0_t0_hblack3|" +
                 //Saracchi Marcelo def
                "Marcel Arahiss:Uruguay:79:61:L:5000:f2_s1_b2_t8_hblack5|" +
                 //Marcao def
                "Markolinhvo:Brazil:79:63:L:2000:f3_s5_b2_t7_hnohair|" +
                 //Antalyali Taylan mid
                "Dylan Ynatalil:Turkey:70:69:L:2300:f4_s2_b0_t0_hblack1|" +
                 //Kilinc Emre mid
                "Em Inic:Turkey:73:70:L:2600:f5_s1_b2_t5_hblack10|" +
                 //Babel Ryan att
                "Bryan Arbel:Netherlands:69:78:L:2600:f6_s5_b2_t4_hnohair|" +
                 //Falcao att
                "Foncaloos:Colombia:70:85:L:3500:f7_s1_b0_t0_hblack5",
                "shoeorange|shoered",
                "gloveblackorange"},
             new string[] {
                "Moscow Greens",
                "59",
                "65",
                "8",
                "0",
                "green|redgreen|redgreen",
                "greenwithwhiteshirt",
                "redblackstripesshort",
                "greensocks",
                "skin_white_7",
                "hair_9",
                "blue",
                //Dmitrij Junior
                "Dmitrij Junior:Russia:60:60:U:0:f8_s1_b0_t0_hblack11|" +

                 //Guilherme gk
                "Ligarmer:Russia:77:49:L:5000:f9_s1_b2_t6_hblack4|" +
                 //Ignatjev Vladislav att
                "Vladi Inajevs:Russia:59:70:L:5000:f0_s2_b0_t0_hblonde1|" +
                 //Cerqueira Murilo def
                "Mur Ereuirar:Brazil:75:58:L:2000:f1_s4_b2_t9_hblack5|" +
                 //Miranchuk Anton mid
                "Anthony Arakukv:Russia:67:65:L:2300:f2_s2_b0_t0_hblack8|" +
                 //Krychowiak Grzegorz mid
                "Greg Krakowski:Poland:83:84:L:2600:f3_s1_b2_t4_hblack15|" +
                 //Eder att
                "Adrere:Portugal:59:74:L:2600:f4_s5_b2_t4_hblack5|" +
                 //Smolov Fedor att
                "Fidor Malovlsk:Russia:69:78:L:3500:f5_s2_b2_t6_hblack9",
                "shoered|shoegreen",
                "gloveblackgreen|gloveredblack"},
             new string[] {
                "Monaco Club",
                "59",
                "65",
                "8",
                "0",
                "red|red|red",
                "whiteredshirt2",
                "redwhitestripesshort",
                "whiteredstripesocks1",
                "skin_white_7",
                "hair_9",
                "blue",
                //Louis Junior
                "Charles Junior:France:60:60:U:0:f6_s1_b2_t9_hblack10|" +

                 //Lecomte Benjamin gk
                "Ben Lacomit:France:80:58:L:1200:f7_s1_b2_t5_hblonde2|" +
                 //Jovetic Stevan att
                "Stiv Juvotic:Montenegro:59:73:L:5000:f8_s3_b2_t10_hblack5|" +
                 //Maripan Guillermo def
                "Giu Mepiran:Chile:79:63:L:2000:f9_s1_b2_t6_hblack3|" +
                 //Golovin Aleksandr mid
                "Alex Glevkin:Russia:74:73:L:2300:f0_s2_b0_t0_hblack9|" +
                 //Diop Sofiane mid
                "Sofe Dropie:France:78:77:L:2600:f1_s4_b0_t0_hblack5|" +
                 //Volland Kevin att
                "Kavi Veliend:Germany:70:80:L:2600:f2_s1_b2_t8_hblack5|" +
                 //Ben Yedder Wissam att
                "Wiso Yadror:France:70:83:L:3500:f3_s1_b2_t4_hblack8",
                "shoeredwhite|shoeblackred",
                "gloveredwhite"},
             new string[] {
                "Moscow Red",
                "59",
                "65",
                "8",
                "0",
                "claret|claretdarkblue|claretdarkblue",
                "blueredshirt",
                "darkblueredstropedn",
                "darkblue",
                "skin_white_7",
                "hair_9",
                "blue",
                //Anton Junior
                "Anton Junior:Russia:60:60:U:0:f4_s1_b0_t0_hblonde4|" +

                //Akinfeev Igor gk
                "Igi Anokiv:Russia:78:57:L:5000:f5_s2_b2_t6_hblack3|" +
                 //Igor Diveev Igor def
                "Igi Davoyev:Russia:76:61:L:2000:f6_s1_b0_t0_hblack4|" +
                 //Kuchaev Konstantin mid
                "Kosti Kovachov:Russia:73:72:L:2300:f7_s3_b0_t0_hblonde2|" +
                 //Vlasic Nikola mid
                "Niko Valorac:Croatia:78:80:L:2600:f8_s1_b2_t9_hblack5|" +
                 //Rondon Salomon att
                "Sali Ramand:Venezuela:70:73:L:2600:f9_s5_b2_t9_hnohair|" +
                 //Chalov Fedor att
                "Fioder Cloav:Russia:70:75:L:3500:f0_s2_b0_t0_hblack5",
                "shoeblackred|shoered",
                "glovebluered|gloveblacblue"},
             new string[] {
                "Petersburg City",
                "59",
                "65",
                "8",
                "0",
                "lightblue|lightblue|lightblue",
                "bluedarkblueshirt",
                "blueshort",
                "bluebluestripesocks",
                "skin_white_7",
                "hair_9",
                 "blue",

                // Aleksiej Junior
                "Aleksiej Junior:Russia:60:60:U:0:f1_s1_b2_t7_hblonde2|" +

                //Kerzhakov Mikail gk
                "Mihaju Kareszkov:Russia:83:61:L:5000:f2_s2_b2_t9_hblack3|" +
                 //Azmoun Sardar att
                "Serdi Amzion:Iran:62:85:L:2000:f3_s1_b2_t4_hblack12|" +
                 //Dzyuba Artem att
                "Atri Duzbua:Russia:63:87:L:2300:f4_s2_b0_t0_hblack1|" +
                 //Mostovoy Andrey mid
                "Andi Mrustvay:Russia:77:76:L:2600:f5_s1_b0_t0_hblack5|" +
                 //Erokhin Aleksandr mid
                "Alex Ehorkin:Russia:76:78:L:2600:f6_s3_b2_t10_hblack4|" +
                 //Santos Douglas def
                "Drogles Santi:Brazil:79:68:L:3500:f7_s4_b0_t0_hblack5",
                "shoebluewhite",
                "glovebluewhite|gloveblacblue"},
             //Vasco da Gama
             new string[] {
                 "Rio V",
                 "59",
                 "65",
                 "8",
                 "0",
                 "black|black|black",
                 "blackwithwhiteshirt",
                 "black",
                 "blackwhitestripe",
                 "skin_white_7",
                 "hair_9",
                 "blue",
                 //Junior
                "Vagner Junior:Brazil:60:60:U:0:f7_s1_b2_t5_hblack13|" +

                //Vanderlei gk
                "Andreils:Brazil:79:49:L:5000:f8_s1_b2_t9_hblack3|" +
                 //Ricardo Graça - def
                "Rich Aracas:Brazil:60:51:L:5000:f9_s1_b2_t6_hblack8|" +
                 //Andrey mid
                "Andrew:Brazil:60:61:L:2000:f1_s4_b2_t9_hblack5|" +
                 //Bruno Gomes mid
                "Brun Cromless:Brazil:65:64:L:2300:f2_s2_b0_t0_hblack3|" +
                 //Talles Magno att
                "Mag Tolsel:Brazil:60:75:L:2600:f3_s1_b0_t0_hblack14|" +
                 //Germán Cano att
                "Ernamo Can:Argentina:50:65:L:3500:f4_s1_b0_t0_hblack10",
                "shoewhiteblack",
                "gloveblackwhite"},
              //Cruzeiro
              new string[] {
                 "B. Horizonte C",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                 "blueshirt",
                 "whiteblueshort",
                 "whitebluesocks",
                 "skin_white_1",
                 "hair_19",
                 "green",
                //Jeferson Jr
                "Jeferson Jr:Brazil:55:55:U:0:f3_s4_b0_t0_hblack12|" +

                //Fábio - gk
                "Fabian:Brazil:71:50:L:5000:f1_s1_b2_t7_hblack10|" +
                //Dedé - def
                "Duedu:Brazil:68:60:L:5000:f2_s4_b2_t10_hnohair|" +
                //Léo - def
                "Leon:Brazil:64:55:L:2000:f3_s1_b2_t4_hblack5|" +
                //Henrique - mid
                "Enrique:Brazil:61:60:L:2300:f4_s1_b2_t8_hblack3|" +
                //Rafael Sóbis - att
                "Raf Osbis:Brazil:45:58:L:2600:f5_s2_b2_t6_hblack8|" +
                //William Pottker - att
                "Willi Proterk:Brazil:49:65:L:3500:f6_s1_b0_t0_hblack2",
                "shoebluewhite",
                "glovebluewhite"},
             new string[] {
                "Salzburg FC",
                "59",
                "65",
                "8",
                "0",
                "red|red|red",
                "whiteredshirt2",
                "redshort",
                "whiteredstripe",
                "skin_white_7",
                "hair_9",
                "blue",

                //Franz Junior
                "Franz Junior:Russia:60:60:U:0:f8_s1_b0_t0_hblack8|" +

                //Stankovic Cican gk
                "Cris Satkoric:Austria:80:58:L:5000:f9_s3_b2_t7_hblack5|" +
                 //Kristensen Rasmus def
                "Rosi Karisten:Denmark:79:59:L:2000:f0_s2_b0_t0_hblonde1|" +
                 //Aaronson Brendan mid
                "Ben Anronson:USA:71:70:L:2300:f1_s1_b0_t0_hblack8|" +
                 //Mwepu Enock mid
                "Ecnuk Moipi:Zambia:70:69:L:2600:f2_s5_b2_t3_hblack1|" +
                 //Berisha Mergim att
                "Margu Brashi:Germany:61:79:L:2600:f3_s1_b2_t4_hblack5|" +
                 //Daka Patson att
                "Peter Diaku:Zambia:64:85:L:3500:f4_s5_b0_t0_hblack1",
                "shoeredwhite",
                "gloveredwhite"},
               new string[] {
                 "Barcelona Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                "barcelona",
                 "blueredstripes",
                 "darkblueredstripes",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Hugo Junior
                "Hugo Junior:Spain:60:60:U:0:f0_s1_b0_t0_hblack4|" +

                //Ter Stegen - gk
                "Marko Stigen:Germany:94:83:L:5000:f1_s2_b0_t0_hblonde3|" +
                //Pique Gerard def
                "Gerald Parqet:Spain:93:85:L:7500:f2_s1_b2_t8_hblack10|" +                
                //Coutinho Philippe - mid
                "Phil Carinios:Brazil:89:89:L:7500:f3_s1_b0_t0_hblack5|" +
                //Ousmane Dembele - att
                "Osmi Deble:France:84:92:L:7900:f5_s1_b0_t0_hblack6|" +
                //Frankie de Jong - mid
                "Frank Jog:Netherlands:95:89:L:8100:f5_s2_b0_t0_hblonde2|" + 
                //Antoine Griezmann - att
                "Anti Grizmo:France:84:90:L:7500:f6_s1_b2_t9_hblack15|" +
                 //Leo Messi - att
                "Lio:Argentina:85:98:L:9700:f8_s1_b2_t9_hblack5",
                "shoeblue|shoered",
                "glovebluered|glovebluewhite"},
                new string[] {
                 "White Madrid",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|purple|purple",
                "whitegoldstripes",
                 "whitegoldstripesshort",
                 "whitegoldsocks",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Alvaro Junior
                "Alvaro Junior:Spain:60:60:U:0:f5_s1_b2_t5_hred4|" +
                
                //Courtois Thibaut - gk
                "Tibo Curtua:Belgium:91:85:L:5000:f6_s3_b0_t0_hblack8|" +
                //Varane Raphael - def
                "Ralph Vorenere:France:91:86:L:7500:f7_s4_b0_t0_hblack5|" +
                //Marcelo - def
                "Manorceo:Brazil:90:87:L:6900:f8_s4_b2_t4_hblack14|" +
                 //Ramos Sergio - def
                "Serho Raemoes:Spain:97:94:L:8900:f9_s1_b2_t6_hblack5|" +
                 //Modric Luka - mid
                "Luci Moardic:Croatia:86:91:L:8000:f0_s1_b0_t0_hblack13|" +
                 //Benzema Karim - att
                "Kamin Bemeza:France:85:93:L:7500:f1_s1_b2_t7_hblack3|" +
                 //Hazard Eden - att
                 "Ed Azedr:Belgium:87:92:L:7500:f2_s2_b0_t0_hblack5|" +
                 //Kroos Toni - mid
                 "Tom Kos:Germany:95:89:L:7530:f3_s2_b0_t0_hblack6",
                 "shoewhite|shoewhiteblack",
                 "globewhiteblack"},
               new string[] {
                 "Madrid FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "redwhiteshirt",
                 "darkblueredstropedn",
                 "red",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //David Junior
                "David Junior:Spain:60:60:U:0:f4_s1_b2_t8_hred3|" +

                 //Jan Oblak gk
                "Johan Olbuk:Czechia:90:78:L:5360:f6_s1_b2_t9_hblonde3|" +
                 //Felipe def
                "Philipe:Brazil:89:79:L:5360:f5_s4_b2_t7_hblack3|" +              
                 //Correa Angel mid
                "Agen Ceroa:Argentina:88:88:L:5000:f7_s4_b0_t0_hblack5|" +                
                 //Llorente Marcos mid
                "Marki Leront:Spain:90:89:L:6900:f8_s3_b0_t0_hblonde4|" +
                 //Joao Felix att
                "Foleks Juo:Portugal:83:89:L:8300:f9_s4_b2_t8_hblack6|" +
                 //Suarez Luis att
                "Lous Sarezzo:Uruguay:85:91:L:9500:f0_s4_b2_t5_hblack5",
                "shoeredwhite|shoeblackred",
                "gloveredwhite"},
              new string[] {
                 "Sevilla Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "whiteredstripeshirt",
                 "whireredstripe",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Lucas Junior
                "Lucas Junior:Spain:60:60:U:0:f1_s1_b2_t7_hblonde2|" +

                //Bono - gk
                "Bani:Morocco:83:65:L:5000:f2_s4_b0_t0_hblack5|" +
                               
                //Diego Carlos - def
                "Dino Crelus:Brazil:85:73:L:7300:f3_s5_b0_t0_hblack3|" +
                //Navas Jesus - def
                "Joe Novens:Spain:86:77:L:7500:f4_s4_b0_t0_hblack5|" +
                 //Rakitic Ivan - mid
                "Ivo Retic:Croatia:89:88:L:9200:f5_s1_b2_t5_hblack3|" +
                 //de Jong Luuk - att
                "Loki Jog:Netherlands:76:85:L:8300:f6_s3_b0_t0_hblonde3|" +
                 //En Nesyri Youssef - att
                "Jusen Nasor:Morocco:75:91:L:8900:f7_s5_b2_t8_hblack3",
                "shoeredwhite|shoeblackred",
                "gloveredwhite|gloveredblack"},
              new string[] {
                 "Valencia Whites",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|black|orange",
                "whitewithblack",
                 "black",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Mario Junior
                "Mario Junior:Spain:60:60:U:0:f8_s1_b0_t0_hblonde3|" +

                //Domenech Jaume - gk
                "Jun Dimerch:Spain:83:64:L:5000:f0_s4_b2_t5_hblack3|" +
                //Gabriel Paulista - def
                "Paulo Gebrista:Brazil:76:69:L:7500:f9_s1_b0_t0_hblack3|" +                
                 //Soler Carlos - mid
                "Crelus Silor:Spain:81:83:L:7800:f1_s4_b0_t0_hblack8|" +
                 //Gaya Jose - def
                "Jusi Goyas:Spain:74:69:L:7500:f2_s1_b2_t8_hblack10|" +
                 //Gomez Maximiliano - att
                "Maks Griomus:Uruguay:71:79:L:8100:f3_s1_b2_t4_hblack4|" +
                 //Wass Daniel - def
                "Danio West:Denmark:73:70:L:8900:f4_s2_b0_t0_hblonde4",
                "shoeredwhite|shoeblackred",
                "gloveredwhite|gloveredblack"},
              new string[] {
                 "Munchen Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "tshirt_red",
                 "redwhitestripesshort",
                 "redwhitesocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Lukas Junior
                 "Lukas Junior:Germany:60:60:U:0:f4_s1_b0_t0_hblack1|" +

                 //Neuer Manuel GK
                 "Mano Nurer:Germany:97:86:L:5000:f5_s1_b2_t5_hblack2|" +
                 //Alaba David def
                 "Davie Abla:Austria:93:88:L:5000:f6_s5_b0_t0_hblack5|" +
                 //Davies Alphonso
                 "Alonso Denis:Canada:96:88:L:5000:f7_s5_b2_t8_hblack3|" +
                 //Kimmich Joshua def
                 "Josh Kominh:Germany:90:88:L:5000:f8_s1_b0_t0_hblack10|" +
                 //Goretzka Leon mid
                 //"Leo Gozetska:Germany:86:89:L:2000:f9_s3_b2_t7_hblack5|" +
                 //Muller Thomas att
                 "Tommy Millor:Germany:90:92:L:2300:f0_s1_b2_t2_hblack10|" +
                 //Sane Leroy - mid
                 "Lee Zeane:Germany:92:91:L:2300:f1_s5_b2_t10_hblack14|" +
                 //Coman Kingsley att
                 "King Cormen:France:81:94:L:2600:f2_s5_b2_t3_hblack5|" +
                 //Lewandowski Robert att
                 "Robert Lengolski:Poland:86:99:L:3500:f3_s1_b0_t0_hblack10",
                 "shoeredwhite|shoered","gloveredwhite"},
               new string[] {
                 "Leipzig Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "whiteredstripeshirt",
                 "redshort",
                 "redwithyellow",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Michael Jr
                "Michael Jr:Germany:60:60:U:0:f6_s1_b2_t9_hblack2|" +

                //Gulacsi Peter gk
                "Pete Gonluci:Hungary:90:71:L:5000:f7_s3_b2_t6_hblack1|" +
                //Orban Willi def
                "Urben Wally:Hungary:83:75:L:5000:f8_s3_b0_t0_hblack3|" +
                 //Nkunku Christopher mid
                "Chris Kukukul:France:81:81:L:2000:f9_s5_b2_t9_hblack8|" +
                 //Sabitzer Marcel mid
                "Marcelo Zutizner:Austria:89:88:L:2300:f0_s2_b2_t4_hblack3|" +
                //Sorloth Alexander
                "Alex Horloth:Norway:76:81:L:2600:f1_s1_b0_t0_hblonde1|" +
                //Forsberg Emil att
                "Em Osfbeng:Sweden:79:84:L:3500:f2_s1_b2_t8_hblonde6",
                "shoeredwhite|shoered",
                "gloveredwhite"},
               new string[] {
                 "Frankfurt FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|black",
                "blackredstripesshirt",
                 "black",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Fabian Junior
                "Fabian Junior:Germany:60:60:U:0:f0_s1_b2_t2_hblack6|" +


                //Trapp Kevin gk
                 "Kev Trops:Germany:83:68:L:5000:f1_s1_b2_t7_hblonde3|" +
                 //Hinteregger Martin def
                "Mark Hutingor:Austria:78:73:L:5000:f2_s2_b0_t0_hblonde4|" +
                 //Kostic Filip att
                "Phil Krosvinc:Serbia:73:79:L:2000:f3_s1_b2_t4_hblack3|" +
                 //Jovic Luka att
                "Lucas Tonvik:Serbia:70:78:L:2300:f4_s1_b2_t8_hblack5|" +
                 //Kamada Daichi mid
                "Dai Kormoeds:Japan:78:80:L:2600:f5_s1_b0_t0_hblack6|" +
                 //Silva Andre att
                "Andi Solvere:Portugal:79:89:L:3500:f6_s1_b2_t9_hblack10",
                "shoeblackred|shoered",
                "gloveredblack"},
              new string[] {
                 "Leverkusen FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "blackredshirt",
                 "black",
                 "blackredstripesocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Markus Junior
                "Markus Junior:Germany:60:60:U:0:f7_s1_b2_t5_hblack7|" +


                //Hradecky Lukas gk
                "Luc Hrodorek:Finland:85:66:L:5000:f0_s2_b0_t0_hblonde2|" +
                //Tah Jonathan def
                "Jon Taranh:Germany:80:73:L:5000:f1_s5_b2_t10_hblack3|" +
                //Wirtz Florian mid
                "Fred Wzitz:Germany:84:80:L:2000:f2_s1_b0_t0_hblonde3|" +
                //Schick Patrik att
                "Patrick Icki:Czechia:71:82:L:2300:f3_s1_b2_t4_hblack6|" +
                //Bailey Leon att
                "Leo Bunlein:Jamaica:71:83:L:2600:f4_s5_b0_t0_hblack5|" +
                //Alario Lucas att
                "Luke Ariano:Argentina:72:83:L:3500:f5_s1_b2_t5_hblack5",
                "shoeblackred|shoered",
                "gloveredblack"},
              new string[] {
                 "Monchengladbach",
                 "60",
                 "58",
                 "0",
                 "0",
                 "darkgreen|darkgreen|darkgreen",
                "whiteshirt",
                 "whiteshort",
                 "white",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Andreas Jr
                "Andreas Jr:Germany:60:60:U:0:f6_s1_b0_t0_hblack8|" +

                //Sommer Yann gk
                "Jens Summer:Switzerland:80:56:L:5000:f7_s1_b2_t5_hblack4|" +
                //Elvedi Nico def
                "Nick Vedeli:Switzerland:79:70:L:5000:f8_s3_b2_t10_hblack5|" +
                //Ginter Matthias def
                "Mat Gonhern:Germany:77:70:L:2000:f9_s1_b0_t0_hblonde4|" +
                //Neuhaus Florian mid
                "Flo Nahousel:Germany:76:79:L:2300:f0_s2_b0_t0_hblack3|" +
                 //Stindl Lars mid
                "Larry Sinstl:Germany:76:83:L:2600:f1_s1_b0_t0_hblack5|" +
                 //Thuram Marcus att
                "Mark Turanu:France:70:79:L:3500:f2_s5_b2_t3_hblack1",
                "shoeredwhite|shoeredwhite",
                "gloveredwhite"},
             new string[] {
                 "Dortmund FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "yellow|yellowblack|yellowblack",
                 "yellowblackshirt",
                 "blackyellowshort",
                 "yellowblackstripessocks",
                 "skin_white_1",
                 "hair_19",
                 "yellow",
                 //Erik Junior
                "Erik Junior:Germany:60:60:U:0:f3_s1_b0_t0_hblack15|" +

                //Burki Roman - gk
                "Remi Bukir:Switzerland:90:70:L:5000:f2_s1_b2_t8_hblack5|" +
                //Piszczek Łukasz - def
                "Lukasz Spiszek:Poland:88:86:L:5000:f0_s2_b0_t0_hblack10|" +
                //Hummels Mats - def
                "Mati Hamiles:Germany:88:83:L:2000:f1_s1_b2_t7_hblack9|" +
                //Reus Marco mid
                "Markus Ross:Germany:89:87:L:2300:f5_s2_b0_t0_hblonde1|" +
                //Sancho Jadon att
                "Jandi Senhus:England:72:88:L:2600:f7_s1_b2_t5_hblack5|" +
                //Haaland Erling att
                "Eric Hellen:Norway:87:95:L:3500:f6_s1_b0_t0_hblonde1",
                "shoeblack|shoeyellow",
                "gloveyellowblack|gloveyellow"},
              new string[] {
                 "ARL London",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                 "redwitwhiteshirt",
                 "whireredstripe",
                 "redwhitesocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Oliver Junior
                "Oliver Junior:England:60:60:U:0:f3_s4_b2_t6_hblack14|" +

                //Leno Bernd - gk
                "Berndt Lonos:Germany:88:72:L:5000:f2_s2_b0_t0_hblonde3|" +
                //Gabriel - def
                "Abriello:Brazil:88:76:L:5000:f1_s5_b0_t0_hblack3|" +
                //Holding Rob - def
                "Bob Haldaing:England:91:79:L:2000:f0_s1_b0_t0_hblonde4|" +
                //Saka Bukayo - mid
                "Auako Sako:England:75:83:L:2300:f9_s5_b0_t0_hblack10|" +
                //Smith Rowe Emile - mid
                "Row Smile:England:90:80:L:2600:f8_s3_b0_t0_hblack8|" +
                //Aubameyang Pierre-Emerick - att
                "Obayeyoung:Gabon:76:90:L:3500:f7_s5_b2_t8_hblack3|" +
                //Lacazette Alexandre - att
                "Alex Locazet:France:75:92:L:3500:f6_s4_b2_t3_hblack3",
                "shoeredwhite|shoered",
                "gloveredwhite"},
               new string[] {
                 "Chelsea Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                 "darkbluewithblacshirt",
                 "darkbluedarkstripeskort",
                 "white",
                 "skin_white_1",
                 "hair_19",
                  "blue",
                 //George Junior
                "George Junior:England:60:60:U:0:f5_s1_b0_t0_hblack1|" +


                //Mendy Edouard - gk
                "Edo Amendi:Senegal:94:83:L:5000:f4_s5_b2_t4_hblack3|" +
                //Zouma Kurt - def
                "Kert Zomual:France:90:85:L:5000:f3_s5_b2_t7_hblack3|" +
                //Silva Thiago - def
                "Tiago Silivan:Brazil:91:87:L:2000:f2_s1_b0_t0_hblack8|" +
                //Mount Mason - mid
                "Mansin Ment:England:90:85:L:2300:f1_s1_b0_t0_hblack5|" +
                //Kante NGolo - mid
                "Galo Kontae:France:93:88:L:2600:f0_s5_b0_t0_hnohair|" +
                //Werner Timo - att
                "Tim Wenrer:Germany:78:91:L:3500:f9_s3_b0_t0_hblonde3|" +
                //Giroud Olivier - att
                "Oli Garound:France:83:90:L:3500:f8_s1_b2_t9_hblack10",
                "shoebluewhite",
                "glovebluewhite|gloveblacblue"},
             new string[] {
                 "Manchester Blue",
                 "60",
                 "58",
                 "0",
                 "0",
                 "lightblue|lightblue|lightblue",
                "lightblueshirt",
                 "whiteshort",
                 "lightbluewhirestripessocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Noah Junior
                "Noah Junior:England:60:60:U:0:f0_s1_b0_t0_hred1|" +

                //Ederson - gk
                "Edmersen:Brazil:94:85:L:5000:f5_s1_b2_t5_hblack10|" +
                //Stones John - def
                "Johnny Tons:England:93:87:L:5000:f6_s3_b0_t0_hblack9|" +
                //Gundogan Ilkay - mid
                "Ingo Gondogen:Germany:96:93:L:2000:f7_s1_b2_t5_hblack3|" +
                //Mahrez Riyad - att
                "Ri Mohraz:Algeria:89:92:L:2300:f8_s1_b2_t9_hblack3|" +
                //Sterling Raheem - att
                "Rahim Serlingg:England:87:94:L:2600:f9_s5_b2_t9_hblack3|" +
                //Gabriel Jesus - att
                "Jesu Gebriol:Brazil:88:93:L:3500:f0_s5_b0_t0_hblack5|" +
                //Aguero Sergio - att
                "Serge Oguerro:Argentina:83:92:L:3500:f1_s1_b2_t7_hblonde3|" +
                //Mendy Benjamin - def
                "Ben Emendi:France:92:88:L:3500:f2_s5_b0_t0_hblack5",
                "shoeblue",
                "glovelightbluewhite|glovebluedarkblue"},
             new string[] {
                 "Manchester Red",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "redshirt",
                 "whireredstripe",
                 "blackwhitestripessocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Arthur Junior
                "Arthur Junior:England:60:60:U:0:f3_s1_b0_t0_hblack2|" +


                //de Gea David - gk
                "Davide Egea:Spain:96:83:L:5000:f4_s1_b2_t8_hblonde1|" +
                //Maguire Harry - def
                "Henry Manireg:England:91:84:L:5000:f5_s2_b0_t0_hblack10|" +
                //Shaw Luke - def
                "Luk Shown:England:92:81:L:2000:f6_s3_b0_t0_hblonde3|" +
                //Pogba Paul - mid
                "Pohga:France:91:88:L:2300:f7_s5_b0_t0_hblack3|" +
                //Rashford Marcus - att
                "Marc Rosferd:England:85:93:L:2600:f8_s5_b0_t0_hblack5|" +
                //Greenwood Mason - att
                "Gennwodd:England:83:90:L:3500:f9_s1_b0_t0_hblack5|" +
                //Cavani Edinson - att
                "Edin Covoanii:Uruguay:86:89:L:3500:f0_s1_b0_t0_hblack13|" +
                //Bruno Fernandes - mid
                "Brun Fratrez:Portugal:95:93:L:3500:f2_s1_b2_t8_hblack3",

                "shoeblackred|shoered",
                "gloveredblack|gloveredwhite"},
             new string[] {
                 "Tottenham Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|blue|blue",
                "whiteblueshirt",
                 "darkblueyellowstripeshort",
                 "whitebluestripesocks",
                "skin_white_1",
                 "hair_19",
                 "white",
                //Harry Junior
                "Harry Junior:England:60:60:U:0:f1_s1_b2_t7_hblonde1|" +

                //Lloris Hugo - gk
                "Hugh Lorres:France:90:68:L:5000:f2_s1_b2_t8_hblack3|" +
                //Sanchez Davinson - def
                "Davi Sonchrez:Colombia:85:79:L:5000:f3_s5_b0_t0_hblack1|" +
                //Alli Dele - mid
                "Del Alil:England:89:84:L:2000:f4_s5_b2_t4_hblack5|" +
                //Lucas - mid
                "Luc:Brazil:88:84:L:2300:f5_s4_b2_t7_hnohair|" +
                //Kane Harry - att
                "Henry Kenee:England:90:95:L:2600:f6_s3_b2_t10_hblack10|" +
                //Bale Gareth - att
                "Garry Belle:Wales:75:86:L:3500:f7_s3_b2_t6_hblack15|" +
                //Son Heung-Min -att
                "Soan Mnn:South Korea:88:92:L:3500:f9_s3_b0_t0_hblack9",
                "shoeblack|shoebluewhite",
                "glovebluewhite|gloveblueyellow"},
             new string[] {
                 "Liverpool Red",
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
                //Oscar Junior
                "Oscar Junior:England:60:60:U:0:f1_s1_b0_t0_hblack7|" +



                //Alisson - gk
                "Alnson:Brazil:95:84:L:5000:f2_s4_b2_t10_hblack4|" +
                //Robertson Andrew - def
                "Andy Rorson:Scotland:90:83:L:5000:f3_s2_b0_t0_hblack3|" +
                //Alexander-Arnold Trent - def
                "Trenton Alex:England:94:84:L:2000:f4_s5_b0_t0_hblack5|" +
                //van Dijk Virgil - def
                "Vorgl de Dajk:Netherlands:98:88:L:2300:f5_s4_b2_t7_hblack15|" +
                //Salah Mohamed - att
                "Moha Salehd:Egypt:85:94:L:2600:f6_s1_b2_t9_hblack14|" +
                //Mane Sadio - att
                "Sadie Monee:Senegal:90:91:L:3500:f7_s5_b0_t0_hblack1|" +
                //Firmino Roberto - att
                "Robert Fomino:Brazil:86:90:L:2600:f8_s1_b0_t0_hblack10|" +
                //Jota Diogo - att
                "Diego Jortt:Portugal:85:91:L:3500:f9_s1_b0_t0_hblack5",
                "shoered|shoeredwhite",
                "gloveredwhite"},
             new string[] {
                 "Leicester Blue",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "bluewithyellowshirt",
                 "bluewhitestripe",
                 "darkbluewhitestripessocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Archie Junior
                "Archie Junior:England:60:60:U:0:f7_s1_b2_t5_hblack9|" +


                //Schmeichel Kasper - gk
                "Kacper Smaichell:Denmark:86:75:L:5000:f8_s3_b0_t0_hblonde2|" +
                //Castagne Timothy - def
                "Timoti Contange:Belgium:88:79:L:5000:f9_s1_b0_t0_hblack3|" +
                //Barnes Harvey - mid
                "Harve Bornesse:England:85:85:L:2000:f0_s2_b0_t0_hred3|" +
                //Vardy Jamie - att
                "James Vordii:England:81:91:L:2300:f1_s1_b2_t7_hblack10|" +
                //Iheanacho Kelechi - att
                "Kel Ineho:Nigeria:80:89:L:2600:f2_s5_b2_t3_hblack3|" +
                //Evans Jonny - def
                "Jon Ivens:England:90:83:L:3500:f3_s2_b0_t0_hblack10",
                "shoebluewhite",
                "glovebluewhite|glovebluewhite"},
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
                 //Toloi Rafael - def
                 "Rafi Toio:Italy:88:73:L:5000:f4_s1_b2_t8_hblack6|" +
                 //Malinovsky Ruslan - mid
                 "Runal Malnowski:Ukraine:82:82:L:5000:f5_s1_b0_t0_hblack5|" +                
                 //Gosens Robin - def
                 "Rob Gones:Germany:89:85:L:2300:f7_s1_b2_t5_hblack10|" +
                 //Muriel Luis - att
                 "Luigi Murle:Colombia:77:90:L:2600:f8_s4_b2_t4_hblack5|" +
                 //Zapata Duvan - att
                 "Daven Zupeta:Colombia:76:88:L:3500:f9_s5_b0_t0_hblack14",
                 "shoebluewhite",
                 "gloveblacblue"},
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
                 //Ronaldo Cristiano - att
                "Ronaldeo:Portugal:86:97:L:2600:f3_s1_b0_t0_hblack10|" +
                 //Dybala Paulo - att
                "Paul Dibale:Argentina:83:88:L:3500:f1_s1_b0_t0_hblonde4|" +
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
                 //Perisic Ivan - att
                "Ian Percis:Croatia:81:84:L:2600:f5_s1_b2_t5_hblack3|" +
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
                 //Insigne Lorenzo - att
                "Lorzo Insage:Italy:76:91:L:2300:f1_s1_b2_t7_hblack3|" +
                 //Lozano Hirving - att
                "Hing Lozro:Mexico:83:90:L:2600:f2_s2_b0_t0_hblack8|" +
                 //Mertens Dries - att
                "Dres Martes:Belgium:78:89:L:2600:f3_s1_b2_t4_hblonde5|" +
                 //Zielinski Piotr - mid
                "Piotr Ziela:Spain:89:88:L:3500:f4_s2_b0_t0_hblack5",
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
                 //Veretout Jordan - mid
                "Jordi Voretut:France:82:80:L:2000:f8_s1_b2_t9_hblack8|" +
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
              //Corinthians
              new string[] {
                 "Sao Paulo Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|black|black",
                "whitewithblack",
                 "black",
                 "whiteblackstripeupsocks",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Guilherme Jr
                "Guilherme Jr:Brazil:60:60:U:0:f0_s4_b0_t0_hblack7|" +

                 //Cassio - gk
                "Cosoo:Brazil:79:61:L:5000:f1_s1_b2_t7_hblack12|" +
                 //Gil - def
                "Jill:Brazil:71:63:L:5000:f2_s5_b2_t3_hblack14|" +
                 //Fagner - def
                "Fonerg:Brazil:73:64:L:2000:f3_s1_b0_t0_hblack6|" +
                //Mateus Vital - mid
                "Matias Vetel:Brazil:68:67:L:2000:f4_s1_b2_t8_hblack5|" +
                //Ramiro - mid
                "Romirno:Brazil:70:71:L:2300:f5_s2_b0_t0_hblack3|" +
                 //Gustavo Silva - att
                "Gustav Solvan:Brazil:63:71:L:2600:f6_s4_b0_t0_hblack7|" +
                 //Jô - att
                "Joaoo:Brazil:61:76:L:3500:f7_s5_b0_t0_hblack5",
                "shoewhiteblack",
                "gloveblackwhite|globewhiteblack"},
              //Flamengo
              new string[] {
                 "Rio FM",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|redblack|redblack",
                 "blackredstripeshorshirt",
                 "whiteredstripesshort",
                 "blackredstripessocks1",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Bruno Junior
                "Bruno Junior:Brazil:60:60:U:0:f5_s5_b2_t8_hblack9|" +


                 //Diego Alves - gk
                "Tiago Elvens:Brazil:85:68:L:5000:f6_s1_b2_t9_hblack3|" +
                 //Filipe Luís - def
                "Felipe Lous:Brazil:87:80:L:5000:f7_s1_b0_t0_hblack13|" +
                 //Giorgian de Arrascaeta - mid
                "Giori Aresenta:Uruguay:84:74:L:2000:f8_s1_b2_t9_hblack10|" +
                //Bruno Henrique - att
                "Breno Enriques:Brazil:69:85:L:2300:f9_s4_b2_t8_hblonde1|" +
                //Pedro - att
                "Eperdo:Brazil:70:87:L:2600:f0_s1_b0_t0_hblack5|" +
                //Gabriel Barbosa - att
                "Gabriele Berbonsa:Brazil:73:89:L:3500:f1_s4_b2_t9_hblack3",
                "shoeblackred|shoered",
                "gloveredblack|gloveredwhite"},
               //INTERNACIONAL
               new string[] {
                 "Porto Alegre Red",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "redwithwhiteshirt",
                "whiteredstripesshort",
                "whiteredstripessocks1",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Anderson Jr
                "Anderson Jr:Brazil:60:60:U:0:f6_s4_b2_t3_hblack1|" +



                 //Marcelo Lomba - gk
                "Marcelo Labama:Brazil:86:63:L:5000:f7_s1_b0_t0_hblack8|" +
                 //Víctor Cuesta - def
                "Vitor Guensa:Argentina:83:70:L:5000:f8_s1_b2_t9_hblack10|" +
                 //Patrick - mid
                "Patricios:Brazil:80:79:L:2000:f9_s4_b2_t8_hblack5|" +
                //Edenilson - mid
                "Edansons:Brazil:78:77:L:2300:f0_s5_b2_t6_hblack8|" +
                //Yuri Alberto - att
                "Juri Olbento:Brazil:74:81:L:2600:f1_s1_b0_t0_hblack7|" +
                //Thiago Galhardo - att
                "Tiago Golnerco:Brazil:73:85:L:3500:f2_s4_b2_t10_hblack3",
                "shoeredwhite|shoered",
                "gloveredwhite"},
            //Atletico-MG (Atlético Mineiro)
             new string[] {
                 "Minas Gerais AM",
                 "60",
                 "58",
                 "0",
                 "0",
                 "black|black|black",
                 "blackwhitesripessfirt",
                 "black",
                 "white",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Eduardo Junior
                "Eduardo Junior:Brazil:60:60:U:0:f3_s4_b0_t0_hblack2|" +


                 //Éverson - gk
                "Evansons:Brazil:74:63:L:5000:f4_s1_b0_t0_hblack1|" +
                 //Guilherme Arana - def
                "Guido Aene:Brazil:79:75:L:5000:f5_s4_b0_t0_hblack3|" +
                 //Guga - def
                "Guggi:Brazil:81:70:L:2000:f6_s4_b2_t3_hblack6|" +
                 //Hyoran - mid
                "Horen:Brazil:80:75:L:2300:f7_s3_b0_t0_hblack5|" +
                //Jefferson Savarino - att
                "Jeff Savelno:Venezuela:71:80:L:2600:f8_s1_b0_t0_hblack10|" +
                //Keno - att
                "Konenos:Brazil:73:81:L:3500:f9_s5_b0_t0_hblack13",
                "shoeblackred|shoewhiteblack",
                "gloveblackwhite|globewhiteblack"},
              //Palmeiras (Sociedade Esportiva Palmeiras)
              new string[] {
                 "Sao Paulo P",
                 "60",
                 "58",
                 "0",
                 "0",
                 "darkgreen|darkgreen|darkgreen",
                 "greenshirt1",
                 "whiteshort",
                 "greensocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Rodrigo Junior
                "Rodrigo Junior:Brazil:60:60:U:0:f9_s4_b2_t8_hblack3|" +


                 //Wéverton - gk
                "Wetrento:Brazil:85:64:L:5000:f8_s4_b2_t4_hblack3|" +
                 //Gustavo Gómez - def
                "Gomi:Paraguay:83:71:L:5000:f7_s3_b0_t0_hblack8|" +
                 //Zé Rafael - mid
                "Zerefell:Brazil:74:72:L:2000:f6_s4_b2_t3_hblack5|" +
                 //Raphael Veiga - mid
                "Vieinga:Brazil:75:83:L:2300:f5_s1_b0_t0_hblack6|" +
                 //Willian - att
                "Wilninens:Brazil:71:85:L:2600:f4_s1_b0_t0_hblack10|" +
                 //Luiz Adriano - att
                "Loizo Adrian:Brazil:72:87:L:3500:f3_s5_b2_t7_hblack14",
                "shoegreen",
                "glovegreenwhite|gloveblackgreen"},
              //Sao Paulo
              new string[] {
                 "Sao Paulo Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|black|red",
                "whiteredblackstripesshirt",
                 "whiteredstripesshort",
                 "whiteredstripessocks1",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Fabio Junior
                "Fabio Junior:Brazil:60:60:U:0:f5_s4_b2_t7_hblack6|" +

                 //Tiago Volpi - gk
                "Thiago Vovi:Brazil:85:61:L:5000:f6_s1_b0_t0_hblack6|" +
                 //Reinaldo - def
                "Roneldanho:Brazil:81:75:L:5000:f7_s5_b0_t0_hblack3|" +
                 //Dani Alves - mid
                "Dan Alavens:Brazil:82:81:L:2000:f8_s1_b0_t0_hblack5|" +
                //Vitor Bueno - mid
                "Viktor Boe:Brazil:75:75:L:2300:f9_s1_b0_t0_hblack10|" +
                //Luciano - att
                "Lucci:Brazil:71:86:L:2600:f0_s1_b2_t2_hblack7|" +
                //Pablo - att
                "Paolo:Brazil:73:79:L:3500:f1_s1_b0_t0_hblack8",
                "shoeblackred|shoered",
                "gloveredblack|gloveredwhite"},
              //Gremio
              new string[] {
                 "Porto Alegre",
                 "60",
                 "58",
                 "0",
                 "0",
                 "lightblue|lightblue|lightblue",
                 "blueblackstripesshirt",
                 "blackwhitestripesshort",
                 "white",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Victor Junior
                "Victor Junior:Brazil:60:60:U:0:f9_s4_b0_t0_hblack14|" +


                 //Paulo Victor - gk
                "Paul Octor:Brazil:78:62:L:5000:f0_s1_b2_t2_hblack6|" +
                 //Diogo Barbosa - def
                "Diego Bcossa:Brazil:76:70:L:5000:f1_s1_b0_t0_hblack3|" +
                 //César Pinares - mid
                "Cesare Ponorens:Chile:73:71:L:2000:f2_s4_b2_t10_hblack5|" +
                 //Jean Pyerre - mid
                "Joao Pireree:Brazil:74:73:L:2300:f3_s4_b2_t6_hblack10|" +
                 //Lucas Silva - mid
                "Luc Solvia:Brazil:70:70:L:2600:f4_s1_b2_t8_hblack9|" +
                 //Diego Souza - att
                "Tiago Sanza:Brazil:64:85:L:3500:f5_s1_b2_t5_hblack6",
                "shoebluewhite|shoeblue",
                "gloveblacblue|gloveblueblue"},
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