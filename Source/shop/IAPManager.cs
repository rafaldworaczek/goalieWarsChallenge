using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using GlobalsNS;
using UnityEngine.UI;
using UnityEngine.Analytics;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAPManager : MonoBehaviour, IStoreListener
{
        public static IAPManager instance = null;
        private Dictionary<string, string> prices;
        private Dictionary<string, int> buyingStatus;

        private static int BUY_NO_ACTION = 1;
        private static int BUY_START = 2;
        private static int BUY_FINISH = 3;
        private static int BUY_FAILED = 4;

        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
        private Shop shop;
   
        private static string removeAds = "removeadsgoaliestriker";
        private static string coin100 = "coin100goaliestriker";
        private static string coin200 = "coin200goaliestriker";
        private static string coin500 = "coin500goaliestriker";
        private static string coin1000 = "coin1000goaliestriker";
        private static string coin2000 = "coin2000goaliestriker";
        private static string coin5000 = "coin5000goaliestriker";
        private static string coin8000 = "coin8000goaliestriker";
        private static string coin10000 = "coin10000goaliestriker";

        private static string diamond100 = "diamond100goaliestriker";
        private static string diamond200 = "diamond200goaliestriker";
        private static string diamond500 = "diamond500goaliestriker";
        private static string diamond1000 = "diamond1000goaliestriker";
        private static string diamond2000 = "diamond2000goaliestriker";
        private static string diamond5000 = "diamond5000goaliestriker";
        private static string diamond8000 = "diamond8000goaliestriker";
        private static string diamond10000 = "diamond10000goaliestriker";

        private static string unlockedteam100 = "unlockedteam100goaliestriker";
        private static string unlockedteam200 = "unlockedteam200goaliestriker";
        private static string unlockedteam500 = "unlockedteam500goaliestriker";
        private static string unlockedteam1000 = "unlockedteam1000goaliestriker";
        private static string unlockedteam2000 = "unlockedteam2000goaliestriker";
        private static string unlockedteam5000 = "unlockedteam5000goaliestriker";
        private static string unlockedteam8000 = "unlockedteam8000goaliestriker";
        private static string unlockedteam10000 = "unlockedteam10000goaliestriker";

        private static string unlockedplayercard100 = "unlockedplayercard100goaliestriker";
        private static string unlockedplayercard200 = "unlockedplayercard200goaliestriker";
        private static string unlockedplayercard500 = "unlockedplayercard500goaliestriker";
        private static string unlockedplayercard1000 = "unlockedplayercard1000goaliestriker";
        private static string unlockedplayercard2000 = "unlockedplayercard2000goaliestriker";
        private static string unlockedplayercard5000 = "unlockedplayercard5000goaliestriker";
        private static string unlockedplayercard8000 = "unlockedplayercard8000goaliestriker";
        private static string unlockedplayercard10000 = "unlockedplayercard10000goaliestriker";

        private static string attack1team = "attack1goaliestriker";
        private static string attack2team = "attack2goaliestriker";
        private static string attack3team = "attack3goaliestriker";
        private static string attack4team = "attack4goaliestriker";
        private static string attack5team = "attack5goaliestriker";
        private static string attack6team = "attack6goaliestriker";
        private static string attack7team = "attack7goaliestriker";
        private static string attack8team = "attack8goaliestriker";
        private static string attack9team = "attack9goaliestriker";
        private static string attack10team = "attack10goaliestriker";

        private static string defense1team = "defense1goaliestriker";
        private static string defense2team = "defense2goaliestriker";
        private static string defense3team = "defense3goaliestriker";
        private static string defense4team = "defense4goaliestriker";
        private static string defense5team = "defense5goaliestriker";
        private static string defense6team = "defense6goaliestriker";
        private static string defense7team = "defense7goaliestriker";
        private static string defense8team = "defense8goaliestriker";
        private static string defense9team = "defense9goaliestriker";
        private static string defense10team = "defense10goaliestriker";

        private static string attackdefense10team = "attackdefense10goaliestriker";

        private static string enlargegoalsize_medium = "enlargegoalsize_medium_goaliestriker";
        private static string extrapromition = "extrapromition_goaliestriker";

        // Product identifiers for all products capable of being purchased: 
        // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
        // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
        // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.
    //public static string kProductIDConsumable = "consumable";
    //public static string kProductIDNonConsumable = "nonconsumable";
    //public static string kProductIDSubscription = "subscription";

    // Apple App Store-specific product identifier for the subscription product.
    //private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

    // Google Play Store-specific product identifier subscription product.
    //private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            instance.init();
        }
        else
        {
            if (instance != this)
            {
                instance.init();
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
                InitializePurchasing();
            } else
            {
                fillPricesHash();
            }           
    }
  
    public void fillPricesHash()
    {
        prices["removeAds"] = GetPrice(removeAds);
        prices["coin100"] = GetPrice(coin100);
        prices["coin200"] = GetPrice(coin200);
        prices["coin500"] = GetPrice(coin500);
        prices["coin1000"] = GetPrice(coin1000);
        prices["coin2000"] = GetPrice(coin2000);
        prices["coin5000"] = GetPrice(coin5000);
        prices["coin8000"] = GetPrice(coin8000);
        prices["coin10000"] = GetPrice(coin10000);

        prices["diamond100"] = GetPrice(diamond100);
        prices["diamond200"] = GetPrice(diamond200);
        prices["diamond500"] = GetPrice(diamond500);
        prices["diamond1000"] = GetPrice(diamond1000);
        prices["diamond2000"] = GetPrice(diamond2000);
        prices["diamond5000"] = GetPrice(diamond5000);
        prices["diamond8000"] = GetPrice(diamond8000);
        prices["diamond10000"] = GetPrice(diamond10000);

        prices["unlockedteam100"] = GetPrice(unlockedteam100);
        prices["unlockedteam200"] = GetPrice(unlockedteam200);
        prices["unlockedteam500"] = GetPrice(unlockedteam500);
        prices["unlockedteam1000"] = GetPrice(unlockedteam1000);
        prices["unlockedteam2000"] = GetPrice(unlockedteam2000);
        prices["unlockedteam5000"] = GetPrice(unlockedteam5000);
        prices["unlockedteam8000"] = GetPrice(unlockedteam8000);
        prices["unlockedteam10000"] = GetPrice(unlockedteam10000);

        prices["unlockedplayercard100"] = GetPrice(unlockedplayercard100);
        prices["unlockedplayercard200"] = GetPrice(unlockedplayercard200);
        prices["unlockedplayercard500"] = GetPrice(unlockedplayercard500);
        prices["unlockedplayercard1000"] = GetPrice(unlockedplayercard1000);
        prices["unlockedplayercard2000"] = GetPrice(unlockedplayercard2000);
        prices["unlockedplayercard5000"] = GetPrice(unlockedplayercard5000);
        prices["unlockedplayercard8000"] = GetPrice(unlockedplayercard8000);
        prices["unlockedplayercard10000"] = GetPrice(unlockedplayercard10000);

        prices["attack1team"] = GetPrice(attack1team);
        prices["attack2team"] = GetPrice(attack2team);
        prices["attack3team"] = GetPrice(attack3team);
        prices["attack4team"] = GetPrice(attack4team);
        prices["attack5team"] = GetPrice(attack5team);
        prices["attack6team"] = GetPrice(attack6team);
        prices["attack7team"] = GetPrice(attack7team);
        prices["attack8team"] = GetPrice(attack8team);
        prices["attack9team"] = GetPrice(attack9team);
        prices["attack10team"] = GetPrice(attack10team);

        prices["defense1team"] = GetPrice(defense1team);
        prices["defense2team"] = GetPrice(defense2team);
        prices["defense3team"] = GetPrice(defense3team);
        prices["defense4team"] = GetPrice(defense4team);
        prices["defense5team"] = GetPrice(defense5team);
        prices["defense6team"] = GetPrice(defense6team);
        prices["defense7team"] = GetPrice(defense7team);
        prices["defense8team"] = GetPrice(defense8team);
        prices["defense9team"] = GetPrice(defense9team);
        prices["defense10team"] = GetPrice(defense10team);

        prices["attackdefense10team"] = GetPrice(attackdefense10team);
        prices["enlargegoalsize_medium"] = GetPrice(enlargegoalsize_medium);
        prices["extrapromotion"] = GetPrice(extrapromition);

    }

    private void init()
    {
        //print("DBGPurchasing UI init");
        prices = new Dictionary<string, string>();
        fillPricesHash();
    }
     
    #region buyProducts
    public void buyRemoveAds()
    {
        BuyProductID(removeAds);
    }

    public void buyCoin100()
    {
        BuyProductID(coin100);
    }

    public void buyCoin200()
    {
        BuyProductID(coin200);
    }

    public void buyCoin500()
    {
        BuyProductID(coin500);
    }

    public void buyCoin1000()
    {
        BuyProductID(coin1000);
    }

    public void buyCoin2000()
    {
        BuyProductID(coin2000);
    }

    public void buyCoin5000()
    {   
        BuyProductID(coin5000);
    }

    public void buyCoin8000()
    {
        BuyProductID(coin8000);
    }

    public void buyCoin10000()
    {
        BuyProductID(coin10000);
    }

    public void buyDiamond100()
    {
        BuyProductID(diamond100);
    }

    public void buyDiamond200()
    {
        BuyProductID(diamond200);
    }

    public void buyDiamond500()
    {
        BuyProductID(diamond500);
    }

    public void buyDiamond1000()
    {
        BuyProductID(diamond1000);
    }

    public void buyDiamond2000()
    {
        BuyProductID(diamond2000);
    }

    public void buyDiamond5000()
    {
        BuyProductID(diamond5000);
    }

    public void buyDiamond8000()
    {
        BuyProductID(diamond8000);
    }

    public void buyDiamond10000()
    {
        BuyProductID(diamond10000);
    }

    public void buyProductByName(string name)
    {
        BuyProductID(name);
    }

    //-----------------//
    public void buyUnlockedteam100()
    {
        BuyProductID(unlockedteam100);
    }

    public void buyUnlockedteam200()
    {
        BuyProductID(unlockedteam200);
    }

    public void buyUnlockedteam500()
    {
        BuyProductID(unlockedteam500);
    }

    public void buyUnlockedteam1000()
    {
        BuyProductID(unlockedteam1000);
    }

    public void buyUnlockedteam2000()
    {
        BuyProductID(unlockedteam2000);
    }

    public void buyUnlockedteam5000()
    {
        BuyProductID(unlockedteam5000);
    }

    public void buyUnlockedteam8000()
    {
        BuyProductID(unlockedteam8000);
    }

    public void buyUnlockedteam10000()
    {
        BuyProductID(unlockedteam10000);
    }

    //---------------------------/
    public void buyAttack1team()
    {
        BuyProductID(attack1team);
    }

    public void buyAttack2team()
    {
        BuyProductID(attack2team);
    }

    public void buyAttack3team()
    {
        BuyProductID(attack3team);
    }

    public void buyAttack4team()
    {
        BuyProductID(attack4team);
    }

    public void buyAttack5team()
    {
        BuyProductID(attack5team);
    }

    public void buyAttack6team()
    {
        BuyProductID(attack6team);
    }

    public void buyAttack7team()
    {
        BuyProductID(attack7team);
    }

    public void buyAttack8team()
    {
        BuyProductID(attack8team);
    }

    public void buyAttack9team()
    {
        BuyProductID(attack9team);
    }

    public void buyAttack10team()
    {
        BuyProductID(attack10team);
    }

    //---------------------------/
    public void buyDefense1team()
    {
        BuyProductID(defense1team);
    }

    public void buyDefense2team()
    {
        BuyProductID(defense2team);
    }

    public void buyDefense3team()
    {
        BuyProductID(defense3team);
    }

    public void buyDefense4team()
    {
        BuyProductID(defense4team);
    }

    public void buyDefense5team()
    {
        BuyProductID(defense5team);
    }

    public void buyDefense6team()
    {
        BuyProductID(defense6team);
    }

    public void buyDefense7team()
    {
        BuyProductID(defense7team);
    }

    public void buyDefense8team()
    {
        BuyProductID(defense8team);
    }

    public void buyDefense9team()
    {
        BuyProductID(defense9team);
    }

    public void buyDefense10team()
    {
        BuyProductID(defense10team);
    }
    //--------------------------/

    public void buyAttackDefense10team()
    {
        BuyProductID(attackdefense10team);
    }

    public void buyEnlargegoalsizeMedium()
    {
        //BuyProductID(enlargegoalsize_medium);
        BuyProductID(extrapromition);
    }

    #endregion

    public void InitializePurchasing()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.
            //builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
            // Continue adding the non-consumable product.
            //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);

            builder.AddProduct(removeAds, ProductType.NonConsumable);
            builder.AddProduct(coin100, ProductType.Consumable);
            builder.AddProduct(coin200, ProductType.Consumable);
            builder.AddProduct(coin500, ProductType.Consumable);
            builder.AddProduct(coin1000, ProductType.Consumable);
            builder.AddProduct(coin2000, ProductType.Consumable);
            builder.AddProduct(coin5000, ProductType.Consumable);
            builder.AddProduct(coin8000, ProductType.Consumable);
            builder.AddProduct(coin10000, ProductType.Consumable);

            builder.AddProduct(diamond100, ProductType.Consumable);
            builder.AddProduct(diamond200, ProductType.Consumable);
            builder.AddProduct(diamond500, ProductType.Consumable);
            builder.AddProduct(diamond1000, ProductType.Consumable);
            builder.AddProduct(diamond2000, ProductType.Consumable);
            builder.AddProduct(diamond5000, ProductType.Consumable);
            builder.AddProduct(diamond8000, ProductType.Consumable);
            builder.AddProduct(diamond10000, ProductType.Consumable);

            builder.AddProduct(unlockedteam100, ProductType.Consumable);
            builder.AddProduct(unlockedteam200, ProductType.Consumable);
            builder.AddProduct(unlockedteam500, ProductType.Consumable);
            builder.AddProduct(unlockedteam1000, ProductType.Consumable);
            builder.AddProduct(unlockedteam2000, ProductType.Consumable);
            builder.AddProduct(unlockedteam5000, ProductType.Consumable);
            builder.AddProduct(unlockedteam8000, ProductType.Consumable);
            builder.AddProduct(unlockedteam10000, ProductType.Consumable);

            builder.AddProduct(unlockedplayercard100, ProductType.Consumable);
            builder.AddProduct(unlockedplayercard200, ProductType.Consumable);
            builder.AddProduct(unlockedplayercard500, ProductType.Consumable);
            builder.AddProduct(unlockedplayercard1000, ProductType.Consumable);
            builder.AddProduct(unlockedplayercard2000, ProductType.Consumable);
            builder.AddProduct(unlockedplayercard5000, ProductType.Consumable);
            builder.AddProduct(unlockedplayercard8000, ProductType.Consumable);
            builder.AddProduct(unlockedplayercard10000, ProductType.Consumable);

            builder.AddProduct(attack1team, ProductType.Consumable);
            builder.AddProduct(attack2team, ProductType.Consumable);
            builder.AddProduct(attack3team, ProductType.Consumable);
            builder.AddProduct(attack4team, ProductType.Consumable);
            builder.AddProduct(attack5team, ProductType.Consumable);
            builder.AddProduct(attack6team, ProductType.Consumable);
            builder.AddProduct(attack7team, ProductType.Consumable);
            builder.AddProduct(attack8team, ProductType.Consumable);
            builder.AddProduct(attack9team, ProductType.Consumable);
            builder.AddProduct(attack10team, ProductType.Consumable);

            builder.AddProduct(defense1team, ProductType.Consumable);
            builder.AddProduct(defense2team, ProductType.Consumable);
            builder.AddProduct(defense3team, ProductType.Consumable);
            builder.AddProduct(defense4team, ProductType.Consumable);
            builder.AddProduct(defense5team, ProductType.Consumable);
            builder.AddProduct(defense6team, ProductType.Consumable);
            builder.AddProduct(defense7team, ProductType.Consumable);
            builder.AddProduct(defense8team, ProductType.Consumable);
            builder.AddProduct(defense9team, ProductType.Consumable);
            builder.AddProduct(defense10team, ProductType.Consumable);

            builder.AddProduct(attackdefense10team, ProductType.Consumable);
            builder.AddProduct(enlargegoalsize_medium, ProductType.Consumable);
            builder.AddProduct(extrapromition, ProductType.Consumable);
        
        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here. 
        /*builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
            { kProductNameAppleSubscription, AppleAppStore.Name },
            { kProductNameGooglePlaySubscription, GooglePlay.Name },
        });*/

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
        }

        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        //public void BuyConsumable()
        //{
            // Buy the consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
        //    BuyProductID(kProductIDConsumable);
        //}

        //public void BuyNonConsumable()
        //{
            // Buy the non-consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
         //   BuyProductID(kProductIDNonConsumable);
        //}

        //public void BuySubscription()
        //{
            // Buy the subscription product using its the general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            // Notice how we use the general product identifier in spite of this ID being mapped to
            // custom store-specific identifiers above.
        //    BuyProductID(kProductIDSubscription);
        //}


        void BuyProductID(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = m_StoreController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("DBGPurchasing Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("DBGPurchasing BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("DBGPurchasing BuyProductID FAIL. Not initialized.");
            }
        }


        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("DBGPurchasing RestorePurchases FAIL. Not initialized.");
                return;
            }

            // If we are running on an Apple device ... 
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                Debug.Log("DBGPurchasing RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
                // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) => {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                    // no purchases are available to be restored.
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                Debug.Log("DBGPurchasing RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


        //  
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("DBGPurchasing OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;

            //fillPricesText();
            fillPricesHash();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("DBGPurchasing OnInitializeFailed InitializationFailureReason:" + error);
       }

       public void OnInitializeFailed(InitializationFailureReason error, string test)
       {
            Debug.Log("DBGPurchasing OnInitializeFailed InitializationFailureReason:" + error);
       }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
        // A consumable product has been purchased by this user.
            if (String.Equals(args.purchasedProduct.definition.id, diamond100, StringComparison.Ordinal))
            {
                Globals.addDiamonds(100);
                ///instance.shop.updateCurrentdiamonds(100);
                //purchaserInstance.shop.showRewardedPanel(100, "diamonds");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("diamond100", 100));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, diamond200, StringComparison.Ordinal))
            {
                Globals.addDiamonds(200);
                //purchaserInstance.shop.updateCurrentdiamonds(200);
                //purchaserInstance.shop.showRewardedPanel(200, "diamonds");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("diamond200", 200));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, diamond500, StringComparison.Ordinal))
            {
                Globals.addDiamonds(500);
                //purchaserInstance.shop.updateCurrentdiamonds(500);
                //purchaserInstance.shop.showRewardedPanel(500, "diamonds");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("diamond500", 500));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, diamond1000, StringComparison.Ordinal))
            {
                Globals.addDiamonds(1000);
                //purchaserInstance.shop.updateCurrentdiamonds(1000);
                //purchaserInstance.shop.showRewardedPanel(1000, "diamonds");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("diamond1000", 1000));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, diamond2000, StringComparison.Ordinal))
            {
                Globals.addDiamonds(2000);
                //purchaserInstance.shop.updateCurrentdiamonds(2000);
                //purchaserInstance.shop.showRewardedPanel(2000, "diamonds");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("diamond2000", 2000));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, diamond5000, StringComparison.Ordinal))
            {
                Globals.addDiamonds(5000);
                //purchaserInstance.shop.updateCurrentdiamonds(5000);
                //purchaserInstance.shop.showRewardedPanel(5000, "diamonds");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("diamond5000", 5000));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, diamond8000, StringComparison.Ordinal))
            {
                Globals.addDiamonds(8000);
                //purchaserInstance.shop.updateCurrentdiamonds(8000);
                //purchaserInstance.shop.showRewardedPanel(8000, "diamonds");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("diamond8000", 8000));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, diamond10000, StringComparison.Ordinal))
            {
                Globals.addDiamonds(10000);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("diamond10000", 10000));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            } else if (String.Equals(args.purchasedProduct.definition.id, removeAds, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                //UNCOMMENTIT
                Globals.adsRemove();
                Globals.purchasesQueue.Enqueue(new PurchaseItem("removeAds", 0));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, coin100, StringComparison.Ordinal))
            {
                Globals.addCoins(100);
                ///instance.shop.updateCurrentCoins(100);
            //purchaserInstance.shop.showRewardedPanel(100, "coins");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("coin100", 100));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, coin200, StringComparison.Ordinal))
            {
                Globals.addCoins(200);    
            //purchaserInstance.shop.updateCurrentCoins(200);
            //purchaserInstance.shop.showRewardedPanel(200, "coins");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("coin200", 200));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, coin500, StringComparison.Ordinal))
            {
                Globals.addCoins(500);
                //purchaserInstance.shop.updateCurrentCoins(500);
                //purchaserInstance.shop.showRewardedPanel(500, "coins");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("coin500", 500));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, coin1000, StringComparison.Ordinal))
            {
                Globals.addCoins(1000);
                //purchaserInstance.shop.updateCurrentCoins(1000);
                //purchaserInstance.shop.showRewardedPanel(1000, "coins");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("coin1000", 1000));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, coin2000, StringComparison.Ordinal))
            {
                Globals.addCoins(2000);
                //purchaserInstance.shop.updateCurrentCoins(2000);
                //purchaserInstance.shop.showRewardedPanel(2000, "coins");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("coin2000", 2000));   
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, coin5000, StringComparison.Ordinal))
            {
                Globals.addCoins(5000);
                //purchaserInstance.shop.updateCurrentCoins(5000);
                //purchaserInstance.shop.showRewardedPanel(5000, "coins");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("coin5000", 5000));    
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, coin8000, StringComparison.Ordinal))
            {
                Globals.addCoins(8000);
                //purchaserInstance.shop.updateCurrentCoins(8000);
                //purchaserInstance.shop.showRewardedPanel(8000, "coins");
                Globals.purchasesQueue.Enqueue(new PurchaseItem("coin8000", 8000));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, coin10000, StringComparison.Ordinal))
            {
                Globals.addCoins(10000);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("coin10000", 10000));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedteam100, StringComparison.Ordinal))
            {
                Globals.unlockedTeam(Globals.purchaseTeamIdx);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedteam100", 0, Globals.purchaseTeamIdx));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedteam200, StringComparison.Ordinal))
            {          
                Globals.unlockedTeam(Globals.purchaseTeamIdx);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedteam200", 0, Globals.purchaseTeamIdx));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));                
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedteam500, StringComparison.Ordinal))
            {
                Globals.unlockedTeam(Globals.purchaseTeamIdx);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedteam500", 0, Globals.purchaseTeamIdx));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedteam1000, StringComparison.Ordinal))
            {      
                Globals.unlockedTeam(Globals.purchaseTeamIdx);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedteam1000", 0, Globals.purchaseTeamIdx));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }   
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedteam2000, StringComparison.Ordinal))
            {  
                Globals.unlockedTeam(Globals.purchaseTeamIdx);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedteam2000", 0, Globals.purchaseTeamIdx));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedteam5000, StringComparison.Ordinal))
            {
                Globals.unlockedTeam(Globals.purchaseTeamIdx);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedteam5000", 0, Globals.purchaseTeamIdx));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedteam8000, StringComparison.Ordinal))
            {           
                Globals.unlockedTeam(Globals.purchaseTeamIdx);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedteam8000", 0, Globals.purchaseTeamIdx));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedteam10000, StringComparison.Ordinal))
            {           
                Globals.unlockedTeam(Globals.purchaseTeamIdx);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedteam10000", 0, Globals.purchaseTeamIdx));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedplayercard100, StringComparison.Ordinal))
            {

                Globals.unlockedPlayerCard(Globals.purchaseTeamIdx,
                                           Globals.purchaseLeagueName,
                                           Globals.purchaseTeamName,
                                           Globals.purchasePlayerDesc);

                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedplayercard100", 
                                                                 Globals.purchaseTeamName,
                                                                 Globals.purchasePlayerDesc,
                                                                 Globals.purchaseLeagueName));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedplayercard200, StringComparison.Ordinal))
            {
                Globals.unlockedPlayerCard(Globals.purchaseTeamIdx,
                                           Globals.purchaseLeagueName,
                                           Globals.purchaseTeamName,
                                           Globals.purchasePlayerDesc);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedplayercard200",
                                                                 Globals.purchaseTeamName,
                                                                 Globals.purchasePlayerDesc,
                                                                 Globals.purchaseLeagueName));
            Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedplayercard500, StringComparison.Ordinal))
            {
                Globals.unlockedPlayerCard(Globals.purchaseTeamIdx,
                                           Globals.purchaseLeagueName,
                                           Globals.purchaseTeamName,
                                           Globals.purchasePlayerDesc);
            Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedplayercard500",
                                                             Globals.purchaseTeamName,
                                                             Globals.purchasePlayerDesc,
                                                             Globals.purchaseLeagueName));
            Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedplayercard1000, StringComparison.Ordinal))
            {
                Globals.unlockedPlayerCard(Globals.purchaseTeamIdx,
                                           Globals.purchaseLeagueName,
                                           Globals.purchaseTeamName,
                                           Globals.purchasePlayerDesc);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedplayercard1000",
                                                                Globals.purchaseTeamName,
                                                                Globals.purchasePlayerDesc,
                                                                Globals.purchaseLeagueName));

            Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedplayercard2000, StringComparison.Ordinal))
            {
                Globals.unlockedPlayerCard(Globals.purchaseTeamIdx,
                                           Globals.purchaseLeagueName,
                                           Globals.purchaseTeamName,
                                           Globals.purchasePlayerDesc);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedplayercard2000",
                                                                 Globals.purchaseTeamName,
                                                                 Globals.purchasePlayerDesc,
                                                                 Globals.purchaseLeagueName));
            Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedplayercard5000, StringComparison.Ordinal))
            {
                Globals.unlockedPlayerCard(Globals.purchaseTeamIdx,
                                           Globals.purchaseLeagueName,
                                           Globals.purchaseTeamName,
                                           Globals.purchasePlayerDesc);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedplayercard5000",
                                                                Globals.purchaseTeamName,  
                                                                Globals.purchasePlayerDesc,
                                                                Globals.purchaseLeagueName));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedplayercard8000, StringComparison.Ordinal))
            {
                Globals.unlockedPlayerCard(Globals.purchaseTeamIdx,
                                           Globals.purchaseLeagueName,
                                           Globals.purchaseTeamName,
                                           Globals.purchasePlayerDesc);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedplayercard8000",
                                                                 Globals.purchaseTeamName,
                                                                 Globals.purchasePlayerDesc,
                                                                 Globals.purchaseLeagueName));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, unlockedplayercard10000, StringComparison.Ordinal))
            {
                Globals.unlockedPlayerCard(Globals.purchaseTeamIdx,
                                           Globals.purchaseLeagueName,
                                           Globals.purchaseTeamName,
                                           Globals.purchasePlayerDesc);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("unlockedplayercard10000",
                                                                 Globals.purchaseTeamName,
                                                                 Globals.purchasePlayerDesc,
                                                                 Globals.purchaseLeagueName));
            Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            } else if (String.Equals(args.purchasedProduct.definition.id, attack1team, StringComparison.Ordinal))
            {            
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack1team", 0, 1, 0));
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 1, 0);
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attack2team, StringComparison.Ordinal))
            {            
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 2, 0);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack2team", 0, 2, 0));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attack3team, StringComparison.Ordinal))
            {           
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 3, 0);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack3team", 0, 3, 0));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attack4team, StringComparison.Ordinal))
            {          
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 4, 0);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack4team", 0, 4, 0));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attack5team, StringComparison.Ordinal))
            {           
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 5, 0);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack5team", 0, 5, 0));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attack6team, StringComparison.Ordinal))
            {           
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 6, 0);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack6team", 0, 6, 0));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attack7team, StringComparison.Ordinal))
            {           
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 7, 0);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack7team", 0, 7, 0));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attack8team, StringComparison.Ordinal))
            {         
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 8, 0);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack8team", 0, 8, 0));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attack9team, StringComparison.Ordinal))
            {       
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 9, 0);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack9team", 0, 9, 0));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attack10team, StringComparison.Ordinal))
            {          
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 10, 0);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attack10team", 0, 10, 0));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense1team, StringComparison.Ordinal))
            {           
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 1);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense1team", 0, 0, 1));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense2team, StringComparison.Ordinal))
            {          
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 2);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense2team", 0, 0, 2));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense3team, StringComparison.Ordinal))
            {            
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 3);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense3team", 0, 0, 3));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense4team, StringComparison.Ordinal))
            {          
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 4);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense4team", 0, 0, 4));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense5team, StringComparison.Ordinal))
            {    
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 5);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense5team", 0, 0, 5));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense6team, StringComparison.Ordinal))
            {            
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 6);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense6team", 0, 0, 6));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense7team, StringComparison.Ordinal))
            {          
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 7);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense7team", 0, 0, 7));    
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense8team, StringComparison.Ordinal))
            {         
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 8);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense8team", 0, 0, 8));    
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense9team, StringComparison.Ordinal))
            {           
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 9);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense9team", 0, 0, 9));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, defense10team, StringComparison.Ordinal))
            {           
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 0, 10);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("defense10team", 0, 0, 10));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, attackdefense10team, StringComparison.Ordinal))
            {           
                Globals.incTeamSkills(Globals.purchaseTeamIdx, 10, 10);
                Globals.purchasesQueue.Enqueue(new PurchaseItem("attackdefense10team", 0, 10, 10));
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            else if (String.Equals(args.purchasedProduct.definition.id, extrapromition, StringComparison.Ordinal))
            {
            //Globals.enlargeGoalSize("MEDIUM");
            //Globals.purchasesQueue.Enqueue(new PurchaseItem("christmaspromotion", "MEDIUM"));
                Globals.addCoins(10000);
                Globals.addDiamonds(20000);
                Globals.adsRemove();

                Globals.purchasesQueue.Enqueue(new PurchaseItem("diamond10000", 20000));
                Globals.purchasesQueue.Enqueue(new PurchaseItem("coin10000", 10001));
                Globals.purchasesQueue.Enqueue(new PurchaseItem("removeAds", 0));

                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
             
            }
            else
            {
                Debug.Log(string.Format("DBGPurchasing ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }

            if (Globals.isAnalyticsEnable) {
                AnalyticsResult analyticsResult = Analytics.CustomEvent("SHOP_EVENTS", new Dictionary<string, object>
                {
                    { "shopPurchase_COMPLETED", args.purchasedProduct.definition.id},
                    { "shopPurchase_COMPLETED_numGameOpen", Globals.numGameOpen}
                });
            }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
            if (Globals.isAnalyticsEnable)
            {
                AnalyticsResult analyticsResult = Analytics.CustomEvent("SHOP_EVENTS", new Dictionary<string, object>
                {
                    { "shopPurchase_FAILED_ID", product.definition.storeSpecificId},
                });
            }

            Debug.Log(string.Format("DBGPurchasing OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }

        public string getPriceByHash(string itemName)
        {
            return prices[itemName];
        }

        public string GetPrice(string productID)
        {
            if (m_StoreController == null)
            {
                //InitializePurchasing();
                return string.Empty;
            }

            return m_StoreController.products.WithID(productID).metadata.localizedPriceString;
        }    
}