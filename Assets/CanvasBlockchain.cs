using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using System;

public class CanvasBlockchain : MonoBehaviour
{
    public Button buttonGold;
    public Button buttonHP;
    public Button buttonx2Gold;
    public Button buttonAccept;

    public Text ClaimingStatusText;

    public Button claimTokenBtn;

    public Text tokenBalanceText;

    public Text coinsText;
    public Text currentHPText;
    public Text x2GoldText;

    public string Address { get; private set; }

    private string TokenAddressSmartContract = "0xa089B9B587a26b42Fe944Dd32A3B43dFA60E7573";

    private string coinsPlayerPrefs = "Coins";

    // Start is called before the first frame update
    public void OnWalletConnected()
    {
        buttonGold.gameObject.SetActive(true);
        buttonHP.gameObject.SetActive(true);
        buttonx2Gold.gameObject.SetActive(true);
        buttonAccept.gameObject.SetActive(true);

        buttonGold.interactable = true;
        buttonHP.interactable = true;
        buttonx2Gold.interactable = true;
        buttonAccept.interactable = true;

        ClaimingStatusText.text = "";

        x2GoldText.gameObject.SetActive(false);

        coinsText.text = PlayerPrefs.GetInt(coinsPlayerPrefs).ToString();

        TokenBalance();
    }

    public async void TokenBalance()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        var result = await contract.ERC20.BalanceOf(Address);
        tokenBalanceText.text = "Token Owned: " + result.displayValue;
        tokenBalanceText.gameObject.SetActive(true);
    }

    public async void ClaimToken()
    {
        buttonAccept.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        int tokenclaimed = 10;
        var result = await contract.ERC20.ClaimTo(Address, tokenclaimed.ToString());
        ClaimingStatusText.text = "+10 Token";
        TokenBalance();
        ButtonManager.TriggerButtonRestore();
        buttonAccept.interactable = true;
    }

    public static int ConvertStringToRoundedInt(string numberStr)
    {
        // Convert the string to a double
        double number = double.Parse(numberStr);

        // Round the number
        double roundedNumber = Math.Round(number);

        // Convert to int and return
        return (int)roundedNumber;
    }

    public async void ClaimGold()
    {
        buttonAccept.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        var numberStr = await contract.ERC20.BalanceOf(Address);
        int roundedInt = ConvertStringToRoundedInt(numberStr.displayValue);
        if (roundedInt <= 0)
        {
            ClaimingStatusText.text = "Not Enough Token!";
            return;
        }
        await contract.ERC20.Burn("1");
        UpdateGold(50);
        coinsText.text = PlayerPrefs.GetInt(coinsPlayerPrefs).ToString();
        Debug.Log("Gold claimed");
        ClaimingStatusText.text = "+50 Gold";
        TokenBalance();
        ButtonManager.TriggerButtonRestore();
        buttonAccept.interactable = true;
    }

    public async void ClaimHP()
    {
        buttonAccept.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        var numberStr = await contract.ERC20.BalanceOf(Address);
        int roundedInt = ConvertStringToRoundedInt(numberStr.displayValue);
        if (roundedInt <= 1)
        {
            ClaimingStatusText.text = "Not Enough Token!";
            return;
        }
        await contract.ERC20.Burn("2");
        BlockchainShopManager.Instance.hp = 1;
        currentHPText.text = "4";
        Debug.Log("HP claimed");
        ClaimingStatusText.text = "+1 HP";
        TokenBalance();
        ButtonManager.TriggerButtonRestore();
        buttonAccept.interactable = true;
    }

    public async void Claimx2Gold()
    {
        buttonAccept.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        var numberStr = await contract.ERC20.BalanceOf(Address);
        int roundedInt = ConvertStringToRoundedInt(numberStr.displayValue);
        if (roundedInt <= 2)
        {
            ClaimingStatusText.text = "Not Enough Token!";
            return;
        }
        await contract.ERC20.Burn("3");
        BlockchainShopManager.Instance.goldx2 = 2;
        x2GoldText.gameObject.SetActive(true);
        Debug.Log("HP claimed");
        ClaimingStatusText.text = "X2 GOLD";
        TokenBalance();
        ButtonManager.TriggerButtonRestore();
        buttonAccept.interactable = true;
    }


    private void UpdateGold(int coinAdded) {
        int coinsLeft = PlayerPrefs.GetInt(coinsPlayerPrefs);
        //Register the item lock state in the player prefs
        PlayerPrefs.SetInt(coinsPlayerPrefs, coinsLeft + coinAdded);
    }
}
