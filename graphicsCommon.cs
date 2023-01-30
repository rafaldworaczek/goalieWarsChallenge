using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using MenuCustomNS;
using GlobalsNS;
using TMPro;
using LANGUAGE_NS;

namespace graphicsCommonNS
{
    public class GraphicsCommon
    {
        private int MAX_SHOES = 31;
        private int MAX_GLOVES = 31;

        private Teams teams;
        private StringCommon strCommon;

        public GraphicsCommon()
        {
            teams = new Teams();
            strCommon = new StringCommon();
        }

        public Texture2D getTexture(string fileName)
        {
            return Resources.Load<Texture2D>(fileName);
        }

        public Material getMaterial(string fileName)
        {
            return Resources.Load(fileName, typeof(Material)) as Material;
        }

        public void setMaterialByName(GameObject gameObject,
                                      string materialName,
                                      int elementIndex)
        {
            Material material = getMaterial(materialName);
            Material[] materials = gameObject.GetComponent<Renderer>().materials;
            materials[elementIndex] = material;
            gameObject.GetComponent<Renderer>().materials = materials;
        }

        public void setMaterialByName(GameObject gameObject,
                                      string materialName,
                                      int[] elementsIndex)
        {
            Material material = getMaterial(materialName);
            Material[] materials = gameObject.GetComponent<Renderer>().materials;
            for (int i = 0; i < elementsIndex.Length; i++)
            {
                materials[elementsIndex[i]] = material;
            }
            gameObject.GetComponent<Renderer>().materials = materials;
        }


        public void setShaderByName(GameObject gameObject,
                                    string shaderName,
                                    int elementIndex)
        {
            Material[] materials = gameObject.GetComponent<Renderer>().materials;
            materials[elementIndex].shader = Shader.Find(shaderName);

            if (shaderName.Contains("transparent"))
                materials[elementIndex].color = Color.clear;

            gameObject.GetComponent<Renderer>().materials = materials;
        }

        public void setShaderByName(GameObject gameObject,
                                    string shaderName,
                                    int[] elementsIndex)
        {
            Material[] materials = gameObject.GetComponent<Renderer>().materials;

            for (int i = 0; i < elementsIndex.Length; i++)
            {
                materials[elementsIndex[i]].shader = Shader.Find(shaderName);

                if (shaderName.Contains("transparent"))
                    materials[elementsIndex[i]].color = Color.clear;
            }

            gameObject.GetComponent<Renderer>().materials = materials;
        }

        public void setAllShaderByName(GameObject gameObject,
                                       string shaderName)
        {
            Material[] materials = gameObject.GetComponent<Renderer>().materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].shader = Shader.Find(shaderName);
                if (shaderName.Contains("UI/Lit/Transparent"))
                    materials[i].color = Color.clear;
            }
           
            gameObject.GetComponent<Renderer>().materials = materials;
        }

        public void setMaterialElement(GameObject gameObject,
                                       Material material,
                                       int elementIndex)
        {
            Material[] materials = gameObject.GetComponent<Renderer>().materials;
            materials[elementIndex] = material;
            gameObject.GetComponent<Renderer>().materials = materials;
        }

        public void setMaterialColor(Material material, Color color)
        {
            material.SetColor("_Color", color);
        }

        public void setMesh(GameObject gameObject,
                            string meshFilePath)

        {
            MeshFilter mesh = gameObject.GetComponent<MeshFilter>();
            mesh.sharedMesh = Resources.Load<Mesh>(meshFilePath);
        }

        public void setFlagRawImage(RawImage image, string team)
        {
            if (Globals.isTeamCustomize(team))
            {

                if (PlayerPrefs.HasKey("CustomizeTeam_logoUploaded"))
                {
                    image.texture = loadTexture(Globals.logoFilePath);
                }
                else
                {
                    image.texture = Resources.Load<Texture2D>(
                                "others/logoFile");
                }

                return;
            }

            team = Regex.Replace(team, "\\s+", "");
            image.texture = Resources.Load<Texture2D>("Flags/" + team.ToLower());
        }

        public string getFlagPath(string teamName)
        {
            string flagPath;

            if (Globals.isTeamCustomize(teamName))
            {
                return Globals.logoFilePath;
            }

            teamName = teamName.ToLower();
            teamName = Regex.Replace(teamName, "\\s+", "");

            Globals.teamLeagueName.TryGetValue(teamName, out flagPath);

            //return default flag
            if (flagPath == null)
                return Globals.logoFilePath;

            //Debug.Log("#DBGTRANDFLAGS " + (flagPath + "/" + teamName));

            return flagPath + "/" + teamName;
        }

        public string getFlagPath(string leagueName, string teamName)
        {
            string dirName = "";
            if (Globals.isTeamCustomize(teamName))
            {
                return Globals.logoFilePath;
            }

            teamName = Regex.Replace(teamName, "\\s+", "").ToLower();
           
            if (Globals.isPlayerCardLeague(leagueName))
            {
                dirName = leagueName.ToLower();
            }
            else
            {
                dirName = "national";
            }

            return dirName + "/" + teamName;
        }

        public string getFlagFullPath(string leagueName, string teamName)
        {
            string dirName = "";

            if (Globals.isTeamCustomize(teamName))
            {               
                    return Globals.logoFilePath;
            }

            teamName = Regex.Replace(teamName, "\\s+", "").ToLower();
            //leagueName = Regex.Replace(leagueName, "\\s+", "").ToLower();

            if (Globals.isPlayerCardLeague(leagueName))
            {
                dirName = leagueName;
            }
            else
            {
                dirName = "national";
            }

            dirName = Regex.Replace(dirName, "\\s+", "").ToLower();
            //Debug.Log("#DBGFLAGPATH " + ("Flags/" + dirName + "/" + teamName));
            return "Flags/" + dirName + "/" + teamName;
        }

        public string getFlagFullPath(string teamName)
        {
            if (Globals.isTeamCustomize(teamName))
            {
                return Globals.logoFilePath;
            }

            return "Flags/" + getFlagPath(teamName);
        }


        public void setTeamName(Text teamText, string team)
        {
            teamText.text = team;
        }
    
        /*public void setPlayerTextures(GameObject playerModel,
                                      GameObject playerModelHair,
                                      int teamId)
        {
            NationalTeams teams = new NationalTeams();

            string tshirtColorName = teams.getTshirtColorByTeamIndex(teamId);
            string shortsColorName = teams.getShortsColorByTeamIndex(teamId);
            string sockColorName = teams.getSocksColorByTeamIndex(teamId);
            string skinColorName = teams.getSkinColorByTeamIndex(teamId);

            Material tshirtMaterial = getMaterial("playerMaterials/" + tshirtColorName);
            Material shortsMaterial = getMaterial("playerMaterials/" + shortsColorName);
            Material socksMaterial = getMaterial("playerMaterials/" + sockColorName);
            Material skinMaterial = getMaterial("playerMaterials/skin/" + skinColorName);


            int shoeNum = 0;
            if (isShoesRandom)
            {
                shoeNum = UnityEngine.Random.Range(0, MAX_SHOES);
            }
            int gloveNum = 0;
            if (isGloveRandom)
            {
                gloveNum = UnityEngine.Random.Range(0, MAX_GLOVES);
            }
            Material shoeMaterial = getMaterial("playerMaterials/shoe/shoe_" + shoeNum.ToString()); ;
            Material gloveMaterial = getMaterial("playerMaterials/glove/glove_" + gloveNum.ToString());
            setMaterialElement(playerModel, shoeMaterial, 6);
            setMaterialElement(playerModel, gloveMaterial, 8);

            setMaterialElement(playerModel, skinMaterial, 0);
            setMaterialElement(playerModel, tshirtMaterial, 1);
            setMaterialElement(playerModel, tshirtMaterial, 2);
            setMaterialElement(playerModel, tshirtMaterial, 3);
            setMaterialElement(playerModel, tshirtMaterial, 4);
            setMaterialElement(playerModel, shortsMaterial, 5);
            setMaterialElement(playerModel, socksMaterial, 7);

            string hairColorName = teams.getHairColorByTeamIndex(teamId);
            Material hairMaterial = getMaterial(
                 "playerMaterials/hair/" + hairColorName + "/Materials/" + hairColorName);

            setMaterialElement(playerModelHair, hairMaterial, 0);
            setMesh(playerModelHair, "playerMaterials/hair/"
                 + hairColorName + "/" + hairColorName);
         }
         */

        public void setPlayerTextures(GameObject playerModel,
                                      GameObject playerModelHair,
                                      int teamId,
                                      string leagueName,
                                      string playerDesc,
                                      bool isShoesRandom,
                                      bool isGloveRandom,
                                      Teams teams)                                      
        {
            //NationalTeams teams = new NationalTeams();
            if (teams == null)
            {
                Debug.Log("DBGFRIENDLY teamsLeague " + leagueName);
                teams = new Teams(leagueName);
            }

            string tshirtColorName = teams.getTshirtColorByTeamIndex(teamId);
            string shortsColorName = teams.getShortsColorByTeamIndex(teamId);
            string sockColorName = teams.getSocksColorByTeamIndex(teamId);
            //string skinColorName = teams.getSkinColorByTeamIndex(teamId);
            //string hairColorName = teams.getHairColorByTeamIndex(teamId);
            string skinColorName = "";
            string hairColorName = "";

            string shoeName = "";
            string glovesName = "";

            Debug.Log("DBGFRIENDLY tShirtColorName " + tshirtColorName + " shortsColorName " + shortsColorName);

            /*if (leagueName.Contains("SPAIN") ||
                leagueName.Contains("WORLD CUP") ||
                leagueName.Contains("ENGLAND") ||
                leagueName.Contains("BRAZIL") ||
                leagueName.Contains("ITALY") ||
                leagueName.Contains("GERMANY") ||
                leagueName.Contains("POLAND") ||
                leagueName.Contains("NATIONAL TEAMS") ||
                leagueName.Contains("NATIONALS"))*/
            //{
                shoeName = teams.getTeamByIndex(teamId)[13];
                string[] shoes = shoeName.Split('|');
                int randShoes = 0;

                if (isShoesRandom)
                    randShoes = 
                        UnityEngine.Random.Range(0, shoes.Length);
                shoeName = shoes[randShoes];

                Debug.Log("shoeName " + shoeName);

                glovesName = teams.getTeamByIndex(teamId)[14];
                string[] gloves = glovesName.Split('|');
                int randGloves = 0;
                if (isGloveRandom) 
                 randGloves =
                    UnityEngine.Random.Range(0, gloves.Length);
                glovesName = gloves[randGloves];

                //Debug.Log("#DBG1034 TEAMNAME " + teams.getTeamByIndex(teamId)[0] +
                //   " shoes " + teams.getTeamByIndex(teamId)[13]
                //    + " gloves " + teams.getTeamByIndex(teamId)[14]
                //   + " shoeNames " + shoeName
                //   + " glovesNames " + glovesName);


            //}

            //Debug.Log("#DBG123 shoeName " + shoeName + " glovesName " + glovesName + " PLAYERDESC " +
            //    playerDesc);


            //if (Globals.isPlayerCardLeague(leagueName))
            //{

            //Debug.Log("#DBG skinColorName playerDesc " + playerDesc);

                string skinHairColorName = playerDesc.Split(':')[6];

                //Debug.Log("skinHARICOLORNAME " + skinHairColorName + " leagueName " + leagueName);

                //Debug.Log("#DBG LeaguName " + leagueName + "playerDesc " + playerDesc);

                int delimeterIndex =
                    strCommon.getIndexOfNOccurence(skinHairColorName, '_', 4);

                //Debug.Log("#DBG LeaguName " + leagueName + " delimeterIndex " + delimeterIndex);
               
                skinColorName = "skin_" +
                     Globals.getSkinHairColorName(
                            skinHairColorName.Substring(0, delimeterIndex));

            Debug.Log("skinHairColorName " + skinHairColorName);

                //string skinHairColorName = Globals.getSkinHairColorName(playerDesc.Split(':')[6]);

                hairColorName =                    
                        skinHairColorName.Substring(delimeterIndex + 1);

                //Debug.Log("#DBG skinColorName " + skinColorName + " HAIRCOLOR " + hairColorName
                //   + " delimiterIndex " + delimeterIndex);
            //}

            Material tshirtMaterial = getMaterial("playerMaterials/" + tshirtColorName);
            Material shortsMaterial = getMaterial("playerMaterials/" + shortsColorName);
            Material socksMaterial = getMaterial("playerMaterials/" + sockColorName);
            Material skinMaterial = getMaterial("playerMaterials/skins/" + skinColorName);

            Material shoeMaterial = getMaterial("playerMaterials/shoe/" + shoeName);
            Material gloveMaterial = getMaterial("playerMaterials/glove/" + glovesName);

            //int shoeNum = 0;
            //if (isShoesRandom)
            //{
            //    shoeNum = UnityEngine.Random.Range(0, MAX_SHOES);
            //}

            //int gloveNum = 30;
            //if (isGloveRandom)
            //{
            //    gloveNum = UnityEngine.Random.Range(0, MAX_GLOVES);
            //}

            //Material shoeMaterial = getMaterial("playerMaterials/shoe/shoe_" + shoeNum.ToString()); ;
            //Material gloveMaterial = getMaterial("playerMaterials/glove/glove_" + gloveNum.ToString());

            setMaterialElement(playerModel, shoeMaterial, 6);
            setMaterialElement(playerModel, gloveMaterial, 8);

            setMaterialElement(playerModel, skinMaterial, 0);
            setMaterialElement(playerModel, tshirtMaterial, 1);
            setMaterialElement(playerModel, tshirtMaterial, 2);
            setMaterialElement(playerModel, tshirtMaterial, 3);
            setMaterialElement(playerModel, tshirtMaterial, 4);
            setMaterialElement(playerModel, shortsMaterial, 5);
            setMaterialElement(playerModel, socksMaterial, 7);
     
            Material hairMaterial = getMaterial(
                 "playerMaterials/hair/" + hairColorName + "/Materials/" + hairColorName);

            setMaterialElement(playerModelHair, hairMaterial, 0);
            setMesh(playerModelHair, "playerMaterials/hair/"
                    + hairColorName + "/" + hairColorName);
        }

        public void setPlayerTextures(GameObject playerModel,
                                      GameObject playerModelHair,
                                      string playerDesc,
                                      string teamDesc)
                            
        {
            string[] jersey = teamDesc.Split('|');
            string tshirtColorName = jersey[0];
            string shortsColorName = jersey[1];
            string sockColorName = jersey[2];
            string skinColorName = "";
            string hairColorName = "";

            string shoeName = "";
            string glovesName = "";

           
            shoeName = jersey[5];
            string[] shoes = shoeName.Split('|');
            int randShoes = 0;
           
            shoeName = shoes[randShoes];

            //Debug.Log("shoeName " + shoeName);

            glovesName = jersey[4];
            string[] gloves = glovesName.Split('|');
            int randGloves = 0;
            glovesName = gloves[randGloves];
         
            string skinHairColorName = jersey[3];
            //Debug.Log("skinHairColorName " + skinHairColorName);

            int delimeterIndex =
                strCommon.getIndexOfNOccurence(skinHairColorName, '_', 4);

            //Debug.Log("#DBG LeaguName " + leagueName + " delimeterIndex " + delimeterIndex);

            skinColorName = "skin_" +
                 Globals.getSkinHairColorName(
                        skinHairColorName.Substring(0, delimeterIndex));

            hairColorName =
                    skinHairColorName.Substring(delimeterIndex + 1);
          
            Material tshirtMaterial = getMaterial("playerMaterials/" + tshirtColorName);
            Material shortsMaterial = getMaterial("playerMaterials/" + shortsColorName);
            Material socksMaterial = getMaterial("playerMaterials/" + sockColorName);
            Material skinMaterial = getMaterial("playerMaterials/skins/" + skinColorName);

            Material shoeMaterial = getMaterial("playerMaterials/shoe/" + shoeName);
            Material gloveMaterial = getMaterial("playerMaterials/glove/" + glovesName);
        
            setMaterialElement(playerModel, shoeMaterial, 6);
            setMaterialElement(playerModel, gloveMaterial, 8);

            setMaterialElement(playerModel, skinMaterial, 0);
            setMaterialElement(playerModel, tshirtMaterial, 1);
            setMaterialElement(playerModel, tshirtMaterial, 2);
            setMaterialElement(playerModel, tshirtMaterial, 3);
            setMaterialElement(playerModel, tshirtMaterial, 4);
            setMaterialElement(playerModel, shortsMaterial, 5);
            setMaterialElement(playerModel, socksMaterial, 7);

            Material hairMaterial = getMaterial(
                 "playerMaterials/hair/" + hairColorName + "/Materials/" + hairColorName);

            setMaterialElement(playerModelHair, hairMaterial, 0);
            setMesh(playerModelHair, "playerMaterials/hair/"
                    + hairColorName + "/" + hairColorName);
        }


        public void setPlayerTexturesSkin(GameObject playerModel,
                                          string skinName)
        {
            Material skinMaterial = getMaterial("playerMaterials/skins/" + skinName);
            setMaterialElement(playerModel, skinMaterial, 0);
        }

        

        public void setPlayerTexturesHair(GameObject playerModelHair,
                                          string hairColorName)
        {

            //Debug.Log("HAIRPATH " +
            //    "playerMaterials/hair/" + hairColorName + "/Materials/" + hairColorName);

            Material hairMaterial = getMaterial(
                 "playerMaterials/hair/" + hairColorName + "/Materials/" + hairColorName);


            setMaterialElement(playerModelHair, hairMaterial, 0);
            setMesh(playerModelHair, "playerMaterials/hair/"
                     + hairColorName + "/" + hairColorName);

            //Debug.Log("HAIRMATERIAL " + hairMaterial);
        }

        public IEnumerator textFadeOut(Text text, float timeSec)
        {
            Color color = text.color;
            float step = 0.05f;
            float nStep = Mathf.Ceil(color.a / step);
            float delay = timeSec / nStep;

            for (int i = 0; i < nStep; i++)
            {
                color.a = Mathf.Max(0f, color.a - step);
                text.color = color;
                yield return new WaitForSeconds(delay);
            }
        }

        public IEnumerator textFadeOut(TextMeshProUGUI text, float timeSec)
        {
            Color color = text.color;
            float step = 0.05f;
            float nStep = Mathf.Ceil(color.a / step);
            float delay = timeSec / nStep;

            for (int i = 0; i < nStep; i++)
            {
                color.a = Mathf.Max(0f, color.a - step);
                text.color = color;
                yield return new WaitForSeconds(delay);
            }
        }

        public string getSkillsStr(string defense, string attack)
        {
            return "D:" + defense + "\n" + "A:" + attack;
        }

        /*(public Texture2D toTexture2D(RenderTexture rTex, string fileName)
        {
            var oldRT = RenderTexture.active;
            var tex = new Texture2D(rTex.width, rTex.height, TextureFormat.ARGB32, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();

            System.IO.File.WriteAllBytes(fileName, tex.EncodeToPNG());
            RenderTexture.active = oldRT;
            return null;
        }*/

        public Texture2D saveTextureToPng(Texture2D tex, string fileName)
        {
            //Texture2D texture1 = 
            //    new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);

            byte[] bytes = tex.EncodeToPNG();
            System.IO.File.WriteAllBytes(fileName, bytes);          
            return null;
        }

        public Texture2D loadTexture(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (System.IO.File.Exists(path))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                return texture;
            }

            return null;
        }

        public Texture2D combine2DTextures(Texture2D Background, 
                                           Texture2D Overlay, 
                                           int sizeX, 
                                           int sizeY)
        {
            Texture2D newTexture = new Texture2D(sizeX, sizeY);
            Vector2 offset =
                new Vector2(((newTexture.width - Overlay.width) / 2), ((newTexture.height - Overlay.height) / 2));
            newTexture.SetPixels(Background.GetPixels());

            Debug.Log("#DBGPixe widht " + Overlay.width + " height " + Overlay.height + " offset " + 
                offset);
            for (int y = 0; y < Overlay.height; y++)
            {
                for (int x = 0; x < Overlay.width; x++)
                {
                    //float PixelColorFore = Overlay.GetPixel(x, y) * Overlay.GetPixel(x, y).a;
                    //float PixelColorBack = newTexture.GetPixel(x + offset.x, y + offset.y) * (1 - PixelColorFore.a);
                    newTexture.SetPixel(x + (int) offset.x, y + (int) offset.y, Overlay.GetPixel(x, y));
                    //Debug.Log("#DBGPixe " + Overlay.GetPixel(x, y));
                        //PixelColorBack + PixelColorFore);
                }
            }

            newTexture.Apply();
            return newTexture;
        }
    }
}