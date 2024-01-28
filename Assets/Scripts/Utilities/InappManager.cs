using System.Collections;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine;
using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Newtonsoft.Json;

public class InappManager : MonoBehaviour, IStoreListener
{
    public static InappManager instance;
	private IStoreController m_StoreController;
	private IExtensionProvider m_StoreExtensionProvider;
	ConfigurationBuilder builder;

	string currentlyPurchasedItem;

	string[] iosProducts = {
		"coins100", "coins800","coins2000", "coins5000"
	};
	string[] androidProducts = {
		"coins100", "coins800","coins2000", "coins5000"
	};

	System.Action<string,string> OnPurchasedSuccess;

	private bool _returnReceipt = false;


	public string environment = "production";

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	//void Start()
	//{
	//	Debug.Log("in app start");
	//	if (m_StoreController == null)
	//	{
	//		Debug.Log("m_StoreController was null");
	//		// Begin to configure our connection to Purchasing
	//		InitializePurchasing();
	//	}
	//}

	async void Start()
	{
		try
		{
			var options = new InitializationOptions()
				.SetEnvironmentName(environment);

			await UnityServices.InitializeAsync(options);

            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
                InitializePurchasing();
            }
        }
		catch (Exception exception)
		{
			Debug.Log("exception: " + exception);

			// An error occurred during services initialization.
		}
	}
//}


//// Start is called before the first frame update
//void Start()
//   {

//}

private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void InitializePurchasing()
	{

		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			Debug.Log("IsInitialized()");

			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		this.builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		// Add a product to sell / restore by way of its identifier, associating the general identifier
		// with its store-specific identifiers.
		AddProducts();
	}

	void AddProducts()
	{
		Debug.Log("Adding Products");

		foreach (string product in GetProducts())
        {
			this.builder.AddProduct(product, ProductType.Consumable);
		}

		UnityPurchasing.Initialize(this, this.builder);
	}

	//temp functions
	string[] GetProducts()
    {
		string[] products = iosProducts;
		if (Application.platform == RuntimePlatform.Android)
			products = androidProducts;
		
		return products;
    }
	//temp functions


	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");
		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;

		//Invoke("buy", 5);
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message) {
		Debug.Log($"OnInitializeFailed InitializationFailureReason:{error}\nmessage:{message}");
	}

	public void PurchaseItem(string itemId, System.Action<string,string> onSuccess)
	{
		Debug.Log("PurchaseItem: " + IsInitialized()) ;

		if (!IsInitialized())
			return;

	//	UIManager.instance.ShowHideLoader();
		this.OnPurchasedSuccess = onSuccess;
		this.currentlyPurchasedItem = itemId;
		//	this.currentlyPurchasedItemInst = item;
		Debug.Log("PurchaseItem " + itemId);

		_returnReceipt = false;

		m_StoreController.InitiatePurchase(m_StoreController.products.WithID(this.currentlyPurchasedItem));
	}

	//public void PurchaseItem(string itemId, Item item, System.Action<string> onSuccess)
	//{

	//	if (!IsInitialized())
	//		return;

	////	UIManager.instance.ShowLoader();
	////	this.onPurchasedSuccess = onSuccess;
	//	this.currentlyPurchasedItem = itemId;
	////	this.currentlyPurchasedItemInst = item;
	//	Debug.Log("PurchaseItem " + itemId + " - " + item.shortCode);

	////	_returnReceipt = false;

	//	m_StoreController.InitiatePurchase(m_StoreController.products.WithID(this.currentlyPurchasedItem));
	//}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		Debug.Log("Purchase Result " + args);
	//	UIManager.instance.HideLoader();

		string payload = "";
		string signature = "";
		string receipt = args.purchasedProduct.receipt;

		if (String.Equals(args.purchasedProduct.definition.id, this.currentlyPurchasedItem))
        {

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
				payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(receipt)["Payload"];
				//            string payload = ParseAppleReceipt(args.purchasedProduct.receipt);
				//            Debug.Log("Success    " + payload);

				this.OnPurchasedSuccess(payload,null);
			}
			else if (Application.platform == RuntimePlatform.Android)
            {
				//            string purchaseData, dataSignature;
				//            ParseGooglePlayReceipt(args.purchasedProduct.receipt, out purchaseData, out dataSignature);
				//            Debug.Log("Success android   " + purchaseData);
				//Debug.Log("args.purchasedProduct.receipt:   " + args.purchasedProduct.receipt);

				//Debug.Log("dataSignature:  " + dataSignature);

				//Debug.Log("dataSignature:  " + args.purchasedProduct.transactionID);


				//this.OnPurchasedSuccess(args.purchasedProduct.receipt,dataSignature);


				payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(receipt)["Payload"];
				Debug.Log("Payload " + payload);

				char[] charsToTrim = { '\\', '"', ':', ',' };

				signature = getBetween(payload, "signature", "skuDetails").Trim(charsToTrim);
				Debug.Log("Signature: " + signature);

				payload = getBetween(payload, "json", "signature").Trim(charsToTrim);

				payload = payload.Replace("\\", "");

			//	postData.Add("signature", signature);
			//	postData.Add("platform", "android");

				this.OnPurchasedSuccess(payload, signature);
			}
            else
            {
                Debug.Log("Cannot redeem on this platform or on editor.");
            }
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
	}

	string getBetween(string strSource, string strStart, string strEnd)
	{
		if (strSource.Contains(strStart) && strSource.Contains(strEnd))
		{
			int Start, End;
			Start = strSource.IndexOf(strStart, 0) + strStart.Length;
			End = strSource.IndexOf(strEnd, Start);
			return strSource.Substring(Start, End - Start);
		}

		return "";
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
	//	UIManager.instance.HideLoader();

	////	UIManager.instance.ShowAlertPopup(failureReason.ToString(), LocalizationManager.Localize("[Failed]")+ "!");

		Debug.Log("Purchase Failed " + failureReason.ToString());
	}

	private string ParseAppleReceipt(string receipt)
	{
		var wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
		var payload = (string)wrapper["Payload"];
		return payload;
	}

	private void ParseGooglePlayReceipt(string receipt, out string purchaseData, out string dataSignature)
	{
		var wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
		var payload = (string)wrapper["Payload"];
		var details = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
		purchaseData = (string)details["json"];
		dataSignature = (string)details["signature"];
	}

	public string GetLocalPrice(string productID)   //useable to show local price 
	{
		if (m_StoreController == null)
		{
			return "0";
		}
		return m_StoreController.products.WithID(productID).metadata.localizedPriceString;
	}

}