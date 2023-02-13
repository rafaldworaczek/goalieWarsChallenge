using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Localization.Settings;

namespace LANGUAGE_NS
{
    public class Languages : MonoBehaviour
    {
        private static int CHOOSEN_LANG_NUMBER = 2;
        private static Dictionary<string, string> langs;

        public static void initLangs()
        {
            langs = new Dictionary<string, string>();

            int currentIdx = 0;
            if (PlayerPrefs.HasKey("LANG_FLAG_IDX"))
                currentIdx = PlayerPrefs.GetInt("LANG_FLAG_IDX");

            if (currentIdx > 0)
            {
                for (int i = 0; i < desc.Length; i++)
                {
                    langs.Add(desc[i][getLangIdx("EN")],
                              desc[i][currentIdx]);
                }
            }
        }

        public static string getTranslate(string textToTranslate,
                                          List<string> dynamicVar)
        {
            string textPreprocessing = textToTranslate;
            for (int i = 0; i < dynamicVar.Count; i++)
            {
                Regex rex = new Regex(dynamicVar[i]);
                textPreprocessing =
                    rex.Replace(textPreprocessing, "VAR_" + i.ToString(), 1);
                //print("#DBGLANG BEFORE i " + textPreprocessing);
            }

            //print("#DBGLANG BEFORE " + textPreprocessing);

            string outHashText;
            langs.TryGetValue(textPreprocessing, out outHashText);
            //print("#DBGLANG HAHS RES " + outHashText);

            if (outHashText != null)
            {
                for (int i = 0; i < dynamicVar.Count; i++)
                {
                    outHashText =
                        Regex.Replace(outHashText, "VAR_" + i.ToString(), dynamicVar[i]);
                }

                return outHashText;
            }
          
            return textToTranslate;
        }

        public static string getTranslate(string textToTranslate)
        {      
            string outHashText = "";
            langs.TryGetValue(textToTranslate, out outHashText);

            if (outHashText != null)
            {
                return outHashText;
            }
            
            return textToTranslate;
        }


        public static int getLangIdx(string lang)
        {
            switch (lang)
            {
                case "EN":
                    return 0;
                case "PL":
                    return 1;
                case "ES":
                    return 2;
                case "PT":
                    return 3;
            }

            return 0;
        }

        private static string[][] desc = new string[][] {
             new string[] {
                //EN
                "if you hit either coin or diamond by ball you get extra bonus!",
                //PL
                "Traf pilka w monety lub diamenty a dostaniesz ekstra bonus!",
                //ES
                "¡Acierta con el balón en las monedas o los diamantes y conseguirás un bono extra!",
                //PT
                "Acerte as moedas ou os diamantes com a bola para receber um bônus extra!",
                "",
                "",
                },
             new string[] {
                //EN
                "Hit the crossbar by ball and extra bonus is your!",
                //PL
                "Uderz w poprzeczke a dostaniesz dodatkowy bonus!",
                //ES
                "¡Golpea en el larguero y conseguirás un bono adicional!",
                //PT
                "Acerte a trave e receba um bônus!",
                "",
                },
             new string[] {
                //EN
                "Ads failed",
                //PL
                "Blad reklamy",
                //ES
                "Error del anuncio",
                //PT
                "Erro no anúncio",
                "",
                },
             new string[] {
                //EN
                "Congratulations!\nYou just earned 20 coins + 20 diamonds!",
                //PL
                "Gratulacje!\nOtrzymujesz 20 monet i 20 diamentow!",
                //ES
                "¡Enhorabuena!\n¡Recibes 20 monedas y 20 diamantes!",
                //PT
                "Parabéns!\nVocê recebeu 20 moedas e 20 diamantes!",
                "",
             },
             new string[] {
                //EN
                "Congratulations!\nYou just earned 20 diamonds!",
                //PL
                "Gratulacje!\nOtrzymujesz 20 diamentów!",
                //ES
                "¡Enhorabuena!\n¡Recibes 20 diamantes!",
                //PT
                "Parabéns!\nVocê recebeu 20 diamantes!",
                "",
             },
             new string[] {
                //EN
                "Congratulations!\nYou just earned 20 coins!",
                //PL
                "Gratulacje!\nOtrzymujesz 20 monet!",
                //ES
                "¡Enhorabuena!\n¡Recibes 20 monedas!",
                //PT
                "Parabéns!\nVocê recebeu 20 moedas!",
                "",
             },
             new string[] {
                //EN
                "Congratulations!\nYou just earned 15 coins + 15 diamonds!",
                //PL
                "Gratulacje!\nOtrzymujesz 15 monet i 15 diamentow!!",
                //ES
                "¡Enhorabuena!\n¡Recibes 15 monedas y 15 diamantes!",
                //PT
                "Parabéns!\nVocê recebeu 15 moedas e 15 diamantes!",
                "",
                "",
                },
             new string[] {
                //EN
                "Congratulations!",
                //PL
                "Gratulacje!",
                //ES
                "¡Enhorabuena!",
                //PT
                "Parabens!",
                "",
                },
             new string[] {
                //EN
                "SORRY\nNO EXTRA COINS THIS TIME...\n" +
                 "TRY AGAIN! GOOD LUCK!",
                //PL
                "SORRY\nNIESTETY NIE OTRZYMASZ EKSTRA MONET TYM RAZEM...\n" +
                 "SPROBUJ PONOWNIE",
                //ES
                "LO SENTIMOS\nPOR DESGRACIA NO RECIBES MONEDAS EXTRAS ESTA VEZ...\n" +
                 "INTÉNTALO DE NUEVO",
                //PT
                "DESCULPE\nINFELIZMENTE VOCE NAO RECEBERA AS MOEDAS EXTRAS DESTA VEZ...\n" +
                "TENTE OUTRA VEZ.",
                "",
                },
             new string[] {
                //EN
                "TRY AGAIN",
                //PL
                "SPROBOJ PONOWNIE",
                //ES
                //INTÉNTALO DE NUEVO
                "INTENTALO DE NUEVO",
                //PT
                "TENTE OUTRA VEZ",
                "",
             },
             new string[] {
                //EN
                "Team name cannot be empty",
                //PL
                "Nazwa zespołu nie moze byc pusta",
                //ES
                "El nombre del equipo no puede estar vacio",
                //PT
                "O nome do time nao pode ficar vazio",
                "",
             },
             new string[] {
                //EN
                "Sorry. Only English characters are allowed",
                //PL
                "Sorry. Tylko znaki angielskie sa akceptowalne",
                //ES
                "Lo sentimos. Solo se aceptan caracteres ingleses",
                //PT
                "Desculpe. Apenas caracteres ingleses são aceitos",
                "",
             },
             new string[] {
                //EN
                "The team with such name already exists",
                //PL
                "Sorry! Taka nazwa zespolu juz istnieje",
                //ES
                "¡Lo sentimos! Este nombre de equipo ya existe",
                //PT
                "Desculpe! Esse nome de time ja existe",
                "",
                "",
                },
             new string[] {
                //EN
                "Player name cannot be empty",
                //PL
                "Nazwa zawodnika nie moze byc pusta",
                //ES
                "El nombre del jugador no puede estar vacio",
                //PT
                "O nome do jogador nao pode ficar vazio",
                "",
                },
             new string[] {
                //EN
                "Team VAR_0" +
                " has been added to the following leageus:\n" +
                "Brazil\n" +
                "England\n" +
                "Italy\n" +
                "Germany\n" +
                "Spain\n" +
                "Champ Cup\n",
                //PL
                "Zespol VAR_0" +
                " zostal dodany do nastepujacych lig:\n" +
                "Brazil\n" +
                "England\n" +
                "Italy\n" +
                "Germany\n" +
                "Spain\n" +
                "Champ Cup\n",
                //ES
                "El equipo VAR_0" +
                " ha sido anadido a las siguientes ligas:\n" +
                "Brazil\n" +
                "England\n" +
                "Italy\n" +
                "Germany\n" +
                "Spain\n" +
                "Champ Cup\n",
                //PT
                "O time VAR_0" +
                " foi adicionado as seguintes ligas:\n" +
                "Brazil\n" +
                "England\n" +
                "Italy\n" +
                "Germany\n" +
                "Spain\n" +
                "Champ Cup\n",
                "",
                },
             new string[] {
                //EN
                "2000 diamonds",
                //PL
                "2000 diamentow",
                //ES
                "2000 diamantes",
                //PT
                "2000 diamantes",
                "",
                "",
                },
             new string[] {
                //EN
                "Image can not have width bigger than 400 and height bigger than 256. " +
                    "It must be in png format",
                //PL
                "Obraz nie moze mieć szerokości wiekszej niż 400 and wysokości wiekszej niż 256. " +
                 "Musi być w formacie png.",
                //ES
                "La imagen no puede tener un ancho mayor de 400 ni un alto mayor de 256. " +
                 "Debe estar en formato png.",
                //PT
                "A imagem nao pode ser mais larga do que 400 e mais alta que 256. " +
                "Deve estar no formato png.",
                "",
                },
             new string[] {
                //EN
                "BUY",
                //PL
                "KUP",
                //ES
                "COMPRAR",
                //PT
                "COMPRE",
                "",
                },
             new string[] {
                //EN
                "PRICE\n2000 diamonds",
                //PL
                "Cena 2000 diamentów",
                //ES
                "Precio de 2000 diamantes",
                //PT
                "Preço por 2000 diamantes",
                "",
             },
             new string[] {
                //EN
                "You don't have enough diamonds to buy this card.\n" +
                    "Go to the shop to buy diamonds",
                //PL
                "Nie masz wystarczającej liczby diamentów żeby kupić tą kartę." +
                "Przejdz do sklepu kupić diamenty",
                //ES
                "No tienes suficientes diamantes para comprUar esta carta." +
                "Ve a la tienda a comprar diamantes",
                //PT
                "Você não tem diamantes suficientes para comprar esta carta." +
                "Vá para a loja para comprar diamantes",
                "",
             },
             new string[] {
                //EN
                "Buy player's card for VAR_0 diamonds",
                //PL
                "Kup kartę zawodnika za VAR_0 diamentów",
                //ES
                "Compra la carta del jugador por VAR_0 diamantes",
                //PT
                "Compre a carta do profissional por VAR_0 diamantes",
                "",
             },
             new string[] {
                //EN
                "Unlock team's player",
                //PL
                "Odblokuj karte zawodnika",
                //ES
                "Desbloquea la carta del jugador",
                //PT
                "Desbloqueie a carta do jogador",
                "",
                "",
                },
             new string[] {
                //EN
                "Defense: VAR_0",
                //PL
                "Obrona: VAR_0",
                //ES
                "Defensa: VAR_0",
                //PT
                "Defesa: VAR_0",
                "",
                },
             new string[] {
                //EN
                "Attack: VAR_0",
                //PL
                "Atak: VAR_0",
                //ES
                "Ataque: VAR_0",
                //PT
                "Ataque: VAR_0",
                "",
                },
             new string[] {
                //EN
                "Buy player's card",
                //PL
                "Kup kartę zawodnika",
                //ES
                "Compra la carta del jugador",
                //PT
                "Compre a carta do profissional",
                "",
             },
             new string[] {
                //EN
                "Sell player",
                //PL
                "Sprzedaj zawodnika",
                //ES
                "Vende al jugador",
                //PT
                "Vender jogador",
                "",
             },
             new string[] {
                //EN
                "You can not sell only player",
                //PL
                "Nie możesz sprzedać jedynego zwodnika",
                //ES
                "No puedes vender al único jugador",
                //PT
                "Você não pode vender seu único jogador",
                "",
             },
             new string[] {
                //EN
                "No offer of buying this player",
                //PL
                "Brak ofert kupna tego zawodnika",
                //ES
                "No hay ofertas de compra para este jugador",
                //PT
                "Sem ofertas de compra para o jogador",
                "",
                "",
                },
             new string[] {
                //EN
                " SELL PLAYER ",
                //PL
                " SPRZEDAJ ZAWODNIKA ",
                //ES
                "VENDE AL JUGADOR ",
                //PT
                " VENDER JOGADOR ",
                "",
                },
             new string[] {
                //EN
                "VAR_0 would like to buy this player \n for VAR_1 diamonds",
                //PL
                "VAR_0 złożył ofertę kupna tego zawodnika za VAR_1 diamentów",
                //ES
                "VAR_0 ha presentado una oferta de compra para este jugador por VAR_1 diamantes",
                //PT
                "VAR_0 fez uma oferta de compra para este jogador por VAR_1 diamantes",
                "",
                },                 
             new string[] {
                //EN
                "You can not have more than VAR_0 player's card.\n" +
                "Sell player by clicking on a card",
                //PL
                "Maksymalna liczba card nie może być większa niż VAR_0\n" +
                "Możesz sprzedać zawodnika klikajac na jego kartę",
                //ES
                "El número máximo de cartas no puede ser mayor que VAR_0\n" +
                "Puedes vender a un jugador haciendo clic en su carta",
                //PT
                "O número máximo de cartas não pode ser superior a VAR_0\n" +
                "Você pode vender o jogador clicando na carta dele",
                "",
                "",
                },                                     
             new string[] {
                //EN
                "SELL PLAYER",
                //PL
                "SPRZEDAJ ZAWODNIKA",
                //ES
                "VENDE AL JUGADOR",
                //PT
                "VENDER JOGADOR",
                "",
                },                                                                               
               new string[] {
                //EN
                "KID",
                //PL
                "DZIECIAK",
                //ES
                "NINO",
                //PT
                "FRALDINHA",
                "",
                "",
                },
             new string[] {
                //EN
                "EASY",
                //PL
                "PROSTY",
                //ES
                "SENCILLO",
                //PT
                "INICIANTE",
                "",
                },
             new string[] {
                //EN
                "NORMAL",
                //PL
                "NORMALNY",
                //ES
                "NORMAL",
                //PT
                "NORMAL",
                "",
                },
             new string[] {
                //EN
                "HARD",
                //PL
                "ZAWODOWIEC",
                //ES
                "PROFESIONAL",
                //PT
                "PROFISSIONAL",
                "",
             },
             new string[] {
                //EN
                "EXPERT",
                //PL
                "EKSPERT",
                //ES
                "EXPERTO",
                //PT
                "EXPERT",
                "",
             },
             new string[] {
                //EN
                "8 SECONDS",
                //PL
                "8 SEKUND",
                //ES
                "8 SEGUNDOS",
                //PT
                "8 SEGUNDOS",
                "",
             },
             new string[] {
                //EN
                "10 SECONDS",
                //PL
                "10 SEKUND",
                //ES
                "10 SEGUNDOS",
                //PT
                "10 SEGUNDOS",
                "",
             },
             new string[] {
                //EN
                "15 SECONDS",
                //PL
                "15 SEKUND",
                //ES
                "15 SEGUNDOS",
                //PT
                "15 SEGUNDOS",
                "",
             },
             new string[] {
                //EN
                "20 SECONDS",
                //PL
                "20 SEKUND",
                //ES
                "20 SEGUNDOS",
                //PT
                "20 SEGUNDOS",
                "",
             },
             new string[] {
                //EN
                "30 SECONDS",
                //PL
                "30 SEKUND",
                //ES
                "30 SEGUNDOS",
                //PT
                "30 SEGUNDOS",
                "",
             },
             new string[] {
                //EN
                "1 MINUTE",
                //PL
                "1 MINUTA",
                //ES
                "1 MINUTO",
                //PT
                "1 MINUTO",
                "",
                "",
                },
             new string[] {
                //EN
                "2 MINUTES",
                //PL
                "2 MINUTY",
                //ES
                "2 MINUTOS",
                //PT
                "2 MINUTOS",
                "",
                },
             new string[] {
                //EN
                "3 MINUTES",
                //PL
                "3 MINUTY",
                //ES
                "3 MINUTOS",
                //PT
                "3 MINUTOS",
                "",
                },
             new string[] {
                //EN
                "4 MINUTES",
                //PL
                "4 MINUTY",
                //ES
                "4 MINUTOS",
                //PT
                "4 MINUTOS",
                "",
             },
             new string[] {
                //EN
                "5 MINUTES",
                //PL
                "5 MINUT",
                //ES
                "5 MINUTOS",
                //PT
                "5 MINUTOS",
                "",
             },
             new string[] {
                //EN
                "NO",
                //PL
                "NIE",
                //ES
                "NO",
                //PT
                "NAO",
                "",
             },
             new string[] {
                //EN
                "YES",
                //PL
                "TAK",
                //ES
                "SI",
                //PT
                "SIM",
                "",
                "",
                },
             new string[] {
                //EN
                "VERY LOW",
                //PL
                "BARDZO NISKA",
                //ES
                "MUY BAJA",
                //PT
                "MUITO BAIXA",
                "",
                },
             new string[] {
                //EN
                "LOW", 
                //PL
                "NISKA",
                //ES
                "BAJA",
                //PT
                "BAIXA",
                "",
                },

             new string[] {
                //EN
                "STANDARD",
                //PL
                "STANDARD",
                //ES
                "ESTANDAR",
                //PT
                "PADRAO",
                "",
             },
             new string[] {
                //EN
                "HIGH",
                //PL
                "WYSOKA",
                //ES
                "ALTA",
                //PT
                "ALTA",
                "",
             },
             new string[] {
                //EN
                "VERY HIGH",
                //PL
                "BARDZO WYSOKA",
                //ES
                "MUY ALTA",
                //PT
                "MUITO ALTA",
                "",
                "",
                },
             new string[] {
                //EN
                "LEFT",
                //PL
                "PO LEWEJ",
                //ES
                "IZQUIERDA",
                //PT
                "ESQUERDA",
                "",
                },
             new string[] {
                //EN
                "RIGHT",
                //PL
                "PO PRAWEJ",
                //ES
                "DERECHA",
                //PT
                "DIREITA",
                "",
                },
             new string[] {
                //EN
                "Excellent! Ads disabled!",
                //PL
                "Super! Reklamy wyłączone",
                //ES
                "¡Súper! Anuncios eliminados",
                //PT
                "Muito bem! Anuncios desligados",
                "",
             },
             new string[] {
                //EN
                "Excellent! +VAR_0 coins awarded!",
                //PL
                "Brawo! Przyznano +VAR_0 monet",
                //ES
                "¡Bravo! Se han concedido +VAR_0 monedas",
                //PT
                "Parabéns! +VAR_0 moedas recebidas",
                "",
             },
             new string[] {
                //EN
                "Excellent! +VAR_0 diamonds awarded!",
                //PL
                "Brawo! Przyznano +VAR_0 diamentów",
                //ES
                "¡Bravo! Se han concedido +VAR_0 diamantes",
                //PT
                "Parabéns! +VAR_0 diamantes recebidos",
                "",
             },
             new string[] {
                //EN
                "Excellent! VAR_0 unlocked!",
                //PL
                "Brawo! VAR_0 odblokowany!",
                //ES
                "¡Bravo! ¡VAR_0 desbloqueado!",
                //PT
                "Parabéns! VAR_0 desbloqueado!",
                "",
                "",
                },
             new string[] {
                //EN
                "Well done! +VAR_0 to attack skills and +VAR_1 to defense skills awarded!",
                //PL
                "Brawo! +VAR_0 do ataku i +VAR_1 do obrony!",
                //ES
                "¡Bravo! ¡+VAR_0 para ataque y +VAR_1 para defensa!",
                //PT
                "Parabéns! +VAR_0 de ataque e +VAR_1 de defesa!",
                "",
                },
             new string[] {
                //EN
                "Excellent! +VAR_0 to defense skills awarded!",
                //PL
                "Brawo! +VAR_0 do obrony!",
                //ES
                "¡Bravo! ¡+VAR_0 para defensa!",
                //PT
                "Parabéns! +VAR_0 de defesa!",
                "",
                },
             new string[] {
                //EN
                "Excellent! +VAR_0 to attack skills awarded!",
                //PL
                "Brawo! +VAR_0 do ataku!",
                //ES
                "¡Bravo! ¡+VAR_0 para ataque!",
                //PT
                "Parabéns! +VAR_0 de ataque",
                "",
                "",
                },
             new string[] {
                //EN
                "Excellent! player's energy refill!",
                //PL
                "Doskonale! Energia uzupełniona!",
                //ES
                "¡Perfecto! ¡Energía completada!",
                //PT
                "Perfeito! Energia completada!",
                "",
                },
             new string[] {
                //EN
                "Ads watching failed",
                //PL
                "Błąd reklamy",
                //ES
                "Error del anuncio",
                //PT
                "Anuncios desligados!",
                "",
                },
             new string[] {
                //EN
                "Ads disabled!",
                //PL
                "Reklamy wyłączone!",
                //ES
                "¡Anuncios eliminados!",
                //PT
                "Muito bem! Anuncios desligados",
                "",
             },
             new string[] {
                //EN
                "VAR_0 coins added!",
                //PL
                "Dodano VAR_0 monet",
                //ES
                "Se han añadido VAR_0 monedas",
                //PT
                "VAR_0 moedas adicionadas",
                "",
             },
             new string[] {
                //EN
                "Wrong code!",
                //PL
                "Zły kod!",
                //ES
                "¡Código incorrecto!",
                //PT
                "Código errado!",
                "",
             },
             new string[] {
                //EN
                "VAR_0 diamonds added!",
                //PL
                "Dodano VAR_0 diamentów",
                //ES
                "Se han añadido VAR_0 diamantes",
                //PT
                "VAR_0 diamantes adicionados",
                "",
                "",
                },
             new string[] {
                //EN
                "Operation failed!",
                //PL
                "Operacja nie udała się!",
                //ES
                "¡La operación ha fallado!",
                //PT
                "A operação falhou!",
                "",
                },
             new string[] {
                //EN
                "Skills improved!",
                //PL
                "Umiejętności ulepszone!",
                //ES
                "¡Habilidades mejoradas!",
                //PT
                "Habilidade melhorada!",
                "",
                },
             new string[] {
                //EN
                "Goal enlarged!",
                //PL
                "Bramka powiększona!",
                //ES
                "¡Portería agrandada!",
                //PT
                "Baliza aumentada!",
                "",
             },
             new string[] {
                //EN
                "Get coins!",
                //PL
                "Zbierz monety!",
                //ES
                "¡Recoge monedas!",
                //PT
                "Colete as moedas!",
                "",
             },                 
             new string[] {
                //EN
                "Unlock VAR_0",
                //PL
                "Odblokuj VAR_0",
                //ES
                "Desbloquea VAR_0",
                //PT
                "Desbloqueie VAR_0",
                "",
                },
             new string[] {
                //EN
                "UNLOCK",
                //PL
                "ODBLOKUJ",
                //ES
                "DESBLOQUEA",
                //PT
                "DESBLOQUEA",
                "",
                },
             new string[] {
                //EN
                "Coins",
                //PL
                "Monety",
                //ES
                "Monedas",
                //PT
                "Moedas",
                "",
                },
             new string[] {
                //EN
                "Improve your team skills!",
                //PL
                "Ulepsz umiejetnosci zespolu!",
                //ES
                "¡Mejora las habilidades del equipo!",
                //PT
                "Melhore a habilidade do time!",
                "",
             },
             new string[] {
                //EN
                "Promotional codes",
                //PL
                "Kody promocyjne",
                //ES
                "Codigos promocionales",
                //PT
                "Codigos promocionais",
                "",
             },
             new string[] {
                //EN
                "Buy this player's card for VAR_0 diamonds",
                //PL
                "Kup tą kartę za VAR_0 diamentów",
                //ES
                "Compra esta carta por VAR_0 diamantes",
                //PT
                "Compre esta carta por VAR_0 diamantes",
                "",
                "",
                },
             new string[] {
                //EN
                "Refill energy",
                //PL
                "Uzupełnij energię",
                //ES
                "Completa la energía",
                //PT
                "Complete a energia",
                "",
                },
             new string[] {
                //EN
                "VAR_0 was not qualified to Champ Cup in the last season. " +
                "\n Only first VAR_1 teams in the league get qualified",
                //PL
                "VAR_0 nie zakfalfikowal się do Champ Cup w ostatnim sezonie " +
                "\n Tylko pierwsze VAR_1 zespoly kwalfikują się",
                //ES
                "VAR_0 no se ha clasificado para la Champ Cup en la ultima temporada " +
                "\n Solo los primeros VAR_1 equipos se clasifican",
                //PT
                "VAR_0 nao se qualificou para a Champ Cup na ultima temporada " +
                "\n Apenas os primeiros VAR_1 times se qualificam",
                "",
                },
             new string[] {
                //EN
                "VAR_0 Summary",
                //PL
                "VAR_0 Podsumowanie",
                //ES
                "VAR_0 Resumen",
                //PT
                "VAR_0 Resumo",
                "",
             },
             new string[] {
                //EN
                "VAR_0 was knocked out of the VAR_0 in the VAR_1",                 
                //PL
                "VAR_0 wyeliminowana z VAR_0 w VAR_1",
                //ES
                "VAR_0 eliminada de VAR_0 en VAR_1",
                //PT
                "VAR_0 eliminada do VAR_0 em VAR_1",
                "",
             }, 
             new string[] {
                //EN
                "New Season",
                //PL
                "Nowy sezon",
                //ES
                "Nueva temporada",
                //PT
                "Nova temporada",
                "",
                "",
                },
             new string[] {
                //EN
                "Season summary",
                //PL
                "Podsumowanie sezonu",
                //ES
                "Resumen de la temporada",
                //PT
                "Resumo da temporada",
                "",
                },
             new string[] {
                //EN
                "Week VAR_0 - ",
                //PL
                "Tydzien VAR_0 - ",
                //ES
                "Semana VAR_0 - ",
                //PT
                "Semana VAR_0 - ",
                "",
                },
             new string[] {
                //EN
                "League: VAR_0 has been ",
                //PL
                "Liga: VAR_0 został",
                //ES
                "Liga: ¡VAR_0 se ha",
                //PT
                "Liga: VAR_0 foi",
                "",
                },
             new string[] {
                //EN
                "Main Menu",
                //PL
                "Menu Główne",
                //ES
                "Menú principal",
                //PT
                "Menu Principal",
                "",
             },                 
              new string[] {
                //EN
                "League: VAR_0 has not been ",
                //PL
                "Liga: VAR_0 nie został ",
                //ES
                "Liga: ¡VAR_0 no se ha",
                //PT
                "Liga: VAR_0 não foi",
                "",
             },
             new string[] {
                //EN
                "qualified to champ cup! ",
                //PL
                "zakfalfikowaany do champ cup! ",
                //ES
                "clasificado para la champ cup!",
                //PT
                "classificado para a Champ Cup! ",
                "",
             },
             new string[] {
                //EN
                "VAR_0 finished at VAR_1 position",
                //PL
                "VAR_0 zakończył na VAR_1 pozycji",
                //ES
                "VAR_0 ha acabado en la VAR_1 posición",
                //PT
                "VAR_0 terminou na VAR_1 posição",
                "",
                "",
                },
             new string[] {
                //EN
                "won a champ cup!",
                //PL
                "wygrał champ cup!",
                //ES
                "¡ha ganado la champ cup!",
                //PT
                "ganhou a Champ Cup!",
                "",
                },
             new string[] {
                //EN
                "was knockout in VAR_0",
                //PL
                "został weliminowany w VAR_0",
                //ES
                "ha sido eliminado en VAR_0",
                //PT
                "foi eliminado em VAR_0",
                "",
                },
             new string[] {
                //EN
                " won a League Cup",
                //PL
                " wygrał Puchar Ligi",
                //ES
                " ha ganado la Copa de la Liga",
                //PT
                " ganhou a Copa da Liga ",
                "",
                },
              new string[] {
                //EN
                "CONGRATULATIONS!",
                //PL
                "GRATULACJE!",
                //ES
                "¡ENHORABUENA!",
                //PT
                "PARABÉNS!",
                "",
                },
             new string[] {
                //EN
                "YOU HAVE WON THE VAR_0",
                //PL
                "WYGRAŁEŚ VAR_0",
                //ES
                "HAS GANADO VAR_0",
                //PT
                "VOCÊ GANHOU VAR_0",
                "",
                },
             new string[] {
                //EN
                " COINS AWARDED!",
                //PL
                " MONET PRZYZNANO!",
                //ES
                " ¡MONEDAS CONCEDIDAS!",
                //PT
                " MOEDAS RECEBIDAS!",
                "",
             },
             new string[] {
                //EN
                " DIAMONDS AWARDED!",
                //PL
                " DIAMENTOW PRZYZNANO!",
                //ES
                " ¡DIAMANTES CONCEDIDOS!",
                //PT
                " DIAMANTES RECEBIDOS!",
                "",
             },
             new string[] {
                //EN
                "IT IS TIME TO CHALLENGE A NEXT LEVEL NOW",
                //PL
                "NADSZEDŁ CZAS NA WYZWANIA W KOLEJNYM POZIOMIE",
                //ES
                "HA LLEGADO LA HORA DE RETOS EN EL SIGUIENTE NIVEL",
                //PT
                "HORA DOS DESAFIOS EM UM NÍVEL MAIS ALTO",
                "",
                "",
                },
             new string[] {
                //EN
                "ARE YOU READY?",
                //PL
                "JESTEŚ GOTOWY?",
                //ES
                "¿ESTÁS LISTO?",
                //PT
                "ESTÁ PRONTO?",
                "",
                },
             new string[] {
                //EN
                "Loading ",
                //PL
                "Loading ",
                //ES
                "Loading ",
                //PT
                "Loading ",
                "",
                },
             new string[] {
                //EN
                "Preparing",
                //PL
                "Przygotowanie",
                //ES
                "Preparación",
                //PT
                "Preparação",
                "",
             },
             new string[] {
                //EN
                "Confirm deleting save: ",                 
                //PL
                "Potwierdz usunięcie zapisu: ",
                //ES
                "Confirma la eliminación de la partida guardada: ",
                //PT
                "Confirme o apagamento do arquivo com o jogo salvo: ",
                "",
             },
             new string[] {
                //EN
                " Week ",
                //PL
                " Tydzień ",
                //ES
                " Semana ",
                //PT
                " Semana ",
                "",
             },
             new string[] {
                //EN
                " - Season ",
                //PL
                " - Sezon ",
                //ES
                " - Temporada ",
                //PT
                " - Temporada ",
                "",
                "",
                },
             new string[] {
                //EN
                "COINS",
                //PL
                "MONETY",
                //ES
                "MONEDAS",
                //PT
                "MOEDAS",
                "",
                },
             new string[] {
                //EN
                "DIAMONDS",
                //PL
                "DIAMENTY",
                //ES
                "DIAMANTES",
                //PT
                "DIAMANTES",
                "",
                },
             new string[] {
                //EN
                "The opponent's goal will be enlarge for the next match. Good luck!",
                //PL
                "Bramka przeciwnika będzie powiększona w następnym meczu. Powodzenia!",
                //ES
                "La portería del contrario será agrandada en el próximo partido. ¡Suerte!",
                //PT
                "A baliza do adversário será aumentada na próxima partida. Boa sorte!",
                "",
                },
             new string[] {
                //EN
                "Excellent! +VAR_0 diamonds and 2000 coins awarded!",
                //PL
                "Brawo! +VAR_0 diamentów i 2000 monet dodane",
                //ES
                "¡Bravo! +VAR_0 diamantes y 2000 monedas añadidos",
                //PT
                "Parabéns! +VAR_0 diamantes e 2000 moedas adicionadas",
                "",
             },
             new string[] {
                //EN
                "Defense: ",                 
                //PL
                "Obrona: ",
                //ES
                "Defensa: ",
                //PT
                "Defesa: ",
                "",
             },
             new string[] {
                //EN
                "Attack: ",
                //PL
                "Atak: ",
                //ES
                "Ataque: ",
                //PT
                "Ataque: ",
                "",
             },
             new string[] {
                //EN
                "Welcome to the training mode. I am going to show you how to play and win!",
                //PL
                "Witaj w trybie treningu! Pokażemy Ci jak grać i wygrywać!",
                //ES
                "¡Bienvenido al modo de entrenamiento! ¡Te enseñaremos cómo jugar y ganar!",
                //PT
                "Bem-vindo ao modo de treino! Vamos mostrar como jogar e ganhar!",
                "",
                "",
                },
             new string[] {
                //EN
                "You move, score and defend from your own half. You cannot cross the halfway line",
                //PL
                "Możesz poruszać się, strzelać i bronić tylko na swojej połowie boiska.",
                //ES
                "Puedes moverte, disparar y defender solo en tu mitad de campo.",
                //PT
                "Você só pode se mover, chutar e defender no seu próprio campo.",
                "",
                },
             new string[] {
                //EN
                "Run to the ball now. Use the joystick in the bottom corner",
                //PL
                "Podbiegnij do piłku. Użyj joysticka znajdującego się w dolnym rogu",
                //ES
                "Corre hasta el balón. Utiliza el joystick situado en la esquina inferior",
                //PT
                "Corra até a bola. Use o controle encontrado no canto inferior",
                "",
                },

             new string[] {
                //EN
                "Brilliant! When you have the ball, you can shoot. Draw a straight line as shown",
                //PL
                "Wspaniale! Kiedy jesteś przy piłce, możesz strzelić. Narysuj linię jak pokazano",
                //ES
                "¡Genial! Cuando estés junto al balón puedes disparar. Dibuja una línea como se muestra",
                //PT
                "Muito bem! Você pode chutar perto da bola. Desenhe uma linha como mostrado",
                "",
                },
              new string[] {
                //EN
                "Great! Good shot!",
                //PL
                "Wspaniale! Super strzał!",
                //ES
                "¡Genial! ¡Un disparo sensacional!",
                //PT
                "Muito bem! Grande chute!",
                "",
             },
             new string[] {
                //EN
                "Now try to curl a shot by drawing a curved line",
                //PL
                "Teraz spróbuj podkręcić piłkę, przez narysowanie krzywej linii",
                //ES
                "Ahora intenta darle efecto al balón dibujando una línea curva",
                //PT
                "Agora tente fazer a bola curvar, desenhando uma linha curva",
                "",
             },
             new string[] {
                //EN
                "Perfect shot!. The more you curve the line, the more the shot will curve",
                //PL
                "Brawo! Im bardziej zakręcisz linię tylko strzał będzie bardziej podkręcony",
                //ES
                "¡Bravo! Cuanto más curves la línea, más efecto tendrá el disparo",
                //PT
                "Parabéns! Quanto mais você curvar a linha, mais curvado será o chute",
                "",
                "",
                },
             new string[] {
                //EN
                "The faster you draw, the higher the ball speed",
                //PL
                "Im szybciej rysujesz tym większa prędkość piłki",
                //ES
                "Cuanto más rápido la dibujes, mayor velocidad del balón",
                //PT
                "Quanto mais rápido você desenhar, mais rápida será a velocidade da bola",
                "",
                },
             new string[] {
                //EN
                "There are 2 buttons in the bottom corner - V (volley) and L (lob) shot",
                //PL
                "W dolnym rogu są dwa przyciski - V (volley) i L (lob) strzał",
                //ES
                "En la esquina inferior hay dos botones: V (volley) y L (lob) disparo",
                //PT
                "No canto inferior há dois botões - V (voleio) i L (cobertura) chute",
                "",
                },
             new string[] {
                //EN
                "Click the (L) lob button and draw a line",
                //PL
                "Naciśnij przycisk L (lob) i narysuj linię strzału na ekranie",
                //ES
                "Presiona el botón L (lob) y dibuja la línea del disparo en la pantalla",
                //PT
                "Pressione o botão L (cobertura) e desenhe uma linha de chute na tela",
                "",
                },
               new string[] {
                //EN
                "Perfect shot!",
                //PL
                "Wspaniały strzał!",
                //ES
                "¡Un disparo sensacional!",
                //PT
                "Ótimo chute!",
                "",
                },
             new string[] {
                //EN
                "Now click the (V) volley button and draw a line",
                //PL
                "Naciśnij teraz przycisk volley (V) i narysuj linię strzału na ekranie",
                //ES
                "Presiona el ahora botón volley (V) y dibuja la línea del disparo en la pantalla",
                //PT
                "Agora pressione o botão de voleio (V) e desenhe uma linha de chute na tela",
                "",
             },
             new string[] {
                //EN
                "Great volley!",                 
                //PL
                "Wspaniały volley!",
                //ES
                "¡Una volea perfecta!",
                //PT
                "Ótimo voleio!",
                "",
             },
             new string[] {
                //EN
                "You can use volley and lob together, take a curve shot or shoot straight at the goal",
                //PL
                "Możesz użyć przyciski V i L razem, Spróbuj strzelić na prosto lub podkręc piłkę",
                //ES
                "Puedes usar los botones V y L juntos. Prueba a disparar recto o a darle efecto al balón",
                //PT
                "Você pode usar os botões V e L juntos, tentar chutar em linha reta ou fazer a bola curvar",
                "",
             },
             new string[] {
                //EN
                "Brilliant! ",
                //PL
                "Wspaniale!",
                //ES
                "¡Genial!",
                //PT
                "Muito bem!",
                "",
                "",
                },
             new string[] {
                //EN
                "To shoot along the ground, draw a line on the grass as shown",
                //PL
                "Żeby strzelić po ziemi narysuj linię jak pokazano",
                //ES
                "Para disparar raso dibuja la línea como se muestra",
                //PT
                "Para dar um chute baixo, desenhe uma linha como mostrado",
                "",
                },
             new string[] {
                //EN
                "The higher you draw, the higher the ball will go. Draw a line close to the crossbar",
                //PL
                "Im wyżej narysujesz koniec lini tym piłka poleci wyżej. Narysuj linię bliżej poprzeczki",
                //ES
                "Cuanto más alto esté el final de la línea, más alto irá el balón. Dibújala más cerca del larguero",
                //PT
                "Quanto mais alto você desenhar o final da linha, mais alto a bola irá. " +
                "Desenhe uma linha mais próxima da barra transversal",
                "",
                },

             new string[] {
                //EN
                "Great shot! You are a fantastic striker!",
                //PL
                "Wspaniały strzał. Jesteś fantastycznym napastnikiem!",
                //ES
                "Un disparo sensacional. ¡Eres un delantero fantástico!",
                //PT
                "Ótimo chute. Você é um atacante fantástico!",
                "",
                },
              new string[] {
                //EN
                "You have 8 seconds to take a shot. You can see the timer at the top",
                //PL
                "Masz 8 sekund na oddanie strzału. Spójrz na zegar w górnym rogu",
                //ES
                "Tienes 8 segundos para disparar. Mira el reloj en la esquina superior",
                //PT
                "Você tem 8 segundos para chutar. Olhe para o relógio no canto superior",
                "",
             },
             new string[] {
                //EN
                "You have to take a shot within 8 seconds. Otherwise your opponent gets the ball",
                //PL
                "Musisz oddać strzał w ciągu 8 sekund. W przeciwnym razie przeciwnik otrzyma piłkę",
                //ES
                "Tienes que disparar en 8 segundos. En caso contrario el adversario recibe el balón",
                //PT
                "Você deve chutar dentro de 8 segundos. Caso contrário, o adversário receberá a bola",
                "",
             },
             new string[] {
                //EN
                "Excellent! You know how to shoot. I am now going to teach you how to defend",
                //PL
                "Brawo! Już wiesz jak strzelać. Teraz nauczę Cię jak bronić",
                //ES
                "¡Bravo! Ya sabes cómo disparar. Ahora te voy a enseñar a defender.",
                //PT
                "Parabéns! Agora você já sabe chutar. Agora vou te ensinar a defender",
                "",
                "",
                },
             new string[] {
                //EN
                "When the ball is heading towards your goal, click the circle to try to save it",
                //PL
                "Kiedy piłka leci w kierunku Twojej bramki, kliknij kółko aby obronić",
                //ES
                "Cuando el balón vuela hacia tu portería, haz clic en el círculo para defender",
                //PT
                "Quando a bola vai na direção do seu gol, aperte a roda para defender.",
                "",
                },
             new string[] {
                //EN
                "You can adjust your goalkeeper's position using the buttons next to the joystick",
                //PL
                "Możesz dostosować ustawienie bramkarza używając 4 przycisków obok joysticka",
                //ES
                "Puedes ajustar la posición del portero utilizando los cuatro botones junto al joystick",
                //PT
                "Você pode ajustar a posição do goleiro usando os 4 botões ao lado do controle",
                "",
                },
             new string[] {
                //EN
                "Now try a two minute training match",
                //PL
                "Teraz rozegrasz 2 minutowy mecz treningowy",
                //ES
                "Ahora vas a jugar un partido de entrenamiento de 2 minutos",
                //PT
                "Agora você vai jogar um jogo-treino de 2 minutos",
                "",
                },
             new string[] {
                //EN
                "Well done!. That is the end of the training. Good luck in the tournaments!",
                //PL
                "Brawo! Dotarłeś do końca treningu. Powodzenia w turniejach i lidze",
                //ES
                "¡Bravo! Has llegado al final del entrenamiento. Suerte en los torneos y en la liga",
                //PT
                "Parabéns! Você chegou no fim do treino. Boa sorte nos torneios e liga",
                "",
                },
             new string[] {
                //EN
                "Hey footballer! Go to the ball. Use joystick in the bottom corner",
                //PL
                "Hej piłkarzu!. Podejrz do piłki. Użyj joysticka znajdującego się w dolnym rogu",
                //ES
                "¡Hey, futbolista! Mira el balón. Utiliza el joystick situado en la esquina inferior",
                //PT
                "Oi, jogador!. Olhe a bola. Use o controle encontrado no canto inferior",
                "",
                },
             new string[] {
                //EN
                "Try once again. Draw a straight line as shown",
                //PL
                "Spróbuj ponownie. Narysuj prostą linię tak jak pokazano",
                //ES
                "Inténtalo de nuevo. Dibuja una línea recta como se muestra",
                //PT
                "Tente outra vez. Desenhe uma linha reta como mostrado",
                "",
             },
             new string[] {
                //EN
                "Try once again. Try to curl more",                 
                //PL
                "Spróbuj ponownie. Podkręć mocniej strzał",
                //ES
                "Inténtalo de nuevo. Dale más efecto al disparo",
                //PT
                "Tente outra vez. Chute com mais força",
                "",
             },
             new string[] {
                //EN
                "Click V button in the bottom corner",
                //PL
                "Kliknij przycisk V w dolnym rogu",
                //ES
                "Haz clic en el botón V en la esquina inferior",
                //PT
                "Clique no botão V no canto inferior",
                "",
             },
             new string[] {
                //EN
                "Click L button in the bottom corner before shot",
                //PL
                "Przed strzałem kliknij przycisk L w dolnym rogu",
                //ES
                "Antes de disparar haz clic en el botón L en la esquina inferior",
                //PT
                "Clique no botão L no canto inferior",
                "",
                "",
                },
             new string[] {
                //EN
                "Click V and L buttons in the bottom corner before shot",
                //PL
                "Przed strzałem kliknij przyciski V i L w dolnym rogu",
                //ES
                "Antes de disparar haz clic en los botones V y L en la esquina inferior",
                //PT
                "Antes de chutar, clique nos botões V e L no canto inferior",
                "",
                },
             new string[] {
                //EN
                "Draw a line on the grass exactly as shown",
                //PL
                "Narysuj linię na trawie dokładnie jak pokazano",
                //ES
                "Dibuja una línea sobre la hierba exactamente como se muestra",
                //PT
                "Desenhe uma linha na grama exatamente como mostrado",
                "",
                },

             new string[] {
                //EN
                "Draw a line closer to the crossbar",
                //PL
                "Narysuj koniec lini bliżej poprzeczki",
                //ES
                "Dibuja el final de la línea más cerca del larguero",
                //PT
                "Desenhe o final da linha mais perto da barra transversal",
                "",
                },
             new string[] {
                //EN
                "League",
                //PL
                "Liga",
                //ES
                "Liga",
                //PT
                "Liga",
                "",
                },
             new string[] {
                //EN
                "Champ Cup",
                //PL
                "Champ Cup",
                //ES
                "Champ Cup",
                //PT
                "Champ Cup",
                "",
                },
             new string[] {
                //EN
                "League Cup",
                //PL
                "Puchar Ligi",
                //ES
                "Copa de la Liga",
                //PT
                "Copa da Liga",
                "",
                },
             new string[] {
                //EN
                "Player name",
                //PL
                "Nazwa zawodnika",
                //ES
                "Nombre del jugador",
                //PT
                "Nome do jogador",
                "",
                },
             new string[] {
                //EN
                "Attack +VAR_0",
                //PL
                "Obrona +VAR_0",
                //ES
                "Ataque: +VAR_0",
                //PT
                "Ataque: +VAR_0",
                "",
                },
             new string[] {
                //EN
                "Defense +VAR_0",
                //PL
                "Obrona +VAR_0",
                //ES
                "Defensa: +VAR_0",
                //PT
                "Defesa: +VAR_0",
                "",
                },
             new string[] {
                //EN
                "Defense & Attack +VAR_0",
                //PL
                "Obrona & Atak +VAR_0",
                //ES
                "Defensa & Ataque +VAR_0",
                //PT
                "Defense & Ataque +VAR_0",
                "",
                },
             new string[] {
                //EN
                "Following saves will be deleted",
                //PL
                "Nastepujace zapisy zostana usuniete",
                //ES
                "Following saves will be deleted",
                //PT
                "Os seguintes jogos salvos serao deletados",
                "",
                },

             //not translated by interpreter!!
             new string[] {
                //EN
                "Sorry. You must have at least 50 coins to play online. Play Friendly, Season or Tournament first",
                //PL
                "Musisz posiadac co najmniej 50 monet żeby grac w trybie online. Zagraj Szybki mecz, Turniej lub Sezon najpierw",
                //ES
                "Debes tener al menos 50 monedas para jugar en línea. Jugar Amistoso, Temporada, o Torneo primero",
                //PT
                "Você deve ter pelo menos 50 moedas para jogar online. Jogar amistoso, Temporada ou Torneio primeiro",
                "",
                },
            //not translated by interpreter!!
            new string[] {
                //EN
                "Sorry. We can't find any opponent now",
                //PL
                "Przepraszamy. Nie mozemy znalezc zadnego przeciwnika w tym momencie",
                //ES
                "Pedimos disculpas. No podemos encontrar ningún oponente en este momento",
                //PT
                "Nós pedimos desculpas. Não podemos encontrar nenhum oponente neste momento",
                "",
                },
             new string[] {
                //EN
                "Your opponent left the game",
                //PL
                "Twoj przeciwnik wyszedl z gry",
                //ES
                "Tu oponente abandonó el juego.",
                //PT
                "Seu oponente saiu do jogo",
                "",
                },
               new string[] {
                //EN
                "Checking for updates",
                //PL
                "Sprawdzanie aktualizacji",
                //ES
                "Comprobando actualizaciones",
                //PT
                "Verificando atualizações",
                "",
                },
                new string[] {
                //EN
                "Congratulations! New National Team Unlocked",
                //PL
                "Gratulacje! Nowy zespol odblokowany",
                //ES
                "¡Enhorabuena! Nuevo equipo nacional desbloqueado",
                //PT
                "Parabens! Nova Selecao Nacional desbloqueada",
                "",
                },
                new string[] {
                //EN
                "Congratulations! New prize unlocked",
                //PL
                "Gratulacje! Nowa nagroda odblokowana",
                //ES
                "¡Enhorabuena! Nuevo premio desbloqueado",
                //PT
                "Parabens! Nova premio desbloqueada",
                "",
                },
                new string[] {
                //EN
                "Welcome to our football game! We hope you will enjoy it. You can create your own player card and team in this menu",
                //PL
                "Witamy w naszej grze pilkarskiej! Mamy nadzieje, ze Ci sie spodoba. W tym menu mozesz stworzyc wlasna karte gracza i druzyne",
                //ES
                "Bienvenido a nuestro juego de futbol! Esperamos que lo disfrutes. Puedes crear tu propia tarjeta de jugador y equipo en este menu",
                //PT
                "Bem-vindo ao nosso jogo de futebol! Esperamos que voce aproveite. Voce pode criar seu proprio cartao de jogador e equipe neste menu",
                "",
                },
                new string[] {
                //EN
                "CUSTOMIZE",
                //PL
                "DOSTOSUJ",
                //ES
                "PERSONALIZAR",
                //PT
                "CUSTOMIZAR",
                "",
                },

                //power 0 desc
                new string[] {
                //EN
                "Power type: Offensive\n\n" +
                "This power adds two additional small goals next to standard one.\n" +
                "You can score to any of 3 goals then\n\n" +
                "When used by opponent you can use:\n" +
                "Bad Weather, Cut Goal Back, Earthquake, Goal Wall, Invisibility, Flares\n\n",
                //PL
                "Typ mocy: Ofensywny\n\n" +
                "Ta moc dodaje dwie dodatkowe małe bramki obok standardowego.\n" +
                "Możesz wtedy strzelić do dowolnej z 3 bramek\n\n" +
                "Kiedy moc jest używana przez przeciwnika, możesz użyć:\n" +
                "Bad Weather, Cut Goal Back, Earthquake, Goal Wall, Invisibility, Flares\n\n",
                //ES
                "Tipo de poder: Ofensivo\n\n" +
                "Este poder agrega dos puertas extra pequeñas además de la estándar.\n" +
                "Entonces puedes disparar a cualquiera de los 3 objetivos\n\n" +
                "Cuando el poder es usado por un oponente, puedes usar:\n" +
                "Bad Weather, Cut Goal Back, Earthquake, Goal Wall, Invisibility, Flares\n\n",
                //PT
                "Tipo de poder: Ofensivo\n\n" +
                "Este poder adiciona dois pequenos portões extras além do padrão.\n" +
                "Você pode então chutar para qualquer um dos 3 gols\n\n" +
                "Quando o poder é usado por um oponente, você pode usar:\n" +
                "Bad Weather, Cut Goal Back, Earthquake, Goal Wall, Invisibility, Flares\n\n",
                "",
                },
                //power 1 desc
                new string[] {
                //EN
                "Type: Defensive\n\n" +
                "This power decreases height of your goal.\n" +
                "It can be used when opponent strikes to minimize chance of scoring goal\n\n" +
                "When used by opponent you can use:\n" +
                "Enlarge Goal, Invisibility, Bad Weather, Earthquake\n\n",
                //PL
                "Typ mocy: Defensywna\n\n" +
                "Ta moc zmniejsza wysokość twojej bramki.\n" +
                "Można go użyć, gdy przeciwnik strzela, aby zminimalizować szansę na zdobycie gola\n\n" +
                "Kiedy jest używany przez przeciwnika, możesz użyć:\n" +
                "Enlarge Goal, Invisibility, Bad Weather, Earthquake\n\n",
                //ES
                "Tipo de poder: Defensivo\n\n" +
                "Este poder reduce la altura de tu objetivo.\n" +
                "Se puede usar cuando un oponente está disparando para minimizar la posibilidad de anotar\n\n" +
                "Cuando lo usa un oponente, puedes usar:\n" +
                "Enlarge Goal, Invisibility, Bad Weather, Earthquake\n\n",
                //PT
                "Tipo de poder: Defensivo\n\n" +
                "Este poder reduz a altura do seu objetivo.\n" +
                "Pode ser usado quando um adversário está arremessando para minimizar a chance de marcar\n\n" +
                "Quando usado por um oponente, você pode usar:\n" +
                "Enlarge Goal, Invisibility, Bad Weather, Earthquake\n\n",
                "",
                },
                //power 2 desc
                new string[] {
                //EN
                "Power type: Offensive\n\n" +
                "This power enlarge opponent goal.\n" +
                "You can used it when shooting\n\n" +
                "When used by opponent you can use:\n" +
                "Cut Goal Back, Goal Wall, Bad Weather, Flares, Earthquake, Invisibility\n\n",
                //PL
                "Typ mocy: Ofensywna\n\n" +
                "Ta moc powiększa bramkę przeciwnika.\n" +
                "Możesz go używać podczas strzelania\n\n" +
                "Kiedy moc jest używana przez przeciwnika, możesz użyć:\n" +
                "Cut Goal Back, Goal Wall, Bad Weather, Flares, Earthquake, Invisibility\n\n",
                //ES
                "Tipo de poder: Ofensivo\n\n" +
                "Este poder agranda la portería del oponente.\n" +
                "Puedes usarlo mientras disparas\n\n" +
                "Cuando el poder es usado por un oponente, puedes usar:\n" +
                "Cut Goal Back, Goal Wall, Bad Weather, Flares, Earthquake, Invisibility\n\n",
                //PT
                "Tipo de poder: Ofensivo\n\n" +
                "Este poder aumenta o objetivo do adversário.\n" +
                "Você pode usá-lo enquanto fotografa\n\n" +
                "Quando o poder é usado por um oponente, você pode usar:\n" +
                "Cut Goal Back, Goal Wall, Bad Weather, Flares, Earthquake, Invisibility\n\n",
                "",
                },
                //power 3 desc
                new string[] {
                //EN
                "Power type: Offensive\n\n" +
                "If you score a goal this power will give you two goals instead of one\n\n" +
                "When used by opponent you can use:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Earthquake\n\n",
                //PL
                "Typ mocy: Ofensywna\n\n" +
                "Jeśli zdobędziesz gola, ta moc da ci dwa gole zamiast jednego\n\n" +
                "Kiedy moc jest używana przez przeciwnika, możesz użyć:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Earthquake\n\n",
                //ES
                "Tipo de poder: Ofensivo\n\n" +
                "Si anotas, este poder te dará dos goles en lugar de uno\n\n" +
                "Cuando el poder es usado por un oponente, puedes usar:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Earthquake\n\n",
                //PT
                "Tipo de poder: Ofensivo\n\n" +
                "Se você marcar, esse poder lhe dará dois gols em vez de um\n\n" +
                "Quando o poder é usado por um oponente, você pode usar:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Earthquake\n\n",
                "",
                },
                //power 4 desc
                new string[] {
                //EN
                "Power type: Offensive, Defensive\n\n" +
                "This power makes an earthquake.\n" +
                "It's more difficult to attack and defend in such conditions\n\n" +
                "When used by opponent you can use:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Earthquake, Flares, Bad Weather\n\n",
                //PL
                "Rodzaj mocy: Ofensywna, Defensywna\n\n" +
                "Ta moc wywołuje trzęsienie ziemi u przeciwnika.\n" +
                "W takich warunkach trudniej jest mu atakować i bronić\n\n" +
                "Kiedy moc jest użyta przez przeciwnika, możesz użyć:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Earthquake, Flares, Bad Weather\n\n",
                //ES
                "Tipo de poder: Ofensivo, Defensivo\n\n" +
                "Este poder provoca un terremoto en el enemigo.\n" +
                "Es más difícil para él atacar y defender en estas condiciones\n\n" +
                "Cuando un oponente usa un poder, puedes usar:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Earthquake, Flares, Bad Weather\n\n",
                //PT
                "Tipo de poder: Ofensivo, Defensivo\n\n" +
                "Este poder causa um terremoto no inimigo.\n" +
                "É mais difícil para ele atacar e defender nessas condições\n\n" +
                "Quando um poder é usado por um oponente, você pode usar:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Earthquake, Flares, Bad Weather\n\n",
                "",
                },
                //power 5 desc
                new string[] {
                //EN
                "Power type: Defensive\n\n" +
                "The power creates a wall that covers standard size goal.\n" +
                "Opponent cannot scores a goal unless Enlarge goal will be used\n\n" +
                "When used by opponent you can use\n" +
                "Enlarge Goal, Extra Goals\n\n",
                //PL
                "Rodzaj mocy: Defensywna\n\n" +
                "Moc tworzy ścianę, która zakrywa bramkę standardowego rozmiaru.\n" +
                "Przeciwnik nie może strzelić gola, chyba że zostanie użyta moc Enlarge goal\n\n" +
                "Kiedy moc jest użyta przez przeciwnika, możesz użyć\n" +
                "Enlarge Goal, Extra Goals\n\n",
                //ES
                "Tipo de poder: Defensivo\n\n" +
                "La Fuerza crea un muro que cubre una puerta de tamaño estándar.\n" +
                "El oponente no puede anotar a menos que use el poder de gol Ampliar\n\n" +
                "Cuando el poder es usado por un oponente, puedes usar\n" +
                "Enlarge Goal, Extra Goals\n\n",
                //PT
                "Tipo de poder: Defensivo\n\n" +
                "A Força cria uma parede que cobre um portão de tamanho padrão.\n" +
                "O adversário não pode marcar a menos que o poder Ampliar gol seja usado\n\n" +
                "Quando o poder é usado por um oponente, você pode usar\n" +
                "Enlarge Goal, Extra Goals\n\n",
                "",
                },
                //power 6 desc
                new string[] {
                //EN
                "Power type: Offensive, Defensive\n\n" +
                "This makes the ball and your player invisible.\n" +
                "You can mislead opponent and strikes from any position on the pitch\n\n" +
                "When used by opponent you can use:\n" +
                "Flares, Goal Wall, Cut Goal Back, Bad Weather, Invisibility, Earthquake\n\n",
                //PL
                "Rodzaj mocy: ofensywna, defensywna\n\n" +
                "To sprawia, że piłka i twój zawodnik są niewidzialni.\n" +
                "Możesz wprowadzać w błąd przeciwnika i uderzać z dowolnej pozycji na boisku\n\n" +
                "Kiedy moc jest użyta przez przeciwnika, możesz użyć:\n" +
                "Flares, Goal Wall, Cut Goal Back, Bad Weather, Invisibility, Earthquake\n\n",
                //ES
                "Tipo de Poder: Ofensivo, Defensivo\n\n" +
                "Esto hace que la pelota y tu jugador sean invisibles.\n" +
                "Puedes engañar a tu oponente y golpear desde cualquier posición en el campo\n\n" +
                "Cuando un oponente usa un poder, puedes usar:\n" +
                "Flares, Goal Wall, Cut Goal Back, Bad Weather, Invisibility, Earthquake\n\n",
                //PT
                "Tipo de Poder: Ofensivo, Defensivo\n\n" +
                "Isso torna a bola e seu jogador invisíveis.\n" +
                "Você pode enganar seu oponente e atacar de qualquer posição no campo\n\n" +
                "Quando um poder é usado por um oponente, você pode usar:\n" +
                "Flares, Goal Wall, Cut Goal Back, Bad Weather, Invisibility, Earthquake\n\n",
                "",
                },
                //power 7 desc
                new string[] {
                //EN
                "Power type: Offensive, Defensive\n\n" +
                "This power makes extreme weather conditions for your opponent like snow, storm and wind.\n" +
                "It decreases his run speed\n\n" +
                "When used by opponent you can use:\n" +
                "Goal wall, Cut Goal Back, Earthquake, Bad Weather\n\n",
                //PL
                "Rodzaj mocy: Ofensywna, Defensywna\n\n" +
                "Ta moc uruchamia ekstremalne warunki pogodowe, takie jak śnieg, burza i wiatr.\n"+
                "Zmniejsza to prędkość biegu przeciwnika\n\n" +
                "Kiedy moc jest użyta przez przeciwnika, możesz użyć:\n" +
                "Goal wall, Cut Goal Back, Earthquake, Bad Weather\n\n",
                //ES
                "Tipo de poder: Ofensivo, Defensivo\n\n" +
                "Este poder desencadena condiciones climáticas extremas como nieve, tormenta y viento.\n"+
                "Esto reduce la velocidad de carrera del oponente\n\n" +
                "Cuando un oponente usa un poder, puedes usar:\n" +
                "Goal wall, Cut Goal Back, Earthquake, Bad Weather\n\n",
                //PT
                "Tipo de poder: Ofensivo, Defensivo\n\n" +
                "Este poder desencadeia condições climáticas extremas, como neve, tempestade e vento.\n"+
                "Isso reduz a velocidade de corrida do oponente\n\n" +
                "Quando um poder é usado por um oponente, você pode usar:\n" +
                "Goal wall, Cut Goal Back, Earthquake, Bad Weather\n\n",
                "",
                },
                //power 8 desc
                new string[] {
                //EN
                "Power type: Offensive, Defensive\n\n" +
                "The power enables flare on your opponent half that limits visibility for him\n\n" +
                "When used by opponent you can use:\n" +
                "Goal Wall, Cut Goal Back, Flares, Earthquake\n\n",
                //PL
                "Rodzaj mocy: Ofensywna, Defensywna\n\n" +
                "Moc uruchamia racę na połowie przeciwnika, która ogranicza jego widoczność\n\n" +
                "Kiedy moc jest użyta przez przeciwnika, możesz użyć:\n" +
                "Goal wall, Cut Goal Back, Earthquake, Bad Weather\n\n",
                //ES
                "Tipo de poder: Ofensivo, Defensivo\n\n" +
                "La Fuerza lanza una bengala en la mitad del oponente que oscurece su visión\n\n" +
                "Cuando un oponente usa un poder, puedes usar:\n" +
                "Goal wall, Cut Goal Back, Earthquake, Bad Weather\n\n",
                //PT
                "Tipo de poder: Ofensivo, Defensivo\n\n" +
                "A Força lança um sinalizador na metade do oponente que obscurece sua visão\n\n" +
                "Quando um poder é usado por um oponente, você pode usar:\n" +
                "Goal wall, Cut Goal Back, Earthquake, Bad Weather\n\n",
                "",
                },
                //power 9 desc
                new string[] {
                //EN
                "Power type: Offensive\n\n" +
                "This is very powerful power.\n" +
                "If you score a goal this power will give you three goals instead of one.\n" +
                "But you have to score first!\n\n" +
                "When used by opponent you can use:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Flares, Earthquake\n\n",
                //PL
                "Rodzaj mocy: Ofensywna\n\n" + 
                "To bardzo potężna moc.\n" +
                "Jeśli zdobędziesz gola, ta moc da ci trzy gole zamiast jednego.\n" +
                "Minuay tej mocy? Najpierw musisz zdobyć gola!\n\n" +
                "Kiedy moc jest użyta przez przeciwnika, możesz użyć:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Flares, Earthquake\n\n",
                //ES
                "Tipo de poder: Ofensivo\n\n" +
                "Es una fuerza muy poderosa.\n" +
                "Si anotas, este poder te dará tres goles en lugar de uno.\n" +
                "¿Minuay de este poder? ¡Tienes que anotar primero!\n\n" +
                "Cuando un oponente usa un poder, puedes usar:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Flares, Earthquake\n\n",
                //PT
                "Tipo de poder: Ofensivo\n\n" +
                "É uma força muito poderosa.\n" +
                "Se você marcar, esse poder lhe dará três gols em vez de um.\n" +
                "Minuay deste poder? Você tem que marcar primeiro!\n\n" +
                "Quando um poder é usado por um oponente, você pode usar:\n" +
                "Goal Wall, Cut Goal Back, Invisibility, Bad Weather, Flares, Earthquake\n\n",
                "",
                },
                new string[] {
                //EN
                "Unlock power",
                //PL
                "Odblokuj moc",
                //ES
                "Desbloquear el poder",
                //PT
                "Desbloquear poder",
                "",
                },                
                new string[] {
                //EN
                "You don't have enough diamonds to buy this power.\n" +
                    "Go to the shop to buy diamonds",
                //PL
                "Nie masz wystarczającej liczby diamentów żeby kupić tą moc." +
                "Przejdz do sklepu kupić diamenty",
                //ES
                "No tienes suficientes diamantes para comprUar esta poder." +
                "Ve a la tienda a comprar diamantes",
                //PT
                "Você não tem diamantes suficientes para comprar esta poder." +
                "Vá para a loja para comprar diamantes",
                "",
             },
                 new string[] {
                //EN
                "Buy power for VAR_0 diamonds",
                //PL
                "Kup moc za VAR_0 diamentów",
                //ES
                "Compra la poder del jugador por VAR_0 diamantes",
                //PT
                "Compre a poder do profissional por VAR_0 diamantes",
                "",
             },


        };        
    }
}
