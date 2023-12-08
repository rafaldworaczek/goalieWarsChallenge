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
    public class LeagueBrazil
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
         * 
         */
        private string[][] teams = new string[][] {          
             //America MG
             new string[] {
                 "B. Horizonte A",
                 "60",
                 "58",
                 "0",
                 "0",
                 "darkgreen|darkgreen|darkgreen",
                 "darkgreengoldstripesshirt",
                 "darggreengoldstripesshort",
                 "darkgreengoldstripessocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                //Gabriel Junior
                "Gabriel Junior:Brazil:60:60:U:0:f2_s4_b0_t0_hblack1|" +

                //Matheus Cavichioli - gk
                "Mati Convinli:Brazil:71:60:L:5000:f1_s1_b2_t7_hblack5|" +
                //Ribamar - att
                "Abirama:Brazil:60:73:L:5000:f0_s5_b2_t6_hblack3|" +
                //Juninho - mid \\TO DO\\
                "Jonniosh:Brazil:64:62:L:2000:f1_s4_b0_t0_hblack1|" +
                //Alê - mid
                "Alless:Brazil:65:64:L:2300:f2_s1_b0_t0_hblack10|" +
                //Ademir - att
                "Domenir:Brazil:57:69:L:2600:f3_s5_b0_t0_hblack5|" +
                //Rodolfo - att
                "Rudonfo:Brazil:59:71:L:3500:f4_s4_b0_t0_hblack2",
                "shoegreen",
                "gloveblackgreen|glovegreenwhite"},
             //Athletico PR Atlético Paranaense)
             new string[] {
                 "Parana Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|redblack|redblack",
                 "blackredstripesshirt",
                 "blacredstripeshort",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Matheus Junior
                "Matheus Junior:Brazil:60:60:U:0:f5_s4_b2_t7_hblack2|" +


                 //Santos - gk
                "Sentions:Brazil:82:63:L:5000:f6_s1_b0_t0_hblack3|" +
                 //Abner - def
                "Abern:Brazil:78:65:L:5000:f7_s4_b0_t0_hblack5|" +
                 //Leo Cittadini - mid
                "Leonel Coltini:Brazil:69:74:L:2000:f8_s1_b0_t0_hblack10|" +
                 //Nikão - mid
                "Nockao:Brazil:69:68:L:2300:f9_s5_b2_t9_hblack10|" +
                //Renato Kayzer - att
                "Ronato Kozier:Brazil:66:79:L:2600:f0_s1_b2_t2_hblack6|" +
                //Carlos Eduardo - att
                "Carlo Endurno:Brazil:63:75:L:3500:f1_s5_b0_t0_hblack1",
                "shoeblackred|shoered",
                "gloveredblack"},
              //Bahia
             new string[] {
                 "Bahia FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|bluered|bluered",
                 "bluered",
                 "blueredstripehorshort",
                 "sock_red",
                 "skin_white_1",
                 "hair_19",
                 "blue",
                 //Pedro Junior
                "Pedro Junior:Brazil:60:60:U:0:f2_s4_b0_t0_hblack6|" +

                 //Douglas Friedrich - gk \\TO DO\\
                "Dany Rotrich:Brazil:77:56:L:5000:f3_s2_b0_t0_hblonde1|" +
                 //Nino Paraíba - def
                "Nino Porebia:Brazil:71:61:L:5000:f4_s5_b0_t0_hblack5|" +
                 //Rodriguinho - mid
                "Rodraninhol:Brazil:64:62:L:2000:f5_s1_b0_t0_hblack10|" +
                 //Danielzinho - mid
                "Donaelzinon:Brazil:63:63:L:2300:f6_s4_b0_t0_hblack3|" +
                 //Gilberto - att
                "Golbentro:Brazil:57:79:L:2600:f7_s1_b2_t5_hblack8|" +
                 //Rossi - att
                "Rasi:Brazil:63:71:L:3500:f8_s1_b2_t9_hblack8",
                "shoered|shoeblackred",
                "glovebluered|gloveblacblue"},
             //Juventude
             new string[] {
                 "Caxias do Sul",
                 "60",
                 "58",
                 "0",
                 "0",
                 "darkgreen|darkgreen|darkgreen",
                 "whitegreenstripesshirt",
                 "greenshorts",
                 "greensocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Leonardo Jr
                "Leonardo Jr:Brazil:60:60:U:0:f9_s4_b2_t8_hblack4|" +


                 //Marcelo Carné - gk
                "Marcel Corner:Brazil:73:57:L:5000:f0_s1_b0_t0_hblack7|" +
                 //Eltinho - def
                "Etinhol:Brazil:64:59:L:5000:f1_s1_b0_t0_hblack1|" +
                 //Cleberson - def
                "Clobensen:Brazil:63:60:L:2000:f2_s4_b0_t0_hblack10|" +
                 //João Paulo - mid
                "Joel Paulen:Brazil:61:61:L:2300:f3_s4_b0_t0_hblack6|" +
                 //Gustavo Bochecha - mid
                "Gustavo Bohiha:Brazil:62:58:L:2600:f4_s4_b0_t0_hblack5|" +
                 //Capixaba - att
                "Copebaba:Brazil:55:69:L:3500:f5_s4_b2_t7_hblack3",
                "shoegreen",
                "gloveblackgreen|glovegreenwhite"},
             //Ceara
             new string[] {
                 "Ceara FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|black|black",
                 "whiteblackstripesshirt",
                 "blackwhitestripesshort",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Felipe Junior
                "Felipe Junior:Brazil:60:60:U:0:f6_s4_b0_t0_hblack13|" +

                 //Richard - gk
                "Ricky Rich:Brazil:79:58:L:5000:f7_s1_b0_t0_hblack9|" +
                 //Bruno Pacheco - def
                "Breno Poheho:Brazil:65:62:L:5000:f8_s4_b2_t4_hblack5|" +
                 //Vina - mid
                "Viini:Brazil:74:85:L:2000:f9_s1_b0_t0_hblack3|" +
                 //Fernando Sobral - mid
                "Fernao Sebroll:Brazil:66:64:L:2300:f0_s5_b2_t6_hblack10|" +
                 //Cléber - att
                "Cloberb:Brazil:63:76:L:2600:f1_s5_b0_t0_hblack13|" +
                 //Lima - att
                "Filipe Limi:Brazil:61:74:L:3500:f2_s4_b0_t0_hblack4",
                "shoegreen",
                "gloveblackwhite|globewhiteblack"},
             //Chapecoense            
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
             //Cuiaba
             new string[] {
                 "Cuiaba Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "darkgreen|darkgreen|darkgreen",
                 "greenshirt1",
                 "greenshorts",
                 "greensocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Luiz Junior
                "Luiz Junior:Brazil:60:60:U:0:f8_s4_b0_t0_hblack8|" +

                 //João Carlos - gk
                "Jo Carles:Brazil:68:55:L:5000:f9_s3_b0_t0_hblonde3|" +
                 //Anderson Conceição - def
                "Andi Coiceo:Brazil:63:59:L:5000:f0_s4_b2_t5_hblack1|" +
                 //Lucas Ramon - def
                "Luc Remons:Brazil:61:57:L:2000:f1_s4_b0_t0_hblack9|" +
                //Élvis - mid
                "Olvins:Brazil:61:60:L:2300:f2_s1_b0_t0_hblack5|" +
                //Élton - att
                "Olten:Brazil:59:69:L:2600:f3_s5_b2_t7_hblack10|" +
                //Felipe Marques - att
                "Filipe Marttens:Brazil:58:65:L:3500:f4_s4_b0_t0_hblack5",
                "shoegreen",
                "gloveblackgreen|glovegreenwhite"},
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
                 //Filipe Luís - def \\TO DO\\
                "Felipe Loeuss:Brazil:87:80:L:5000:f7_s1_b0_t0_hblack13|" +
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
             //Fluminense
             new string[] {
                 "Rio FE",
                 "60",
                 "58",
                 "0",
                 "0",
                 "darkgreen|darkclaretdarkgreen|darkclaretdarkgreen",
                 "darkredgreenstripesshirt",
                 "whiteredstripeshort",
                 "white",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Gustavo Junior
                "Gustavo Junior:Brazil:60:60:U:0:f2_s4_b0_t0_hblack10|" +

                 //Muriel - gk
                "Morieln:Brazil:81:62:L:5000:f3_s4_b2_t6_hblack1|" +
                 //Luccas Claro - def
                "Lucas Clerons:Brazil:79:64:L:5000:f4_s5_b2_t4_hblack3|" +
                 //Nino - def
                "Nanio:Brazil:80:71:L:2000:f5_s1_b0_t0_hblack10|" +
                //Nenê - mid
                "Noenen:Brazil:80:84:L:2300:f6_s1_b2_t9_hblack4|" +
                //Marcos Paulo - att
                "Pedro Marcess:Portugal:73:80:L:2600:f7_s4_b0_t0_hblack5|" +
                //Fred - att
                "Friennd:Brazil:69:78:L:3500:f0_s1_b2_t2_hblack9",
                "shoegreen|shoered",
                "gloveblackgreen"},
             //Fortaleza Esporte Clube
             new string[] {
                 "Fortaleza FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                 "redbluestripesshirt",
                 "blueshort",
                 "whiteredbluestripessocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Paulo Junior
                "Paulo Junior:Brazil:60:60:U:0:f8_s4_b2_t4_hblack11|" +


                 //Felipe Alves - gk
                "Filipe Olvens:Brazil:71:54:L:5000:f9_s1_b0_t0_hblack3|" +
                 //Tinga - def
                "Tongoa:Brazil:61:59:L:5000:f0_s5_b0_t0_hblack5|" +
                 //Bruno Melo - def
                "Brunon Moleo:Brazil:63:58:L:2000:f1_s4_b2_t9_hblack5|" +
                 //Juninho - mid
                "Jonointho:Brazil:63:61:L:2300:f2_s4_b0_t0_hblack10|" +
                 //Wellington Paulista - att
                "Wally Polinsta:Brazil:62:71:L:2600:f3_s4_b0_t0_hblack1|" +
                //David - att
                "Dovind:Brazil:61:70:L:3500:f4_s5_b0_t0_hblack3",
                "shoebluewhite",
                "gloveredwhite|gloveblueblue"},
             //Sport Recife
             new string[] {
                 "Recife FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|redblack|redblack",
                 "blackredstripeshorshirt",
                 "blacredstripeshort",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Douglas Jr
                "Douglas Jr:Brazil:60:60:U:0:f5_s4_b2_t7_hblack12|" +

                //Luan Polli - gk
                "Leon Palins:Brazil:75:54:L:5000:f6_s1_b2_t9_hblack5|" +
                //Iago Maidana - def
                "Tiago Modinad:Brazil:63:63:L:5000:f7_s1_b0_t0_hblack10|" +
                //Patric - def
                "Patricio Patr:Brazil:64:59:L:2000:f8_s4_b2_t4_hblack1|" +
                //Thiago Neves - mid
                "Tiago Novens:Brazil:64:63:L:2300:f9_s4_b0_t0_hblack9|" +
                //Marquinhos - att
                "Moriniques:Brazil:58:73:L:2600:f0_s4_b2_t5_hblack3|" +
                //Leandro Barcia - att
                "Leon Borca:Uruguay:59:71:L:3500:f1_s1_b0_t0_hblack10",
                "shoeblackred",
                "gloveredblack"},
             //Atletico GO (Atlético Goianiense)
             new string[] {
                 "Goiania Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|redblack|redblack",
                 "redblackstripeshirt",
                 "whiteredstripehorshort",
                 "redblackstripessocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Thiago Junior
                "Thiago Junior:Brazil:60:60:U:0:f2_s4_b0_t0_hblack13|" +

                //Fernando Miguel - gk
                "Fernao Minuels:Brazil:75:53:L:5000:f3_s1_b2_t4_hblack5|" +
                //Éder - def
                "Odern:Brazil:67:59:L:5000:f4_s4_b0_t0_hblack1|" +
                //Dudu - def
                "Doludu:Brazil:66:58:L:2000:f5_s1_b0_t0_hblack1|" +
                //Chico - mid
                "Hinco:South Korea:67:66:L:2300:f6_s4_b0_t0_hblack10|" +
                //Janderson - att
                "Jondersan:Brazil:60:71:L:2600:f7_s5_b0_t0_hblack5|" +
                //Zé Roberto - att
                "Zi Roberson:Brazil:59:71:L:3500:f8_s1_b2_t9_hblack7",
                "shoeblackred|shoered",
                "gloveredblack|gloveredwhite"},
             //Gremio
             new string[] {
                 "Porto Alegre",
                 "60",
                 "58",
                 "0",
                 "0",
                 //"lightblue|lightblue|lightblue",
                 "blue|blue|lightblue",
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
                 //Luiz Adriano - att \\TO DO\\
                "Leo Adrian:Brazil:72:87:L:3500:f3_s5_b2_t7_hblack14",
                "shoegreen",
                "glovegreenwhite|gloveblackgreen"},
             //Bragantino
             new string[] {
                 "Braganca",
                 "60",
                 "58",
                 "0",
                 "0",
                 "white|black|black",
                "whiteshirt",
                 "whiteshort",
                 "white",
                "skin_white_1",
                 "hair_19",
                 "white",
                 //Leandro Junior
                "Leandro Junior:Brazil:60:60:U:0:f2_s4_b0_t0_hblack4|" +


                 //Cleiton - gk
                "Centinio:Brazil:79:59:L:5000:f1_s1_b2_t7_hblack5|" +
                 //Claudinho - mid
                "Cloudanitho:Brazil:85:86:L:5000:f0_s5_b2_t6_hblack14|" +
                 //Edimar - def
                "Demirar:Brazil:73:69:L:2000:f4_s4_b0_t0_hblack1|" +
                 //Leo Ortiz - def
                "Leon Obintz:Brazil:71:65:L:2300:f5_s1_b2_t5_hblack3|" +
                 //Helinho - att
                "Honhilono:Brazil:68:73:L:2600:f6_s5_b0_t0_hblack5|" +
                 //Ytalo - att
                "Talono:Brazil:70:81:L:3500:f7_s5_b2_t8_hblack10",
                "shoeredwhite|shoeblue",
                "gloveblackwhite|globewhiteblack"},
            //Santos
             new string[] {
                 "Santos Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "black|black|black",
                 "whiteshirt",
                 "whiteblueshort",
                 "white",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Fernando Jr
                "Fernando Jr:Brazil:60:60:U:0:f8_s4_b2_t4_hblack5|" +

                 //João Paulo - gk
                "Paulinhons:Brazil:79:57:L:2000:f1_s4_b0_t0_hblack3|" +

                 //Felipe Jonatan - def
                "Jonathanos:Brazil:69:63:L:5000:f9_s4_b2_t8_hblack5|" +
                 //Madson - def
                "Midens:Brazil:76:70:L:5000:f0_s4_b0_t0_hblack5|" +                 
                 //Luan Peres - def
                "Leonel Paens:Brazil:76:65:L:2300:f2_s4_b2_t10_hblack9|" +
                 //Kaio Jorge  - att
                "Jorgeninio:Brazil:64:74:L:2600:f3_s1_b0_t0_hblack1|" +
                 //Marinho - att
                "Marheninio:Brazil:69:89:L:3500:f4_s5_b2_t4_hblack3",
                "shoered|shoeblue",
                "gloveblackwhite|globewhiteblack"},
             //Sao Paulo
             new string[] {
                 "Sao Paulo Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|redblack|redblack",
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
             new string[] {
                 "Chapeco",
                 "60",
                 "58",
                 "0",
                 "0",
                 "darkgreen|darkgreen|darkgreen",
                 "greenshirt",
                 "greenwhitestripesshort",
                 "greensocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Joao Junior
                "Joao Junior:Brazil:60:60:U:0:f3_s4_b2_t6_hblack6|" +

                 //Keiller - gk
                "Koilern:Brazil:73:54:L:5000:f4_s2_b0_t0_hblack5|" +
                 //Kadu - def\\TO DO\\
                "Kotuus:Brazil:65:61:L:5000:f5_s1_b0_t0_hblack1|" +
                 //Joílson - def
                "Jonillsen:Brazil:63:59:L:2000:f6_s5_b0_t0_hblack5|" +
                 //Fernandinho - att
                "Fornentinho:Brazil:60:64:L:2300:f7_s5_b0_t0_hblack8|" +
                 //Paulinho - att
                "Polintho:Brazil:63:71:L:2600:f8_s1_b0_t0_hblack10|" +
                 //Anselmo Ramon - att
                "Anselo Romeno:Brazil:62:75:L:3500:f9_s3_b0_t0_hblack3",
                "shoegreen",
                "gloveblackgreen|glovegreenwhite"},
             //Customize team
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
 LIST OF NOT TAKEN NAMES
 joao vitor
henrique
willian
Carlos
marcelo
Alexandre
alex
italo
raphael
FLÁVIO
bernardo
Andre
luciano
ricardo
luis
vagner
Ramon
jeferson
David
geovanne
wesley
http://www.studentsoftheworld.info/penpals/stats.php?Pays=BRA

*/
