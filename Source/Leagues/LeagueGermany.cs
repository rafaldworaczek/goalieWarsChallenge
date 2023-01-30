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
    public class LeagueGermany
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
                 "Wolfsburg FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "green|green|green",
                "greenwhiteshirt",
                 "greenwhitestripesshort",
                 "greenwhitesocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                //Philipp Junior
                "Philipp Junior:Germany:60:60:U:0:f3_s1_b0_t0_hblack4|" +



                //Casteels Koen gk
                "Carl Comtenl:Belgium:81:69:L:5000:f4_s2_b2_t9_hblack3|" +
                //Brooks John def
                "Johnny Bro:USA:80:71:L:5000:f5_s1_b2_t5_hnohair|" +
                //Baku Bote mid
                "Bon Bukul:Germany:79:79:L:2000:f6_s5_b2_t4_hblack5|" +
                 //Arnold Maximilian mid
                "Maxwell Arni:Germany:77:76:L:2300:f7_s3_b2_t6_hblonde2|" +
                //Brekalo Josip att
                "Jose Berolo:Croatia:72:77:L:2600:f8_s1_b2_t9_hblack10|" +
                //Weghorst Wout att
                "Walter Vege:Netherlands:75:89:L:3500:f9_s1_b0_t0_hblonde1",
                "shoegreen",
                "gloveblackgreen|glovegreenwhite"},
             new string[] {
                 "Frankfurt FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|black|black",
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
                 "Berlin Red",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "redshirt",
                 "redshort",
                 "redwhitesocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Sebastian Jr
                "Sebastian Jr:Germany:60:60:U:0:f6_s1_b0_t0_hblack8|" +

                //Luthe Andreas gk
                "Andi Larths:Germany:80:69:L:5000:f7_s1_b0_t0_hblack5|" +
                //Friedrich Marvin def
                "Mav Findrick:Germany:75:70:L:5000:f8_s1_b2_t9_hblonde2|" +
                //Andrich Robert mid
                "Bob Andre:Germany:76:78:L:2000:f9_s1_b2_t6_hnohair|" +
                //Becker Sheraldo att
                "Sheli Bocern:Netherlands:70:73:L:2300:f0_s5_b2_t6_hblack3|" +
                //Ingvartsen Marcus att
                "Mark Onverston:Denmark:70:75:L:2600:f1_s1_b2_t7_hblonde2|" +
                //Kruse Max att
                "Maxi Kreuzer:Germany:73:81:L:3500:f2_s2_b0_t0_hblack10",
                "shoeredwhite",
                "gloveredwhite"},
              new string[] {
                 "Berlin Blue",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "whitedarkbluestripesshirt",
                 "whiteblackstripeshort",
                 "whitebluesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Julian Junior
                "Julian Junior:Germany:60:60:U:0:f3_s1_b0_t0_hred1|" +


                //Schwolow Alexander gk
                "Alex Follow:Germany:76:55:L:5000:f4_s2_b0_t0_hblonde1|" +
                //Stark Niklas def
                "Nico Stars:Germany:67:60:L:5000:f5_s1_b2_t5_hblack5|" +
                //Guendouzi Matteo - mid
                "Matt Uouzi:France:65:64:L:2000:f6_s4_b0_t0_hblack14|" +
                //Cordoba Jhon att
                "Joe Candrora:Colombia:57:74:L:2300:f7_s5_b2_t8_hblack5|" +
                //Cunha Matheus att
                "Mat Cuholno:Brazil:59:76:L:2600:f8_s5_b0_t0_hblack10|" +
                 //Piątek Krzysztof att
                "Kris Pontek:Poland:58:74:L:3500:f9_s1_b0_t0_hblack3",
                "shoebluewhite|shoeblue",
                "glovebluewhite"},
             new string[] {
                 "Freiburg Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "redwitwhiteshirt",
                 "redwhitestripesshort",
                 "redwhitestripesocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Simon Junior
                "Simon Junior:Germany:60:60:U:0:f1_s1_b0_t0_hblonde1|" +


                //Muller Florian gk
                "Flo Malern:Germany:80:55:L:5000:f2_s2_b0_t0_hblonde2|" +
                 //Lienhart Philipp def
                "Phil Linerd:Austria:75:63:L:5000:f3_s1_b0_t0_hblonde1|" +
                //Grifo Vincenzo mid
                "Vincent Gufii:Italy:78:79:L:2000:f4_s1_b2_t8_hblack3|" +
                 //Demirovic Ermedin mid
                "Ernad Teminovic:Bosnia:73:71:L:2300:f5_s1_b0_t0_hblack5|" +
                //Sallai Roland mid
                "Rol Selizo:Italy:74:73:L:2600:f6_s3_b0_t0_hblack1|" +
                //Petersen Nils att
                "Nili Oteson:Germany:60:74:L:3500:f7_s1_b0_t0_hblonde4",
                "shoeredwhite|shoered",
                "gloveredwhite"},
             new string[] {
                 "Stuttgart FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|red|red",
                "whitewithredshirt",
                 "whiteredstripesshort",
                 "whiteredstripe",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Felix Junior
                "Felix Junior:Germany:60:60:U:0:f5_s1_b0_t0_hblack7|" +


                 //Kobel Gregor gk
                 "Gregg Cowell:Switzerland:79:59:L:5000:f6_s1_b2_t9_hblack5|" +
                 //Kempf Marc-Oliver def
                 "Ollie Kram:Germany:77:69:L:5000:f7_s3_b0_t0_hblack3|" +
                 //Didavi Daniel mid
                 "Dani Dorvani:Germany:71:69:L:2000:f8_s5_b2_t5_hblack5|" +
                 //Mangala Ore  mid
                 "Oli Moralen:Germany:70:71:L:2300:f9_s5_b2_t9_hblack3|" +
                 //Kalajdzic Sasa att
                 "Sase Kolvincic:Austria:69:85:L:2600:f0_s2_b0_t0_hblack10|" +
                 //Wamangituka Silas att
                 "Sam Omarwuka:Congo:70:81:L:3500:f1_s5_b2_t10_hblack3",
                "shoeredwhite|shoered",
                "gloveredwhite"},
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
                 "Hoffenheim FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "bluwithblueshirt",
                 "bluebluestripesshort",
                 "blubluestripesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Sven Junior
                "Sven Junior:Germany:60:60:U:0:f3_s1_b0_t0_hblonde3|" +


                //Baumann Oliver gk
                "Oli Bumense:Germany:80:60:L:5000:f4_s1_b2_t8_hblonde2|" +
                 //Vogt Kevin def
                "Kev Vangrot:Germany:78:70:L:5000:f5_s2_b2_t6_hblonde1|" +
                 //Baumgartner Christoph mid
                "Chris Magnarher:Austria:75:73:L:2000:f6_s1_b0_t0_hblonde1|" +
                 //Dabbur Munas mid
                "Munu Durbub:Israel:70:80:L:2300:f7_s3_b2_t6_hblack3|" +
                 //Adamyan Sargis att
                "Sergi Aymant:Armenia:68:75:L:2600:f8_s1_b2_t9_hblack1|" +
                 //Kramaric Andrej att
                "Andrew Kavenic:Croatia:70:89:L:3500:f9_s1_b2_t6_hblack6",
                "shoeblue",
                "gloveblueblue|glovelightbluewhite"},
             new string[] {
                 "Bremen Greens",
                 "60",
                 "58",
                 "0",
                 "0",
                 "green|green|green",
                "greenwithwhiteshirt",
                 "whitewithgreenshort",
                 "greenwhitesocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Dennis Junior
                "Dennis Junior:Germany:60:60:U:0:f7_s1_b0_t0_hred4|" +

                //Pavlenka Jiri gk
                 "Jiri Poentka:Czechia:79:54:L:5000:f8_s1_b0_t0_hblack3|" +
                 //Gebre Selassie Theodor def
                "Theo Thedore:Czechia:79:58:L:5000:f9_s5_b2_t9_hblack5|" +
                 //Bittencourt Leonardo mid
                "Leon Botenlord:Germany:69:63:L:2000:f0_s1_b2_t2_hblack9|" +
                 //Mohwald Kevin mid
                "Kev Monwell:Germany:70:68:L:2300:f1_s1_b2_t7_hblack3|" +
                 //Fullkrug Niclas att
                "Nico Fankronk:Germany:61:70:L:2600:f2_s1_b2_t8_hblonde1|" +
                 //Sargent Joshua att
                "Josh Sunhet:USA:60:75:L:3500:f5_s1_b0_t0_hred4",
                "shoegreen",
                "gloveblackgreen|glovegreenwhite"},
             new string[] {
                 "Augsburg Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "green|redgreen|redgreen",
                 "whiteredstripeshirt",
                 "whiteshort",
                 "white",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Moritz Junior
                "Moritz Junior:Germany:60:60:U:0:f8_s1_b0_t0_hblack10|" +

                //Gikiewicz Rafał gk
                "Rafi Glikowski:Poland:79:59:L:5000:f0_s1_b0_t0_hblack3|" +
                //Gouweleeuw Jeffrey def
                "Jeff Legowellu:Netherlands:74:60:L:5000:f1_s2_b2_t8_hblonde2|" +
                 //Caligiuri Daniel mid
                "Dani Ciluring:Germany:75:70:L:2000:f2_s1_b2_t8_hblack10|" +
                 //Hahn Andre att
                "Andy Holnh:Germany:60:73:L:2300:f3_s2_b0_t0_hblonde2|" +
                 //Vargas Ruben att
                "Rob Virgens:Switzerland:61:72:L:2600:f4_s1_b0_t0_hblack3|" +
                 //Niederlechner Florian - att
                "Flo Naderhene:Germany:63:76:L:3500:f0_s2_b0_t0_hblonde4",
                "shoered|shoeblue",
                "gloveredwhite|glovebluewhite"},
             new string[] {
                 "Mainz Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "redwhitearmshirt",
                 "whiteredstripeshort",
                 "red",
                "skin_white_1",
                 "hair_19",
                 "red",
                //Jonathan Jr
                "Jonathan Jr:Germany:60:60:U:0:f9_s1_b0_t0_hblack14|" +


                //Zentner Robin - gk
                "Rob Zothern:Germany:81:60:L:5000:f1_s1_b2_t7_hred3|" +
                //Burkardt Jonathan att
                "Jon Baketr:Germany:60:75:L:5000:f2_s2_b0_t0_hblonde1|" +
                //Niakhate Moussa - def
                "Musa Nehotel:France:73:70:L:2000:f3_s5_b0_t0_hblack3|" +
                //Quaison Robin att
                "Robbie Ainssen:Sweden:69:75:L:2300:f4_s5_b0_t0_hblack1|" +
                 //Stoger Kevin mid
                "Kev Stronger:Austria:75:74:L:2600:f5_s2_b2_t6_hblack10|" +
                 //Onisiwo Karim att
                "Kari Owistno:Austria:69:76:L:3500:f6_s5_b0_t0_hblack5",
                "shoeredwhite|shoered",
                "gloveredwhite"},
             new string[] {
                 "Koln Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "whiteredshirt",
                 "whiteredstripeshort",
                 "whiteredstripesverticalsocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Tobias Junior
                "Tobias Junior:Germany:60:60:U:0:f7_s1_b0_t0_hblack13|" +

                //Horn Timo gk
                "Tim Honni:Germany:75:54:L:5000:f0_s2_b2_t4_hred2|" +
                //Drexler Dominick mid
                "Dom Daksla:Germany:63:61:L:5000:f1_s1_b2_t7_hblack3|" +
                //Duda Ondrej - mid
                "Andrew Dudek:Slovakia:62:63:L:2000:f2_s2_b0_t0_hblonde1|" +
                //Rexhbecaj Elvis mid
                "Elvi Ronhehe:Germany:75:78:L:2300:f3_s1_b0_t0_hblack5|" +
                 //Skhiri Ellyes mid
                "El Sanhris:Tunisia:72:73:L:2600:f4_s1_b0_t0_hblack3|" +
                 //Wolf Marius att
                "Mario Walven:Germany:58:69:L:3500:f5_s1_b0_t0_hblonde3",
                "shoeredwhite|shoered",
                "gloveredwhite"},
             new string[] {
                 "Gelsenkirchen",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "bluewithwhiteshirt",
                 "whiteblueshort",
                 "bluewhitestripesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Dominik Junior
                "Dominik Junior:Germany:55:55:U:0:f6_s1_b0_t0_hblonde3|" +

                //Fahrmann Ralf
                "Ralph Fomenh:Germany:75:48:L:5000:f7_s3_b2_t6_hred3|" +
                //Oczipka Bastian - def
                "Seb Ozickan:Germany:65:50:L:5000:f8_s1_b2_t9_hblack3|" +
                //Harit Amine mid
                "Am Tarit:Morocco:54:53:L:2000:f9_s1_b2_t6_hblack5|" +
                //Serdar Suat mid
                "Seth Saduer:Germany:55:54:L:2300:f0_s1_b2_t2_hblack10|" +
                 //Hoppe Matthew att
                "Matt Hopes:USA:51:65:L:2600:f1_s2_b0_t0_hblack10|" +
                //Raman Benito att
                "Ben Romoen:Belgium:50:66:L:3500:f0_s1_b0_t0_hblack1",
                "shoebluewhite",
                "glovebluewhite"},
             new string[] {
                 "Bielefeld City",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "whitebluestripeshirt",
                 "black",
                 "blackwhitestripe",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Marco Junior
                "Marco Junior:Germany:60:60:U:0:f2_s1_b0_t0_hblonde4|" +


                 // Stefan - gk
                 "Steve Onterga:Germany:73:55:L:5000:f3_s2_b2_t5_hblack6|" +
                 //Kunze Fabian def
                "Febien Kruze:Germany:65:59:L:5000:f4_s1_b0_t0_hblonde1|" +
                 //Brunner Cedric - def
                "Ceric Bruno:Switzerland:62:56:L:2000:f5_s2_b0_t0_hblack3|" +
                 //Doan Ritsu mid
                "Rit Colan:Japan:69:68:L:2300:f6_s3_b0_t0_hblack9|" +
                 //Prietl Manuel mid
                "Emmanuel Petr:Austria:65:60:L:2600:f7_s1_b2_t5_hblack3|" +
                 //Klos Fabian att
                "Fabe Koss:Germany:55:65:L:3500:f8_s1_b2_t9_hblack5",
                "shoebluewhite|shoewhiteblack",
                "glovebluewhite|gloveblacblue"},
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
 GERMANS NAME NOT TAKEN  
 Ludwig
Hendrik
Mario
Oliver
Lucas
Anton
Timo
Sven
marc
Andre
Ben
Jannik
Niklas
Peter
mark
johannes
Konrad
Alexander
Pierre
Jason
http://www.studentsoftheworld.info/penpals/stats.php?Pays=GER
*/


