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
    public class LeagueSpain
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
                 "Bilbao Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                "redwhitestripes",
                 "black",
                 "blacksocks",
                "skin_white_1",
                 "hair_19",
                 "red",
                 //Miguel Junior
                "Miguel Junior:Spain:60:60:U:0:f8_s1_b2_t9_hblack2|" +


                //Simon Unai - gk
                "Simi Umain:Spain:81:68:L:3000:f9_s3_b2_t7_hblack10|" +               
                //Capa Ander - def
                "Andru Capu:Spain:82:72:L:5000:f1_s1_b0_t0_hblack6|" +
                //Garcia Raul - mid
                "Rui Gornce:Spain:75:75:L:5000:f2_s1_b2_t8_hblack8|" +
                //Williams Inaki - att
                "Ikani Willi:Spain:69:80:L:3300:f3_s5_b0_t0_hblack5|" +
                //Berenguer Alejandro - att
                "Alex Berero:Spain:67:81:L:3800:f4_s2_b0_t0_hblack4|" +
                 //Muniain Iker - mid
                "Ikar Man:Spain:77:75:L:400:f5_s2_b2_t6_hred6",
                "shoeblackred|shoeredwhite",
                "gloveredwhite"},
             new string[] {
                 "Eibar FC",
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
                 "blue",
                 //Pablo Junior
                "Pablo Junior:Spain:60:60:U:0:f5_s1_b0_t0_hblack1|" +

                //Dmitrovic Marko - gk 
                "Mark Mivictro:Serbia:70:60:L:5000:f5_s1_b2_t5_hblonde3|" +
                 //Kike Garcia - att
                "Kiko Gerci:Spain:60:78:L:3300:f0_s1_b0_t0_hblack5|" +
                //Burgos Esteban - def
                "Esti Bogosa:Argentina:70:50:L:7500:f6_s1_b2_t9_hblack3|" +               
                //Inui Takashi - mid
                "Tokishi Ina:Japan:61:60:L:1300:f7_s1_b0_t0_hblack9|" +
                //Enrich Sergio - att
                "Sergo Enryk:Spain:51:69:L:1800:f8_s1_b2_t9_hblack10|" +
                //Gil Bryan  - att
                "Brajan:Spain:57:70:L:2300:f9_s3_b0_t0_hblack5",
                "shoeblackred|shoered",
                "gloveblacblue|glovebluewhite"},
             new string[] {
                 "Granada FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                 "redwhitehor",
                 "bluewhitestripe",
                 "whiteredstripe",
                 "skin_white_1",
                 "hair_19",
                 "red",
                //Martin Junior
                "Martin Junior:Spain:60:60:U:0:f1_s1_b0_t0_hblack13|" +

                //Silva Rui - gk 
                "Ruis Slive:Portugal:80:67:L:3000:f2_s1_b2_t8_hblack4|" +
                //Sanchez German def
                "Garry Sonhrez:Spain:83:73:L:7500:f3_s1_b2_t4_hblack5|" +
                //Herrera Yangel - mid
                "Yoel Herare:Venezuela:75:75:L:5000:f4_s4_b2_t10_hblack3|" +                              
                 //Molina Jorge - att
                "Jurge Melis:Spain:72:83:L:3600:f6_s1_b2_t9_hblack5|" +
                 //Soldado Roberto - att
                "Robi Saldo:Spain:73:85:L:4000:f7_s1_b2_t5_hblack1|" +
                 //Kenedy - att
                "Kanadie:Brazil:70:81:L:3400:f5_s4_b2_t7_hblack3",
                "shoeredwhite|shoebluewhite",
                "gloveredwhite|gloveredwhite"},
             //Levante
             new string[] {
                 "Levante Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "claret|claretdarkblue|claretdarkblue",
                 "bluered",
                 "darkblueshort",
                 "darkblue",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Alejandro Jr
                "Alejandro Jr:Spain:60:60:U:0:f8_s1_b0_t0_hblack6|" +
                 
                 //Fernandez Abarisketa Aitor - gk
                "Ferni Ariea:Spain:78:61:L:5000:f9_s1_b2_t6_hblack6|" +
                 //Duarte Oscar - def
                "Oski Darute:Costarica:76:69:L:7500:f0_s1_b0_t0_hblack3|" +
                 //Vezo Ruben - def
                "Vizon Runeb:Portugal:77:66:L:2300:f1_s4_b0_t0_hblack5|" +
                 //Morales Jose - mid
                "Jones Maes:Spain:73:76:L:4100:f2_s4_b2_t10_hblack5|" +
                 //Roger Marti - att
                "Motir Ragi:Spain:65:79:L:3200:f3_s1_b2_t4_hblack3|" +
                 //Melero Gonzalo - mid
                "Gonzo Mreon:Spain:73:73:L:3100:f4_s4_b0_t0_hblack5",
                "shoered|shoeblackred",
                "glovebluered|gloveblacblue"},
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
                 "Villarreal Town",
                 "60",
                 "58",
                 "0",
                 "0",
                 "yellow|yellowblue|yellowblue",
                "yellowtshirt",
                "yellowshort",
                "yellowsocks",
                "skin_white_1",
                 "hair_19",
                 "yellow",
                 //Diego Junior
                "Diego Junior:Spain:60:60:U:0:f8_s1_b0_t0_hred4|" +
                 
                 //Asenjo Sergio gk
                "Serho Aronje:Spain:83:63:L:5000:f6_s1_b2_t9_hblack3|" +
                 //Albiol Raul def
                "Rui Olbial:Spain:82:68:L:5300:f7_s1_b2_t5_hblack5|" +
                 //Parejo Daniel mid
                "Danio Preho:Spain:81:74:L:6100:f8_s1_b2_t9_hblack13|" +
                 //Trigueros Manuel mid
                "Mano Teoser:Spain:82:75:L:7500:f9_s1_b2_t6_hblack7|" +
                 //Moreno Gerard att
                "Geri Mrieno:Spain:74:89:L:7900:f0_s1_b2_t2_hblack8|" +
                //Alcacer Paco - att
                "Pato Alencer:Spain:73:88:L:7900:f1_s1_b0_t0_hblonde2",
                "shoeyellow|shoeblack",
                "gloveyellow|gloveyellowblack"},
             new string[] {
                 "Cadiz FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "yellow|yellowblue|yellowblue",
                "yellowwithblueskhirt",
                 "blueyellowstripesshort",
                 "yellowbluestripecsocks",
                "skin_white_1",
                 "hair_19",
                 "yellow",
                //Manuel Junior
                "Manuel Junior:Spain:60:60:U:0:f2_s1_b2_t8_hblack1|" +
              
                 //Ledesma Jeremias gk
                "Jaremo Landsem:Argentina:80:66:L:5000:f3_s1_b2_t4_hblack15|" +
                 //Espino Alfonso - def
                "Alonso Epinas:Uruguay:77:69:L:5000:f4_s4_b0_t0_hblack4|" +                 
                //Juan Cala def
                "Johan Colo:Spain:79:68:L:3500:f5_s1_b2_t5_hblack6|" +
                 //Alejandro "Álex" Fernández Iglesias mid
                "Aleixo:Spain:78:75:L:4000:f6_s1_b0_t0_hred3|" +
                 //Negredo Alvaro att
                "Alveiro Nagrudo:Spain:73:78:L:4500:f7_s1_b2_t5_hblack3|" +
                 //Lozano Anthony att
                "Antonio Loranzo:Honduras:71:77:L:4900:f8_s5_b2_t5_hblack5",
                "shoeyellow|shoeblack",
                "gloveyellowwhite"},
             new string[] {
                 "Elche City",
                 "60",
                 "58",
                 "0",
                 "0",
                 "green|green|green",
                "whitewithgreenshirt",
                 "whitewithgreenshort",
                 "white",
                "skin_white_1",
                 "hair_19",
                 "green",
                 //Leo Junior
                "Mauro Junior:Spain:60:60:U:0:f9_s1_b2_t6_hblack2|" +
                 
                 //Badia Edgar gk
                "Egrad Bondio:Spain:78:59:L:5000:f0_s1_b0_t0_hblonde3|" +
                 //Verdu Gonzalo def
                "Gonzo Vadeur:Spain:74:61:L:3100:f1_s1_b2_t7_hblack3|" +
                 //Fidel mid 
                "Friled:Spain:66:63:L:3800:f2_s1_b2_t8_hblack10|" +
                 //José Antonio Ferrández Pomares(Josan)  mid
                "Hosen:Spain:65:64:L:3800:f3_s1_b2_t4_hblonde1|" +
                 //Boye Lucas att
                "Luki Bayo:Argentina:60:75:L:4100:f4_s2_b2_t9_hblack5|" +
                 //Milla Pere att
                "Paho Melsa:Spain:61:75:L:4500:f5_s1_b0_t0_hblack7",
                "shoegreen",
                "glovegreenwhite"},
             new string[] {
                 "Getafe Blue",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "darkbluewithblacshirt",
                 "darkbluedarkstripeskort",
                 "darkblue",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Mateo Junior
                "Mateo Junior:Spain:60:60:U:0:f6_s1_b2_t9_hblack3|" +

                //Soria David gk
                "Dani Seora:Spain:77:61:L:35000:f9_s1_b2_t6_hblack11|" +
                 //Nyom Allan - def
                "Ali Nomoyn:Cameroon:74:63:L:7500:f7_s5_b0_t0_hblack13|" +
                 //Cucurella Marc def
                "Marko Cruola:Spain:76:63:L:5000:f8_s1_b2_t9_hblack1|" +             
                 //Arambarri Mauro mid
                "Mruo Amrao:Uruguay:75:73:L:4300:f0_s1_b2_t2_hblack3|" +
                 //Ángel Rodríguez att
                "Anhelo Redro:Spain:73:80:L:4800:f1_s1_b2_t7_hblack6|" +
                 //Mata Jaime att
                "James Motes:Spain:78:81:L:5500:f2_s1_b2_t8_hblack5",
                "shoebluewhite",
                "glovebluewhite"},
             new string[] {
                 "Aragon Huesca",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|blue|red",
                "darkbluestripehor",
                 "darkblueredstropedn",
                 "darkblueredstripes",
                "skin_white_1",
                 "hair_19",
                 "red",      
                 //Javier Junior
                "Javier Junior:Spain:60:60:U:0:f1_s1_b0_t0_hblack4|" +
              
                //Fernandez Alvaro gk
                "Alveiro Farni:Spain:76:55:L:5000:f4_s1_b0_t0_hblack5|" +
                //Siovas Dimitrios - def
                "Dimi Sovaes:Greece:74:62:L:7500:f5_s1_b2_t5_hblack3|" +
                //Pulido Jorge def
                "Jurhe Paldie:Spain:73:61:L:2900:f6_s1_b2_t9_hblonde3|" +
                //Ferreiro David mid 
                "Dani Foreino:Spain:67:64:L:3100:f7_s1_b2_t5_hblack1|" +
                 //Galan Javier att
                "Xaver Goluns:Spain:63:71:L:3400:f8_s1_b2_t9_hblack6|" +
                 //Mir Vicente Rafael att
                "Rapha Vincces:Spain:71:87:L:3800:f9_s1_b0_t0_hblack10",
                "shoeblackred|shoered",
                "gloveblacblue|gloveredblack"},
             //Osasuna
             new string[] {
                 "Pamplona Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "red|red|red",
                 "redshirt",
                 "darkblueredstropedn",
                 "blacredstripessocks",
                "skin_white_1",
                 "hair_19",
                 "red",   
                //Marcos Junior
                "Marcos Junior:Spain:60:60:U:0:f0_s2_b0_t0_hred2|" +

                //Herrera Sergio gk
                "Serho Erhea:Spain:80:67:L:5000:f2_s1_b0_t0_hblack3|" +
                //Garcia David - def
                "Dave Arganca:Spain:75:69:L:7500:f1_s1_b0_t0_hblack5|" +               
                //Chimy Ávila att
                "Avi Kimly:Argentina:67:74:L:4800:f3_s1_b0_t0_hblack7|" +
                //Garcia Ruben mid 
                "Ruis Gerci:Spain:76:73:L:5200:f4_s1_b2_t8_hblack1|" +
                //Calleri Jonathan att
                "Jones Cleari:Argentina:69:75:L:5800:f5_s4_b2_t7_hblack6|" +
                //Budimir Ante att
                "Antonio Bandim:Croatia:70:81:L:6200:f6_s1_b0_t0_hblack4",
                "shoeblackred|shoered",
                "gloveredblack|gloveblacblue"},
             new string[] {
                 "Valladolid FC",
                 "60",
                 "58",
                 "0",
                 "0",
                 "purple|purple|purple",
                "whitepurpleshirt",
                 "whireredstripe",
                 "whiteredsocks",
                "skin_white_1",
                 "hair_19",
                 "blue", 
                 //Alex Junior
                "Alex Junior:Spain:60:60:U:0:f7_s3_b2_t6_hblack6|" +
                 
                //Masip Jordi gk
                "Joid Moseb:Spain:75:61:L:2000:f9_s1_b0_t0_hblack1|" +
                //Gonzalez Bruno def
                "Berni Gonzo:Spain:71:64:L:5000:f8_s1_b2_t9_hblack3|" +               
                 //Plano Oscar mid
                "Oski Ponola:Spain:65:65:L:2300:f0_s1_b2_t2_hblack5|" +
                 //Hervias Pablo - mid
                "Pab Honrvas:Spain:69:68:L:7500:f1_s1_b0_t0_hblack7|" +
                 //Weissman Shon att
                "Shin Mans:Israel:62:75:L:2600:f2_s2_b0_t0_hblack4|" +
                 //Orellana Fabian att
                "Faibien Olena:Chile:63:73:L:3500:f3_s4_b0_t0_hblack6",
                "shoewhite",
                "gloveredwhite"},
             new string[] {
                 "Vigo Club",
                 "60",
                 "58",
                 "0",
                 "0",
                 "lightblue|lightblue|lightblue",
                "lightbluestripes",
                 "whiteblueshort",
                 "whitebluesocks",
                "skin_white_1",
                 "hair_19",
                 "blue", 
                //Sergio Junior
                "Sergio Junior:Spain:60:60:U:0:f4_s4_b2_t10_hred5|" +
                 
                //Blanco Ruben gk
                "Ruis Blenk:Spain:79:66:L:5000:f5_s1_b0_t0_hblack3|" +
                 //Mallo Hugo def
                "Ugo Mole:Spain:75:70:L:2000:f6_s1_b0_t0_hblack5|" +
                 //Tapia Renato - def
                 "Renan Atpra:Peru:74:65:L:2300:f7_s5_b0_t0_hblack1|" +
                 //Mendez Brais mid
                "Baris Munez:Spain:75:73:L:2300:f8_s1_b2_t9_hblack10|" +
                 //Mina Santi att
                "Sonte Main:Spain:69:74:L:2600:f9_s1_b2_t6_hblack4|" +
                 //Aspas Iago att
                "Igo Aps:Spain:70:78:L:3500:f8_s1_b0_t0_hblack6",
                "shoebluewhite|shoeblue",
                "glovelightbluewhite"},
             //Alaves
             new string[] {
                 "Alava Team",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "whitebluestripesshirt",
                 "bluewhitestripe",
                 "whitebluesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                 //Marc Junior
                "Marc Junior:Spain:60:60:U:0:f0_s4_b0_t0_hblack13|" +


                 //Pacheco Fernando gk
                "Frans Pikoko:Spain:79:55:L:2000:f3_s1_b2_t4_hblack10|" +
                //Battaglia Rodrigo - def
                "Rod Bolatga:Argentina:69:61:L:7500:f1_s1_b0_t0_hblack5|" +
                 //Lejeune Florian def
                "Florius Lajuhne:France:68:59:L:5000:f2_s2_b2_t9_hblack3|" +                
                 //Mendez Edgar mid
                "Egrad Manezdo:Spain:68:65:L:2300:f4_s1_b2_t8_hblack7|" +
                 //Joselu att
                "Jussoleu:Spain:64:78:L:2600:f5_s4_b2_t7_hblack1|" +
                 //Perez Lucas att
                "Luci Paroz:Spain:65:74:L:3500:f6_s1_b2_t9_hblack7",
                "shoebluewhite",
                "glovebluewhite"},
             //Real Sociedad
             new string[] {
                 "San Sebastian",
                 "60",
                 "58",
                 "0",
                 "0",
                 "blue|blue|blue",
                "whitebluestripesshirt",
                 "whitebluestripeshort",
                 "whitebluesocks",
                "skin_white_1",
                 "hair_19",
                 "blue",
                //Carlos Junior
                "Carlos Junior:Spain:60:60:U:0:f7_s1_b2_t5_hblack11|" +    
                 
                //Remiro Alejandro gk
                "Alex Ramio:Spain:83:61:L:5000:f8_s1_b0_t0_hblack3|" +
                 //Le Normand Robin - def
                "Rob Nementr:France:75:71:L:7500:f9_s3_b0_t0_hblack8|" +
                 //Gorosabel Andoni def
                "Andi Gureble:Spain:78:70:L:5100:f0_s1_b0_t0_hblack3|" +
                 //Oyarzabal Mikel att
                "Mechael Ozlab:Spain:72:83:L:5800:f1_s1_b0_t0_hblack10|" +
                 //Portu mid
                "Parta:Spain:74:81:L:6500:f2_s1_b2_t8_hblack6|" +
                //Isak Alexander att
                "Alex Ozaks:Sweden:71:85:L:7000:f3_s5_b2_t7_hblack3",
                "shoebluewhite",
                "glovebluewhite"},
            //Betis Sevilla
             new string[] {
                 "Andalusia BS",
                 "60",
                 "58",
                 "0",
                 "0",
                 "green|green|green",
                "whitegreenstripesshirt",
                 "white",
                 "whitegreensocks",
                "skin_white_1",
                 "hair_19",
                 "green",
                //Jorge Junior
                "Jorge Junior:Spain:60:60:U:0:f4_s1_b0_t0_hblack12|" +
               
                //Bravo Claudio gk
                "Klaus Brovis:Chile:81:65:L:5000:f5_s4_b0_t0_hblack5|" +
                 //Emerson def
                "Jeferson:Brazil:83:68:L:4800:f6_s5_b0_t0_hblack3|" +
                 //Canales Sergio mid
                "Serho Celanos:Spain:84:83:L:5000:f7_s1_b0_t0_hblack6|" +
                //Carvalho William mid
                "Willi Convelo:Portugal:80:73:L:7500:f8_s5_b0_t0_hblack5|" +
                 //Tello Cristian att
                "Kris Talleo:Spain:69:75:L:5600:f9_s1_b0_t0_hblack1|" +
                 //Iglesias Borja att
                "Berho Ilasiens:Spain:73:84:L:5900:f0_s4_b2_t5_hblack5",
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
