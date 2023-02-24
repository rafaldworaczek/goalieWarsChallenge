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
    public class LeagueEngland
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

                //Leno Bernd - gk \\TO DO\\
                "Berndt Loenoss:Germany:88:72:L:5000:f2_s2_b0_t0_hblonde3|" +
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
                //Werner Timo - att \\TO DO\\
                "Tim Wenrert:Germany:78:91:L:3500:f9_s3_b0_t0_hblonde3|" +
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
                //Sterling Raheem - att \\TO DO\\
                "Rahim Serliggs:England:87:94:L:2600:f9_s5_b2_t9_hblack3|" +
                //Gabriel Jesus - att
                "Jesu Gebriol:Brazil:88:93:L:3500:f0_s5_b0_t0_hblack5|" +
                //Aguero Sergio - att \\TO DO\\
                "Serge Ogerros:Argentina:83:92:L:3500:f1_s1_b2_t7_hblonde3|" +
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
                //Cavani Edinson - att \\TO DO\\
                "Ed Convoanil:Uruguay:86:89:L:3500:f0_s1_b0_t0_hblack13|" +
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
                 "Brighton City",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "bluewithwhiteshirt",
                 "whitegoldstripesshort",
                 "blueblackstripesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Leo Junior
                "Leo Junior:England:60:60:U:0:f9_s1_b0_t0_hblonde3|" +



                //Sanchez Robert - gk
                "Rob Sonhrez:Spain:83:60:L:5000:f0_s1_b2_t2_hblack3|" +
                //Dunk Lewis - def
                "Lewie Dunnsk:England:73:63:L:5000:f1_s1_b0_t0_hblack5|" +
                //Trossard Leandro - mid
                "Leon Trosor:Belgium:73:71:L:2000:f2_s2_b0_t0_hblack3|" +
                //Moder Jakub - mid
                "Kuba Modrek:Poland:69:67:L:2300:f3_s1_b2_t4_hblack6|" +
                //Maupay Neal - att
                "Neil Mupyya:France:64:75:L:2600:f4_s1_b2_t8_hblack1|" +
                //Welbeck Danny
                "Dan Wollbenck:England:70:82:L:3500:f5_s5_b0_t0_hblack5",
                "shoebluewhite|shoeblue",
                "glovebluewhite|gloveblacblue"},
             new string[] {
                 "Everton Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "bluewithwhiteshirt",
                 "whiteblueshort",
                 "whiteblustripesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Jack Junior
                "Jack Junior:England:60:60:U:0:f6_s1_b0_t0_hblack4|" +

                //Pickford Jordan - gk
                "Jordy Packfrod:England:85:70:L:5000:f7_s1_b0_t0_hblonde2|" +
                //Sigurdsson Gylfi - mid
                "Gyli Igursonss:Iceland:76:77:L:5000:f8_s3_b0_t0_hblack10|" +
                //Calvert-Lewin Dominic - att
                "Nicky Lcoventt:England:75:90:L:2000:f9_s4_b2_t8_hblack9|" +
                //Richarlison - att
                "Richallnsan:Brazil:73:84:L:2300:f0_s4_b0_t0_hblack3|" +
                //Gomes Andre - mid
                "Andi Omens:Portugal:75:69:L:2600:f1_s1_b2_t7_hblack5|" +
                //Doucoure Abdoulaye - mid
                "Abul Docuree:France:73:74:L:3500:f2_s5_b0_t0_hblack3",
                "shoebluewhite|shoeblue",
                "glovebluewhite"},
             new string[] {
                 "White Leeds",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|yellowblue|yellowblue",
                "whitewithblueshirt",
                 "whiteblueshort",
                 "whitebluesocks",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Charlie Junior
                "Charlie Junior:England:60:60:U:0:f3_s1_b0_t0_hblack6|" +

                //Meslier Illan - gk
                "Ian Moser:France:87:70:L:5000:f4_s2_b0_t0_hblack7|" +
                //Dallas Stuart - def
                "Stu Deles:England:81:75:L:5000:f5_s1_b2_t5_hblack10|" +
                //Harrison Jack - mid
                "Jacky Harry:England:78:78:L:2000:f6_s3_b2_t10_hblack3|" +
                //Klich Mateusz - mid
                "Mati Klik:Poland:79:74:L:2300:f7_s3_b2_t6_hnohair|" +
                //Bamford Patrick - att
                "Patrik Bemorfd:England:70:86:L:2600:f8_s3_b0_t0_hblonde5|" +
                //Raphinha - att
                "Ropinia:Brazil:69:83:L:3500:f9_s4_b0_t0_hblack5",
                "shoebluewhite|shoeblue",
                "glovebluewhite"},
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
                 "West Ham Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                 "westhamshirt",
                 "whiteshort",
                 "westhamsocks",
                 "skin_white_1",
                 "hair_19",
                 "red",
                 //Jacob Junior
                "Jacob Junior:England:60:60:U:0:f0_s1_b2_t2_hblack8|" +


                //Fabiański Łukasz - gk
                "Lukasz Fobinski:Poland:90:71:L:5000:f1_s1_b2_t7_hblack3|" +
                //Cresswell Aaron - def
                "Ron Costwel:England:85:73:L:5000:f2_s1_b0_t0_hblack10|" +
                //Coufal Vladimir - def
                "Vlad Cufol:Czechia:83:74:L:2000:f3_s1_b0_t0_hblonde1|" +
                //Lingard Jesse - mid
                "Liguard:England:87:85:L:2300:f4_s5_b0_t0_hblack5|" +
                //Soucek Tomas - mid
                "Tom Socet:Czechia:75:86:L:2600:f5_s1_b0_t0_hblack2|" +
                //Yarmolenko Andriy - att
                "Andry Jomolienko:Ukraine:72:82:L:3500:f6_s1_b0_t0_hblack1|" +
                //Bowen Jarrod - att
                "Jarrod Bawn:England:76:83:L:3500:f7_s1_b0_t0_hblack3",
                "shoeredwhite|shoered",
                "gloveredwhite"},
             new string[] {
                 "Aston RB",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                 "westhamshirt",
                 "whiteshort",
                 "lightblueredstripesocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Henry Junior
                "Henry Junior:England:60:60:U:0:f8_s1_b2_t9_hblack9|" +


                //Martinez Emiliano - gk
                "Emi Mattinee:Argentina:84:69:L:5000:f9_s1_b0_t0_hblack8|" +
                //Mings Tyrone - def
                "Tyron Wings:England:78:70:L:5000:f0_s5_b0_t0_hblack14|" +
                //Grealish Jack - mid
                "Jack Grenlesh:England:82:80:L:2000:f1_s1_b2_t7_hblack10|" +
                //El-Ghazi Anwar - mid
                "Anwer Goziih:Netherlands:81:80:L:2300:f2_s1_b0_t0_hblack3|" +
                //Watkins Ollie - att
                "Oliver Matwins:England:73:85:L:2600:f3_s5_b0_t0_hblack5|" +
                //McGinn John - mid
                "Jackie Tinn:Scotland:74:69:L:3500:f4_s2_b0_t0_hblack10",
                "shoeblue",
                "gloveredwhite|glovelightbluewhite"},
             new string[] {
                 "Burnley Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                "westhamshirt",
                 "whireredstripe",
                 "redbluestripessocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Theo Junior
                "Theo Junior:England:60:60:U:0:f5_s1_b0_t0_hred4|" +


                //Pope Nick - gk
                "Dominic Pep:England:83:60:L:5000:f6_s3_b0_t0_hblack3|" +
                //Tarkowski James - def
                "Jim Trakovski:England:79:65:L:5000:f7_s3_b2_t6_hblack9|" +
                //Gudmundsson Johann - mid
                "Johan Gadumsonn:Iceland:73:71:L:2000:f8_s1_b0_t0_hblonde2|" +
                //McNeil Dwight - mid
                "Dan Noill:England:74:72:L:2300:f9_s1_b0_t0_hblack3|" +
                //Vydra Matej
                "Mat Vetraa:Czechia:69:79:L:2600:f0_s2_b0_t0_hblonde1|" +
                //Wood Chris - att
                "Kris Vod:Australia:70:84:L:3500:f1_s1_b0_t0_hblack10",
                "shoeredwhite|shoered",
                "gloveredwhite|glovelightbluewhite"},
             new string[] {
                 "Crystal BR",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                "blueredshirt",
                 "darkblueshort",
                 "darkblueredstripes",
                "skin_white_1",
                 "hair_19",
                 "red",
                //William Junior
                "William Junior:England:60:60:U:0:f2_s1_b0_t0_hblonde6|" +



                //Guaita - gk
                "Ganuite:Spain:82:61:L:5000:f3_s1_b2_t4_hblack4|" +
                //Riedewald Jairo - def
                "Jaro Rodadl:Netherlands:73:63:L:5000:f5_s1_b2_t5_hblack14|" +
                //Schlupp Jeffrey - mid
                "Jeff Slub:Ghana:68:66:L:2000:f5_s5_b2_t8_hnohair|" +
                //Eze Eberechi - mid
                "Eber Izze:England:72:72:L:2300:f6_s5_b0_t0_hblack14|" +
                //Benteke Christian - att
                "Chris Bonotoke:Belgium:69:74:L:2600:f7_s5_b2_t8_hblack3|" +
                //Zaha Wilfried - att
                "Wili Sahaa:Ivory Coast:71:80:L:3500:f8_s5_b2_t5_hblack5",
                "shoebluewhite|shoeblackred",
                "gloveblacblue|glovebluered"},
             new string[] {
                 "Fulham Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|black|black",
                "whitewithblackshirt",
                 "blackwhitestripesshort",
                 "whiteblacstripessocks",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Theodore Jr
                "Theodore Jr:England:60:60:U:0:f9_s1_b0_t0_hblack8|" +

                //Areola Alphonse - gk
                "Alphoe Aerolas:France:79:54:L:5000:f1_s5_b2_t10_hblack1|" +
                //Aina Ola - def
                "Aeinae:Nigeria:73:61:L:5000:f2_s5_b2_t3_hblack3|" +
                //Robinson Antonee - def
                "Anton Robensen:USA:70:60:L:2000:f3_s1_b0_t0_hblack5|" +
                //Reid Bobby - mid
                "Bob Rait:Jamaica:64:61:L:2300:f4_s5_b2_t4_hblack14|" +
                //Cavaleiro Ivan - att
                "Ivo Covenalero:Portugal:55:71:L:2600:f5_s5_b2_t8_hblack3|" +
                //Lookman Ademola - att
                "Adam Lokemon:England:58:70:L:3500:f6_s5_b2_t4_hblack1",
                "shoeredwhite|shoeblackred",
                "globewhiteblack|gloveblackwhite"},
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


                //Schmeichel Kasper - gk \\TO DO\\
                "Kacper Smaihelt:Denmark:86:75:L:5000:f8_s3_b0_t0_hblonde2|" +
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
             new string[] {
                 "Newcastle City",
                 "60",
                 "58",
                 "0",
                 "0",
                 "black|black|black",
                "blacwhiteshirt",
                 "black",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                //James Junior
                "James Junior:England:60:60:U:0:f4_s1_b2_t8_hblack11|" +

                //Darlow Karl - gk
                "Carl Dalown:England:81:65:L:5000:f5_s1_b0_t0_hblack10|" +
                //Clark Ciaran - def
                "Kieran Clarke:Ireland:75:67:L:5000:f6_s3_b2_t10_hblack3|" +
                //Hayden Isaac - def
                "Isaak Hyde:England:73:65:L:2000:f7_s5_b2_t8_hblack3|" +
                //Wilson Callum - att
                "Cal Wisoon:England:69:79:L:2300:f8_s5_b2_t5_hblack1|" +
                //Joelinton - att
                "Jonition:Brazil:64:76:L:2600:f9_s5_b2_t9_hblonde2|" +
                //Saint-Maximin Allan - mid
                "Allen Max:France:70:70:L:3500:f0_s5_b2_t6_hblack14",
                "shoeblack|shoewhiteblack",
                "globewhiteblack|gloveblackwhite"},
             new string[] {
                 "Southampton",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "whitewithredshirt",
                 "black",
                 "whiteredstripe",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Tommy Junior
                "Tommy Junior:England:60:60:U:0:f1_s1_b0_t0_hblack12|" +

                //McCarthy Alex - gk
                "Aleck Corthin:England:85:59:L:5000:f2_s1_b2_t8_hblack5|" +
                //Bednarek Jan - def
                "Janek Bedorek:Poland:79:70:L:5000:f3_s2_b0_t0_hblack10|" +
                //Vestergaard Jannik - def
                "Jan Stergadr:Denmark:75:72:L:2000:f4_s2_b0_t0_hblack13|" +
                //Ward-Prowse James - mid
                "Jim Wapose:England:79:78:L:2300:f5_s1_b0_t0_hblack3|" +
                //Ings Danny - att
                "Dan Igss:England:69:84:L:2600:f6_s1_b2_t9_hblack5|" +
                //Adams Che - att
                "Adam Cee:Scotland:68:80:L:3500:f7_s5_b0_t0_hblack3",
                "shoeredwhite|shoeblackred",
                "gloveredwhite|gloveredblack"},
             new string[] {
                 "West Bromwich",
                 "60",
                 "58",
                 "0",
                 "0",
                 "black|black|black",
                "whitedarkbluestripesshirt",
                 "whitepurpleshort",
                 "blacwhitestripessocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Max Junior
                "Max Junior:England:60:60:U:0:f8_s1_b2_t9_hblack13|" +

                //Johnstone Sam - gk
                "Sem John:England:79:55:L:5000:f9_s1_b0_t0_hnohair|" +
                //Ajayi Semi - def
                "Somi Ajayaya:Nigeria:71:59:L:5000:f1_s5_b0_t0_hblack3|" +
                //Grosicki Kamil - mid
                "Kamil Grosik:Poland:72:60:L:2000:f2_s2_b0_t0_hblonde1|" +
                //Pereira Matheus - mid
                "Mateus Erera:Brazil:65:71:L:2300:f3_s5_b2_t7_hblack5|" +
                //Diagne Mbaye - att
                "Maye Dagee:Senegal:59:69:L:2600:f4_s5_b2_t4_hblack1|" +
                //Robinson Callum - att
                "Callem Roseson:Ireland:65:73:L:3500:f5_s1_b0_t0_hblack5",
                "shoebluewhite",
                "glovebluewhite"},
             new string[] {
                 "Wolverhampton",
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
                //Teddy Junior
                "Teddy Junior:England:60:60:U:0:f6_s1_b0_t0_hblonde5|" +


                //Patricio Rui - gk
                "Ruis Parincio:Portugal:83:65:L:5000:f7_s1_b2_t5_hblack5|" +
                //Coady Conor - def
                "Coner Candy:England:81:70:L:5000:f8_s1_b2_t9_hblack3|" +
                //Semedo Nelson - def
                "Nelse Nemedos:Portugal:82:73:L:2000:f9_s5_b2_t9_hblack3|" +
                //Neves Ruben - mid
                "Rubem Novs:Portugal:79:75:L:2300:f0_s1_b2_t2_hblack10|" +
                //Traore Adama - att
                "Adam Arore:Spain:66:75:L:2600:f0_s5_b2_t6_hblack5|" +
                //Jimenez Raul - att
                "Rui Jinnezm:Mexico:65:71:L:3500:f2_s1_b2_t8_hblack5",
                "shoeorange",
                "gloveblackorange"},
             new string[] {
                "Sheffield Team",
                "60",
                "58",
                "0",
                "0",
                "red|red|red",
                "redwhiteblackshirt",
                "blackwhitestripesshort",
                "redwhitesocks",
                "skin_white_1",
                "hair_19",
                "red",
                 //Ian Junior
                "Ian Junior:England:60:60:U:0:f3_s1_b0_t0_hred3|" +


                //Ramsdale Aaron - gk
                "Aren Rasdel:England:78:50:L:5000:f4_s1_b0_t0_hblonde2|" +
                //Egan John - def
                "Johnny Igann:Ireland:70:55:L:5000:f5_s2_b0_t0_hblack5|" +
                //Berge Sander - mid
                "Sande Boree:Norway:61:59:L:2000:f6_s3_b0_t0_hblack11|" +
                //McGoldrick David - att
                "David Codrinck:Ireland:60:75:L:2300:f7_s5_b2_t8_hnohair|" +
                //Burke Oliver - att
                "Oli Barkes:Scotland:55:69:L:2600:f8_s1_b2_t9_hblack8|" +
                //Osborn Ben - mid
                "Ban Osbenr:Spain:63:60:L:3500:f9_s1_b0_t0_hred3",
                "shoeredwhite|shoeblackred",
                "gloveredwhite|gloveredblack"},
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
 * Elijah
Adam
Daniel
Samuel
Louie
Mason
Reuben
Albie
Rory
Jaxon
Hugo
Luca
Zachary
Reggie
Hunter
Louis
Dylan
Albert
David
Jude
Frankie
Roman
Ezra
Toby
Riley
Carter
Ronnie
Frederick
Gabriel
Stanley
Bobby
Jesse
Michael
Elliot
Grayson
Mohammad
Liam
Jenson
Ellis
Harley
Harvey
Jayden
Jake
Ralph
Rowan
Elliott
Jasper
Ollie
Charles
Finn
Felix
Caleb
Chester
Jackson
Hudson
Leon
Ibrahim
Ryan
Blake
Alfred
Oakley
Matthew
Luke
*/
